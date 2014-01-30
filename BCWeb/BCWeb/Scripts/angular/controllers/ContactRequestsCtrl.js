angular.element(document).ready(function () {
    "use strict";

    var crApp = angular.module('ContactRequestsApp', []);

    function ContactRequestsCtrl(scope, http) {
        
        http.get('/api/ContactRequest').success(function (result) {
            scope.allRequests = result;
        });
    };

    ContactRequestsCtrl.$inject = ['$scope', '$http'];

    crApp.controller('ContactRequestsCtrl', ContactRequestsCtrl);

    angular.bootstrap(document, ['ContactRequestsApp']);
});