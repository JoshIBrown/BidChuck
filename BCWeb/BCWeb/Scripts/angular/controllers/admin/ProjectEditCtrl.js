angular.element(document).ready(function () {
    var app = angular.module('projectEdit', ['ScopePickerDirective', 'filters']);
    app.controller('ProjectCtrl', ['$scope', '$http', function ($scope, $http) {
        
       
    }]);
    angular.bootstrap(document, ['projectEdit'])
});