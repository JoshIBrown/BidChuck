var app = angular.module('regApp', []).controller('RegCtrl', function ($scope, $http) {
    $scope.states = [];
    $scope.counties = [];

    $http.get("/api/Account/GetStates").success(function (data) {
        $scope.states = data;
    });

    $scope.updateCounties = function () {
        $http.get("/api/Account/GetCounties/?stateId=" + $scope.selectedState)
            .success(function (data) {
                $scope.counties = data;
            });
    };
});