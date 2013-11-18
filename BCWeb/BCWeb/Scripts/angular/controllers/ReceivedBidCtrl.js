angular.element(document).ready(function () {
    var app = angular.module('receivedBids', []);

    app.controller('ReceivedBidsCtrl', ['$scope', '$http', function ($scope, $http) {
        $scope.projectId = angular.element('#ProjectId').val();

        $scope.GetScopes = function () {
            if ($scope.selectedBP) {
                $http.get('/api/Bid/GetScopesForBidPackages/?bidPackageId=' + $scope.selectedBP)
                    .success(function (result) {
                        $scope.bpScopes = result;
                    });

            } else {
                $scope.bpScopes = [];
            }
        };

        $http.get('/api/Bid/GetBidPackagesForProject/?projectId=' + $scope.projectId)
            .success(function (result) {
                $scope.bidPackages = result;

            });


    }]);

    angular.bootstrap(document, ['receivedBids']);
});