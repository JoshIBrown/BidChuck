var app = angular.module('projectIndex', ['filters'])
    .controller('ProjectIndexCtrl', function ($scope, $http) {

        $http.get('/api/Projects/GetMyCreated').success(function (result) {
            $scope.myCreateProjects = result;
        });
    });