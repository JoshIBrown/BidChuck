angular.element(document).ready(function () {
    var app = angular.module('projectDetails', ['filters']);
    app.controller('ProjectDetailsCtrl', ['$scope', '$http', '$compile', function ($scope, $http, $compile) {

        $scope.token = angular.element('input[name=__RequestVerificationToken]').val();

        $scope.ProjectId = angular.element('#ProjectId').val();

        $http.get('/api/BidPackage/GetInvitedPackagesForProject/?projectId=' + $scope.ProjectId)
            .success(function (result) {
                $scope.myData = result;
                var wrapper = angular.element('#bidPackageWrapper');
                $compile(wrapper)($scope);
            });

        $scope.accept = function (bpId) {

            $http.post('/api/Invitation/PostAccept/?bidPackageId=' + bpId, null, {
                xsrfHeaderName: "X-XSRF-Token",
                xsrfCookieName: '__RequestVerificationToken',
                headers: { "X-XSRF-Token": $scope.token, "X-Requested-With": "XMLHttpRequest" }
            })
                .success(function (result) {
                    // set invitation response
                    $scope.SetInviteResponse(bpId, true);
                });
        };

        $scope.decline = function (bpId) {

            $http.post('/api/Invitation/PostDecline/?bidPackageId=' + bpId, null, {
                xsrfHeaderName: "X-XSRF-Token",
                xsrfCookieName: '__RequestVerificationToken',
                headers: { "X-XSRF-Token": $scope.token, "X-Requested-With": "XMLHttpRequest" }
            })
                .success(function (result) {
                    // set invitation response
                    $scope.SetInviteResponse(bpId, false);
                });
        };

        $scope.SetInviteResponse = function (bpId, resp) {
            for (i = 0; i < $scope.myData.BidPackages.length; i++) {
                if ($scope.myData.BidPackages[i].BidPackageId === bpId) {
                    $scope.myData.BidPackages[i].InviteResponse = resp;
                    return;
                }
            }
        };


    }]);
    app.filter('IsScopeIncluded', function () {
        return function (scope, selectedScopes) {
            for (i = 0; i < selectedScopes.length; i++) {
                if (selectedScopes[i] === scope) {
                    return "inc";
                }
            }
            return "---";
        };
    });

    angular.bootstrap(document, ['projectDetails']);
});
