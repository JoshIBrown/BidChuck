angular.element(document).ready(function () {
    'use strict';
    var app = angular.module('createBidPackage', ['bcCsiScopePicker', 'filters']);
    app.controller('BidPackageCtrl', ['$scope', '$http', function ($scope, $http) {

        $scope.templateId = angular.element('#TemplateId').val();
        $scope.theServiceUrl = '/api/Scopes/GetScopesForBidPackage/?type=new&ident=' + $scope.templateId;

        // get passed back selected scopes if server side validation fails
        var passBackSelectedScopes = angular.element('#passBackScopes').children("input[name^='SelectedScope']");
        if (passBackSelectedScopes.length > 0) {
            $scope.passBackSelection = $.map(passBackSelectedScopes, function (item) {
                return parseInt(item.value);
            });
        }

        // delete selected scopes because they are going to be handled by the scope picker after this
        angular.element('#passBackScopes').html('');
        angular.element(document).remove('#passBackScopes');
    }]);

    angular.bootstrap(document, ['createBidPackage']);
});