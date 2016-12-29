(function () {
    "use strict";
    angular.module("app-trips").controller("tripEditorController", tripEditorController);

    function tripEditorController($routeParams, $http) {
        var vm = this;

        vm.tripName = $routeParams.tripName;

        vm.stops = [];
        vm.errorMessage = "";
        vm.isBusy = true;
        vm.newStop = {};

        $http.get("/api/trips/" + vm.tripName + "/stops")
        .then(function (response) {
            //success
            vm.errorMessage = "";
            angular.copy(response.data, vm.stops);
            showMap(vm.stops);

        }, function (error) {
            //fail
            vm.errorMessage = "Failed to get the trip: " + error;
        }).finally(function () {
            vm.isBusy = false;
        });

        vm.addStop = function () {
            vm.isBusy = true;

            $http.post("/api/trips/" + vm.tripName + "/stops", vm.newStop)
            .then(function (response) {
                //success
                vm.errorMessage = "";
                vm.stops.push(response.data);
                showMap(vm.stops);
                vm.newStop = {};
            }, function (error) {
                //fail
                vm.errorMessage = "Failed to save the trip: " + error;
            }).finally(function () {
                vm.isBusy = false;
            });
        };
    };

    function showMap(stops) {
        if (stops && stops.length > 0) {
            //format with underscore.js
            var mapStops = _.map(stops, function (item) {
                return {
                    lat: item.latitude,
                    long: item.longitude,
                    info: item.name
                };
            });


            travelMap.createMap({
                stops: mapStops,
                selector: "#map",
                currentStop: 1,
                initialZoom: 3
            });
        }
    };

})();