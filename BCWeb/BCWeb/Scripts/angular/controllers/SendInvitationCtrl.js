angular.element(document).ready(function () {
    'use strict';
    var app = angular.module('sendInvitation', []);

    function SendInvitationCtrl($scope, $http) {
        $scope.invited = [];
        $scope.filterId = [];
        $scope.companies = [];
        $scope.bpId = angular.element('#BidPackageId').val();


        $http.get('/api/Companies/Search/BidPackageIdForScopes/' + $scope.bpId, {
            params: {
                type: ['SubContractor', 'MaterialsVendor', 'MaterialsMfg']
            }
        }).success(function (result) {
            $scope.companies = result;
        });

        $scope.getFieldName = function (i) {
            return "CompanyId[" + i + "]";
        };

        $scope.getFieldId = function (i) {
            return "CompanyId_" + i + "_";
        };

        $scope.CheckAll = function () {
            // get all checkboxes
            var checkbox = angular.element('input[type="checkbox"]');

            // if checkboxes are found
            if (checkbox) {
                // loop through collection of checkboxes
                for (var i = 0; i < checkbox.length; i++) {
                    checkbox[i].checked = true;
                }
            }
        };

        $scope.UncheckAll = function () {
            // get all checkboxes
            var checkbox = angular.element('input[type="checkbox"]');

            // if checkboxes are found
            if (checkbox) {
                // loop through collection of checkboxes
                for (var i = 0; i < checkbox.length; i++) {
                    checkbox[i].checked = false;
                }
            }
        };
    };



    SendInvitationCtrl.$inject = ['$scope', '$http'];

    app.controller('SendInvitationCtrl', SendInvitationCtrl);

    app.filter('scopeMatchFilter', function () {
        return function (company, min) {
            var arrayToReturn = [];
            if (min && company) {
                for (var i = 0; i < company.length; i++) {
                    if (company[i].ScopesOfWork) {
                        if (company[i].ScopesOfWork.length >= min) {
                            arrayToReturn.push(company[i]);
                        }
                    }
                }
            }
            return arrayToReturn;
        };
    });

    angular.bootstrap(document, ['sendInvitation']);
});

