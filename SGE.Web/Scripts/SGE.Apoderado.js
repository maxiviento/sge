$(document).ready(function () {

    $('#dialogExistsApoderadoAgregar').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                var f = $("#FormularioApoderado");
                $('#FormularioApoderado').attr('action', '/Apoderados/AgregarApoderado');
                $("#FormularioApoderado").submit()
                $(this).dialog("close");
            },
            "Cancel": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogExistsApoderadoModificar').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                var f = $("#FormularioApoderado");
                $('#FormularioApoderado').attr('action', '/Apoderados/ModificarApoderado');
                $("#FormularioApoderado").submit()
                $(this).dialog("close");
            },
            "Cancel": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEliminar').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                var f = $("#FormularioApoderado");
                $('#FormularioApoderado').attr('action', '/Apoderados/EliminarApoderado');
                $("#FormularioApoderado").submit()
                $(this).dialog("close");
            },
            "Cancel": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogLimpiarFechaSolicitud').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                var f = $("#FormularioApoderado");
                $('#FormularioApoderado').attr('action', '/Apoderados/LimpiarFecha');
                $("#FormularioApoderado").submit()
                $(this).dialog("close");
            },
            "Cancel": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConfirmacionCambio').dialog({
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

    $('#dialogCambioEstadoActivo').dialog({
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


    $("#Cuil").mask("99-99999999-9");
    $("#NumeroDocumento").mask("99999999");

    $('.input-validation-error').removeClass("input-validation-error");
    $('.field-validation-error').addClass("field-validation-valid");


    $('.input-validation-error').removeClass("input-validation-error");
    $('.field-validation-error').addClass("field-validation-valid");

    $('#Buscar').click('click', function () {
        $('#FormularioApoderado').attr('action', '/Apoderados/BuscarApoderado');
        $("#FormularioApoderado").submit()
    });

    $('#Agregar').click('click', function () {
        $.ajax({
            type: "POST",
            url: "/Apoderados/ExisteApoderado",
            data: "{nroDocumento:'" + $("#NumeroDocumento").val() + "', idBeneficiario: '" + $("#IdBeneficiario").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data == true) {
                    $('#dialogExistsApoderadoAgregar').dialog("open");
                }
                else {
                    $('#FormularioApoderado').attr('action', '/Apoderados/AgregarApoderado');
                    $("#FormularioApoderado").submit()
                }
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });

    $('#Modificar').click('click', function () {
        $.ajax({
            type: "POST",
            url: "/Apoderados/ExisteApoderado",
            data: "{nroDocumento:'" + $("#NumeroDocumento").val() + "', idBeneficiario: '" + $("#IdBeneficiario").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data == true) {
                    $('#dialogExistsApoderadoModificar').dialog("open");
                }
                else {
                    $('#FormularioApoderado').attr('action', '/Apoderados/ModificarApoderado');
                    $("#FormularioApoderado").submit()
                }
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });


    $('#VolverFormBeneficiario').click('click', function () {
        var f = $("#FormularioApoderado");
        $('#FormularioApoderado').validate().cancelSubmit = true;
        $('#FormularioApoderado').attr('action', '/Apoderados/VolverFormularioBeneficiario');
        $("#FormularioApoderado").submit()
    });

    $('#btnEliminarApoderado').click('click', function () {
        $('#dialogEliminar').dialog("open");
    });




    $('#Programas_Selected').change('click', function () {
        $('#GenerarFile').hide();
    });

    $('#Programas_Selected').change('click', function () {
        $('#GenerarFile').hide();
    });


    $("#Localidades_Selected").change(function () {
        if ($("#Localidades_Selected").val() != "25" && $("#Localidades_Selected").val() != "0") {
            //Sucursales_Selected
            $.ajax({
                type: "POST",
                url: "/Apoderados/BuscarSucursal",
                data: "{idlocalidad:'" + $("#Localidades_Selected").val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data != null) {
                        $("#Sucursales_Selected").val(data.IdSucursal);
                    }
                    else {
                        $("#Sucursales_Selected").val("0");
                    }
                },
                error: function (msg) {
                    alert(msg);
                }
            });

        }
        else {
            if ($("#Localidades_Selected").val() != "25") {
                $("#Sucursales_Selected").val($("#Sucursales_Selected").val());
            } else
            { $("#Sucursales_Selected").val("143"); }
        }
    });


    $('#CambiarEstadoApoderado').click('click', function () {
        if ($('#EstadosApoderados_Selected').val() == 1) {
            $.ajax({
                type: "POST",
                url: "/Apoderados/ExisteApoderadoActivo",
                data: "{nroDocumento:'" + $("#NumeroDocumento").val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data == true) {
                        $('#dialogCambioEstadoActivo').dialog("open");
                    }
                    else {
                        var f = $("#FormularioApoderado");
                        $('#FormularioApoderado').validate().cancelSubmit = true;
                        $('#FormularioApoderado').attr('action', '/Apoderados/CambiarEstado');
                        $("#FormularioApoderado").submit()
                    }
                },
                error: function (msg) {
                    alert(msg);
                }
            });
        }
        else {
            var f = $("#FormularioApoderado");
            $('#FormularioApoderado').validate().cancelSubmit = true;
            $('#FormularioApoderado').attr('action', '/Apoderados/CambiarEstado');
            $("#FormularioApoderado").submit()
        }

    });


    $('#LimpiarFechaApoderado').click('click', function () {
        $('#dialogLimpiarFechaSolicitud').dialog("open");
    });

    if ($('#RealizoElCambio').val() == "True") {
        $('#dialogConfirmacionCambio').dialog("open");
    }



});