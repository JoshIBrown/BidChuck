// requires moment.js
var inputValidators = angular.module('inputValidators', []);
inputValidators.directive("date", function () {
    return {
        require: "ngModel",
        link: function (scope, elm, attrs, ctrl) {
            ctrl.$parsers.unshift(function (viewValue) {
                if (moment(viewValue, "MM/DD/YYYY").isValid()
                || moment(viewValue, "YYYY-MM-DD").isValid()
                || moment(viewValue, "MM-DD-YYYY").isValid()) {
                    // it is valid
                    ctrl.$setValidity("date", true);
                    return viewValue;
                } else {
                    // it is invalid, return undefined (no model update)
                    ctrl.$setValidity("date", false);
                    return undefined;
                }
            });
        }
    };
});

var INTEGER_REGEXP = /^\-?\d*$/;
inputValidators.directive('integer', function () {
    return {
        require: 'ngModel',
        link: function (scope, elm, attrs, ctrl) {
            ctrl.$parsers.unshift(function (viewValue) {
                if (INTEGER_REGEXP.test(viewValue)) {
                    // it is valid
                    ctrl.$setValidity('integer', true);
                    return viewValue;
                } else {
                    // it is invalid, return undefined (no model update)
                    ctrl.$setValidity('integer', false);
                    return undefined;
                }
            });
        }
    };
});


var PERSON_NAME_REGEXP = /^[a-zA-Z\-\']+$/;
inputValidators.directive('personName', function () {
    return {
        require: 'ngModel',
        link: function (scope, elm, attrs, ctrl) {
            ctrl.$parsers.unshift(function (viewValue) {
                if (PERSON_NAME_REGEXP.test(viewValue)) {
                    // it is valid
                    ctrl.$setValidity('personName', true);
                    return viewValue;
                } else {
                    // it is invalid, return undefined (no model update)
                    ctrl.$setValidity('personName', false);
                    return undefined;
                }
            });
        }
    };
});

// test if value is unique with in an object.
// usage <input type="text" unique="myData.column1" />
// mydata is an array of objects
// column1 is a property name in an object
inputValidators.directive('unique', function () {
    return {
        require: 'ngModel',
        link: function (scope, elem, attrs, ctrl) {
            elem.on('blur', function (evt) {
                scope.$apply(function () {
                    var val = elem.val();
                    var hiers = attrs.unique.split('.');
                    var foo = scope[hiers[0]];
                    var valid = true;
                    for (i = 0; i < scope[hiers[0]].length; i++) {
                        var x = foo[i][hiers[1]];
                        if (x.toLowerCase() === val.toLowerCase()) {
                            valid = false;
                            break;
                        }
                    }
                    ctrl.$setValidity('unique', valid);
                })
            })
        }
    };
});