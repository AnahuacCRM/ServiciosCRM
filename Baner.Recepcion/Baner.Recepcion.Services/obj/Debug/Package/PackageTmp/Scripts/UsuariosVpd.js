var app = angular.module("MyApp", ['angular-loading-bar', 'ngAnimate', 'ngRoute', 'blockUI']);

app.config(['$locationProvider', 'blockUIConfig', function ($locationProvider, blockUIConfig) {
    blockUIConfig.message = '. . Procesando . .';
    // In order to get the query string from the
    // $location object, it must be in HTML5 mode.
    $locationProvider.html5Mode(true);
}]);

app.controller("ContrlUser", function ($scope, $http, $location, $window) {

    function getInfoUser() {
        $http.get('/api/srvGestionCoincidencias?LeadId=' + $scope.id).
            success(function (data, status, headers, config) {
                $scope.Data = data;


                $http.post('/api/srvGestionCoincidencias', $scope.Data).
                    success(function (data, status, headers, config) {
                        if (status === 200)
                            $scope.customers = data;
                        else
                            $scope.customers = null;
                    }).
                    error(function (data, status, headers, config) {
                        $scope.customers = null;
                        // log error
                        if (status === null || status === undefined) {
                            $scope.ErrorMesage = "Servicio no disponible";
                        }
                        else {
                            if (status === -1) {
                                $scope.ErrorMesage = "Servicio no disponible";
                            }
                            else if (status === 406) {
                                $scope.ErrorMesage = data.Message;
                            }
                            else
                                $scope.ErrorMesage = "Error :" + status + ", " + data.Message;
                        }
                    });
            }).
            error(function (data, status, headers, config) {
                $scope.customers = null;
                // log error
                if (status === null || status === undefined) {
                    $scope.ErrorMesage = ".Servicio no disponible";
                }
                else {
                    if (status === -1) {
                        $scope.ErrorMesage = ".Servicio no disponible";
                    }
                    else
                        $scope.ErrorMesage = ".Error :" + status + ", " + data.Message;
                }
            });
    }
});