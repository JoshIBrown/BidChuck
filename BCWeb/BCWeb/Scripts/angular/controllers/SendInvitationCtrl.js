angular.element(document).ready(function () {
    'use strict';
    var app = angular.module('sendInvitation', []);
    app.controller('SendInvitationCtrl', ['$scope', '$http', function ($scope, $http) {
        $scope.invited = [];
        $scope.filterId = [];
        $scope.companies = [];
        // search system for companies matching query string
        $scope.searchCompanies = function () {
            var searchString = $scope.companySearchString;
            $http.get('/api/Company/GetSearch/?query=' + searchString)
                .success(function (result) {
                    $scope.companies = result;
                });
        };

        $scope.filteredSearch = function () {
            // filter array of companies to exclude ones chosen for invite
            return $scope.companies.filter(function (company) {
                return $scope.filterId.indexOf(company.Id) === -1;
            });

        };

        $scope.invite = function (id) {
            // add user with id to list of invites
            var searchResult = $scope.companies;
            for (var i = 0; i < searchResult.length; i++) {
                if (searchResult[i].Id === id) {
                    $scope.invited.push(searchResult[i]);
                    $scope.filterId.push(searchResult[i].Id);
                    break;
                }
            }

        };

        $scope.uninvite = function (id) {
            for (var i = 0; i < $scope.invited.length; i++) {
                if ($scope.invited[i].Id === id) {
                    $scope.invited.splice(i, 1);
                    break;
                }
            };
            for (var i = 0; i < $scope.filterId.length; i++) {
                if ($scope.filterId[i] === id) {
                    $scope.filterId.splice(i, 1);
                    break;
                }
            };

        }
    }]);
    angular.bootstrap(document, ['sendInvitation']);
});

