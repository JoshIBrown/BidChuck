
angular.element(document).ready(function () {
    var app = angular.module('homePage', ['filters']);


    function HomeCtrl($scope, $http) {
        console.log('poo');
        $scope.newCompanies = [];
        $http.get('/api/Users/GetNewestCompanies')
            .success(function (data) {
                $scope.newCompanies = data;
            });

    }

    HomeCtrl.$inject = ['$scope', '$http'];

    app.controller('HomeCtrl', HomeCtrl);

    angular.bootstrap(document, ['homePage']);
});