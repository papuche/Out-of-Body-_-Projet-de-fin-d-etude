menu.controller('OobCtrl', function ($scope, $state, $http) {
$scope.morphing = false;
$scope.baton = false;
$scope.message = '';
$scope.next = function () {
	if ($scope.baton && $scope.morphing){
		$http.get('/baton_morphing');
		$scope.message = 'avec baton et morphing';
	}	
	else if ($scope.morphing){
		$http.get('/morphing');
		$scope.message = 'avec morphing seulement';
	}	
	else if ($scope.baton){
		$http.get('/baton');
		$scope.message = 'avec baton seulement';
	}
	else {
		$http.get('/nothing');
		$scope.message = 'sans baton ni morphing';
	}
	$state.go('runOob');
};
$scope.exit = function () {
	$http.get('/stop');
	$state.go('mainMenu');
}
});

