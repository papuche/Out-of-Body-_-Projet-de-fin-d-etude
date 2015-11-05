menu.controller('OobCtrl', function ($scope, $state, $http, $rootScope) {
	// $rootScope.chemin = 'Accueil';
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
			$state.go('runOob', {params: 'morphing_baton'});
		}	
		else if ($scope.morphing){
			$http.get('/morphing');
			$state.go('runOob', {params: "morphing"});
		}	
		else if ($scope.baton){
			$http.get('/baton');
			$state.go('runOob', {params: "baton"});
		}
		else {
			$http.get('/nothing');
			$state.go('runOob', {params: ""});
		}
	};
	$scope.exit = function () {
		$http.get('/stop');
		$state.go('mainMenu');
	}
	});

	menu.config(function($stateProvider){	
		$stateProvider
		.state('runOob',{
			url: "/sortie_de_corps/application_en_cours/:params",
			templateUrl: "client/templates/runOob.html"
	});
});
