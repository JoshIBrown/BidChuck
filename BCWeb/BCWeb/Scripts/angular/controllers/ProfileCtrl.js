angular.element(document).ready(function () {
    "use strict";

    var profileApp = angular.module('profileApp', ['bcContactButton']);

    function ProfileCtrl(scope) {

        scope.companyId = angular.element("#Id").val();

    };

    profileApp.controller('ProfileCtrl', ['$scope', ProfileCtrl]);

    angular.bootstrap(document, ['profileApp']);

});