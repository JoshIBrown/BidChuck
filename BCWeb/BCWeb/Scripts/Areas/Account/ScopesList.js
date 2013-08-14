angular.element(document).ready(function () {
    angular.bootstrap(document);
});

function ScopesCtrl($scope, $http) {
    $http.get('/api/Scopes/GetList')
         .success(function (data) {
             $scope.Scopes = data;

         });

    $scope.parentIdEqual = function (parentId) {
        return function (scope) {
            var test = parseInt(parentId);
            return scope.ParentId === test;
        }
    }

    $scope.secondChanged = function () {
        var x = $scope.selectedSecondT;
        var foo = $scope.Scopes;
        var selected = $.grep(foo, function (n, i) {
            return n.ParentId === x;
        });
        $scope.selectedThirdT = $.map(selected, function (item) {
            return item.Id;
        });
    };
}