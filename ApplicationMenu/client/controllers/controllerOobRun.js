menu.controller('OobRunCtrl', function ($scope, $state, $http, $rootScope, $document) {
	$rootScope.chemin = 'Accueil';
	$rootScope.chemin1 = 'Sortie de corps';
	$rootScope.stateChemin1 = 'oob';
	$rootScope.chemin2 = 'Application en cours';

	$scope.morphing = $state.params.params.split('_')[0];

	// enlever le retour de backspace key
	$document.on('keydown', function(e){
		console.log(e);
    	if(e.which === 8){
        	e.preventDefault();
      	}
 	 });	
	$scope.ghost = function () {
		$http.get('/ghost');
	}

	$scope.exit = function () {
		$http.get('/stop');
		$state.go('mainMenu');
	}
});
