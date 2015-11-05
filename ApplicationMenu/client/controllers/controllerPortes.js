menu.config(function($stateProvider, $urlRouterProvider){
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

menu.controller('PortesCtrl', function ($scope,$state, $rootScope) {
	$rootScope.chemin = 'Accueil';
	$rootScope.chemin1 = 'Exercice des portes';
	$rootScope.stateChemin1 = $state.current.name;
	$rootScope.chemin2 = '';

	$scope.suivant_click = function(type){
			$state.go(type);
	}
	$scope.previous = function () {
		// $http.get('/stop');
		$state.go('mainMenu');
	}
});