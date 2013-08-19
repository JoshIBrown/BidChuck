﻿$(function () {
    $(document).tooltip({ position: { my: "left+15 center", at: "right center", collision: "none" } });
});

var app = angular.module('scopePicker', ['filters']).controller('ScopesCtrl', function ($scope, $http) {
    $scope.t1Parent = 0;
    $scope.t2Parent = 0;
    $scope.selectedScopes = [];

    $http.get('/api/Scopes/GetList')
         .success(function (data) {
             $scope.Scopes = data;

         });

    $scope.t1Expand = function (value) {
        $scope.t1Parent = value;
        console.log('t1 choice: ' + value);
    };

    $scope.t2Expand = function (value) {
        $scope.t2Parent = value;
        console.log('t2 choice: ' + value);
    };

    $scope.foo = function (value) {
        console.log('foo : ' + value);
    };

    $scope.thing = function (data) {
        var found = false;
        angular.forEach($scope.selectedScopes, function (v, k) {
            if (v.Id === data.Id) {
                found = true;
                v.checked = data.checked;
                return;
            }
        });

        if ($scope.selectedScopes.length === 0 || !found) {
            $scope.selectedScopes.push({
                Id: data.Id,
                Desc: data.Description,
                checked: data.checked
            });
        }

        console.log(data);
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