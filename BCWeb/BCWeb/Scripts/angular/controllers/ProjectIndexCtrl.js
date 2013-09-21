var app = angular.module('projectIndex', ['filters'])
    .controller('ProjectIndexCtrl', ['$scope', '$http', function ($scope, $http) {

        $http.get('/api/Projects/GetMyCreated').success(function (result) {
            $scope.myCreateProjects = result;
        });
    }]);