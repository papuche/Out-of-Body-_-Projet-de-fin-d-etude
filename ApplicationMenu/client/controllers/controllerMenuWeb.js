var modules = ['ui.router'];
var menu = angular.module('menu', modules);


menu.config(function($stateProvider, $urlRouterProvider){
	$urlRouterProvider.otherwise("");
	$stateProvider
	.state('mainMenu',{
		url: "",
		templateUrl: "client/templates/mainMenu.html"
	})
	.state('portes',{
		url: "/portes",
		templateUrl: "client/templates/portes.html"
	})
	.state('oob',{
		url: "/SortieDeCorps",
		templateUrl: "client/templates/oob.html"
	});
});

menu.controller('MenuWebCtrl', function ($scope,$state) {

	$scope.chemin = 'Accueil >';

	$scope.exp_click = function(){

		$scope.chemin = "Accueil > Experience";    	
	}

	$scope.portes_click = function(){
		$scope.chemin = "Accueil > Portes";
		$state.go("portes");
	}

});

menu.controller('mainMenuCtrl', function ($scope,$state) {
	$scope.oob = "Sortie de Corps";
	$scope.portes = "Exercice des portes";
});

