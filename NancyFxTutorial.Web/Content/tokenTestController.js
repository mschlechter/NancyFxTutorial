app.controller("tokenTestController", function ($scope, $http) {

  $scope.authRequest = {
    naam: "",
    wachtwoord: ""
  };

  $scope.getToken = function () {

    $http.post("/auth/token", $scope.authRequest).then(function (response) {
      alert(response.data);
    });


  };

});
