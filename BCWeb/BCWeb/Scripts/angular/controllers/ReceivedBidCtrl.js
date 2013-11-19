angular.element(document).ready(function () {
    var app = angular.module('receivedBids', ['filters']);

    app.controller('ReceivedBidsCtrl', ['$scope', '$http', function ($scope, $http) {
        $scope.projectId = angular.element('#ProjectId').val();
        $scope.bpScopes = [];
        $scope.companyBP = [];

        $scope.GetBids = function () {
            if ($scope.selectedBP) {
                GetScopes($scope.selectedBP);
                GetCompanyBids($scope.selectedBP);
            } else {
                $scope.bpScopes = [];
                $scope.companyBP = [];
            }
        };

        var GetCompanyBids = function (bpId) {
            $http.get('/api/Bid/GetBidsToReviewForBidPackage/?bidPackageId=' + bpId)
            .success(function (result) {
                $scope.companyBP = result;
            });
        };

        var GetScopes = function (bpId) {

            $http.get('/api/Bid/GetScopesForBidPackages/?bidPackageId=' + bpId)
                .success(function (result) {
                    $scope.bpScopes = result;
                });
        };

        $http.get('/api/Bid/GetBidPackagesForProject/?projectId=' + $scope.projectId)
            .success(function (result) {
                $scope.bidPackages = result;
                if ($scope.bidPackages.length === 1 && $scope.bidPackages[0].Text === "") {
                    angular.element('#bpDropDown').remove();
                    $scope.selectedBP = $scope.bidPackages[0].Value;
                    $scope.GetBids();
                };
            });


    }]);
    app.filter('CsiAmount', function () {
        return function (bids, scopeId) {
            if (bids) {
                for (i = 0; i < bids.length; i++) {
                    if (bids[i].ScopeId === scopeId) {
                        return bids[i].BidAmount;
                    }
                };
            }
        };
    });
    angular.bootstrap(document, ['receivedBids']);
});