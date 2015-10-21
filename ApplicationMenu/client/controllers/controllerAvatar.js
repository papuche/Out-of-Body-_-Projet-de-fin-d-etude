// menu.config(function($stateProvider, $urlRouterProvider){
// 	$urlRouterProvider.otherwise("");
// 	$stateProvider
// 	.state('avatar_choix',{
// 		url: "/avatar/choix",
// 		templateUrl: "client/templates/portesConfig.html"
// 	});
// });



menu.controller('AvatarCtrl', function ($scope,$state, $rootScope) {
	$rootScope.chemin = 'Accueil > Avatar';
	$scope.previous = function () {
		// $http.get('/stop');
		$state.go('mainMenu');
	}

	changeAvatar = function () {
		$http.get("/avatar");
	}
});