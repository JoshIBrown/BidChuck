$(function () {
    $(document).tooltip({ position: { my: "left+15 center", at: "right center", collision: "none" } });
});

var app = angular.module('scopePicker', ['filters']).controller('ScopesCtrl', function ($scope, $http) {
    $scope.t1Parent = 0;

    $http.get('/api/Scopes/GetList')
         .success(function (data) {
             $scope.Scopes = data;

         });

    //$scope.parentIdEqual = function (parentId) {
    //    return function (scope) {
    //        var test = parseInt(parentId);
    //        return scope.ParentId === test;
    //    }
    //}

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

    $scope.t1Expand = function (value) {
        $scope.t1Parent = value;
    };

    $scope.t2Expand = function (value) {
        $scope.t2Parent = value;
    };

    $scope.foo = function () {
        alert('ooo');
    };
});

// usage: ng-repeat="foo in bar | parentIdEqual: {{thing}}
app.filter('parentIdEqual', ['$filter', function ($filter) {
    var standardFilter = $filter('filter');
    return function (scopes, parentId) {
        return standardFilter(scopes, function (scope) {
            return scope.ParentId === parentId;
        });
    };
}]);

var flt = angular.module('filters', []);
flt.filter('truncate', function () {
    return function (text, length, end) {
        if (isNaN(length))
            length = 10;

        if (end === undefined)
            end = "...";

        if (text.length <= length || text.length - end.length <= length) {
            return text;
        }
        else {
            return String(text).substring(0, length - end.length) + end;
        }

    };
});
//flt.filter('parentIdEquals', function () {
//    return function (scope, parentId) {
//        var test = parseInt(parentId);
//        return scope.ParentId === test;
//    }
//});