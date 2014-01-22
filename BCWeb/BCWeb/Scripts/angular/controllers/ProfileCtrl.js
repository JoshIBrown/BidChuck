angular.element(document).ready(function () {

    var profileApp = angular.module("profileApp", []);

    function ProfileCtrl(scope, http, compile) {
    };

    profileApp.controller("ProfileCtrl", ProfileCtrl);

    ProfileCtrl.$inject = ['$scope', '$http', '$compile'];

    angular.bootstrap(document, ["profileApp"]);
});