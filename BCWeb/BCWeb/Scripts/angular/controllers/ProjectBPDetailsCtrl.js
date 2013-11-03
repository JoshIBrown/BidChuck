angular.element(document).ready(function () {
    var app = angular.module('bpDetails', []);
    app.controller('BPDetailsCtrl', ['$scope', '$http', '$compile', function ($scope, $http, $compile) {
        $scope.inviteId = angular.element('#inviteId').val();
        $scope.token = angular.element('input[name=__RequestVerificationToken]').val();

        $scope.accept = function () {
            $http.post('/api/Invitation/PostAccept/' + $scope.inviteId, null, {
                xsrfHeaderName: "X-XSRF-Token",
                xsrfCookieName: '__RequestVerificationToken',
                headers: { "X-XSRF-Token": $scope.token }
            })
                .success(function (result) {

                    // change buttons so that only decline is showing
                    var wrapper = angular.element('#inviteResponseWrapper').html('<input id="declineBtn" type="button" value="Decline Invite" class="small alert button" ng-click="decline()" />');
                    $compile(wrapper)($scope);
                    angular.element(inviteStatusWrapper).html('Accepted: ' + result.data.date);
                });
        };
        $scope.decline = function () {
            $http.post('/api/Invitation/PostDecline/' + $scope.inviteId, null, {
                xsrfHeaderName: "X-XSRF-Token",
                xsrfCookieName: '__RequestVerificationToken',
                headers: { "X-XSRF-Token": $scope.token }
            })
                .success(function (result) {
                    var wrapper = angular.element('#inviteResponseWrapper').html('<input id="acceptBtn" type="button" value="Accept Invite" class="small success button" ng-click="accept()" />');
                    $compile(wrapper)($scope);
                    angular.element(inviteStatusWrapper).html('Declined: ' + result.data.date);
                    // hide decline button
                });
        };

        $scope.join = function () {
            $http.post('/api/Invitation/PostJoin', null,
                {
                    xsrfHeaderName: "X-XSRF-Token",
                    xsrfCookieName: '__RequestVerificationToken',
                    headers: { "X-XSRF-Token": $scope.token }
                })
            .success(function (result) {
                var wrapper = angular.element('#joinProjectWrapper').html('<input id="leaveBtn" type="button" value="Leave Project" class="small alert button" ng-click="leave()" />');
                $compile(wrapper)($scope);
                angular.element(inviteStatusWrapper).html('Declined: ' + result.data.date);

            });
        };

        $scope.leave = function () {
            $http.post('/api/Invitation/PostLeave', null,
                {
                    xsrfHeaderName: "X-XSRF-Token",
                    xsrfCookieName: "__RequestVerificationToken",
                    headers: { "X-XSRF-Token": $scope.token }
                })
            .success(function (result) {
                var wrapper = angular.element('#joinProjectWrapper').html('<input id="joinBtn" type="button" value="Join Project" class="small success button" ng-click="join()" />');
                $compile(wrapper)($scope);
                angular.element(inviteStatusWrapper).html('Declined: ' + result.data.date);

            });
        };
    }]);
    angular.bootstrap(document, ['bpDetails']);
});
