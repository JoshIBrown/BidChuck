angular.element(document).ready(function () {
    var app = angular.module('bpDetails');
    app.controller('BPDetailsCtrl', ['$scope', '$http','$compile', function ($scope, $http,$compile) {
        $scope.inviteId = angular.element('#BidPackageId').val();

        $scope.accept = function () {
            //$http.post('/api/Invitation/Accept/' + $scope.inviteId)
            //    .success(function (result) {
            //        alert(result);
            //    });
        };
        $scope.decline = function () {
            //$http.post('/api/Invitation/Decline/' + $scope.inviteId)
            //    .success(function (result) {
            //        alert(result);
            //        // hide decline button
            //    });
        };
    }]);
    angular.bootstrap(document, ['bpDetails']);
});
