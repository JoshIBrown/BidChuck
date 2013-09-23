angular.element(document).ready(function () {
    'use strict';
    var app = angular.module('sendInvitation', []);
    app.controller('SendInvitationCtrl', ['$scope', '$http', function ($scope, $http) {
        $scope.searchCompanies = function () {
            // possibly employ a $timeout to prevent click spamming
            var searchString = $scope.companySearchString;
            $http.get('/api/Company/GetSearch/?query=' + searchString)
                .success(function (result) {
                    $scope.companies = result;
                });
        };
    }]);
    angular.bootstrap(document, ['sendInvitation']);
});

