﻿(function () {
    
    var $sidebarAndWrapper = $("#sidebar,#wrapper");
    var $icon = $("#sidebarButton i.fa");


    $("#sidebarButton").on("click", function () {
        $sidebarAndWrapper.toggleClass("hide-sidebar");
        if($sidebarAndWrapper.hasClass("hide-sidebar")){
            $icon.removeClass("fa-angle-left")
            $icon.addClass("fa-angle-right")
        }else{
            $icon.removeClass("fa-angle-right")
            $icon.addClass("fa-angle-left")
        }
    });


})();