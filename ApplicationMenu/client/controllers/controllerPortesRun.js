menu.controller('PortesRunCtrl', function ($scope,$state,$http, $rootScope) {
	$rootScope.chemin = 'Accueil > Exercice des portes > Application en cours';

	$scope.end_click = function () {
		$http.get('stop');
		$state.go('mainMenu');
	};
});