$(document).ready(function () {
    $('.input-validation-error').removeClass("input-validation-error");
    $('.field-validation-error').addClass("field-validation-valid");

    $('#Buscar').click('click', function () {
        $('#FormIndexCausaRechazo').attr('action', '/CausasRechazo/BuscarCausaRechazo');
        $("#FormIndexCausaRechazo").submit();
    });

    $('#Agregar').click('click', function () {
        $('#FormularioCausaRechazo').attr('action', '/CausasRechazo/AgregarCausaRechazo');
        $("#FormularioCausaRechazo").submit()
    });

    $('#Modificar').click('click', function () {
        $('#FormularioCausaRechazo').attr('action', '/CausasRechazo/ModificarCausaRechazo');
        $("#FormularioCausaRechazo").submit()
    });

    $('#Eliminar').click('click', function () {

        $.ajax({
            type: "POST",
            url: "/CausasRechazo/ValidarEliminar",
            data: "{idCausaRechazo:'" + $("#Id").val() + "'}",
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
                $('#FormularioCausaRechazo').attr('action', '/CausasRechazo/EliminarCausaRechazo');
                $("#FormularioCausaRechazo").submit();
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