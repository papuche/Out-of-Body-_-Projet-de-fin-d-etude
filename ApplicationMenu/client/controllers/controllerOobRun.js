menu.controller('OobRunCtrl', function ($scope, $state, $http, $rootScope, $document) {
	// enlever le retour de backspace key
	$document.on('keydown', function(e){
          if(e.which === 8 && e.target.nodeName !== "INPUT" || e.target.nodeName !== "SELECT"){ // you can add others here.
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
