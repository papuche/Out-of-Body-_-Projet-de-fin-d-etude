menu.controller('OobCtrl', function ($scope, $state, $http) {
$scope.morphing = false;
$scope.baton = false;
$scope.message = '';
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
