var modules = ['ui.router'];
var menu = angular.module('menu', modules);


menu.config(function($stateProvider, $urlRouterProvider){
	$urlRouterProvider.otherwise("");
	$stateProvider
	.state('portes',{
		url: "/portes",
		templateUrl: "client/templates/portes.html" /*function ($stateParams, $http){
			return $http({
				method: 'GET',
				url: '/portes'
			}).then(function successCallback(response) {
    			return response;
			}, function errorCallback(response) {
				return response;
			});
		}*/
	});
});

menu.controller('MainCtrl', function ($scope,$state) {

	$scope.chemin = 'Accueil >';

	$scope.exp_click = function(){

		$scope.chemin = "Accueil > Experience";    	
	}

	$scope.portes_click = function(){
		$scope.chemin = "Accueil > Portes";
		$state.go("portes");
	}

});

