var app = angular.module('angularServiceDashboard', ['SignalR']);

/*app.config(['$httpProvider', function ($httpProvider) {
     
}]);*/

/*app.config(function ($routeProvider) {
    $routeProvider

        // route for the home page
        .when('/', {
            templateUrl: 'app/views/PointingRoom.html' 
        });

      ;
});*/

app.value('backendServerUrl', 'http://localhost:52527/');
app.run(function ($rootScope) {
    $rootScope.roomMembers = [];
});
