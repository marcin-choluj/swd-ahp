angular.module('controllers', ['ui.sortable'])
    .controller('formController',
    function ($scope, $http, $rootScope) {
        console.log($rootScope.logged);
        $scope.universities = [];
        $http.get('/universities/average/')
            .then(function (result) {
                $scope.universities = result.data.Result;
            });
        $scope.easyness = 1;
        $scope.job = 1;
        $scope.financies = 1;
        $scope.fun = 1;
        $scope.prestige = 1;

        $scope.showFormVar = false;
        $scope.showForm = function () {
            $scope.showFormVar = true;
        };
        $scope.addUniversity = function () {
            $scope.showFormVar = false;
            $scope.universities.push({ Name: $scope.newName });
            $scope.newName = "";
        };

        $scope.save = function () {
            $http.post('/universities/add/', {
                Name: $scope.name,
                Easyness: $scope.easyness,
                Job: $scope.job,
                Financies: $scope.financies,
                Fun: $scope.fun,
                Prestige: $scope.prestige
            });
        };
    })
    .controller('homeController',
    function ($scope, $http) {

    })
    .controller('averagesController',
    function ($scope, $http) {

    })
    .controller('decisionController',
    function ($scope, $http) {
        $scope.list = [{ name: "Rozrywka", value: "fun" },
            { name: "Prestiż", value: "prestige" },
            { name: "Zdawalność", value: "easyness" },
            { name: "Praca", value: "job" },
            { name: "Dofinansowanie", value: "financies" }];

        $scope.sortableOptions = {
            placeholder: "app-ph"
        };
        $scope.calculate = function () {
            var pref = "";
            $scope.list.forEach(function (entry) {
                pref += entry.value + ",";
            });
            $http.post('/calculatedecision/', { Preferences: pref })
			.then(function (result) {
			    $scope.result = result.data.Result;
			});
        };
    });
