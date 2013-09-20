var app = angular.module('scopePicker', ['filters']);
app.controller('ScopesCtrl', function ($scope, $http, $window) {
    $scope.t1Parent = 0;
    $scope.t2Parent = 0;
    $scope.selectedScopes = [];
    $scope.saved = false;
    $scope.saving = false;
    $scope.cantsave = false;

    // may have to change this later, but for now it works
    $scope.queryString = $window.location.search;

    // this will allow pulling parameters from query string
    $scope.getParams = function (str) {
        var queryString = str || $scope.queryString || '';
        var keyValPairs = [];
        var params = {};
        queryString = queryString.replace(/.*?\?/, "");

        if (queryString.length) {
            keyValPairs = queryString.split('&');
            for (pairNum in keyValPairs) {
                var key = keyValPairs[pairNum].split('=')[0];
                if (!key.length) continue;
                if (typeof params[key] === 'undefined')
                    params[key] = [];
                params[key].push(keyValPairs[pairNum].split('=')[1]);
            }
        }
        return params;
    }

    $scope.getUri = '/api/Scopes/GetScopesToManage' + $scope.queryString;

    var identREGEX = new RegExp('(type|ident)=(.*)');
    var typeREGEX = new RegExp('[\\?\\&]type=(.*)[\\&]?');

    $http.get($scope.getUri)
         .success(function (data) {
             $scope.Scopes = data;
             $scope.selectedScopes = $.map($scope.Scopes, function (data) {
                 if (data.Checked) {
                     return {
                         Id: data.Id,
                         Desc: data.Description,
                         Checked: data.Checked,
                         Csi: data.CsiNumber
                     };
                 }
             });
             angular.element('#ScopePickerWrapper').show();
             angular.element('#LoadingWrapper').hide();
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
                Checked: data.Checked,
                Csi: data.CsiNumber
            });
        }

        // update parents
        $scope.updateParent(data);

        // update children
        $scope.updateChildren(data);
    };

    // update scope selection if x is clicked on tag in basket
    $scope.removeTag = function (data) {
        var scope = '';
        // find the scope i the list
        angular.forEach($scope.Scopes, function (v, k) {
            if (v.Id === data.Id) {
                scope = v;
            }
        });
        // uncheck the scope
        scope.Checked = false;

        // cascade 
        $scope.changeScopeSelection(scope);
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
                        Checked: v.Checked,
                        Csi: v.CsiNumber
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
                        Checked: v.Checked,
                        Csi: v.CsiNumber
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
        $scope.saving = true;
        $scope.saved = false;
        $scope.cantsave = false;
        // create a new array containing only the id's of the selected scopes
        var selected = $.map($scope.selectedScopes, function (data) {
            // if selected scopes
            if (data.Checked) {
                return data.Id;
            }
        });

        // get type and ident
        var params = $scope.getParams();


        var toPut = { "Selected": selected, "Ident": params['ident'] ? params['ident'][0].replace('%40', '@') : "", "Type": params['type'] ? params['type'][0] : "" };
        // get anti forgery token
        var token = angular.element("input[name='__RequestVerificationToken']").val();

        // put the new list to the server
        $http.put('/api/Scopes/PutSelectedScopes', angular.toJson(toPut),
            {
                headers: { "X-XSRF-Token": token },
                xsrfCookieName: '__RequestVerificationToken'
            })
            .success(function (result) {
                if (result.success) {
                    $scope.saved = true;
                } else {
                    $scope.cantsave = true;
                }
            });
        $scope.saving = false;
    };
});



