(function () {
    "use strict";

    angular.module("app-trips", ["simpleControls", "ngRoute"]) //simpleControls and angular-route
        .config(function ($routeProvider) { 
//configure angular routes
            $routeProvider.when("/", {
                controller: "tripsController",
                controllerAs: "vm",
                templateUrl: "/views/tripsView.html"
            });

            $routeProvider.when("/editor/:tripName", { //specify a route parameter
                controller: "tripEditorController",
                controllerAs: "vm",
                templateUrl: "/views/tripEditorView.html"
            });

            $routeProvider.otherwise({
                redirectTo: "/"
            });


        });


})();