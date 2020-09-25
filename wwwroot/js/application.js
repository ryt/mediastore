// application.js v1

function setCookie(name, value, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires="+d.toUTCString();
    document.cookie = name + "=" + value + ";" + expires + ";path=/";
}

function getCookie(name) {
    var name = name + "=";
    var ca = document.cookie.split(';');
    for ( var i = 0; i < ca.length; i++ ) {
        var c = ca[i];
            while ( c.charAt(0) == ' ' ) {
            c = c.substring(1);
        }
        if ( c.indexOf(name) == 0 ) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

$(document).ready(function(){

    $(".upload-file").change(function(){
        $(".upload-button").removeClass("hide");
    });

    $(".upload-button").click(function(){
        var self = $(this);
        self.find(".bload").removeClass("hide");
        self.find(".btxt").text("Uploading...");
        setTimeout(function(){
            self.attr("disabled", "disabled");
        }, 500);
    });

    $(".teacher-toggle").click(function(){
        var teacher = prompt("Change current teacher to:", "");
        if ( teacher != null ) {
            setCookie("teacher", teacher, 30);
            $(this).text(teacher);
            window.location.reload();
        }
    });

});