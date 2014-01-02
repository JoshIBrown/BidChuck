angular.element(document).ready(function () {
    'use strict';
    var app = angular.module('sendInvitation', []);

    function SendInvitationCtrl($scope, $http) {
        $scope.invited = [];
        $scope.filterId = [];
        $scope.companies = [];
        $scope.bpId = angular.element('#BidPackageId').val();

        $http.get('/api/Invitation/GetCompaniesToInvite/?bidPackageId=' + $scope.bpId)
                .success(function (result) {
                    $scope.companies = result;
                });

        $scope.getFieldName = function (i) {
            return "CompanyId[" + i + "]";
        };

        $scope.getFieldId = function (i) {
            return "CompanyId_" + i + "_";
        };

    };



    SendInvitationCtrl.$inject = ['$scope', '$http'];

    app.controller('SendInvitationCtrl', SendInvitationCtrl);

    app.filter('scopeMatchFilter', function () {
        return function (company, min) {
            if (company.ScopesOfWork) {
                if (company.ScopesOfWork.length >= min) {
                    return company;
                }
            }
        };
    });

    angular.bootstrap(document, ['sendInvitation']);
});

