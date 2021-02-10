$(document).ready(function () {

    $('#dialogConfirmarEliminacion').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                var f = $("#FormularioObservacion");
                $('#FormularioObservacion').attr('action', '/FichaObservacion/EliminarObservacion');
                $("#FormularioObservacion").submit()
                $(this).dialog("close");
            },
            "Cancel": function () {
                document.execCommand('SaveAs', '1', 'give img location here');
                $(this).dialog("close");
            }
        }
    });

    $('.input-validation-error').removeClass("input-validation-error");
    $('.field-validation-error').addClass("field-validation-valid");

  
    $('#Agregar').click('click', function () {
        $('#FormularioObservacion').attr('action', '/FichaObservacion/AgregarObservacion');
        $("#FormularioObservacion").submit()
    });

    $('#Modificar').click('click', function () {
        $('#FormularioObservacion').attr('action', '/FichaObservacion/ModificarObservacion');
        $("#FormularioObservacion").submit()
    });


    $('#Eliminar').click('click', function () {
        $('#dialogConfirmarEliminacion').dialog("open");
    });

    $('#VolverFormListado').click('click', function () {
        $('#FormularioObservacion').validate().cancelSubmit = true;
        $('#FormularioObservacion').attr('action', '/FichaObservacion/VolverListadoObservaciones');
        $("#FormularioObservacion").submit()
    });

    $('#VolverFormFicha').click('click', function () {
        $('#FormIndexObservaciones').attr('action', '/FichaObservacion/VolverFormFicha');
        $("#FormIndexObservaciones").submit()
    });

});