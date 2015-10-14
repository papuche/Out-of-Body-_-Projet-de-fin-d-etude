menu.controller('mainMenuCtrl', function ($scope, $state, $rootScope, $http) {
	$rootScope.chemin = 'Accueil';
	$scope.changeAvatar = function () {
		$http.get("/avatar");
	}
});

