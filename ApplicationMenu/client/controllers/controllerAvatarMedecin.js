menu.controller('AvatarMedecinCtrl', function ($scope, $state, $rootScope, $http) {
	$rootScope.chemin = 'Accueil';
	$rootScope.chemin3 = 'Choix Médecin';
	$rootScope.suivant = false;

	$scope.previous = function () {
		$state.go('avatar');
		$http.get("stop");
	}

	$scope.next = function () {
		$http.get("validerAvatar/" + $scope.facteurAvatar);
		$state.go('mainMenu');
	}
});
