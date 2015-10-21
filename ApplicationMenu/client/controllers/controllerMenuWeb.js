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
	.state('avatar',{
		url: "/avatar",
		templateUrl: "client/templates/avatar.html"
	})
	.state('portes',{
		url: "/portes",
		templateUrl: "client/templates/portes.html"
	})
	.state('oob',{
		url: "/sortie_de_corps",
		templateUrl: "client/templates/oob.html"
	});
});