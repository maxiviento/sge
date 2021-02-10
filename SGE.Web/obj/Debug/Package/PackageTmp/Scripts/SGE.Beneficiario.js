

function CargarFecha(fecha) {
    var inicio = fecha.replace('+', '');
    var fin = inicio.replace('+', '');
    $('#FechaLimpieza').val(fin);
    $('#dialogConfirmar').dialog("open");
}


$(document).ready(function () {


    $('#Paginador').smartpaginator({ totalrecords: $('#ContBenef').val()
, recordsperpage: 10, datacontainer: 'grilla', dataelement: 'tr',
        initval: 0, next: 'Next', prev: 'Prev', first: 'First', last: 'Last', theme: 'green'
    });


    if ($('#Programas_Selected').val() != 3) {
        $('#trModalidad').hide();

    }

    $("#Cuil").mask("99-99999999-9");
    $("#Numero_Documento").mask("99999999");
    $("#CuilBusqueda").mask("99-99999999-9");
    $("#NumeroDocumentoBusqueda").mask("99999999");


    $('#Buscar').click('click', function () {
        var f = $("#FormIndexBeneficiarios");

        if ($("#ProgPorRol").val() == 'S') {
            if ($("#Programas_Selected").val() == '-1') {

                $("#dialog_SelPrograma").dialog("open");
                return false;
            }

        }
        $('#FormIndexBeneficiarios').attr('action', '/Beneficiarios/BuscarBeneficiario');
        $("#FormIndexBeneficiarios").submit()
    });

    //16/04/2013 - SE COMENTA PARA PRUEBAS
    $('#GenerarFile').click('click', function () {
        if ($('#Programas_Selected').val() != 0) {
            var f = $("#FormIndexBeneficiarios");
            //$('#procesando').dialog("open");
            $('#FormIndexBeneficiarios').attr('action', '/Beneficiarios/GenerarFile');
            $("#FormIndexBeneficiarios").submit();
        }
        else {
            $('#dialog').dialog("open");
        }
    });

    $('#Programas_Selected').change('click', function () {
        $('#GenerarFile').hide();
    });

    $("#archivo").change(function () {
        var ext = $('#archivo').val().split('.').pop().toLowerCase();
        if ($.inArray(ext, ['xls', 'xlsx']) == -1) {
            $('#btnsubir').css('display', 'none');
            alert('Archivo inválido, por favor seleccione otro archivo.');
        }
        else {
            $('#btnsubir').show();
        }
    });


    $('#btnsubir').click('click', function () {
        var f = $("#FormImportar");
        $('#FormImportar').attr('action', '/Beneficiarios/ImportarCuentas');
        $("#FormImportar").submit()
    });

    $('#ExportBeneficiario').click('click', function () {
        var f = $("#FormIndexBeneficiarios");
        $('#FormIndexBeneficiarios').attr('action', '/Beneficiarios/BeneficiariosXls');
        $("#FormIndexBeneficiarios").submit();
    });

    $('#btnGuardar').click('click', function () {
        $("#FormularioBeneficiario").attr('action', '/Beneficiarios/ModificarBeneficiario');
        $("#FormularioBeneficiario").submit();
    });

    $('#btnProcesarCuentas').click('click', function () {
        var f = $("#FormImportar");
        $('#FormImportar').attr('action', '/Beneficiarios/CargarCuentas');
        $("#FormImportar").submit()
    });

    $('.ConApoderado').change('click', function () {
        if ($('input[name=ConApoderado]:checked').val() == "N") {
            $("#ApellidoNombreApoderadoBusqueda").attr("disabled", "disabled");
        }
        else {
            $("#ApellidoNombreApoderadoBusqueda").removeAttr("disabled");
        }
    });




    if ($("#ProgPorRol").val() == "S") //verificar si tiene la acción activa para consultar los subprogramas
    {
        if ($("#idFormulario").val() == 'FormIndexBeneficiarios') {

            //******************************************************

            $.ajax({
                type: "POST",
                url: "/Subprogramas/SubprogramasRoles",
                data: "{idPrograma:'" + $("#Programas_Selected").val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data != null) {

                        var options = '';
                        var cbo = data;
                        $.each(cbo, function (index, obj) {
                            options += '<option value="' + obj.IdSubprograma + '">' + obj.NombreSubprograma + '</option>';
                        });

                        $("#ComboSubprogramas_Selected").html(options);

                        if ($('#idSubprograma').val() > 0) {
                            $('#ComboSubprogramas_Selected').val($('#idSubprograma').val());
                        }
                        else {
                            $('#idSubprograma').val($('#ComboSubprogramas_Selected').val());
                        }
                    }
                    else {

                    }
                },
                error: function (msg) {
                    alert(msg);
                }
            });
            //******************************************************
            $("#ComboSubprogramas_Selected").show();
            $("#lblSubprogramas").show();
            if ($('#Programas_Selected').val() == 3) {
                $("#ComboTiposPpp_Selected").show();
                $("#lblTipoPpp").show();
            }
            else {
                $("#ComboTiposPpp_Selected").hide();
                $("#ComboTiposPpp_Selected").val(0);
                $("#lblTipoPpp").hide();
            }

        }


    }
    else {

        if ($("#idFormulario").val() == 'FormIndexBeneficiarios') {

            //******************************************************

            $.ajax({
                type: "POST",
                url: "/Subprogramas/SubprogramasProg",
                data: "{idPrograma:'" + $("#Programas_Selected").val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data != null) {

                        var options = '';
                        var cbo = data;
                        $.each(cbo, function (index, obj) {
                            options += '<option value="' + obj.IdSubprograma + '">' + obj.NombreSubprograma + '</option>';
                        });

                        $("#ComboSubprogramas_Selected").html(options);

                        if ($('#idSubprograma').val() > 0) {
                            $('#ComboSubprogramas_Selected').val($('#idSubprograma').val());
                        }
                        else {
                            $('#idSubprograma').val($('#ComboSubprogramas_Selected').val());
                        }
                    }
                    else {

                    }
                },
                error: function (msg) {
                    alert(msg);
                }
            });
            //******************************************************
            $("#ComboSubprogramas_Selected").show();
            $("#lblSubprogramas").show();
            if ($('#Programas_Selected').val() == 3) {
                $("#ComboTiposPpp_Selected").show();
                $("#lblTipoPpp").show();
            }
            else {
                $("#ComboTiposPpp_Selected").hide();
                $("#ComboTiposPpp_Selected").val(0);
                $("#lblTipoPpp").hide();
            }

        }

    }





    $('#Programas_Selected').change(function () {
        if ($('#Programas_Selected').val() == 3) {
            $('#trModalidad').show();
        }
        else {
            $('#trModalidad').hide();
        }


        //*****************
        if (this.form.id == 'FormIndexBeneficiarios') {
            var idTipo = $("#Programas_Selected").val();
            if ($("#ProgPorRol").val() == "S") //verificar si tiene la acción activa para consultar los subprogramas
            {

                //******************************************************

                $.ajax({
                    type: "POST",
                    url: "/Subprogramas/SubprogramasRoles",
                    data: "{idPrograma:'" + $("#Programas_Selected").val() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        if (data != null) {

                            var options = '';
                            var cbo = data;
                            $.each(cbo, function (index, obj) {
                                options += '<option value="' + obj.IdSubprograma + '">' + obj.NombreSubprograma + '</option>';
                            });

                            $("#ComboSubprogramas_Selected").html(options);
                            $('#idSubprograma').val($('#ComboSubprogramas_Selected').val());
                        }
                        else {

                        }
                    },
                    error: function (msg) {
                        alert(msg);
                    }
                });
                //******************************************************

                $("#ComboSubprogramas_Selected").show();
                $("#lblSubprogramas").show();
                if ($('#ComboSubprogramas_Selected').val() > 0) {
                    $('#idSubprograma').val($('#ComboSubprogramas_Selected').val());
                } else { $('#idSubprograma').val(0); }

                if ($('#Programas_Selected').val() == 3) {
                    $("#ComboTiposPpp_Selected").show();
                    $("#lblTipoPpp").show();
                }
                else {
                    $("#ComboTiposPpp_Selected").hide();
                    $("#ComboTiposPpp_Selected").val(0);
                    $("#lblTipoPpp").hide();
                }


            }
            else {

                //******************************************************

                $.ajax({
                    type: "POST",
                    url: "/Subprogramas/SubprogramasProg",
                    data: "{idPrograma:'" + $("#Programas_Selected").val() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        if (data != null) {

                            var options = '';
                            var cbo = data;
                            $.each(cbo, function (index, obj) {
                                options += '<option value="' + obj.IdSubprograma + '">' + obj.NombreSubprograma + '</option>';
                            });

                            $("#ComboSubprogramas_Selected").html(options);
                            $('#idSubprograma').val($('#ComboSubprogramas_Selected').val());
                        }
                        else {

                        }
                    },
                    error: function (msg) {
                        alert(msg);
                    }
                });
                //******************************************************

                $("#ComboSubprogramas_Selected").show();
                $("#lblSubprogramas").show();
                if ($('#ComboSubprogramas_Selected').val() > 0) {
                    $('#idSubprograma').val($('#ComboSubprogramas_Selected').val());
                } else { $('#idSubprograma').val(0); }

                if ($('#Programas_Selected').val() == 3) {
                    $("#ComboTiposPpp_Selected").show();
                    $("#lblTipoPpp").show();
                }
                else {
                    $("#ComboTiposPpp_Selected").hide();
                    $("#ComboTiposPpp_Selected").val(0);
                    $("#lblTipoPpp").hide();
                }

            }
        }
        //*****************

    });

    $('#ComboSubprogramas_Selected').change('change', function () {

        if ($('#ComboSubprogramas_Selected').val() > 0) {
            $('#idSubprograma').val($('#ComboSubprogramas_Selected').val());
        }
        else
        { $('#idSubprograma').val($('#ComboSubprogramas_Selected').val()); }
    });

    $('#dialog_SelPrograma').dialog({
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


    if ($('#Accion').val() != "Ver")
    { $(".Accion").removeAttr("disabled"); }
    else
    { $('#btnGuardar').hide(); }

    $('#dialog').dialog({
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

    $('#dialogErrorDatos').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                $('#ErrorDatos').val('False');
                $(this).dialog("close");
            }

        }
    });


    $('#dialogFechaSolicitud').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                $('#FormularioBeneficiario').validate().cancelSubmit = true;
                $('#FormularioBeneficiario').attr('action', '/Beneficiarios/LimpiarFechaSolucitud');
                $("#FormularioBeneficiario").submit()
            },
            "Cancelar": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConfirmar').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                $('#FormArchivos').attr('action', '/Beneficiarios/LimpiarFecha');
                $("#FormArchivos").submit()
            },
            "Cancelar": function () {
                $(this).dialog("close");
            }
        }
    });


    $('#dialogFechaSolicitudResultado').dialog({
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



    $('#LimpiarFecha').click('click', function () {
        $('#dialogFechaSolicitud').dialog("open");
    });


    if ($('#Proceso').val() == "True")
    { $('#dialog').dialog("open"); }

    if ($('#ClearFechaSolicitud').val() == "True") {
        $('#dialogFechaSolicitudResultado').dialog("open");
    }


    if ($('#HdTieneApoderado').val() == "true" || $('#HdTieneApoderado').val() == "True") {
        $("#NumeroCuenta").attr("disabled", "disabled");
        $("#Cbu").attr("disabled", "disabled");
        $("#Sucursales_Selected").attr("disabled", "disabled");
    }
    else {
        $("#NumeroCuenta").removeAttr("disabled");
        $("#Cbu").removeAttr("disabled");
        $("#Sucursales_Selected").removeAttr("disabled");
    }

    $('#TieneApoderado').change(function () {

        if ($("#TieneApoderado").is(":checked")) {
            $("#NumeroCuenta").attr("disabled", "disabled");
            $("#Cbu").attr("disabled", "disabled");
            $("#Sucursales_Selected").attr("disabled", "disabled");

        }
        else {
            $("#NumeroCuenta").removeAttr("disabled");
            $("#Cbu").removeAttr("disabled");
            $("#Sucursales_Selected").removeAttr("disabled");
        }


    });

    if ($('#SeGuardo').val() == "True")
    { $('#dialogFechaSolicitudResultado').dialog("open"); }

    if ($('#ErrorDatos').val() == "True")
    { $('#dialogErrorDatos').dialog("open"); }

    $("#Monedas_Selected").attr("disabled", "disabled");



    $('#procesando').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        //        buttons: {
        //            "Ok": function () {
        //                $('#ErrorDatos').val('False');
        //                $(this).dialog("close");
        //            }
        //        }
        closeOnEscape: false,
        beforeclose: function (event, ui) { return false; } //,
        //dialogClass: "noclose"

    });


    //$('#GenerarFile').click('click', function () {
    //    $('#procesando').dialog({ beforeclose: function (event, ui) { return false; } });
    //    $('#procesando').dialog("open");

    //    if ($('#Programas_Selected').val() != 0) {
    //    $.ajax({
    //        type: "POST",
    //        url: "/Beneficiarios/GenerarFile",
    //        data:   "{NombreBusqueda:'"+$('#NombreBusqueda').val() +
    //                "',ApellidoBusqueda:'" + $('#ApellidoBusqueda').val() +
    //                "',CuilBusqueda:'" + $('#CuilBusqueda').val() +
    //                "',NumeroDocumentoBusqueda:'" + $('#NumeroDocumentoBusqueda').val() +
    //                "',Programas:" + $('#Programas_Selected').val() +
    //                ",ConApoderado:'T'" +
    //                ",ConModalidad:'T'" +
    //                ",ConDiscapacidad:'T'" +
    //                ",ConAltaTemprana:'T'" +
    //                ",EstadosBeneficiarios:" + $('#EstadosBeneficiarios_Selected').val() + "}",
    //        contentType: "application/json; charset=utf-8",
    //        dataType: "text",//"json",
    //        success: function (data) {
    //            $.fileDownload(data);
    //            $('#procesando').dialog({ beforeclose: function (event, ui) { return true; } });
    //            $('#procesando').dialog("Close");
    //        },
    //        error: function (msg) {
    //            $('#procesando').dialog({ beforeclose: function (event, ui) { return true; } });
    //            
    //            $('#procesando').dialog("Close");
    //            alert(msg);
    //        }
    //    });
    //    
    //        }
    //else {
    //            $('#dialog').dialog("open");
    //        }
    //    });



    $('#TraerEtapa').click('click', function () {
        var f = $("#FormIndexBeneficiarios");
        //$('#FormularioEtapa').validate().cancelSubmit = true;
        $("#programaSel").val($("#EtapaProgramas_Selected").val());
        $("#filtroEjerc").val($("#ejerc").val());
        $("#BuscarPorEtapa").val("S");
        $('#FormIndexBeneficiarios').attr('action', '/Beneficiarios/IndexTraerEtapas');
        $("#FormIndexBeneficiarios").submit()
    });

    // 17/06/2013 - Quita el filtro de etapas
    $('#QuitarEtapa').click('click', function () {
        var f = $("#FormIndexBeneficiarios");
        //$('#FormularioEtapa').validate().cancelSubmit = true;
        $("#BuscarPorEtapa").val("N");
        $('#FormIndexBeneficiarios').attr('action', '/Beneficiarios/Index');
        $("#FormIndexBeneficiarios").submit()
    });



    //FIN READY
});



