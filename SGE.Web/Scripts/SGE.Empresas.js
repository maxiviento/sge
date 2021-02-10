$(document).ready(function () {
    $("#CuitBusqueda").mask("99-99999999-9");
    $("#CBU").mask("9999999999999999999999");
    $("#CUENTA").mask("999999999");

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

    $('#dialogCuit').dialog({
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
        //open: function (event, ui) { $('.ui-draggable').attr("style", "outline: 0px; height: auto; width: 450px; top: 350px; left: 444px; display: block; z-index: 1002;"); },
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                var f = $("#FormularioEmpresa");
                $('#FormularioEmpresa').attr('action', '/Empresas/EliminarEmpresa');
                $("#FormularioEmpresa").submit();
                $(this).dialog("close");
            },
            "Cancel": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConfirmarDesvinculacion').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                var f = $("#FormConfirmTraspaso");
                $('#FormConfirmTraspaso').attr('action', '/Empresas/ConfirmarTraspasoFichas');
                $("#FormConfirmTraspaso").submit()
                $(this).dialog("close");
            },
            "Cancel": function () {
                $(this).dialog("close");
            }
        }
    });




    $("#tabs").tabs();


    $('#tabs').tabs('select', $('#TabSeleccionado').val());


    $("#Cuil").mask("99-99999999-9");
    $("#Cuit").mask("99-99999999-9");
    $("#BusquedaFicha").mask("99999999");

    $('.input-validation-error').removeClass("input-validation-error");
    $('.field-validation-error').addClass("field-validation-valid");

    $('#Buscar').click('click', function () {
        $('#FormIndexEmpresa').attr('action', '/Empresas/BuscarEmpresa');
        $("#FormIndexEmpresa").submit()
    });

    $('#Agregar').click('click', function () {
        var groupname = '';
        var groupnamevalidate = '';

        $(this).attr('data-val-valgroup-name', function (i, val) {
            groupnamevalidate = val;
        });

        $("#FormularioEmpresa").find(':input').each(function () {
            var rules = $(this).rules("remove");
            $(this).data("rulesBackup", rules);

            $(this).attr('data-val-valgroup-name', function (i, val) {
                if (groupnamevalidate == val) {
                    $(this).rules("add", $(this).data("rulesBackup"));
                }
            });
        });

        $('#FormularioEmpresa').attr('action', '/Empresas/AgregarEmpresa');
        $("#FormularioEmpresa").submit()

        $("#FormularioEmpresa").find(':input').each(function () {
            $(this).attr('data-val-valgroup-name', function (i, val) {
                $(this).rules("add", $(this).data("rulesBackup"));
            });
        });
    });

    $('#Modificar').click('click', function () {
        var groupname = '';
        var groupnamevalidate = '';

        $(this).attr('data-val-valgroup-name', function (i, val) {
            groupnamevalidate = val;
        });

        $("#FormularioEmpresa").find(':input').each(function () {
            var rules = $(this).rules("remove");
            $(this).data("rulesBackup", rules);

            $(this).attr('data-val-valgroup-name', function (i, val) {
                if (groupnamevalidate == val) {
                    $(this).rules("add", $(this).data("rulesBackup"));
                }
            });
        });

        $('#FormularioEmpresa').attr('action', '/Empresas/ModificarEmpresa');
        $("#FormularioEmpresa").submit()

        $("#FormularioEmpresa").find(':input').each(function () {
            $(this).attr('data-val-valgroup-name', function (i, val) {
                $(this).rules("add", $(this).data("rulesBackup"));
            });
        });
    });

    $('#Eliminar').click('click', function () {
        $.ajax({
            type: "POST",
            url: "/Empresas/Validar",
            data: "{idEmpresa:'" + $("#Id").val() + "'}",
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

    $('#AgregarUsuario').click('click', function () {
        var groupname = '';
        var groupnamevalidate = '';

        $(this).attr('data-val-valgroup-name', function (i, val) {
            groupnamevalidate = val;
        });

        $("#FormularioEmpresa").find(':input').each(function () {
            var rules = $(this).rules("remove");
            $(this).data("rulesBackup", rules);

            $(this).attr('data-val-valgroup-name', function (i, val) {
                if (groupnamevalidate == val) {
                    $(this).rules("add", $(this).data("rulesBackup"));
                }
            });
        });

        $('#FormularioEmpresa').attr('action', '/Empresas/AgregarUsuario');
        $("#FormularioEmpresa").submit()

        $("#FormularioEmpresa").find(':input').each(function () {
            $(this).attr('data-val-valgroup-name', function (i, val) {
                $(this).rules("add", $(this).data("rulesBackup"));
            });
        });

    });

    $('#ModificarUsuario').click('click', function () {

        var groupname = '';
        var groupnamevalidate = '';

        $(this).attr('data-val-valgroup-name', function (i, val) {
            groupnamevalidate = val;
        });

        $("#FormularioEmpresa").find(':input').each(function () {
            var rules = $(this).rules("remove");
            $(this).data("rulesBackup", rules);

            $(this).attr('data-val-valgroup-name', function (i, val) {
                if (groupnamevalidate == val) {
                    $(this).rules("add", $(this).data("rulesBackup"));
                }
            });
        });

        $('#FormularioEmpresa').attr('action', '/Empresas/ModificarUsuario');
        $("#FormularioEmpresa").submit()

        $("#FormularioEmpresa").find(':input').each(function () {
            $(this).attr('data-val-valgroup-name', function (i, val) {
                $(this).rules("add", $(this).data("rulesBackup"));
            });
        });

    });

    $('#BuscarEmpAvincular').click('click', function () {
        $('#FormDesvincularFichas').attr('action', '/Empresas/BuscarEmpresaAvincular');
        $("#FormDesvincularFichas").submit()
    });

    $('#DesvincularFichas').click('click', function () {
        $('#FormDesvincularFichas').attr('action', '/Empresas/DesvincularFichas');
        $("#FormDesvincularFichas").submit()
    });

    $('#SeleccionarDestino').click('click', function () {
        var f = $("#FormDesvincularFichas");
        $('#FormDesvincularFichas').attr('action', '/Empresas/SeleccionarDestino');
        $("#FormDesvincularFichas").submit()
    });

    $('#Confirmar').click('click', function () {
        $('#dialogConfirmarDesvinculacion').dialog("open");
    });

    $('#VolverFormEmpresa').click('click', function () {
        var f = $("#FormDesvincularFichas");
        $('#FormDesvincularFichas').attr('action', '/Empresas/VolverFormularioEmpresa');
        $("#FormDesvincularFichas").submit()
    });

    $('#VolverFormSelecDestino').click('click', function () {
        var f = $("#FormConfirmTraspaso");
        $('#FormConfirmTraspaso').attr('action', '/Empresas/VolverFormularioSeleccionarDestino');
        $("#FormConfirmTraspaso").submit()
    });

    $('#VolverCancelar').click('click', function () {
        var f = $("#FormularioEmpresa");
        $('#FormularioEmpresa').attr('action', '/Empresas/VolverCancelar');
        $("#FormularioEmpresa").submit()
    });

    $('#BuscarSede').click('click', function () {

        var groupname = '';
        var groupnamevalidate = '';

        $(this).attr('data-val-valgroup-name', function (i, val) {
            groupnamevalidate = val;
        });

        $("#FormularioEmpresa").find(':input').each(function () {
            var rules = $(this).rules("remove");
            $(this).data("rulesBackup", rules);

            $(this).attr('data-val-valgroup-name', function (i, val) {
                if (groupnamevalidate == val) {
                    $(this).rules("add", $(this).data("rulesBackup"));
                }
            });
        });

        $('#FormularioEmpresa').attr('action', '/Empresas/BuscarSede');
        $("#FormularioEmpresa").submit()

        $("#FormularioEmpresa").find(':input').each(function () {
            $(this).attr('data-val-valgroup-name', function (i, val) {
                $(this).rules("add", $(this).data("rulesBackup"));
            });
        });



    });

    $('#BuscarFicha').click('click', function () {
        var groupname = '';
        var groupnamevalidate = '';

        $(this).attr('data-val-valgroup-name', function (i, val) {
            groupnamevalidate = val;
        });

        $("#FormularioEmpresa").find(':input').each(function () {
            var rules = $(this).rules("remove");
            $(this).data("rulesBackup", rules);

            $(this).attr('data-val-valgroup-name', function (i, val) {
                if (groupnamevalidate == val) {
                    $(this).rules("add", $(this).data("rulesBackup"));
                }
            });
        });

        $('#FormularioEmpresa').attr('action', '/Empresas/BuscarFicha');
        $("#FormularioEmpresa").submit()

        $("#FormularioEmpresa").find(':input').each(function () {
            $(this).attr('data-val-valgroup-name', function (i, val) {
                $(this).rules("add", $(this).data("rulesBackup"));
            });
        });

    });

    // 06/03/2013 - DI CAMPLI LEANDRO
    $('#BlanquearClave').click('click', function () {
        var f = $("#FormularioEmpresa");
        $('#FormularioEmpresa').attr('action', '/Empresas/BlanquearCuentaUsr');
        $("#FormularioEmpresa").submit()
    });

    // 16/01/2014 - DI CAMPLI LEANDRO
    $('#btnExportarFichasEmp').click('click', function () {
        $('#FormularioEmpresa').validate().cancelSubmit = true;
        $('#FormularioEmpresa').attr('action', '/Empresas/ExportarFichasEmpresa');
        $("#FormularioEmpresa").submit()
    });



    $(".ClassVerificada").click(function () {
        $("#verificada").val($('input[name=verificada]:checked').val());


    });

    $(".ClassEsCooperativa").click(function () {
        $("#EsCooperativa").val($('input[name=EsCooperativa]:checked').val());


    });

    $(".ClassTipoCuenta").click(function () {
        $("#TIPOCUENTA").val($('input[name=TIPOCUENTA]:checked').val());


    });

    $(".ClassCBU").change(function () {
        //alert("cambio");
        //        var ext = $('#archivoFichas').val().split('.').pop().toLowerCase();
        //        if ($.inArray(ext, ['xls']) == -1) {
        //            $('#btnsubirArchivoFichas').css('display', 'none');
        //            alert('Archivo inválido, por favor seleccione otro archivo.');
        //        }
        //        else {
        //            $('#btnsubirArchivoFichas').show();
        //        }
        var str = $(".ClassCBU").val();
        var str2 = "";
        if ($(".ClassCBU").val() == "") {

            $("#CUENTA").val("");
        }
        else {

            if (str.substring(0, 3) == "020") {
                strb = str.substring(13, 20);
                //alert(strb);
                str2 = strb + "0" + str.substring(20, 21);
                //alert(str2);
                $("#CUENTA").val(str2);

                var ele = document.getElementsByName('TIPOCUENTA');
                if (str.substring(8, 9) == "1") {
                    $("#TIPOCUENTA").val('146');
                   
                }
                else {
                    $("#TIPOCUENTA").val('145');
                }
                
                for (i = 0; i < ele.length; i++) {
                    if (str.substring(8, 9) == "1") {
                        if (ele[i].value == 146) {
                            ele[i].checked = "checked";
                        }
                    }
                    else {
                        if (ele[i].value == 145) {
                            ele[i].checked = "checked";
                        }
                    }
                }

            } else

            { $("#CUENTA").val("000000000"); }

        }


    });

});