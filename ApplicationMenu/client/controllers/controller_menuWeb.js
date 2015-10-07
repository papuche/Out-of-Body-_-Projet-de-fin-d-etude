var menu = angular.module('menu', []);

menu.controller('MainCtrl', ['$scope', function ($scope) {
    
    $scope.chemin = 'Accueil >';

    $scope.exp_click = function(){
    	$scope.chemin = "Accueil > Experience";    	
    }

    $scope.portes_click = function(){
    	$scope.chemin = "Accueil > Portes";    	
    }
    
}]);