angular.element(document).ready(function () {
    "use strict";

    var contactsApp = angular.module('contactsApp', []);

    function ContactsCtrl(scope, http) {

        scope.companyId = angular.element("#CompanyId").val();
        
        http.get('/api/Contacts', { params: { companyId: scope.companyId } })
            .success(function (result) {
                scope.contacts = result;

            });
    };

    contactsApp.controller('ContactsCtrl', ['$scope', '$http', ContactsCtrl]);

    angular.bootstrap(document, ['contactsApp']);

});