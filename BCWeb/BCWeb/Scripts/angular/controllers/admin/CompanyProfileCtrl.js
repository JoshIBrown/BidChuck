angular.element(document).ready(function () {
    var app = angular.module('companyProfileList', ['DataTablesDirective']);
    app.controller('CompanyProfileCtrl', ['$scope', '$http', '$compile', function ($scope, $http, $compile) {

        $scope.myToolbar = '<input type="button" class="small button" value="Add Company" ng-click="Add()" />';

        $scope.options = {
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
            "bServerSide": true,
            "sAjaxSource": "/api/Companies",
            "sDom": '<"toolbar">lfrtip',
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) { // compile any angular code in the row
                $compile(nRow)($scope);
            }
        };
        $scope.columnDefs = [
            {
                "mDataProp": "Id", "aTargets": [0], "mRender": function (data, type, full) {
                    return '<a href="/Admin/Company/Details/' + data + '">' + data + '</a>';
                }
            },
            { "mDataProp": "CompanyName", "aTargets": [1] },
            { "mDataProp": "BusinessType", "aTargets": [2] },
            { "mDataProp": "Manager", "aTargets": [3] },
            { "mDataProp": "State", "aTargets": [4] },
            { "mDataProp": "PostalCode", "aTargets": [5] },
            { "mDataProp": "Published", "aTargets": [6] }
        ];

        $scope.Add = function () {
            window.location = "/Admin/Company/Create/";
        }
    }]);
    angular.bootstrap(document, ['companyProfileList']);
});