var portes = angular.module('portes', []);

portes.controller('PortesCtrl', function ($scope,$state) {

	$scope.entiere_click = function(){

		//$parent.$scope.chemin = "Accueil > Portes > Entieres";    	
	}

	$scope.etagere_click = function(){
		//$parent.$scope.chemin = "Accueil > Portes > Etageres"; 
	}

	$scope.portique_click = function(){
		//$parent.$scope.chemin = "Accueil > Portes > Portiques";
	}

});