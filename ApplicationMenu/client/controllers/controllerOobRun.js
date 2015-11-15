menu.controller('OobRunCtrl', function ($scope, $state, $http, $rootScope, $document) {
	$rootScope.chemin = 'Accueil';
	$rootScope.chemin1 = 'Sortie de corps';
	$rootScope.stateChemin1 = 'oob';
	$rootScope.chemin2 = '';
	$rootScope.chemin3 = 'Application en cours';
	$rootScope.stateChemin3 = $state.current.name;
	$rootScope.suivant = false;

	// enlever le retour de backspace key
	$document.on('keydown', function(e){
		console.log(e);
    	if(e.which === 8){
        	e.preventDefault();
      	}
 	 });

	$scope.exit = function () {
		$http.get('/stop');
		$state.go('mainMenu');
	}
});
