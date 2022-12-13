$(document).ready(function () {
    $("#glossary").click(function (evt) {
        //$(".glossaryMain").fadeIn(500);

        /*$('.glossaryMain').css({
            'top': evt.pageY+20,
            'left': evt.pageX - 125
            //'padding': '0 0px 0px'
        });*/
        
        $('#ctl00_headerBanner_glsdiv').css({
            'top': '60px',
            'left': '450px',
            'position': 'fixed',
            'background': '#FFFFFF'
        });
        
        $("#ctl00_headerBanner_glsdiv").show(300);
    });

    $(".expandstory").click(function () {
       // $(".description").slideUp();
      //  $(this).next('.description').slideToggle();
    });

    $("#glsClose").click(function () {
        $("#ctl00_headerBanner_glsdiv").hide(300);
    });
});