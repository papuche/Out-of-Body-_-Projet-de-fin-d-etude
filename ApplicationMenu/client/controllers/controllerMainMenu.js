menu.controller('mainMenuCtrl', function ($scope, $state, $rootScope, $http) {
	$scope.exit = function() {
		$http.get('/exit');
	}
	$rootScope.chemin = 'Accueil';
	$rootScope.chemin1 = '';
	$rootScope.chemin2 = '';
	$rootScope.chemin3 = '';

});

