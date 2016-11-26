app.controller("tokenTestController", function ($scope, $http) {

  $scope.authRequest = {
    naam: "",
    wachtwoord: ""
  };

  $scope.token = null;

  $scope.getToken = function () {
    $scope.melding = null;

    $http.post("/auth/token", $scope.authRequest).then(function (response) {
      $scope.token = response.data;
    }, function (response) {
      if (response.status === 400)
      {
        $scope.melding = response.statusText;
      }
    });

  };

});
