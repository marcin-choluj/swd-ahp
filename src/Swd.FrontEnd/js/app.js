angular.module('app', ['ngRoute', 'controllers'])
    .controller('appController',function($scope, $http){
    })
    .config(['$routeProvider',
        function($routeProvider) {
            $routeProvider.
                when('/rate', {
                    templateUrl: 'templates/form.html',
                    controller: 'formController'
                }).
                when('/decision', {
                    templateUrl: 'templates/decision.html',
                    controller: 'decisionController'
                }).
                when('/averages', {
                    templateUrl: 'templates/averages.html',
                    controller: 'averagesController'
                }).
                when('/home', {
                    templateUrl: 'templates/home.html',
                    controller: 'homeController'
                }).
                otherwise({
                    redirectTo: '/home'
                });
    }]);
