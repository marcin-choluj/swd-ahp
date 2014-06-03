angular.module('app', ['ngRoute', 'controllers', 'ui'])
    .controller('appController', function ($scope, $http, $rootScope) {
        $scope.logged = false;
        $rootScope.logged = false;
        $scope.getAuthUser = function () {
            $http.get("/authinfo").success(function (result) {
                if (result.Result !== undefined) {
                    $scope.logged = true;
                    $rootScope.logged = true;
                    if (result.Result.DisplayName !== undefined) {
                        var tekst = result.Result.DisplayName;
                        $scope.userAgent = tekst;
                    }
                } else {
                    $scope.logged = false;
                    $rootScope.logged = false;
                }
            }).error(function (result) {
                $scope.logged = false;
                $rootScope.logged = false;
            });
        };
    })
    .config([
        '$routeProvider',
        function ($routeProvider) {
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
        }
    ]);
