menu.controller('mainMenuCtrl', function ($scope, $state, $rootScope, $http) {
	$scope.exit = function() {
		$http.get('/exit');
	}
	$rootScope.chemin = 'Accueil';
	$rootScope.chemin1 = '';
	$rootScope.chemin2 = '';
	$rootScope.chemin3 = '';
	$rootScope.suivant = true;

	$rootScope.exit = function(state){
		$http.get('stop');
		if(state)
			$state.go(state);
		else
			$state.go('mainMenu');
	}

});

