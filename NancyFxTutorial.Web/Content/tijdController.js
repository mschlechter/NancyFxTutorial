app.controller("tijdController", function ($scope, $http) {
  
  $scope.tijdLaden = function () {
    $http.get("/tijd").then(function (response) {
      $scope.tijd = response.data;
    });
  };

  $scope.tijdLaden();

  $scope.vernieuwen = function () {
    $scope.tijdLaden();
  };


});