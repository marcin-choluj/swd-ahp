angular.module('controllers', ['ui.slider'])
    .controller('formController',
    function ($scope, $http) {
        $scope.demoVals = {
            sliderExample3: 14,
            sliderExample4: 14,
            sliderExample5: 50,
            sliderExample8: 0.34,
            sliderExample9: [-0.52, 0.54],
            sliderExample10: -0.37
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

    });
