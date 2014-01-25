angular.element(document).ready(function () {

    var profileApp = angular.module("profileApp", []);

    function ProfileCtrl(scope, http, compile) {
        scope.companyId = angular.element("#Id").val();

        scope.contactButtonUrl = '';

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
                    case "BlackListed":
                        break;
                    case "NotConnected":
                        scope.contactButtonUrl = '/Company/NotConnected';
                        break;
                    case "Self":
                        scope.contactButtonUrl = '/Company/Self';
                        break;
                }
            });

        scope.sendContactRequest = function () {

            http.post('/api/ConnectionRequest', { id: scope.companyId })
                .success(function (result) {
                    // change button to request sent

                    // compile

                });
        };
    };

    profileApp.controller("ProfileCtrl", ProfileCtrl);

    ProfileCtrl.$inject = ['$scope', '$http', '$compile'];

    angular.bootstrap(document, ["profileApp"]);
});