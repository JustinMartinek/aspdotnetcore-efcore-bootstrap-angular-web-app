(function () {
    "use strict";

    angular.module("simpleControls", [])
    .directive("waitCursor", waitCursor);

    function waitCursor() {
        return {
            scope: {
                displayWhen: "=displayWhen" //first part is the attribute on the template, the second part is the attribute name on the consumer (using dash lowercase)
            },
            restrict: "E", //restrict to element only
            templateUrl:"/views/waitCursor.html" //ref to wwwroot folder
        };
    };
})();