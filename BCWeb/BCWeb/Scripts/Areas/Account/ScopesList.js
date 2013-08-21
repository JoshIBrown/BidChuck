var app = angular.module('scopePicker', ['filters']).controller('ScopesCtrl', function ($scope, $http) {
    $scope.t1Parent = 0;
    $scope.t2Parent = 0;
    $scope.selectedScopes = [];

    $http.get('/api/Scopes/GetScopesToManage')
         .success(function (data) {
             $scope.Scopes = data;
             $scope.selectedScopes = $.map($scope.Scopes, function (data) {
                 if (data.Checked) {
                     return {
                         Id: data.Id,
                         Desc: data.Description,
                         Checked: data.Checked
                     };
                 }
             });
         });

    $scope.t1Expand = function (value) {
        $scope.t1Parent = value;
    };

    $scope.t2Expand = function (value) {
        $scope.t2Parent = value;
    };


    $scope.changeScopeSelection = function (data) {
        var found = $scope.updateScopeIfInSelection(data);

        // if first selection or if a match was not found, 
        // add a new selection to the array
        if ($scope.selectedScopes.length === 0 || !found) {
            $scope.selectedScopes.push({
                Id: data.Id,
                Desc: data.Description,
                Checked: data.Checked
            });
        }


        $scope.updateParent(data);

        $scope.updateChildren(data);
    };


    // recursive function that goes through the lineage until epoch
    $scope.updateParent = function (aScope) {
        angular.forEach($scope.Scopes, function (v, k) {
            // if found parent and this is Checked, update parent
            if (v.Id === aScope.ParentId && aScope.Checked) {
                v.Checked = true;
                // recurse through lineage until epoch;
                $scope.updateParent(v);
                // if parent was selected before, then update, else push onto array
                if (!$scope.updateScopeIfInSelection(v)) {
                    $scope.selectedScopes.push({
                        Id: v.Id,
                        Desc: v.Description,
                        Checked: v.Checked
                    });
                }
            }
        });
    };

    // recursive function that goes through all offspring over n generatoins
    $scope.updateChildren = function (aScope) {
        angular.forEach($scope.Scopes, function (v, k) {
            // if found child update children, check or uncheck
            if (v.ParentId === aScope.Id) {
                v.Checked = aScope.Checked;
                // recurse through children
                $scope.updateChildren(v);
                // if child has been selected before, then update, else push onto array
                if (!$scope.updateScopeIfInSelection(v)) {
                    $scope.selectedScopes.push({
                        Id: v.Id,
                        Desc: v.Description,
                        Checked: v.Checked
                    });
                }
            }
        });
    };

    // returns true if found update, returns false if not found
    $scope.updateScopeIfInSelection = function (aScope) {
        var found = false;
        // loop through selected scopes
        angular.forEach($scope.selectedScopes, function (v, k) {
            // if match found for data
            if (v.Id === aScope.Id) {
                found = true;
                v.Checked = aScope.Checked;
                return;
            }
        });
        return found;
    };

    // save selection to server
    $scope.saveChanges = function () {
        // create a new array containing only the id's of the selected scopes
        var toPut = angular.toJson($.map($scope.selectedScopes, function (data) {
            // if selected scopes
            if (data.Checked) {
                return data.Id;
            }
        }));

        // put the new list to the server
        $http({ url: '/api/Scopes/PutSelectedScopes', method: 'PUT', data: toPut })
            .success(function (result) {
                alert(result.message);
            });
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