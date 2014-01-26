var app = angular.module('projectIndex', ['filters'])
    .controller('ProjectIndexCtrl', ['$scope', '$http', function ($scope, $http) {

        $http.get('/api/Projects', { params: { type: 'IamArchitect' } }).success(function (result) {
            $scope.myProjects = result;
        });

        $http.get('/api/Projects', { params: { type: 'MyCreated' } }).success(function (result) {
            $scope.myCreateProjects = result;
        });

        $http.get('/api/Projects', { params: { type: 'InvitedTo' } }).success(function (result) {
            $scope.invitedProjects = result;
        });

        $http.get('/api/Projects', { params: { type: 'Open' } }).success(function (result) {
            $scope.publicProjects = result;
        });
    }]);