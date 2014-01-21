angular.element(document).ready(function () {

    var searchApp = angular.module('searchApp', []);

    function SearchCtrl(scope, http) {

        scope.performSearch = function () {

            if (scope.queryString) {
                // as long as there is something to seach for
                // otherwise, don't send an empty search request
                if (scope.queryString.length > 0) {

                    http.get('/api/Companies',
                        {
                            params: { query: scope.queryString, }
                        })
                        .success(function (result) {
                            scope.searchResults = result;
                        });
                }
            }
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
    };

    searchApp.controller('SearchCtrl', SearchCtrl);

    SearchCtrl.$inject = ['$scope', '$http'];

    angular.bootstrap(document, ['searchApp']);
});