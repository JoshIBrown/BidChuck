angular.element(document).ready(function () {
    "use strict";

    var profileApp = angular.module('profileApp', ['bcContactButton']);

    function ProfileCtrl(scope, http) {

        scope.companyId = angular.element("#Id").val();

        http.get('/api/Contacts', { params: { companyId: scope.companyId } })
            .success(function (result) {
                scope.contacts = result;
            });
    };

    profileApp.controller('ProfileCtrl', ['$scope', '$http', ProfileCtrl]);

    angular.bootstrap(document, ['profileApp']);

});