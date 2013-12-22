var app = angular.module('bcCsiScopePicker', ['filters']);

function csiScopePicker($http) {
    // link function per directive signature
    function link(scope, element, attrs) {
        scope.pickerIsReady = false;
        scope.t1Parent = 0;
        scope.t2Parent = 0;
        scope.selectedCsiScopes = [];

        // fill selected scopes

        $http.get(scope.serviceUrl)
			 .success(function (data) {
			     scope.myCsiScopes = data;

			     // if scopes not passed in at constructions
			     if (!scope.selectedScopes) {
			         scope.selectedCsiScopes = $.map(scope.myCsiScopes, function (data) {
			             if (data.Checked) {
			                 return {
			                     Id: data.Id,
			                     Desc: data.Description,
			                     Checked: data.Checked,
			                     Csi: data.CsiNumber
			                 };
			             }
			         });
			     } else {
			         // loop available scopes
			         for (x = 0; x < scope.myCsiScopes.length; x++) {
			             // loop passed in scope selection
			             for (y = 0; y < scope.selectedScopes.length; y++) {
			                 // if scope is selected
			                 if (scope.myCsiScopes[x].Id === scope.selectedScopes[y]) {
			                     // update checked status
			                     scope.myCsiScopes[x].Checked = true;

			                     // push selected scope onto selection
			                     scope.selectedCsiScopes.push({
			                         Id: scope.myCsiScopes[x].Id,
			                         Desc: scope.myCsiScopes[x].Description,
			                         Checked: scope.myCsiScopes[x].Checked,
			                         Csi: scope.myCsiScopes[x].CsiNumber
			                     });
			                 }
			             }
			         }
			     }
			     // this may need to be fixed.  not sure who dom is affected by this.
			     scope.pickerIsReady = true;
			 });

        scope.t1Expand = function (value) {
            scope.t1Parent = value;
        };

        scope.t2Expand = function (value) {
            scope.t2Parent = value;
        };


        scope.changeScopeSelection = function (data) {
            var found = scope.updateScopeIfInSelection(data);

            // if first selection or if a match was not found, 
            // add a new selection to the array
            if (scope.selectedCsiScopes.length === 0 || !found) {
                scope.selectedCsiScopes.push({
                    Id: data.Id,
                    Desc: data.Description,
                    Checked: data.Checked,
                    Csi: data.CsiNumber
                });
            }

            // update parents
            scope.updateParent(data);

            // update children
            scope.updateChildren(data);
        };

        // update scope selection if x is clicked on tag in basket
        scope.removeTag = function (data) {
            var scope = '';
            // find the scope i the list
            angular.forEach(scope.myCsiScopes, function (v, k) {
                if (v.Id === data.Id) {
                    scope = v;
                }
            });
            // uncheck the scope
            scope.Checked = false;

            // cascade 
            scope.changeScopeSelection(scope);
        };

        // recursive function that goes through the lineage until epoch
        scope.updateParent = function (aScope) {
            angular.forEach(scope.myCsiScopes, function (v, k) {
                // if found parent and this is Checked, update parent
                if (v.Id === aScope.ParentId && aScope.Checked) {
                    v.Checked = true;
                    // recurse through lineage until epoch;
                    scope.updateParent(v);
                    // if parent was selected before, then update, else push onto array
                    if (!scope.updateScopeIfInSelection(v)) {
                        scope.selectedCsiScopes.push({
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
        scope.updateChildren = function (aScope) {
            angular.forEach(scope.myCsiScopes, function (v, k) {
                // if found child update children, check or uncheck
                if (v.ParentId === aScope.Id) {
                    v.Checked = aScope.Checked;
                    // recurse through children
                    scope.updateChildren(v);
                    // if child has been selected before, then update, else push onto array
                    if (!scope.updateScopeIfInSelection(v)) {
                        scope.selectedCsiScopes.push({
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
        scope.updateScopeIfInSelection = function (aScope) {
            var found = false;
            // loop through selected myCsiScopes
            angular.forEach(scope.selectedCsiScopes, function (v, k) {
                // if match found for data
                if (v.Id === aScope.Id) {
                    found = true;
                    v.Checked = aScope.Checked;
                    return;
                }
            });
            return found;
        };

        scope.getFieldName = function (i) {
            return "SelectedScope[" + i + "]";
        };
        scope.getFieldId = function (i) {
            return "SelectedScope_" + i + "_";
        };
    } // end of link function

    return {
        restrict: 'E', // restrict to searching for an element
        templateUrl: '/AngularTemplate/ScopePicker',	// best solution using mvc to deliver a static page,
        scope: {
            serviceUrl: '=serviceUrl',
            selectedScopes: '=selectedScopes'
        },
        link: link
    };
};

csiScopePicker.$inject = ["$http"];
app.directive('csiScopePicker', csiScopePicker);