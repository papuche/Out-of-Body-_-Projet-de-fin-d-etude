menu.config(function($stateProvider, $urlRouterProvider){
	$urlRouterProvider.otherwise("");
	$stateProvider
	.state('avatar_choix',{
		url: "/avatar/choix",
		templateUrl: "client/templates/avatarMedecin.html"
	});
});



menu.controller('AvatarCtrl', function ($scope, $state, $rootScope, $http) {
	$rootScope.chemin = 'Accueil > Avatar';
	var sexe = "F";

	$scope.previous = function () {
		$state.go('mainMenu');
	}

	$scope.next = function () {
		$http.get("/" + sexe + "_avatar");
		$state.go('avatar_choix');
	}

	changeAvatar = function () {
		$http.get("/avatar");
	}

	$scope.sexe_selected = function(type){
        sexe=type;
	}
});


