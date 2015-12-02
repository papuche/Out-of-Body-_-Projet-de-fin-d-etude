menu.controller('controllerChrono', function($scope, $timeout) {
	// partie chronomètre
	$scope.chronoSeconde = 0;
	$scope.chronoMinute = 0;

	var chronometer = function(){
		$scope.chronoSeconde ++;
		if ($scope.chronoSeconde == 60) {
			$scope.chronoSeconde = 0;
			$scope.chronoMinute ++;
		};
		if ($scope.chronoSeconde < 10) {
			$scope.chronoSeconde  = String("0" + $scope.chronoSeconde)
		}
		if ($scope.chronoMinute < 10) {
			$scope.chronoMinute = parseInt($scope.chronoMinute);
			$scope.chronoMinute  = String("0" + $scope.chronoMinute)
		}
		$timeout(chronometer, 1000);
	}

	chronometer();
})
menu.directive('chrono', function() {
  return {
    restrict: 'E',
    controller: "controllerChrono",
    templateUrl: 'client/directives/templateChrono.html'
  };
});