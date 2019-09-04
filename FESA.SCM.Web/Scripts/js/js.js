
$(document).ready(function () {
    $(".btn-orden1").addClass("Active");

    url = $(location).attr('pathname');
     if (url == "/User") {
         $(".btn-orden").removeClass("Active");
         $(".btn-orden1").addClass("Active");
     }
     if (url == "/Calendar") {
         $(".btn-orden").removeClass("Active");
         $(".btn-orden2").addClass("Active");
     }
 
});

$(document).ready(function () {
    $('.datepicker-field').datepicker({ dateFormat: "mm/dd/yy" }).val()
});

