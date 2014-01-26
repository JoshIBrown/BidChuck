angular.element(document).ready(function () {

    var profileApp = angular.module("profileApp", []);

    function ProfileCtrl(scope, http, compile) {
        scope.companyId = angular.element("#Id").val();

        scope.contactButtonUrl = '';


        // initial connection status check
        scope.checkConnectionStatus();

        // check connection status
        scope.checkConnectionStatus = function () {
            http.get('/api/ConnectionStatus', { params: { companyId: scope.companyId } })
                .success(function (result) {
                    var foo = result;
                    switch (result.content) {
                        case "Connected":
                            scope.contactButtonUrl = '/Company/Connected';
                            break;
                        case "InvitationSent":
                            scope.contactButtonUrl = '/Company/RequestSent';
                            break;
                        case "InvitationPending":
                            scope.contactButtonUrl = '/Company/PendingRequest';
                            break;
                        case "Blocked":
                            break;
                        case "NotConnected":
                            scope.contactButtonUrl = '/Company/NotConnected';
                            break;
                        case "Self":
                            scope.contactButtonUrl = '/Company/Self';
                            break;
                    }
                });
        }

        // send connection request
        scope.sendContactRequest = function () {
            http.post('/api/ContactRequest', { recipientId: scope.companyId })
                .success(function (result) {
                    // change button to request sent
                    scope.checkConnectionStatus();
                });
        };

        // accept request

        scope.acceptContactRequest = function () {
            http.put('/api/ContactRequest', { senderId: scope.companyId, accept: true })
                .success(function (result) {
                    scope.checkConnectionStatus();
                });
            };
        // decline request
        // block company
        // cancel request

    };

    profileApp.controller("ProfileCtrl", ProfileCtrl);

    ProfileCtrl.$inject = ['$scope', '$http', '$compile'];

    angular.bootstrap(document, ["profileApp"]);
});