var modules = ['ui.router'];
var menu = angular.module('menu', modules);


menu.config(function($stateProvider, $urlRouterProvider, $locationProvider){
	$urlRouterProvider.otherwise("");
	//use the HTML5 History API
    $locationProvider.html5Mode(true);
	$stateProvider
	.state('mainMenu',{
		url: "/",
		templateUrl: "client/templates/mainMenu.html"
	})
	.state('portes',{
		url: "/portes",
		templateUrl: "client/templates/portes.html"
	})
	.state('oob',{
		url: "/sortie_de_corps",
		templateUrl: "client/templates/oob.html"
	})
	.state('runOob',{
		url: "/sortie_de_corps/en_cours",
		templateUrl: "client/templates/runOob.html"
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

