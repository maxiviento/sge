$(document).ready(function () {

    $('#dialogConfirmarEliminar').dialog({
        open: function (event, ui) { $('.ui-draggable').attr("style", "outline: 0px; height: auto; width: 450px; top: 350px; left: 444px; display: block; z-index: 1002;"); },
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                var f = $("#FormularioInstitucion");
                $('#FormularioInstitucion').attr('action', '/Instituciones/EliminarInstitucion');
                $("#FormularioInstitucion").submit()
                $(this).dialog("close");
            },
            "Cancel": function () {
                $(this).dialog("close");
            }
        }
    });

    $('.input-validation-error').removeClass("input-validation-error");
    $('.field-validation-error').addClass("field-validation-valid");

    $('#Buscar').click('click', function () {
        $('#FormIndexInstitucion').attr('action', '/Instituciones/BuscarInstitucion');
        $("#FormIndexInstitucion").submit()
    });

    $('#Agregar').click('click', function () {
        $('#FormularioInstitucion').attr('action', '/Instituciones/AgregarInstitucion');
        $("#FormularioInstitucion").submit()
    });

    $('#Modificar').click('click', function () {
        $('#FormularioInstitucion').attr('action', '/Instituciones/ModificarInstitucion');
        $("#FormularioInstitucion").submit()
    });

    $('#Eliminar').click('click', function () {
        $('#dialogConfirmarEliminar').dialog("open");
    });



    $('#AsociarSector').click('click', function () {
        //$('#ABMNombreSede').removeClass("input-validation-error");
        //$('#ABMSedeTelefonoContacto').removeClass("input-validation-error");

        $.ajax({
            type: "POST",
            url: "/Instituciones/AsociarSector",
            data: "{idInstitucion:" + $('#Id').val() + ", accion: '" + $('#Accion').val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('#grilla').empty();
                $('#grilla').append(data);
                $('#dialogSector').dialog("open");
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });


    $('#dialogSector').dialog({
        autoOpen: false,
        width: 1200,
        modal: true,
        resizable: false,
        buttons: {
            "Cerrar": function () {
                $(this).dialog("close");

            }
        }
    });

//    $('#QuitarSector').click('click', function() {
//        $('#FormularioInstitucion').attr('action', '/Instituciones/delSectorInstitucion');
//        $("#FormularioInstitucion").submit()
//    
//    });


    //fin ready
});


function SeleccionarSector(idInstitucion, idSector, accion) {
    $('#FormularioInstitucion').validate().cancelSubmit = true;
    $('#idInstitucion').val(idInstitucion);
    $('#idSector').val(idSector);
    $('#acc').val(accion);
    $('#dialogSector').dialog("close");
    $('#FormularioInstitucion').attr('action', '/Instituciones/AddSectorInstitucion');
    $("#FormularioInstitucion").submit();

}
