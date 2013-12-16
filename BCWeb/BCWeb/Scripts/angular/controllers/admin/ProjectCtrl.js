angular.element(document).ready(function () {
    var app = angular.module('projectList', ['ngDataTables']);
    app.controller('ProjectCtrl', ['$scope', '$http', '$compile', function ($scope, $http, $compile) {

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
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) { // compile any angular code in the row
                $compile(nRow)($scope);
            }
        };
        $scope.columnDefs = [
            { "mDataProp": "Id", "aTargets": [0] },
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
    }]);
    angular.bootstrap(document, ['projectList']);
});