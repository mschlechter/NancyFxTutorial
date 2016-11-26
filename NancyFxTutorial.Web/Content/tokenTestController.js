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

  $scope.testToken = function () {
    $scope.melding = null;

    var config = {
      headers: {
        'Authorization': 'Bearer ' + $scope.token,
        'Accept': 'application/json'
      }
    };

    $http.get("/secure/hallo", config).then(function (response) {
      $scope.melding = response.data;
    }, function (response) {
      if (response.status === 401) {
        $scope.melding = response.statusText;
      }
    });

  };



});
