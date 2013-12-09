var mimirDirectives = angular.module('DataTablesDirective', []);

mimirDirectives.directive('ngDatatable', ['$compile', function ($compile) {
    return function (scope, element, attrs) {
        // apply DataTable options, use defaults if none specified by user
        var options = {};
        if (attrs.ngDatatable.length > 0) {
            options = scope.$eval(attrs.ngDatatable);
        } else {
            options = {
                "bStateSave": true,
                "iCookieDuration": 2419200, /* 1 month */
                "bJQueryUI": false,
                "bPaginate": true,
                "bLengthChange": true,
                "bFilter": true,
                "bSort": true,
                "bInfo": true,
                "bDestroy": true,
                "bProcessing": true,
                "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) { // compile any angular code in the row
                    $compile(nRow)(scope);
                }
            };
        }

        // Tell the dataTables plugin what columns to use
        // We can either derive them from the dom, or use setup from the controller           
        var explicitColumns = [];
        element.find('th').each(function (index, elem) {
            explicitColumns.push($(elem).text());
        });
        if (explicitColumns.length > 0) {
            options["aoColumns"] = explicitColumns;
        } else if (attrs.aoColumns) {
            options["aoColumns"] = scope.$eval(attrs.aoColumns);
        }

        // aoColumnDefs is dataTables way of providing fine control over column config
        if (attrs.aoColumnDefs) {
            options["aoColumnDefs"] = scope.$eval(attrs.aoColumnDefs);
        }


        // apply the plugin
        var dataTable = element.dataTable(options);


        // if there is a custom toolbar, render it.  will need to use toolbar in sdom for this to work
        if (options.sDom && attrs.toolbar) {
            var toolbar = scope.$eval(attrs.toolbar);
            var toobarDiv = angular.element('div.toolbar').html(toolbar);
            $compile(toobarDiv)(scope);
        }

        // this is to fix auto column sizing issues when hiding columns
        dataTable.width('100%');


        // watch for any changes to our data, rebuild the DataTable
        scope.$watch(attrs.aaData, function (value) {
            var val = value || null;
            if (val) {
                dataTable.fnClearTable();
                dataTable.fnAddData(scope.$eval(attrs.aaData));

            }
        }, true);

        if (attrs.selectable) {
            // respond to click for selecting a row
            dataTable.on('click', 'tbody tr', function (e) {
                var foo = e.currentTarget;
                var classes = foo.className.split(' ');
                var isSelected = false;
                for (i = 0; i < classes.length; i++) {
                    if (classes[i] === 'row_selected') {
                        isSelected = true;
                    }
                };


                if (isSelected) {
                    foo.className = foo.className.replace(' row_selected', '');
                }
                else {
                    dataTable.find('tbody tr.row_selected').removeClass('row_selected');
                    foo.className = foo.className + ' row_selected';
                    scope.selectedRow = dataTable.fnGetData(foo);
                }
            });


        }

    };
}]);