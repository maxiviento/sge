$(document).ready(function () {
    $('.input-validation-error').removeClass("input-validation-error");
    $('.field-validation-error').addClass("field-validation-valid");

    $('#Buscar').click('click', function () {
        $('#FormIndexTipoRechazo').attr('action', '/TiposRechazo/BuscarTipoRechazo');
        $("#FormIndexTipoRechazo").submit()
    });

    $('#Agregar').click('click', function () {
        $('#FormularioTipoRechazo').attr('action', '/TiposRechazo/AgregarTipoRechazo');
        $("#FormularioTipoRechazo").submit()
    });

    $('#Modificar').click('click', function () {
        $('#FormularioTipoRechazo').attr('action', '/TiposRechazo/ModificarTipoRechazo');
        $("#FormularioTipoRechazo").submit()
    });

    $('#Eliminar').click('click', function () {

        $.ajax({
            type: "POST",
            url: "/TiposRechazo/ValidarEliminar",
            data: "{idTipoRechazo:'" + $("#Id").val() + "'}",
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
                $('#FormularioTipoRechazo').attr('action', '/TiposRechazo/EliminarTipoRechazo');
                $("#FormularioTipoRechazo").submit();
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