menu.controller('OobCtrl', function ($scope, $state, $http, $rootScope) {
	$rootScope.chemin = 'Accueil';
	$rootScope.chemin1 = 'Sortie de corps';
	$rootScope.stateChemin1 = $state.current.name;
	$rootScope.chemin2 = '';
	$rootScope.chemin3 = '';
	$rootScope.suivant = true;

	$scope.morphing = false;
	$scope.baton = false;
	$scope.message = '';

	$scope.previous = function () {
		// $http.get('/stop');
		$state.go('mainMenu');
	}

	$scope.next = function () {
		if ($scope.baton && $scope.morphing && $scope.ghost){
			$http.get('oob/1_1_1');
		}	
		else if ($scope.baton && $scope.morphing){
			$http.get('oob/1_1_0');
		}		
		else if ($scope.morphing && $scope.ghost){
			$http.get('oob/0_1_1');
		}			
		else if ($scope.morphing){
			$http.get('oob/0_1_0');
		}	
		else if ($scope.baton){
			$http.get('oob/1_0_0');
		}
		else {
			$http.get('oob/0_0_0');
		}
		$state.go('runOob');
	};
	$scope.exit = function () {
		$http.get('/stop');
		$state.go('mainMenu');
	}
	});

	menu.config(function($stateProvider){	
		$stateProvider
		.state('runOob',{
			url: "/sortie_de_corps/application_en_cours",
			templateUrl: "client/templates/runOob.html"
	});
});
