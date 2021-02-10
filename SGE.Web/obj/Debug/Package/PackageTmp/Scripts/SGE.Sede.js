$(document).ready(function () {

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

    $('#dialogExisteSede').dialog({
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

    $('#dialogConfirmarEliminar').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                var f = $("#FormularioSede");
                $('#FormularioSede').attr('action', '/Sedes/EliminarSede');
                $("#FormularioSede").submit();
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
        $('#FormIndexSede').attr('action', '/Sedes/BuscarSede');
        $("#FormIndexSede").submit()
    });

    $('#VolverFormEmpresa').click('click', function () {
        $('#FormularioSede').validate().cancelSubmit = true;
        $('#FormularioSede').attr('action', '/Sedes/VolverFormularioEmpresa');
        $("#FormularioSede").submit()
    });

    $('#Agregar').click('click', function () {
        $.ajax({
            type: "POST",
            url: "/Sedes/ValidarNombre",
            data: "{id:'" + $("#Id").val() + "', descripcion: '" + $("#Descripcion").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data == true) {
                    $('#dialogExisteSede').dialog("open");
                }
                else {
                    $('#FormularioSede').attr('action', '/Sedes/AgregarSede');
                    $("#FormularioSede").submit()
                }
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });

    $('#Modificar').click('click', function () {
        $('#FormularioSede').attr('action', '/Sedes/ModificarSede');
        $("#FormularioSede").submit()
    });

    $('#Eliminar').click('click', function () {

        $.ajax({
            type: "POST",
            url: "/Sedes/Validar",
            data: "{idSede:'" + $("#Id").val() + "'}",
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
    

});