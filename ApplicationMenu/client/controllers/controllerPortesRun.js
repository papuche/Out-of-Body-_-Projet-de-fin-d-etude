menu.controller('PortesRunCtrl', function ($scope,$state,$http) {


	$scope.end_click = function () {
		$http.get('stop');
		$state.go('mainMenu');
	};
});