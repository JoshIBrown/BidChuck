angular.element(document).ready(function () {

    var profileApp = angular.module("profileApp", ['bcContactButton']);

    



    function ProfileCtrl(scope) {

        scope.token = angular.element('input[name=__RequestVerificationToken]').val();

        scope.companyId = angular.element("#Id").val();

    };

    profileApp.controller("ProfileCtrl", ProfileCtrl);

    ProfileCtrl.$inject = ['$scope'];

    

    angular.bootstrap(document, ['profileApp']);

});