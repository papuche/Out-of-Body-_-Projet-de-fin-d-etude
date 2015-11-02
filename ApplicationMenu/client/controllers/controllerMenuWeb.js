var modules = ['ui.router'];
var menu = angular.module('menu', modules);


menu.config(function($stateProvider, $urlRouterProvider){
	$urlRouterProvider.otherwise("");
	$stateProvider
	.state('mainMenu',{
		url: "",
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