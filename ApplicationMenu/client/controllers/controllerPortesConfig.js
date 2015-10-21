menu.config(function($stateProvider, $urlRouterProvider){
	$urlRouterProvider.otherwise("");
	$stateProvider
	.state('runPortes',{
		url: "/portes/en_cours",//+$state.current.name+"/en_cours",
		templateUrl: "client/templates/runPortes.html"
	});
});

menu.controller('PortesConfigCtrl', function ($scope,$state,$http, $rootScope) {
	$rootScope.chemin = 'Accueil > Exercice des portes > Portes ' + $state.current.name;

	if($state.current.name == "entiere"){
		$scope.hauteur = true;
	}
	else{
		$scope.hauteur = false;
	}

	$scope.$watch('nbRepet + nbTaille + nbTailleH', function() {
		$scope.nbEssai = ($scope.nbTaille * $scope.nbRepet) + ($scope.nbTailleH * $scope.nbRepet);
	});

	$scope.previous = function () {
		// $http.get('/stop');
		$state.go('portes');
	}

	$scope.executer_click = function(){
		$scope.informationsDonnees ="";
		if ($scope.nbEssai > 0 & $scope.nbTaille == 1){
			if($scope.hauteur){
				testerH();
			}
			else{
				console.log("envoyer les donnees !");
				sendMessage();
			}
		}
		else if ($scope.nbEssai > 0 & $scope.nbTaille > 1 & $scope.diffTaille != 0){
			if($scope.hauteur){
				testerH();
			}
			else{
				sendMessage();
			}
		}
		else{
			$scope.informationsDonnees = "Tous les champs nécessaires ne sont pas remplis";
		}
	}

	testerH = function(){
		if($scope.nbTailleH == 1){
			sendMessage();
		}
		else if ($scope.nbTailleH > 1 & $scope.diffTailleH != 0){
			sendMessage();
		}
		else{
			$scope.informationsDonnees = "Tous les champs nécessaires ne sont pas remplis";
		}
	}

	var sendMessage = function () {
		var type ="";
		if($state.current.name == "entiere")
			type ="e";
		else if ($state.current.name == "demi_haut")
			type ="dh";
		else
			type = "db";

		var message = type + '/' + $scope.nbRepet + '_' + $scope.nbTaille + '_' + $scope.diffTaille + '_' + $scope.nbTailleH + '_' + $scope.diffTailleH;
		$http.get(message);
		$state.go('runPortes');
	};
});