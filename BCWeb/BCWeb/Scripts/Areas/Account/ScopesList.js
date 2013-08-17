//$(function () {
//    cl = jQuery.fn.jColumnListView({
//        id: 'cl1',
//        width: 1200,
//        columnWidth: 300,
//        columnHeight: 300,
//        columnMargin: 8,
//        paramName: 'columnview',
//        columnNum: 3,
//        appendToId: 't1',
//        elementId: 'scopes',
//        removeAfter: false,
//        columnMinWidth: 120,
//        columnMaxWidth: 300,
//        childIndicator: true,
//        childIndicatorTextFormat: '%cvl-count%',
//        leafMode: true,
//        onItemChecked: function (ci) { console.log(ci); },
//        onItemUnchecked: function (ci) { console.log(ci); },
//        checkAllChildren: true,
//    });
//});



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