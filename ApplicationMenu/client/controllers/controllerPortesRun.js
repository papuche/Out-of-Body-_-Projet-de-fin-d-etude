menu.controller('PortesRunCtrl', function ($scope,$state,$http, $rootScope, $document) {
		// enlever le retour de backspace key
	$document.on('keydown', function(e){
          if(e.which === 8 && e.target.nodeName !== "INPUT" || e.target.nodeName !== "SELECT"){ // you can add others here.
              e.preventDefault();
          }
 	 });	
	
	$rootScope.chemin = 'Accueil > Exercice des portes > Application en cours';

	$scope.end_click = function () {
		$http.get('stop');
		$state.go('mainMenu');
	};
});