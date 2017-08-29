var app = angular.module("MyApp", ['angular-loading-bar', 'ngAnimate', 'ngRoute', 'blockUI']);

app.config(['$locationProvider', 'blockUIConfig', function ($locationProvider, blockUIConfig) {
    blockUIConfig.message = '. . Procesando . .';
    // In order to get the query string from the
    // $location object, it must be in HTML5 mode.
    $locationProvider.html5Mode(true);
}]);

app.controller("PostsCtrl",
    function ($scope, $http, $location, $window) {
        $scope.id = "";
        $scope.isDisabled = false;
        //ConsultaURL
        if (!$location.search().hasOwnProperty('id')) {
            $scope.ErrorMesage = 'No se proporciono el registro a buscar';
            return;
        }
        $scope.id = $location.search().id;

        if ($scope.id.length === 0) {
            $scope.ErrorMesage = 'No se proporciono el registro a buscar';
            return;
        }

        //Recuperar contacto
        getInfo();

        
        $scope.choseItem = function (source) {

            sendItem(source);
            $scope.isDisabled = true;
        };

        $scope.NuevoRegistro = function () {
            $scope.isDisabled = true;
            var source = {};
            source.IdBanner = "";
            sendItem(source);
        };

        $scope.Cierra = function () {
            $window.close();
        };

        $scope.ValidateToShow = function (source) {
            if ((source.IdCRM === null || source.IdCRM === undefined || source.IdCRM.length === 0) && (source.ContieneHA === null || source.ContieneHA === undefined || source.ContieneHA.length === 0))
                return true;
            else
                return false;
        };
        $scope.ShowAlert = function (source) {
            if (source.IdCRM === null || source.IdCRM === undefined || source.IdCRM.length === 0)
                return false;
            else
                return true;
        };

        $scope.ShowContieneHA = function (source) {
            if (source.ContieneHA === null || source.ContieneHA === undefined || source.ContieneHA.length === 0)
                return false;
            else
                return true;
        };
        

        function sendItem(source) {
            var po = {};
            po.LeadId = $scope.id;
            po.IdBanner = source.IdBanner;
            var jsondata = JSON.stringify(po);

            $http.post('/api/srvAsociaBannerProspecto', jsondata).
           success(function (data, status, headers, config) {
               $window.alert('Se envio satisfactoriamente ' + source.IdBanner);
               $window.close();
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
                   else {
                       $scope.ErrorMesage = data.Message;
                   }
               }
           });
        }

        function getInfo() {
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