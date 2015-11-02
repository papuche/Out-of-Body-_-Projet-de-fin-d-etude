menu.controller('OobRunCtrl', function ($scope, $state, $http, $rootScope, $document) {
	$scope.morphing = $state.params.params.split('_')[0];

	// enlever le retour de backspace key
	$document.on('keydown', function(e){
		console.log(e);
    	if(e.which === 8){
        	e.preventDefault();
      	}
 	 });	
	$rootScope.chemin = 'Accueil > Sortie de corps > Application en cours';
	$scope.ghost = function () {
		$http.get('/ghost');
	}

	$scope.exit = function () {
		$http.get('/stop');
		$state.go('mainMenu');
	}
});
