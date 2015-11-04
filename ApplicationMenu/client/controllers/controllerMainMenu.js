menu.controller('mainMenuCtrl', function ($scope, $state, $rootScope, $http) {
	$rootScope.chemin = 'Accueil';
	$scope.exit = function() {
		$http.get('/exit');
	}
});

