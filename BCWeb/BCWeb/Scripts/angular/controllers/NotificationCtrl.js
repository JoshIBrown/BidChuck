angular.element(document).ready(function () {
    var app = angular.module('notificationList', []);

    function NotificationCtrl(scope, http) {
        http.get('/api/Notification/GetAll')
            .success(function (result) {
                scope.DatePool = result.DatePool;
            });
    };

    NotificationCtrl.$inject = ['$scope', '$http'];

    app.controller('NotificationCtrl', NotificationCtrl);

    angular.bootstrap(document, ['notificationList']);
});