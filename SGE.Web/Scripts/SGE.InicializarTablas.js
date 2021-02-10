$(document).ready(function () {
    $('.input-validation-error').removeClass("input-validation-error");
    $('.field-validation-error').addClass("field-validation-valid");

    $('#Insertar').click('click', function () {
        $('#FormIndexInicializarTablas').attr('action', '/InicializarTablas/Insertar');
        $("#FormIndexInicializarTablas").submit()
    });

    $('#Eliminar').click('click', function () {

        $.ajax({
            type: "POST",
            url: "/Carreras/ValidarEliminar",
            data: "{idCarrera:'" + $("#Id").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data == true) {
                    $('#dialog').dialog("open");
                }
                else {
                    $('#dialogConfirmarEliminar').dialog("open");
                }
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });

    $('#dialogConfirmarEliminar').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                $('#FormularioInicializarTablas').attr('action', '/InicializarTablas/EliminarInicializarTablas');
                $("#FormularioInicializarTablas").submit();
                $(this).dialog("close");
            },
            "Cancel": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialog').dialog({
        autoOpen: false,
        width: 350,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                $(this).dialog("close");
            }
        }
    });
});