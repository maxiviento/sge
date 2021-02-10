$(document).ready(function () {
    if ($('#Accion').val() != "Ver") {
        if ($('#Accion').val() == "Nuevo") {
            $(".Ver").removeAttr("disabled");
            $('#Formulario').attr('action', '/Conceptos/Agregar');
        }
        else {
            $("#Observacion").removeAttr("disabled");
            $('#Formulario').attr('action', '/Conceptos/ModificarConcepto');
        }
    }
    else {
        $('#btnGuardar').hide();
    }

    $("#Año").focus();

    $('#btnGuardar').click('click', function () {
        $("#Formulario").submit();
    });

    $('#btnBuscar').click('click', function () {
        $("#FormConceptos").submit();
    });
});