/*
* angular controller to be used on customer facing pages for creating and editing a project.
* has duplicate code from the scope picker that needs to be offloaded into an angular directive
*/

angular.element(document).ready(function () { // same as $(document).ready()
    // declare the angular application
    var app = angular.module('projectEdit', ['filters', 'bcCsiScopePicker']);

    // declare the controller for the application
    function ProjectCtrl($scope, $http, $window) {
        'use strict';

        // create web service url for scope picker
        $scope.projectId = angular.element('#Id').val();
        $scope.theServiceUrl = '/api/Scopes/GetScopesToManage/?type=project&ident=' + $scope.projectId;

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

        // set walkthru value in the angular model.  
        $scope.walkThru = angular.element('#WalkThruStatus').val();

        // set bid date and time fields from hidden field value
        var bidDate = angular.element('#BidDateTime').val();
        angular.element('#BidDateTB').val(moment(bidDate).format('MM/DD/YYYY'));
        angular.element('#BidTimeTB').val(moment(bidDate).format('hh:mm a'));

        //  set walk thru date time fields
        if ($scope.walkThru === 'WalkThruIncluded'
            && angular.element('#WalkThruDateTime').val() !== '') {
            var walkThruDate = angular.element('#WalkThruDateTime').val();
            angular.element('#WalkThruDateTB').val(moment(bidDate).format('MM/DD/YYYY'));
            angular.element('#WalkThruTimeTB').val(moment(bidDate).format('hh:mm a'));
        }

        // function to update hidden field for walk thru date time
        $scope.setWalkThru = function () {
            var date = $scope.WalkThruDateTB;
            var time = $scope.WalkThruTimeTB

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

    };

    /// inject resources
    ProjectCtrl.$inject = ['$scope', '$http', '$window'];

    // set controller
    app.controller('ProjectCtrl', ProjectCtrl);

    angular.bootstrap(document, ['projectEdit']);
});