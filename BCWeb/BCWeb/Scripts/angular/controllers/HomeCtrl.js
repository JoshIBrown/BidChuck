﻿
var app = angular.module('myApp', ['filters'])
    .controller('HomeCtrl', ['$scope','$http',function ($scope, $http) {
        console.log('poo');
        $scope.newCompanies = [];
        $http.get('/api/Users/GetNewestCompanies')
            .success(function (data) {
                $scope.newCompanies = data;
            });

    }]);