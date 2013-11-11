angular.element(document).ready(function () {
    var app = angular.module('projectDetails', []);
    app.controller('ProjectDetailsCtrl', ['$scope', '$http', '$compile', function ($scope, $http, $compile) {

        $scope.token = angular.element('input[name=__RequestVerificationToken]').val();
        $scope.ProjectId = angular.element('#ProjectId').val();

        $http.get('/api/BidPackage/GetInvitedPackagesForProject/?projectId=' + $scope.ProjectId)
            .success(function (result) {
                $scope.myData = result;
            });

        $scope.accept = function (bpId) {

            $http.post('/api/Invitation/PostAccept/?bidPackageId=' + bpId, null, {
                xsrfHeaderName: "X-XSRF-Token",
                xsrfCookieName: '__RequestVerificationToken',
                headers: { "X-XSRF-Token": $scope.token }
            })
                .success(function (result) {

                    // change buttons so that only decline is showing
                    var wrapper = angular.element('#inviteResponseWrapper').html('<input id="declineBtn" type="button" value="Decline Invite" class="small alert button" ng-click="decline()" />');
                    // recompile for angular so that angular events are fired/detected
                    $compile(wrapper)($scope);
                    angular.element(inviteStatusWrapper).html('Accepted: ' + result.data.date);
                });
        };
        $scope.decline = function (bpId) {

            $http.post('/api/Invitation/PostDecline/?bidPackageId=' + bpId, null, {
                xsrfHeaderName: "X-XSRF-Token",
                xsrfCookieName: '__RequestVerificationToken',
                headers: { "X-XSRF-Token": $scope.token }
            })
                .success(function (result) {
                    var wrapper = angular.element('#inviteResponseWrapper').html('<input id="acceptBtn" type="button" value="Accept Invite" class="small success button" ng-click="accept()" />');
                    // recompile for angular so that angular events are fired/detected
                    $compile(wrapper)($scope);
                    angular.element(inviteStatusWrapper).html('Declined: ' + result.data.date);
                });
        };
    }]);
    app.filter('IsScopeIncluded', function () {
        return function (scope, selectedScopes) {
            for (i = 0; i < selectedScopes.length; i++) {
                if (selectedScopes[i] === scope) {
                    return "Inc";
                }
            }
            return "-";
        };
    });

    angular.bootstrap(document, ['projectDetails']);
});
