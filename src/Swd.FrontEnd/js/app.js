angular.module('app', ['ngRoute', 'controllers', 'ui'])
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
    }])
    .config(function($httpProvider) {
        //Enable cross domain calls
        $httpProvider.defaults.useXDomain = true;

        //Remove the header used to identify ajax call  that would prevent CORS from working
        delete $httpProvider.defaults.headers.common['X-Requested-With'];
  });
