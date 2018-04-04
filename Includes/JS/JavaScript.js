$(document).ready(function () {
    $(".linkSub").click(function () {
        $(".iframeDat").attr("src", $(".linkTxt").val());
    });


    $(".iframeWidth").keyup(function () {
        if ($(".iframeWidth").val() < 1500) {
            $(".iframeWrapper").css("width", $(".iframeWidth").val() + "px");
        } else {
            $(".iframeWrapper").css("width", "1500px");
        }
    });



    $(".iframeHeight").keyup(function () {
        if ($(".iframeHeight").val() < 1000) {
            $(".iframeWrapper").css("height", $(".iframeHeight").val() + "px");
        } else {
            $(".iframeWrapper").css("height", "1000px");
        }
        
    });
});