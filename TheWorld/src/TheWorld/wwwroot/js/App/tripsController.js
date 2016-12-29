(function () {
    "use strict";
    angular.module("app-trips").controller("tripsController", tripsController);

    function tripsController($http) {

        var vm = this;

        vm.errorMessage = "";

        vm.isBusy = true;

        vm.trips = [];

        vm.newTrip = {};

        $http.get("/api/trips")
        .then(function (response) {
            //success
            angular.copy(response.data, vm.trips);
        },
        function (error) {
            //fail
            vm.errorMessage = "Failed to load data. " + error;
        }).finally(function () {
            vm.isBusy = false;
        });
        vm.addTrip = function () {
            vm.isBusy = true;
            vm.errorMessage = "";
            $http.post("/api/trips", vm.newTrip)
            .then(function (response) {
                //success
                vm.trips.push(response.data);
                //clear form
                vm.newTrip = {};
            }, function (error) {
                //fail
                vm.errorMessage = "Failed to save new trip. " + error;
            }).finally(function () {
                vm.isBusy = false;
                
            });

        };
    };


})();