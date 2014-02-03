angular.element(document).ready(function () {
    "use strict";
    var searchApp = angular.module('searchApp', ['bcCsiScopePicker']);

    function SearchCtrl(scope, http) {

        scope.performSearch = function () {
            scope.searchResults = [];
            // validate city or postal.  one or the other must be provided if doing geography search.
            var city = scope.searchForm.city.$viewValue;
            var distance = scope.searchForm.distance.$viewValue;
            var state = scope.searchForm.state.$viewValue;
            var postal = scope.searchForm.postal.$viewValue;


            // city and blank state and blank postal
            if (city && !state && !postal) {
                scope.invalidGeo = true;
                return;
            } else {
                scope.invalidGeo = false;
            }

            // build array of selected scope id's
            var selectedScopes = [];

            angular.forEach(scope.selectedScopes, function (value, key) {
                if (value.Checked) {
                    selectedScopes.push(value.Id)
                }
            });

            // build array of business types
            var businessTypes = [];

            angular.forEach(scope.BusinessType, function (value, key) {
                if (value)
                    businessTypes.push(key);
            });


            // submit search
            http.get('/api/Companies',
                {
                    params: {
                        query: scope.searchForm.query.$viewValue ? scope.searchForm.query.$viewValue : '',
                        city: scope.searchForm.city.$viewValue ? scope.searchForm.city.$viewValue : '',
                        distance: scope.searchForm.distance.$viewValue ? scope.searchForm.distance.$viewValue : '',
                        state: scope.searchForm.state.$viewValue ? scope.searchForm.state.$viewValue : '',
                        postal: scope.searchForm.postal.$viewValue ? scope.searchForm.postal.$viewValue : '',
                        type: businessTypes,
                        scopeId: selectedScopes
                    }
                })
                .success(function (result) {
                    scope.searchResults = result;
                });
        };

        scope.clearResults = function () {
            scope.searchResults = [];
            angular.element('#query').val('');
        };

        scope.hasResults = function () {
            if (scope.searchResults) {
                if (scope.searchResults.length > 0) {
                    return true;
                }
            }
            return false
        };

        scope.theServiceUrl = '/api/Scopes';
    };

    searchApp.controller('SearchCtrl', SearchCtrl);

    SearchCtrl.$inject = ['$scope', '$http'];

    angular.bootstrap(document, ['searchApp']);
});