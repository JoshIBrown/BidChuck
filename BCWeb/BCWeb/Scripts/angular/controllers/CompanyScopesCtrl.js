angular.element(document).ready(function () {
    var app = angular.module('companyScopePicker', ['filters', 'bcCsiScopePicker']);

    function CompanyScopesCtrl(scope, http) {

        scope.companyId = angular.element('#CompanyId').val();
        scope.theServiceUrl = '/api/CompanyScopes/?id=' + scope.companyId;

        // get passed back selected scopes if server side validation fails
        var passBackSelectedScopes = angular.element('#passBackScopes').children("input[name^='SelectedScope']");
        if (passBackSelectedScopes.length > 0) {
            scope.passBackSelection = $.map(passBackSelectedScopes, function (item) {
                return parseInt(item.value);
            });
        }

        // delete selected scopes because they are going to be handled by the scope picker after this
        angular.element('#passBackScopes').html('');
        angular.element(document).remove('#passBackScopes');
    }

    CompanyScopesCtrl.$inject = ['$scope', '$http'];

    app.controller('CompanyScopesCtrl', CompanyScopesCtrl);

    angular.bootstrap(document, ['companyScopePicker']);
});