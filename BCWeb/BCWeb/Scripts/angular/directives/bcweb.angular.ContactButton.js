var contactButtonApp = angular.module('bcContactButton', []);

function ContactButtonDirective(http, compile) {
    "use strict";

    // linker function
    function link(scope, elm, attrs) {
        scope.$watch('contactStatus', function (nVal) {

            if (nVal) {
                // do the replace innter html here
                var buttonContent = '';
                var contactButtonUrl = '';

                switch (nVal) {
                    case "Connected":
                        contactButtonUrl = '/Company/Connected';
                        break;
                    case "InvitationSent":
                        contactButtonUrl = '/Company/RequestSent';
                        break;
                    case "InvitationPending":
                        contactButtonUrl = '/Company/PendingRequest';
                        break;
                    case "NotConnected":
                        contactButtonUrl = '/Company/NotConnected';
                        break;
                    case "Self":
                        contactButtonUrl = '/Company/Self';
                        break;
                    case "BlackListed":
                        contactButtonUrl = '/Company/BlackListed';
                        break;
                        // TODO: maybe i should throw exception if not one of above
                }

                http.get(contactButtonUrl).success(function (result) {
                    elm.html(result);

                    angular.element(document).foundation();

                    compile(elm.contents())(scope);
                });
            }
        });
    };

    function controller($scope, $http) {
        // check connection status
        $scope.checkConnectionStatus = function () {
            $http.get('/api/ConnectionStatus', { params: { companyId: $scope.companyId } })
                .success(function (result) {

                    var foo = result;
                    $scope.contactStatus = result.content;
                });
            $(document).foundation();
        }

        // send connection request
        $scope.sendContactRequest = function () {

            var token = angular.element('input[name=__RequestVerificationToken]').val();

            $http.post('/api/ContactRequest', null, {
                params: { recipientId: $scope.companyId },
                xsrfHeaderName: "X-XSRF-Token",
                xsrfCookieName: "__RequestVerificationToken",
                headers: { "X-XSRF-Token": token, "X-Requested-With": "XMLHttpRequest" }
            })
                .success(function (result) {
                    // change button to request sent
                    $scope.checkConnectionStatus();
                });
        };

        // accept request
        $scope.acceptContactRequest = function () {

            var token = angular.element('input[name=__RequestVerificationToken]').val();

            $http.put('/api/ContactRequest', null, {
                params: { senderId: $scope.companyId, accept: true },
                xsrfHeaderName: "X-XSRF-Token",
                xsrfCookieName: "__RequestVerificationToken",
                headers: { "X-XSRF-Token": token, "X-Requested-With": "XMLHttpRequest" }
            })
                .success(function (result) {
                    $scope.checkConnectionStatus();
                });
        };

        // decline request
        $scope.declineContactRequest = function () {

            var token = angular.element('input[name=__RequestVerificationToken]').val();

            $http.put('/api/ContactRequest', null, {
                params: { senderId: $scope.companyId, accept: false },
                xsrfHeaderName: "X-XSRF-Token",
                xsrfCookieName: "__RequestVerificationToken",
                headers: { "X-XSRF-Token": token, "X-Requested-With": "XMLHttpRequest" }
            })
                .success(function (result) {
                    $scope.checkConnectionStatus();
                });
        };

        $scope.removeContact = function () {

            var token = angular.element('input[name=__RequestVerificationToken]').val();

            http({
                url: '/api/Contacts',
                method: 'DELETE',
                params: { companyToDeleteId: $scope.companyId },
                xsrfHeaderName: "X-XSRF-Token",
                xsrfCookieName: "__RequestVerificationToken",
                headers: { "X-XSRF-Token": token, "X-Requested-With": "XMLHttpRequest" }
            }).success(function (result) {
                $scope.checkConnectionStatus();
            });
        };


        // blacklist company
        $scope.addToBlackList = function () {

            var token = angular.element('input[name=__RequestVerificationToken]').val();

            $http.post('/api/BlackList/', null, {
                params: { companyToBlackList: $scope.companyId },
                xsrfHeaderName: "X-XSRF-Token",
                xsrfCookieName: "__RequestVerificationToken",
                headers: { "X-XSRF-Token": token, "X-Requested-With": "XMLHttpRequest" }
            })
            .success(function (result) {
                $scope.checkConnectionStatus();
            });
        };


        // de-blacklist company
        $scope.removeFromBlackList = function () {

            var token = angular.element('input[name=__RequestVerificationToken]').val();

            http({
                url: '/api/BlackList',
                method: 'DELETE',
                params: { companyToBlackList: $scope.companyId },
                xsrfHeaderName: "X-XSRF-Token",
                xsrfCookieName: "__RequestVerificationToken",
                headers: { "X-XSRF-Token": token, "X-Requested-With": "XMLHttpRequest" }
            }).success(function (result) {
                $scope.checkConnectionStatus();
            });
        };

        // cancel request
        $scope.cancelRequest = function () {

            var token = angular.element('input[name=__RequestVerificationToken]').val();

            http({
                url: '/api/ContactRequest',
                method: 'DELETE',
                params: { recipientId: $scope.companyId },
                xsrfHeaderName: "X-XSRF-Token",
                xsrfCookieName: "__RequestVerificationToken",
                headers: { "X-XSRF-Token": token, "X-Requested-With": "XMLHttpRequest" }
            }).success(function (result) {
                $scope.checkConnectionStatus();
            });
        };

        // initial connection status check
        $scope.checkConnectionStatus();
    };

    var directiveObject = {
        restrict: 'A',
        scope: { 'companyId': '=' },
        link: link,
        controller: controller
    };

    return directiveObject;
};

ContactButtonDirective.$inject = ['$http', '$compile'];

contactButtonApp.directive('contactButton', ContactButtonDirective);