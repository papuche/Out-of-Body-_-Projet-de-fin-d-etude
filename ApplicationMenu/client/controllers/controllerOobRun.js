menu.controller('OobRunCtrl', function ($scope, $state, $http, $rootScope, $document) {
	// Breadcrumb settings
	$rootScope.chemin = 'Accueil';
	$rootScope.chemin1 = 'Sortie de corps';
	$rootScope.stateChemin1 = 'oob';
	$rootScope.chemin2 = '';
	$rootScope.chemin3 = 'Application en cours';
	$rootScope.stateChemin3 = $state.current.name;
	$rootScope.suivant = false;

	// Block backspace key action to go back
	$document.on('keydown', function(e){
    	if(e.which === 8){
        	e.preventDefault();
      	}
 	 });

	// Exit function on "go to menu" button click : send "stop" to finish exercice AND go to menu state
	$scope.exit = function () {
		$http.get('stop');
		$state.go('mainMenu');
	}
});
