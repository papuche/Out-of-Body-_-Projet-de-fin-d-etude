menu.controller('OobCtrl', function ($scope, $state, $http) {
$scope.morphing = false;
$scope.baton = false;
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
};
});

