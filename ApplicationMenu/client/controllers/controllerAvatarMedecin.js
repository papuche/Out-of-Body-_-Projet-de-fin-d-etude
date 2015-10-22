menu.controller('AvatarMedecinCtrl', function ($scope, $state, $rootScope, $http) {
	$rootScope.chemin = 'Accueil > Avatar > Choix Médecin';

	$scope.previous = function () {
		$state.go('avatar');
	}

	$scope.next = function () {
		$http.get("validerAvatar");
		$http.get($scope.facteurAvatar);
		$state.go('mainMenu');
	}
});
