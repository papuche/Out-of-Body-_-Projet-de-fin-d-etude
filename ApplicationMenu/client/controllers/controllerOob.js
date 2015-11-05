menu.controller('OobCtrl', function ($scope, $state, $http, $rootScope) {
	$rootScope.chemin = 'Accueil';
	$rootScope.chemin1 = 'Sortie de corps';
	$rootScope.stateChemin1 = $state.current.name;
	$rootScope.chemin2 = '';

	$scope.morphing = false;
	$scope.baton = false;
	$scope.message = '';

	$scope.previous = function () {
		// $http.get('/stop');
		$state.go('mainMenu');
	}

	$scope.next = function () {
		if ($scope.baton && $scope.morphing){
			$http.get('/baton_morphing');
		}	
		else if ($scope.morphing){
			$http.get('/morphing');
		}	
		else if ($scope.baton){
			$http.get('/baton');
		}
		else {
			$http.get('/nothing');
		}
		if($scope.ghost){
			$http.get('/ghost');
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
