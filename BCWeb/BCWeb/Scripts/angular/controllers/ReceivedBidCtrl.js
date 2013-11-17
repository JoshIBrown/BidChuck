angular.element(document).ready(function () {
    var app = angular.module('receivedBids', []);

    app.controller('ReceivedBidsCtrl',['$scope','$http',function($scope,$http){
        $scope.projectId = angular.element('#ProjectId').val();
        $http.get('/api/Bid/GetBidPackagesForProject/?projectId=' + $scope.projectId)
            .success(function (result) {
                $scope.bidPackages = result;
            });

        $http.get('/api/Bid/GetBidsToReviewForProject/?projectId=' + $scope.projectId)
            .success(function (result) {
                $scope.myData = result;
            });
    }]);

    angular.bootstrap(document, ['receivedBids']);
});