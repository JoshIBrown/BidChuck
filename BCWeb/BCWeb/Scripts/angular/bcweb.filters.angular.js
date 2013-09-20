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


// usage: ng-repeat="foo in bar | parentIdEqual: {{thing}}
flt.filter('parentIdEqual', function () {
    return function (scopes, parentId) {
        var arrayToReturn = [];

        if (scopes) {
            for (var i = 0; i < scopes.length; i++) {
                if (scopes[i].ParentId === parentId)
                    arrayToReturn.push(scopes[i]);
            }
        }
        return arrayToReturn;
    };
});
