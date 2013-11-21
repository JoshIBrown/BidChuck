angular.element(document).ready(function () {
    var app = angular.module('svComposeBid', []);
    app.controller('ComposeBidCtrl', ['$scope', '$http', '$compile', function ($scope, $http, $compile) {
        $scope.projectId = angular.elment('#ProjectId').val();



    }]);
    angular.bootstrap(document, ['svComposeBid']);
});