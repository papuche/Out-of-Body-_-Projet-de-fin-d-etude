menu.controller('OobRunCtrl', function ($scope, $state, $http, $rootScope) {
	$rootScope.chemin = 'Accueil > Sortie de corps > Application en cours';

	$scope.exit = function () {
		$http.get('/stop');
		$state.go('mainMenu');
	}
});
