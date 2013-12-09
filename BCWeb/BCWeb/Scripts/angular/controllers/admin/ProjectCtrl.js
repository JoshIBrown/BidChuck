angular.element(document).ready(function () {
    var app = angular.module('projectList', ['DataTablesDirective']);
    app.controller('ProjectCtrl', ['$scope', '$http', '$compile', function ($scope, $http, $compile) {

        $scope.myToolbar = '<input type="button" class="small button" value="Add Project" ng-click="Add()" />';

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
            "sAjaxSource": "/api/Project/GetDataTable",
            "sDom": '<"toolbar">lfrtip',
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) { // compile any angular code in the row
                $compile(nRow)($scope);
            }
        };

        $scope.columnDefs = [
            { "mDataProp": "Id", "aTargets": [0], "mRender": function (data, type, full) {
                return '<a href="/Admin/Project/Details/' + data + '">' + data + '</a>';
            }},
            { "mDataProp": "Number", "aTargets": [1] },
            { "mDataProp": "Title", "aTargets": [2] },
            { "mDataProp": "BidDate", "aTargets": [3] },
            { "mDataProp": "State", "aTargets": [4] },
            { "mDataProp": "CreatedBy", "aTargets": [5] },
            { "mDataProp": "Architect", "aTargets": [6] },
            { "mDataProp": "ProjectType", "aTargets": [7] },
            { "mDataProp": "ConstructionType", "aTargets": [8] },
            { "mDataProp": "BuildingType", "aTargets": [9] }
        ];

        $scope.Add = function () {
            window.location = "/Admin/Project/Create/";
        }
    }]);
    angular.bootstrap(document, ['projectList']);
});