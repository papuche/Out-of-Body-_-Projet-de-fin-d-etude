menu.controller('PortesRunCtrl', function ($scope,$state,$http, $rootScope, $document, $timeout) {
	var pool = function () {
		if($state.current.name !== 'runPortes')
			return;
		$http.get('/porte').then(function(resp){
			console.log(resp);
			$state.go('mainMenu');
			return;
		});
		$timeout(pool, 200);

	}
		// enlever le retour de backspace key
	$document.on('keydown', function(e){
          if(e.which === 8){
              e.preventDefault();
          }
 	 });	
	pool();
	$rootScope.chemin = 'Accueil > Exercice des portes > Application en cours';

	$scope.end_click = function () {
		$http.get('stop');
		$state.go('mainMenu');
	};
});