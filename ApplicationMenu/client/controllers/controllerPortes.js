menu.config(function($stateProvider, $urlRouterProvider){
	$urlRouterProvider.otherwise("");
	$stateProvider
	.state('entiere',{
		url: "/portes/entiere",
		templateUrl: "client/templates/portesConfig.html"
	})
	.state('demi_haut',{
		url: "/portes/demi_haut",
		templateUrl: "client/templates/portesConfig.html"
	})
	.state('demi_bas',{
		url: "/portes/demi_bas",
		templateUrl: "client/templates/portesConfig.html"
	});
});

menu.controller('PortesCtrl', function ($scope,$state) {

		$scope.suivant_click = function(type){
			$state.go(type);
	}
});