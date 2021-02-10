jQuery(function () {
    jQuery('ul.sf-menu').superfish();
});

$(document).ready(function () {
    

    if ($("#UltimaActividad").val() == '') {
        $("#UltimaActividad").val($.datepicker.formatDate('dd/mm/yy', new Date()))
    }

    $('#dialog60s').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogExpiracion').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                $(this).dialog("close");
            }
        }
    });




});

