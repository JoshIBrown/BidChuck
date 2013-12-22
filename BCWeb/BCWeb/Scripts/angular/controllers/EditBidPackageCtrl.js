angular.element(document).ready(function () {
    'use strict';
    var app = angular.module('editBidPackage', ['bcCsiScopePicker', 'filters']);

    function BidPackageCtrl($scope, $http) {

        $scope.useProjectBidDateChange = function () {
            angular.element('#UseProjectBidDate').val($scope.UseProjectBidDate);
            if (!$scope.UseProjectBidDate) {
                // set bid date and time fields from hidden field value
                angular.element('#BidDateTB').val(moment($scope.bidDate).format('MM/DD/YYYY'));
                angular.element('#BidTimeTB').val(moment($scope.bidDate).format('hh:mm a'));
            } else {
                angular.element('#BidDateTB').val('');
                angular.element('#BidTimeTB').val('');
            }
        };

        $scope.useProjectWalkThruChange = function () {
            angular.element('#UseProjectWalkThru').val($scope.UseProjectWalkThru);
        };

        $scope.walkThruStatusChange = function () {
            //  set walk thru date time fields
            if ($scope.walkThru === 'WalkThruIncluded'
                && $scope.walkDate !== '') {

                angular.element('#WalkThruDateTB').val(moment($scope.walkDate).format('MM/DD/YYYY'));
                angular.element('#WalkThruTimeTB').val(moment($scope.walkDate).format('hh:mm a'));
            } else {
                $scope.WalkThruDateTB = '';
                $scope.WalkThruTimeTB = '';
            }
        };

        // function to update hidden field for walk thru date time
        $scope.setWalkThru = function () {
            var date = $scope.WalkThruDateTB;
            var time = $scope.WalkThruTimeTB;

            var result = date + ' ' + time;

            // requires moment.js
            if (moment(result).isValid()) {
                angular.element('#WalkThruDateTime').val(result);
            }
        };

        // function to update hidden field for bid date time
        $scope.setBidDeadline = function () {
            var date = $scope.BidDateTB;
            var time = $scope.BidTimeTB;

            var result = date + ' ' + time;

            // requires moment.js
            if (moment(result).isValid()) {
                angular.element('#BidDateTime').val(result);
            }
        };


        $scope.bpId = angular.element('#Id').val();
        $scope.theServiceUrl = '/api/Scopes/GetScopesForBidPackage/?type=existing&ident=' + $scope.bpId;

        $scope.bidDate = angular.element('#BidDateTime').val();
        $scope.walkDate = angular.element('#WalkThruDateTime').val();

        // get passed back selected scopes if server side validation fails
        var passBackSelectedScopes = angular.element('#passBackScopes').children("input[name^='SelectedScope']");
        if (passBackSelectedScopes.length > 0) {
            $scope.passBackSelection = $.map(passBackSelectedScopes, function (item) {
                return parseInt(item.value);
            });
        }

        // delete selected scopes because they are going to be handled by the scope picker after this
        angular.element('#passBackScopes').html('');
        angular.element(document).remove('#passBackScopes');

        // set model value from dom initialized value
        $scope.UseProjectBidDate = angular.element('#UseProjectBidDate').val() === "False" ? false : true;

        // if not use project settings
        if (!$scope.UseProjectBidDate) {
            // set bid date and time fields from hidden field value
            $scope.useProjectBidDateChange();
        }


        // set model value from dom initialized value
        $scope.UseProjectWalkThru = angular.element("#UseProjectWalkThru").val() === "False" ? false : true;

        // set walkthru value in the angular model.  
        $scope.walkThru = angular.element('#WalkThruStatus').val();

        // if not use project settings
        if (!$scope.UseProjectWalkThru) {
            //  set walk thru date time fields
            $scope.walkThruStatusChange();
        }

    };

    BidPackageCtrl.$inject = ['$scope', '$http'];

    app.controller('BidPackageCtrl', BidPackageCtrl);


    angular.bootstrap(document, ['editBidPackage']);
});
