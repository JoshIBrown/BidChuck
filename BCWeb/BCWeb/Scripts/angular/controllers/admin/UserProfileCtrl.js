angular.element(document).ready(function () {
    var app = angular.module('userProfileList', ['ngDataTables']);
    app.controller('UserProfileCtrl', ['$scope', '$http', '$compile', function ($scope, $http, $compile) {

        $scope.myToolbar = '<input type="button" class="small button" value="Add User" ng-click="Add()" />';
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
            "sAjaxSource": "/api/User/GetDataTable",
            "sDom": '<"toolbar">lfrtip',
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                // compile any angular code in the row
                $compile(nRow)($scope);
            }
        };

        $scope.columnDefs = [
            {
                "mDataProp": "Id", "aTargets": [0], "mRender": function (data, type, full) {
                    return '<a href="/Admin/User/Details/' + data + '">' + data + '</a>';
                }
            },
            { "mDataProp": "Email", "aTargets": [1] },
            { "mDataProp": "LastName", "aTargets": [2] },
            { "mDataProp": "FirstName", "aTargets": [3] },
            { "mDataProp": "JobTitle", "aTargets": [4] },
            { "mDataProp": "CompanyId", "aTargets": [5] },
            { "mDataProp": "Confirmed", "aTargets": [6] }
        ];

        $scope.Add = function () {
            window.location = "/Admin/User/Create"
        }
    }]);
    angular.bootstrap(document, ['userProfileList']);
});