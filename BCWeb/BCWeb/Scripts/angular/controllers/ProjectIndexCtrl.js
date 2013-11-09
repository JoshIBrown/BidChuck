var app = angular.module('projectIndex', ['filters'])
    .controller('ProjectIndexCtrl', ['$scope', '$http', function ($scope, $http) {

        $http.get('/api/Project/GetByMyCompanyList').success(function (result) {
            $scope.myProjects = result;
        });

        $http.get('/api/Project/GetMyCreatedList').success(function (result) {
            $scope.myCreateProjects = result;
        });

        $http.get('/api/Project/GetProjectsInvitedToList').success(function (result) {
            $scope.invitedProjects = result;
        });

        $http.get('/api/Project/GetPublicList').success(function (result) {
            $scope.publicProjects = result;
        });
    }]);