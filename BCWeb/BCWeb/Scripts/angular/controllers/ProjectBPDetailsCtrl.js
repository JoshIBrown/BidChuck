﻿angular.element(document).ready(function () {
    var app = angular.module('bpDetails', []);

    function BPDetailsCtrl($scope, $http, $compile) {

        $scope.ProjectId = angular.element('#ProjectId').val();
        $scope.bidPackageId = angular.element('#BidPackageId').val();
        $scope.token = angular.element('input[name=__RequestVerificationToken]').val();

        $scope.accept = function () {
            // post choice to server
            $http.put('/api/Projects/' + $scope.ProjectId + '/Invitations/' + $scope.bidPackageId + '?rsvp=accept', null, {
                xsrfHeaderName: "X-XSRF-Token",
                xsrfCookieName: '__RequestVerificationToken',
                headers: { "X-XSRF-Token": $scope.token, "X-Requested-With": "XMLHttpRequest" }
            })
                .success(function (result) {
                    // change buttons so that only decline is showing
                    var wrapper = angular.element('#inviteResponseWrapper').html('<input id="declineBtn" type="button" value="Decline Invite" class="small alert button" ng-click="decline()" />');
                    // recompile for angular so that angular events are fired/detected
                    $compile(wrapper)($scope);
                    // add status message
                    angular.element('#inviteStatusWrapper').html('Accepted: ' + result.date);
                });
        };


        $scope.decline = function () {
            // post choice to server

            $http.put('/api/Projects/' + $scope.ProjectId + '/Invitations/' + $scope.bidPackageId + '?rsvp=decline', null, {
                xsrfHeaderName: "X-XSRF-Token",
                xsrfCookieName: '__RequestVerificationToken',
                headers: { "X-XSRF-Token": $scope.token, "X-Requested-With": "XMLHttpRequest" }
            })
                .success(function (result) {
                    // change buttons so that only Accept is showing
                    var wrapper = angular.element('#inviteResponseWrapper').html('<input id="acceptBtn" type="button" value="Accept Invite" class="small success button" ng-click="accept()" />');
                    // recompile for angular so that angular events are fired/detected
                    $compile(wrapper)($scope);
                    // add status message
                    angular.element('#inviteStatusWrapper').html('Declined: ' + result.date);
                });
        };


        $scope.join = function () {
            // post choice to server
            $http.post('/api/Projects/' + $scope.ProjectId + '/Proffer', null,
                {
                    xsrfHeaderName: "X-XSRF-Token",
                    xsrfCookieName: '__RequestVerificationToken',
                    headers: { "X-XSRF-Token": $scope.token, "X-Requested-With": "XMLHttpRequest" }
                })
            .success(function (result) {
                // change buttons so that only leave is showing
                var wrapper = angular.element('#inviteResponseWrapper').html('<input id="leaveBtn" type="button" value="Leave Project" class="small alert button" ng-click="leave()" />');
                // recompile for angular so that angular events are fired/detected
                $compile(wrapper)($scope);
                // add status message
                angular.element('#inviteStatusWrapper').html('Joined: ' + result.date);
            });
        };


        $scope.leave = function () {
            // post choice to server. long hand method.  for some reason headers weren't making it over in the request
            $http({
                url: '/api/Projects/' + $scope.ProjectId + '/Proffer',
                method: 'DELETE',
                xsrfHeaderName: "X-XSRF-Token",
                xsrfCookieName: "__RequestVerificationToken",
                headers: { "X-XSRF-Token": $scope.token, "X-Requested-With": "XMLHttpRequest" }
            })
            .success(function (result) {
                // change buttons so that only join is showing
                var wrapper = angular.element('#inviteResponseWrapper').html('<input id="joinBtn" type="button" value="Join Project" class="small success button" ng-click="join()" />');
                // recompile for angular so that angular events are fired/detected
                $compile(wrapper)($scope);
                // add status message
                angular.element('#inviteStatusWrapper').html('Left on: ' + result.date);
            });
        };
    }

    app.controller('BPDetailsCtrl', BPDetailsCtrl);

    BPDetailsCtrl.$inject = ['$scope', '$http', '$compile'];

    angular.bootstrap(document, ['bpDetails']);
});
