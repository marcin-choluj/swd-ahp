angular.module('controllers', ['ui.sortable'])
    .controller('formController',
    function ($scope, $http) {
        $scope.showFormVar=false;
        $scope.showForm = function(){
            $scope.showFormVar = true;
        }
        $scope.addUniversity = function(){
            $scope.showFormVar = false;
        }


    })
    .controller('homeController',
    function ($scope, $http) {

    })
    .controller('averagesController',
    function ($scope, $http) {

    })
    .controller('decisionController',
    function ($scope, $http) {
        $scope.list = [{name:"Rozrywka", value:"fun"},
            {name:"Prestiż", value:"prestige"},
            {name:"Zdawalność", value:"easyness"},
            {name:"Praca", value:"job"},
            {name:"Dofinansowanie", value:"financies"}];

	    $scope.sortableOptions = {
	        placeholder: "app-ph"
	    };
		$scope.calculate=function(){
			var pref = "";
			$scope.list.forEach(function(entry) {
			    pref+=entry.value+",";
			});
			$http.post('http://localhost:1337/calculatedecision/', {Preferences:pref})
			.then(function(result) {
			    $scope.result = result.data.Result;
			});
		}

    });
