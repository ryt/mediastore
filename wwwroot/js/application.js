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

function prelog(str) {
    $(".data-pre").html(str);
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

    var trackStart = new Date();
    
    if ( $("#media-video").length ) {

        var player = videojs("media-video");

        player.ready(function() {
        
            prelog("Player has loaded.");

            this.on('play', function(){
                prelog("Player is playing. Started at: " + (Math.round(this.currentTime()*100)/100) + ".");
            });

            this.on('pause', function(){
                prelog("Player is paused.");
            });

            this.on('ended', function(){
                prelog("Player has ended.");
            });

        });
    }

    $(".media-frame").each(function(){
        prelog("Document has loaded.");
    });

    $(".media-image").each(function(){
        prelog("Image has loaded.");
    });

    $(window).on("beforeunload", function() {
        var trackEnd = new Date();
        var timeSpent = trackEnd - trackStart;
            timeSpent /= 1000;
            timeSpent = Math.round(timeSpent*100)/100;
        if ( $(".media-player").length ) {
            prelog("Total time spent: " + timeSpent);
        }
     });

});