//var app = angular.module('regApp', []).controller('RegCtrl', function ($scope, $http) {
//    $scope.states = [];
//    $scope.counties = [];

//    $http.get("/api/Account/GetStates").success(function (data) {
//        $scope.states = data;
//    });
//});

//app.directive('zurbSelect', function () {
//    return {
//        scope: {
//            values: "=zurbSelect",
//            selectCallback: "=",
//            selectName: "=",
//            selectModel: "="
//        },
//        template: '<select name="selectName" style="display:none;"' +
//                  'ng-options="s.Id as s.Value for s in values"'+
//                  'ng-model="selectModel" class="medium"></select>' +
//                  '<div class="custom dropdown medium" ng-init="menuOpen=false" ng-class="{open: menuOpen}" ng-click="menuOpen = !menuOpen">' +
//                      '<a href="#" class="current custom-select">Select...</a>' +
//                      '<a href="#" class="selector custom-select"></a>' +
//                      '<ul>' +
//                      '    <li ng-repeat="v in values" class="custom-select">{{v.Value}}</li>' +
//                      '</ul>' +
//                  '</div>',
//        link: function (scope, elem, attr) {

//        }
//    }
//});

//$("#customDropdown").trigger('change');
//$("#customDropdown a.current").trigger('click');
//$("#customDropdown li.selected").trigger('click');