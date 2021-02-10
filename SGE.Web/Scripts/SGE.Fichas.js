$(document).ready(function () {

    $('.input-validation-error').removeClass("input-validation-error");
    $('.field-validation-error').addClass("field-validation-valid");
    $("#Cuil").mask("99-99999999-9");
    $("#CuitEmpresa").mask("99-99999999-9");
    $("#FichaPpp_CuitEmpresa").mask("99-99999999-9");
    $("#FichaVat_CuitEmpresa").mask("99-99999999-9");
    $("#FichaPppp_CuitEmpresa").mask("99-99999999-9");
    $("#FichaReconversion_CuitEmpresa").mask("99-99999999-9");
    $("#FichaEfectores_CuitEmpresa").mask("99-99999999-9");
    $("#ABMEmpresaCuit").mask("99-99999999-9");
    $("#cuil_busqueda").mask("99-99999999-9");
    $("#NumeroDocumento").mask("99999999");
    $("#nro_doc_busqueda").mask("99999999");

    $("#FichaConfVos_Apo_Cuil").mask("99-99999999-9");
    $("#FichaConfVos_CuitEmpresa").mask("99-99999999-9");
    //$("#FichaConfVos_curso_NRO_COND_CURSO").mask("99999999");
    $("#FichaConfVos_ong_CUIT").mask("99-99999999-9");
    $("#FichaPpp_ong_CUIT").mask("99-99999999-9");
    $("#ABMONGCUIT").mask("99-99999999-9");

    //$("#nrotramite_busqueda").mask("9999999999999999999999999");

    $('#Paginador').smartpaginator({ totalrecords: $('#ContFichas').val()
, recordsperpage: 10, datacontainer: 'tblFichas', dataelement: 'tr',
        initval: 0, next: 'Next', prev: 'Prev', first: 'First', last: 'Last', theme: 'green'
    });

    var dates = $("#FichaPpp_FechaInicioActividad,#FichaPpp_FechaFinActividad").datepicker({
        changeMonth: true,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 2,
        onSelect: function (selectedDate) {
            var option = this.id == "FichaPpp_FechaFinActividad" ? "maxDate" : "minDate",
					instance = $(this).data("datepicker"),
					date = $.datepicker.parseDate(
						instance.settings.dateFormat ||
						$.datepicker._defaults.dateFormat,
						selectedDate, instance.settings);
            dates.not(this).datepicker("option", option, date);
        }
    });

    var dates = $("#FichaVat_FechaInicioActividad,#FichaVat_FechaFinActividad").datepicker({
        changeMonth: true,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 2,
        onSelect: function (selectedDate) {
            var option = this.id == "FichaVat_FechaFinActividad" ? "maxDate" : "minDate",
					instance = $(this).data("datepicker"),
					date = $.datepicker.parseDate(
						instance.settings.dateFormat ||
						$.datepicker._defaults.dateFormat,
						selectedDate, instance.settings);
            dates.not(this).datepicker("option", option, date);
        }
    });

    var dates = $("#FichaPppp_FechaInicioActividad,#FichaPppp_FechaFinActividad").datepicker({
        changeMonth: true,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 2,
        onSelect: function (selectedDate) {
            var option = this.id == "FichaPppp_FechaFinActividad" ? "maxDate" : "minDate",
					instance = $(this).data("datepicker"),
					date = $.datepicker.parseDate(
						instance.settings.dateFormat ||
						$.datepicker._defaults.dateFormat,
						selectedDate, instance.settings);
            dates.not(this).datepicker("option", option, date);
        }
    });

    var dates = $("#FichaReconversion_FechaInicioActividad,#FichaReconversion_FechaFinActividad").datepicker({
        changeMonth: true,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 2,
        onSelect: function (selectedDate) {
            var option = this.id == "FichaReconversion_FechaFinActividad" ? "maxDate" : "minDate",
					instance = $(this).data("datepicker"),
					date = $.datepicker.parseDate(
						instance.settings.dateFormat ||
						$.datepicker._defaults.dateFormat,
						selectedDate, instance.settings);
            dates.not(this).datepicker("option", option, date);
        }
    });

    var dates = $("#FichaEfectores_FechaInicioActividad,#FichaEfectores_FechaFinActividad").datepicker({
        changeMonth: true,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 2,
        onSelect: function (selectedDate) {
            var option = this.id == "FichaEfectores_FechaFinActividad" ? "maxDate" : "minDate",
					instance = $(this).data("datepicker"),
					date = $.datepicker.parseDate(
						instance.settings.dateFormat ||
						$.datepicker._defaults.dateFormat,
						selectedDate, instance.settings);
            dates.not(this).datepicker("option", option, date);
        }
    });

    // 18/02/2017

    var dates = $("#ABMCURSOF_INICIO,#ABMCURSOF_FIN").datepicker({
        changeMonth: true,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 2,
        onSelect: function (selectedDate) {
            var option = this.id == "ABMCURSOF_FIN" ? "maxDate" : "minDate",
					instance = $(this).data("datepicker"),
					date = $.datepicker.parseDate(
						instance.settings.dateFormat ||
						$.datepicker._defaults.dateFormat,
						selectedDate, instance.settings);
            dates.not(this).datepicker("option", option, date);
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


    $('#dialog_convertir').dialog({
        autoOpen: false,
        width: 350,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                $('#FormularioFichas').attr('action', '/Fichas/ConvertirFicha');
                $('#idEtapa').val($('#SelEtapa').val());
                $("#FormularioFichas").submit();
                $(this).dialog("close");
            },
            "Cancel": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialog_cambio_estado').dialog({
        autoOpen: false,
        width: 350,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                $('#FormularioFichas').validate().cancelSubmit = true;
                $('#FormularioFichas').attr('action', '/Fichas/ModificarFicha');
                $("#FormularioFichas").submit();
                $(this).dialog("close");
            },
            "Cancel": function () {
                $(this).dialog("close");
            }
        }
    });


    $('#dialogFichaResultado').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                $('#strMessageUpdate').val("");
                $(this).dialog("close");
            }
        }
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

    $('#dialogDesvincularSubP').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {

                //Fichas Ppp-------------------------
                $("#IdSubprograma").val("");
                if ($('#TipoFicha').val() == 3) {

                    $("#FichaPpp_Subprograma").val("");
                    $("#FichaPpp_monto").val("");
                }
                else {

                    $("#Subprograma").val("");
                    $("#monto").val("");
                }

                //-------------------------------------------------
                //-------------------------------------------------
                $(this).dialog("close");
            },
            "Cancelar": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogDesvincularEsc').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {

                //Fichas Ppp-------------------------
                $("#FichaPpp_Id_Escuela").val("");
                $("#FichaPpp_N_ESCUELA").val("");
                $("#FichaPpp_CALLEEsc").val("");
                $("#FichaPpp_NUMEROEsc").val("");
                $("#FichaPpp_TELEFONOEsc").val("");
                $("#FichaPpp_CueEsc").val("");
                //-------------------------------------------------
                //-------------------------------------------------
                $(this).dialog("close");
            },
            "Cancelar": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogDesvincularEmp').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                $('#IdEmpresa').val("");
                //Fichas CONFIAMOS EN VOS-------------------------
                $('#FichaConfVos_DescripcionEmpresa').val("");
                $('#FichaConfVos_Empresa_NombreLocalidad').val("");
                $('#FichaConfVos_Empresa_CodigoActividad').val("");
                $('#FichaConfVos_Empresa_CantidadEmpleados').val("");
                $('#FichaConfVos_Empresa_Calle').val("");
                $('#FichaConfVos_Empresa_Numero').val("");
                $('#FichaConfVos_Empresa_Piso').val("");
                $('#FichaConfVos_Empresa_Dpto').val("");
                $('#FichaConfVos_Empresa_CodigoPostal').val("");
                $('#FichaConfVos_Empresa_DomicilioLaboralIdem').val("");
                $('#FichaConfVos_CuitEmpresa').val("");
                $('#benefActivosEmp').val(0);
                //-------------------------------------------------
                //-------------------------------------------------
                $(this).dialog("close");
            },
            "Cancelar": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogDesvincularOng').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {

                //Fichas CONFIAMOS EN VOS-------------------------
                $("#FichaConfVos_ID_ONG").val("");
                $("#FichaConfVos_ong_CUIT").val("");
                $("#FichaConfVos_ong_N_NOMBRE").val("");
                $("#FichaConfVos_ong_CELULAR").val("");
                $("#FichaConfVos_ong_TELEFONO").val("");
                $("#FichaConfVos_ong_MAIL_ONG").val("");
                $("#FichaConfVos_ong_RES_APELLIDO").val("");
                $("#FichaConfVos_ong_RES_NOMBRE").val("");
                $("#FichaConfVos_ong_RES_CELULAR").val("");
                $("#FichaConfVos_ong_RES_N_LOCALIDAD").val("");
                $("#FichaConfVos_ong_RES_MAIL").val("");
                $("#FichaConfVos_ong_CON_APELLIDO").val("");
                $("#FichaConfVos_ong_CON_NOMBRE").val("");
                $("#FichaConfVos_ong_CON_CELULAR").val("");
                $("#FichaConfVos_ong_CON_N_LOCALIDAD").val("");
                $("#FichaConfVos_ong_CON_MAIL").val("");
                //Fichas PPP-------------------------
                $("#FichaPpp_ID_ONG").val("");
                $("#FichaPpp_ong_CUIT").val("");
                $("#FichaPpp_ong_N_NOMBRE").val("");
                $("#FichaPpp_ong_CELULAR").val("");
                $("#FichaPpp_ong_TELEFONO").val("");
                $("#FichaPpp_ong_MAIL_ONG").val("");
                $("#FichaPpp_ong_RES_APELLIDO").val("");
                $("#FichaPpp_ong_RES_NOMBRE").val("");
                $("#FichaPpp_ong_RES_CELULAR").val("");
                $("#FichaPpp_ong_RES_N_LOCALIDAD").val("");
                $("#FichaPpp_ong_RES_MAIL").val("");
                $("#FichaPpp_ong_CON_APELLIDO").val("");
                $("#FichaPpp_ong_CON_NOMBRE").val("");
                $("#FichaPpp_ong_CON_CELULAR").val("");
                $("#FichaPpp_ong_CON_N_LOCALIDAD").val("");
                $("#FichaPpp_ong_CON_MAIL").val("");
                //-------------------------------------------------
                //-------------------------------------------------
                $(this).dialog("close");
            },
            "Cancelar": function () {
                $(this).dialog("close");
            }
        }
    });


    $('#dialogDesvincularGestor').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {

                //Fichas CONFIAMOS EN VOS-------------------------
                $("#FichaConfVos_ID_GESTOR").val("");
                $("#FichaConfVos_gestor_DNI").val("");
                $("#FichaConfVos_gestor_APELLIDO").val("");
                $("#FichaConfVos_gestor_NOMBRE").val("");
                $("#FichaConfVos_gestor_CELULAR").val("");
                $("#FichaConfVos_gestor_MAIL").val("");
                $("#FichaConfVos_gestor_N_BARRIO").val("");
                $("#FichaConfVos_gestor_N_LOCALIDAD").val("");
                //-------------------------------------------------
                //-------------------------------------------------
                $(this).dialog("close");
            },
            "Cancelar": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogDesvincularFacMon').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {

                //Fichas CONFIAMOS EN VOS-------------------------
                $("#FichaConfVos_ID_FACILITADOR").val("");
                $("#FichaConfVos_facilitador_monitor_DNI").val("");
                $("#FichaConfVos_facilitador_monitor_APELLIDO").val("");
                $("#FichaConfVos_facilitador_monitor_NOMBRE").val("");
                $("#FichaConfVos_facilitador_monitor_CELULAR").val("");
                $("#FichaConfVos_facilitador_monitor_MAIL").val("");
                $("#FichaConfVos_facilitador_monitor_N_BARRIO").val("");
                $("#FichaConfVos_facilitador_monitor_N_LOCALIDAD").val("");
                //-------------------------------------------------
                //-------------------------------------------------
                $(this).dialog("close");
            },
            "Cancelar": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogDesvincularCurso').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {

                //Fichas CONFIAMOS EN VOS-------------------------
                $("#FichaConfVos_ID_CURSO").val("");
                $("#FichaConfVos_curso_NRO_COND_CURSO").val("");
                $("#FichaConfVos_curso_N_CURSO").val("");
                $("#FichaConfVos_curso_F_INICIO").val("");
                $("#FichaConfVos_curso_F_FIN").val("");
                $("#FichaConfVos_curso_DURACION").val("");
                $("#FichaConfVos_curso_EXPEDIENTE_LEG").val("");
                $("#FichaConfVos_curso_DOC_APELLIDO").val("");
                $("#FichaConfVos_curso_DOC_NOMBRE").val("");
                $("#FichaConfVos_curso_DOC_CELULAR").val("");
                $("#FichaConfVos_curso_DOC_N_LOCALIDAD").val("");
                $("#FichaConfVos_curso_DOC_MAIL").val("");
                $("#FichaConfVos_curso_TUT_APELLIDO").val("");
                $("#FichaConfVos_curso_TUT_NOMBRE").val("");
                $("#FichaConfVos_curso_TUT_CELULAR").val("");
                $("#FichaConfVos_curso_TUT_N_LOCALIDAD").val("");
                $("#FichaConfVos_curso_TUT_MAIL").val("");
                //-------------------------------------------------
                //Fichas Ppp-------------------------
                $("#FichaPpp_ID_CURSO").val("");
                $("#FichaPpp_cursoPpp_NRO_COND_CURSO").val("");
                $("#FichaPpp_cursoPpp_N_CURSO").val("");
                $("#FichaPpp_cursoPpp_F_INICIO").val("");
                $("#FichaPpp_cursoPpp_F_FIN").val("");
                $("#FichaPpp_cursoPpp_DURACION").val("");
                //    $("#FichaPpp_curso_EXPEDIENTE_LEG").val("");
                $("#FichaPpp_cursoPpp_Meses").val("");
                $("#FichaPpp_cursoPpp_NEscuelaCurso").val("");
                $("#FichaPpp_cursoPpp_CueCurso").val("");
                $("#FichaPpp_cursoPpp_regiseCurso").val("");

                //-------------------------------------------------
                //-------------------------------------------------
                $(this).dialog("close");
            },
            "Cancelar": function () {
                $(this).dialog("close");
            }
        }
    });


    if ($('#strMessageUpdate').val() != "") {
        $('#dialogFichaResultado').dialog("open");
        $('#strMessageUpdate').val("");
    }


    if ($('#ProcesarExcel').val() == "True") {
        $('#dialog').dialog("open");
    }

    if ($('#CambioTipo').val() == "True") {//22/01/2013 -Req. Cambio de becas Univ. a Terciaria y viceversa, o guarda la conversion-
        $('#CambioTipo').val('False');  // $('#CambioTipo').val('False') - Se añade para que no informe 
        $('#dialog').dialog("open");    //que los cambios se los cambios se realizaron correctamente cuando se vuelve 
    }                                   //a recargar el formulario de fichas. Di Campli Leandro

    $("#accordion").accordion();

    $('#Agregar').click('click', function () {
        if (ValidarFechaInicioFin()) {
            $('#FormularioFichas').attr('action', '/Fichas/AgregarFicha');
            $("#FormularioFichas").submit()
        }

    });

    $('#VerObservacion').click('click', function () {
        if (ValidarFechaInicioFin()) {
            $('#FormularioFichas').attr('action', '/Fichas/VerObservaciones');
            $("#FormularioFichas").submit()
        }

    });

    $('#Cancelar').click('click', function () {
        $('#FormularioFichas').validate().cancelSubmit = true;
        $('#FormularioFichas').attr('action', '/Fichas/Cancelar');
        $("#FormularioFichas").submit()
    });


    $('#Buscar').click('click', function () {
        var f = $("#FormIndexFichas");
        //        if ($("#ProgPorRol").val() == 'S') {
        //            if ($("#ComboTipoFicha_Selected").val() == '-1') {

        //                $("#dialog_SelPrograma").dialog("open");
        //               return false;
        //            }

        //        }
        $('#FormIndexFichas').attr('action', '/Fichas/BuscarFichas');
        $("#FormIndexFichas").submit()
    });

    $('#convertirunvterc').click('click', function () {


        $.ajax({
            type: "POST",
            url: "/Fichas/TraerEtapas",
            data: "{idPrograma:'" + $("#TipoFicha").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data != null) {


                    var options = data;

                    $("#dialog_convertir").html("Seleccione la etapa:" + options);
                    $('#dialog_convertir').dialog("open");

                }
                else {
                    //me da opcion para agregarla

                }
            },
            error: function (msg) {
                alert(msg);
            }
        });



    });



    $('#btnProcesarExcel').click('click', function () {
        var f = $("#FormImportarExcel");
        $('#FormImportarExcel').attr('action', '/Fichas/ProcesarExcel');
        $("#FormImportarExcel").submit()
    });

    $('#Importar').click('click', function () {
        var f = $("#FormIndexFichas");
        $('#FormIndexFichas').attr('action', '/Fichas/ImportarExcel');
        $("#FormIndexFichas").submit()
    });

    $("#Departamentos_Selected").change(function () {
        var f = $("#FormularioFichas");
        $('#FormularioFichas').validate().cancelSubmit = true;
        $('#FormularioFichas').attr('action', '/Fichas/CambiarCombo');
        $("#FormularioFichas").submit()
    });

    // 06/05/204 - Departamentos de confiamos en vos para apoderados
    $("#DepartamentosApo_Selected").change(function () {
        var f = $("#FormularioFichas");
        $('#FormularioFichas').validate().cancelSubmit = true;
        $('#FormularioFichas').attr('action', '/Fichas/CambiarCombo');
        $("#FormularioFichas").submit()
    });

    $("#SectorProductivo_Selected").change(function () {
        var f = $("#FormularioFichas");
        $('#FormularioFichas').validate().cancelSubmit = true;
        $('#FormularioFichas').attr('action', '/Fichas/CambiarCombo');
        $("#FormularioFichas").submit()
    });

    $("#Institucion_Selected").change(function () {
        var f = $("#FormularioFichas");
        $('#FormularioFichas').validate().cancelSubmit = true;
        $('#FormularioFichas').attr('action', '/Fichas/CambiarCombo');
        $("#FormularioFichas").submit()
    });

    $("#ComboTitulos_Selected").change(function () {
        $('#FichaPppp_IdTitulo').val($("#ComboTitulos_Selected").val());
    });

    $("#ComboUniversidad_Selected").change(function () {
        var f = $("#FormularioFichas");
        $('#FormularioFichas').validate().cancelSubmit = true;
        $('#FormularioFichas').attr('action', '/Fichas/CambiarCombo');
        $("#FormularioFichas").submit()
    });

    if ($("#Localidades_Selected").val() == 25) {
        //alert($("#Localidades_Selected").val());
        $.ajax({
            type: "POST",
            url: "/Barrios/BarrioLocalidad",
            data: "{idLocalidad:'" + $("#Localidades_Selected").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data != null) {

                    var options = '';
                    var cbo = data;
                    $.each(cbo, function (index, obj) {
                        options += '<option value="' + obj.ID_BARRIO + '">' + obj.N_BARRIO + '</option>';
                    });
                    //alert($('#idBarrio').val());
                    $("#comboBarrios_Selected").html(options);
                    $('#comboBarrios_Selected').val($('#idBarrio').val());
                    if ($("#Accion").val() == "Ver" || $("#Accion").val() == "CambiarEstado") {
                        $("#comboBarrios_Selected").attr("disabled", "disabled");
                    } else { $("#comboBarrios_Selected").removeAttr("disabled"); }
                    $('#Barrio').val("");
                    $('#Barrio').hide();
                    $('#lblBarrio').hide();
                }
                else {
                    $('#idBarrio').val("0");
                    $("#comboBarrios_Selected").html("");
                    $("#comboBarrios_Selected").attr("disabled", "disabled");
                    $('#Barrio').show();
                    $('#lblBarrio').show();
                }
            },
            error: function (msg) {
                alert(msg);
            }
        });
        //******************************************************
    }
    else {
        $('#idBarrio').val("0");
        $("#comboBarrios_Selected").html("");
        $("#comboBarrios_Selected").attr("disabled", "disabled");
        $('#Barrio').show();
        $('#lblBarrio').show();
    }



    $("#Localidades_Selected").change(function () {
        var f = $("#FormularioFichas");
        //******************************************************
        if ($("#Localidades_Selected").val() == 25) {
            $.ajax({
                type: "POST",
                url: "/Barrios/BarrioLocalidad",
                data: "{idLocalidad:'" + $("#Localidades_Selected").val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data != null) {

                        var options = '';
                        var cbo = data;
                        $.each(cbo, function (index, obj) {
                            options += '<option value="' + obj.ID_BARRIO + '">' + obj.N_BARRIO + '</option>';
                        });

                        $("#comboBarrios_Selected").html(options);
                        $('#idBarrio').val($('#comboBarrios_Selected').val());
                        $("#comboBarrios_Selected").removeAttr("disabled");
                        $('#Barrio').val("");
                        $('#Barrio').hide();
                        $('#lblBarrio').hide();
                    }
                    else {

                    }
                },
                error: function (msg) {
                    alert(msg);
                }
            });
            //******************************************************
        }
        else {

            $("#comboBarrios_Selected").html("");
            $("#comboBarrios_Selected").attr("disabled", "disabled");
            $('#idBarrio').val("0");
            $('#Barrio').val("");
            $('#Barrio').show();
            $('#lblBarrio').show();
        }

    });

    $("#comboBarrios_Selected").change(function () {

        $('#idBarrio').val($('#comboBarrios_Selected').val());
        $('#Barrio').val($('#comboBarrios_Selected option:selected').text());


    });
    //    $("#DepartamentosEscuela_Selected").change(function () {
    //        var f = $("#FormularioFichas");
    //        if ($('#TipoFicha').val()!=8) 
    //        {
    //            $('#FormularioFichas').validate().cancelSubmit = true;
    //            $('#FormularioFichas').attr('action', '/Fichas/CambiarCombo');
    //            $("#FormularioFichas").submit()
    //        }
    //    });

    //    $("#LocalidadesEscuela_Selected").change(function () {
    //        var f = $("#FormularioFichas");
    //        $('#FormularioFichas').validate().cancelSubmit = true;
    //        $('#FormularioFichas').attr('action', '/Fichas/CambiarCombo');
    //        $("#FormularioFichas").submit()
    //    });

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

    $('#Estado_Selected').change('change', function () {

        if ($('#Estado_Selected').val() == 4) {
            $('#trTipoRechazo').attr('style', 'display:block');
        }
        else {
            $('#trTipoRechazo').attr('style', 'display:none');
        }
    });

    $('#cambiarEstado').click('click', function () {
        $('#dialog_cambio_estado').dialog("open");
    });

    $('#ComboEstadoFicha_Selected').change('change', function () {

        if ($('#ComboEstadoFicha_Selected').val() == 4) {
            $('#trTipoRechazo').attr('style', 'display:block');
        }
        else {
            $('#trTipoRechazo').attr('style', 'display:none');
        }
    });

    $('#ComboSubprogramas_Selected').change('change', function () {

        if ($('#ComboSubprogramas_Selected').val() > 0) {
            $('#idSubprograma').val($('#ComboSubprogramas_Selected').val());
        }
        else
        { $('#idSubprograma').val($('#ComboSubprogramas_Selected').val()); }
    });

    //    $("#ComboTiposPpp_Selected").change(function () {

    //        $('#tipoPrograma').val($("#ComboTiposPpp_Selected").val());

    //    });

    $("#ComboTiposPpp_Selected").change(function () {
        //alert("Test");
        $('#tipoPrograma').val($("#ComboTiposPpp_Selected").val());
        var f = $("#" + this.form.id);
        if (this.form.id == 'FormularioFichas') {
            if ($('#Accion').val() == "Agregar") {
                //$('#FormularioFichas').validate().cancelSubmit = true;
                $('#FormularioFichas').attr('action', '/Fichas/CambiarCombo');
                $("#TipoFicha").val($("#ComboTipoFicha_Selected").val());
                $("#FormularioFichas").submit();
            }
        }

    });

    if ($("#InstitucionSector_Selected").val() == 0) {
        $("#CarreraSector_Selected").attr("disabled", "disabled");
        $("#n_cursado_ins").removeAttr("disabled");
        $("#n_descripcion_t").removeAttr("disabled");
    }
    $("#CarreraSector_Selected").css('width', '600px');
    $("#InstitucionSector_Selected").css('width', '600px');
    $("#InstitucionSector_Selected").change(function () {
        var f = $("#FormularioFichas");
        //******************************************************
        if ($("#InstitucionSector_Selected").val() != 0) {
            $.ajax({
                type: "POST",
                url: "/Carreras/GetCarreras",
                data: "{idInstitucion:'" + $("#InstitucionSector_Selected").val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data != null) {

                        var options = '';
                        var cbo = data;
                        options += '<option value="0">Seleccione una Carrera</option>';
                        $.each(cbo, function (index, obj) {

                            options += '<option value="' + obj.IdCarrera + '">' + obj.NombreCarrera + '</option>';
                        });

                        $("#CarreraSector_Selected").html(options);
                        $('#FichaPpp_id_carrera').val($('#CarreraSector_Selected').val());
                        $("#CarreraSector_Selected").removeAttr("disabled");
                        $("#FichaPpp_n_cursado_ins").attr("disabled", "disabled");
                        $("#FichaPpp_n_descripcion_t").attr("disabled", "disabled");
                        $('#FichaPpp_n_cursado_ins').val("");
                        $('#FichaPpp_n_descripcion_t').val("");
                        $("#CarreraSector_Selected").css('width', '600px');
                        //$('#Barrio').hide();
                        //$('#lblBarrio').hide();
                    }
                    else {

                    }
                },
                error: function (msg) {
                    alert(msg);
                }
            });
            //******************************************************
        }
        else {
            //habilitar los controles y deshabilitar carreras
            $("#CarreraSector_Selected").html('<option value="0">Seleccione una Carrera</option>');
            $("#CarreraSector_Selected").attr("disabled", "disabled");
            $("#FichaPpp_n_cursado_ins").removeAttr("disabled");
            $("#FichaPpp_n_descripcion_t").removeAttr("disabled");
            $('#FichaPpp_id_carrera').val("0");
        }

    });


    $("#CarreraSector_Selected").change(function () {
        $('#FichaPpp_id_carrera').val($('#CarreraSector_Selected').val());
    });

    $("#ComboTipoFicha_Selected").change(function () {
        var f = $("#" + this.form.id);
        if (this.form.id == 'FormularioFichas') {
            //$('#FormularioFichas').validate().cancelSubmit = true;
            $('#FormularioFichas').attr('action', '/Fichas/CambiarCombo');
            $("#TipoFicha").val($("#ComboTipoFicha_Selected").val());
            $("#FormularioFichas").submit();

        }
        else if (this.form.id == 'FormIndexFichas') {
            var idTipo = $("#ComboTipoFicha_Selected").val();
            if ($("#ProgPorRol").val() == "S") //verificar si tiene la acción activa para consultar los subprogramas
            {
                //******************************************************

                $.ajax({
                    type: "POST",
                    url: "/Subprogramas/SubprogramasRoles",
                    data: "{idPrograma:'" + $("#ComboTipoFicha_Selected").val() + "'}",
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

                if ($('#ComboTipoFicha_Selected').val() == 3) {
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
                    data: "{idPrograma:'" + $("#ComboTipoFicha_Selected").val() + "'}",
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

                if ($('#ComboTipoFicha_Selected').val() == 3) {
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
    });

    /* Fichas PPP, VAT, PPP PROF, RECONVERSION PRODUCTIVA, EFECTORES SOCIALES*/

    $(".ClassNivelEscolaridad").click(function () {
        $("#FichaPpp_IdNivelEscolaridad").val($('input[name=NivelEscolaridad]:checked').val());
        $("#FichaVat_IdNivelEscolaridad").val($('input[name=NivelEscolaridad]:checked').val());
        $("#FichaReconversion_Id_Nivel_Escolaridad").val($('input[name=NivelEscolaridad]:checked').val());
        $("#FichaEfectores_Id_Nivel_Escolaridad").val($('input[name=NivelEscolaridad]:checked').val());
        $("#FichaConfVos_IdNivelEscolaridad").val($('input[name=NivelEscolaridad]:checked').val());
    });

    $(".ClassNivelClip").click(function () {
        
        if ($('input[name=NivelEscolaridad]:checked').val() == 4) {

            $('#InstitucionSector_Selected').attr("disabled", "disabled");
            $('#BuscarEscuela').show(); //
            $('#DesvincularEscuela').show(); //
            $('#FichaPpp_N_ESCUELA').removeAttr("disabled");
        }
        else {

            $('#InstitucionSector_Selected').removeAttr("disabled");
            $('#BuscarEscuela').hide(); //attr("disabled", "disabled");
            $('#DesvincularEscuela').hide();
            $('#FichaPpp_N_ESCUELA').attr("disabled", "disabled");

        }
        $("#FichaPpp_IdNivelEscolaridad").val($('input[name=NivelEscolaridad]:checked').val());
        $("#FichaVat_IdNivelEscolaridad").val($('input[name=NivelEscolaridad]:checked').val());
        $("#FichaReconversion_Id_Nivel_Escolaridad").val($('input[name=NivelEscolaridad]:checked').val());
        $("#FichaEfectores_Id_Nivel_Escolaridad").val($('input[name=NivelEscolaridad]:checked').val());
        $("#FichaConfVos_IdNivelEscolaridad").val($('input[name=NivelEscolaridad]:checked').val());
    });

    $(".ClassModalidad").click(function () {
        if ($('input[name=Modalidad]:checked').val() == 1) {
            $(".ClassAltaTemprana").attr("disabled", "disabled");
            $(".ClassAltaTemprana").attr("checked", true);
            $("#ComboModalidadAfip_Selected").attr("disabled", "disabled");
            $("#FichaPppp_FechaInicioActividad").attr("disabled", "disabled");
            $("#FichaPppp_FechaFinActividad").attr("disabled", "disabled");
            $("#FichaVat_FechaInicioActividad").attr("disabled", "disabled");
            $("#FichaVat_FechaFinActividad").attr("disabled", "disabled");
            $("#FichaPpp_FechaInicioActividad").attr("disabled", "disabled");
            $("#FichaPpp_FechaFinActividad").attr("disabled", "disabled");
            $("#FichaReconversion_FechaInicioActividad").attr("disabled", "disabled");
            $("#FichaReconversion_FechaFinActividad").attr("disabled", "disabled");
            $("#FichaEfectores_FechaInicioActividad").attr("disabled", "disabled");
            $("#FichaEfectores_FechaFinActividad").attr("disabled", "disabled");
            $("#FichaConfVos_FechaInicioActividad").attr("disabled", "disabled");
            $("#FichaConfVos_FechaFinActividad").attr("disabled", "disabled");
        }
        else {
            $(".ClassAltaTemprana").removeAttr("disabled");
            if ($('input[name=AltaTemprana]:checked').val() != "S") {

                $("#ComboModalidadAfip_Selected").attr("disabled", "disabled");
                $("#FichaPppp_FechaInicioActividad").attr("disabled", "disabled");
                $("#FichaPppp_FechaFinActividad").attr("disabled", "disabled");
                $("#FichaVat_FechaInicioActividad").attr("disabled", "disabled");
                $("#FichaVat_FechaFinActividad").attr("disabled", "disabled");
                $("#FichaPpp_FechaInicioActividad").attr("disabled", "disabled");
                $("#FichaPpp_FechaFinActividad").attr("disabled", "disabled");
                $("#FichaReconversion_FechaInicioActividad").attr("disabled", "disabled");
                $("#FichaReconversion_FechaFinActividad").attr("disabled", "disabled");
                $("#FichaEfectores_FechaInicioActividad").attr("disabled", "disabled");
                $("#FichaEfectores_FechaFinActividad").attr("disabled", "disabled");
                $("#FichaConfVos_FechaInicioActividad").attr("disabled", "disabled");
                $("#FichaConfVos_FechaFinActividad").attr("disabled", "disabled");
            }
            else {
                $("#ComboModalidadAfip_Selected").removeAttr("disabled");
                $("#FichaPppp_FechaInicioActividad").removeAttr("disabled");
                $("#FichaPppp_FechaFinActividad").removeAttr("disabled");
                $("#FichaVat_FechaInicioActividad").removeAttr("disabled");
                $("#FichaVat_FechaFinActividad").removeAttr("disabled");
                $("#FichaPpp_FechaInicioActividad").removeAttr("disabled");
                $("#FichaPpp_FechaFinActividad").removeAttr("disabled");
                $("#FichaReconversion_FechaInicioActividad").removeAttr("disabled");
                $("#FichaReconversion_FechaFinActividad").removeAttr("disabled");
                $("#FichaEfectores_FechaInicioActividad").removeAttr("disabled");
                $("#FichaEfectores_FechaFinActividad").removeAttr("disabled");
                $("#FichaConfVos_FechaInicioActividad").removeAttr("disabled");
                $("#FichaConfVos_FechaFinActividad").removeAttr("disabled");
            }
        }

        $("#FichaPpp_Modalidad").val($('input[name=Modalidad]:checked').val());
        $("#FichaVat_Modalidad").val($('input[name=Modalidad]:checked').val());
        $("#FichaPppp_Modalidad").val($('input[name=Modalidad]:checked').val());
        $("#FichaReconversion_Modalidad").val($('input[name=Modalidad]:checked').val());
        $("#FichaEfectores_Modalidad").val($('input[name=Modalidad]:checked').val());
        $("#FichaConfVos_Modalidad").val($('input[name=Modalidad]:checked').val());
    });

    $(".ClassCofinanciamiento").click(function () {

        $("#FichaPpp_co_financiamiento").val($('input[name=Cofinanciamiento]:checked').val());
    });

    $(".ClassAltaTemprana").click(function () {
        $("#FichaPpp_AltaTemprana").val($('input[name=AltaTemprana]:checked').val());
        $("#FichaVat_AltaTemprana").val($('input[name=AltaTemprana]:checked').val());
        $("#FichaPppp_AltaTemprana").val($('input[name=AltaTemprana]:checked').val());
        $("#FichaReconversion_AltaTemprana").val($('input[name=AltaTemprana]:checked').val());
        $("#FichaEfectores_AltaTemprana").val($('input[name=AltaTemprana]:checked').val());
        $("#FichaConfVos_AltaTemprana").val($('input[name=AltaTemprana]:checked').val());

        if ($('input[name=AltaTemprana]:checked').val() != "S") {

            $("#ComboModalidadAfip_Selected").attr("disabled", "disabled");
            $("#FichaPppp_FechaInicioActividad").attr("disabled", "disabled");
            $("#FichaPppp_FechaFinActividad").attr("disabled", "disabled");
            $("#FichaVat_FechaInicioActividad").attr("disabled", "disabled");
            $("#FichaVat_FechaFinActividad").attr("disabled", "disabled");
            $("#FichaPpp_FechaInicioActividad").attr("disabled", "disabled");
            $("#FichaPpp_FechaFinActividad").attr("disabled", "disabled");
            $("#FichaReconversion_FechaInicioActividad").attr("disabled", "disabled");
            $("#FichaReconversion_FechaFinActividad").attr("disabled", "disabled");
            $("#FichaEfectores_FechaInicioActividad").attr("disabled", "disabled");
            $("#FichaEfectores_FechaFinActividad").attr("disabled", "disabled");
            $("#FichaConfVos_FechaInicioActividad").attr("disabled", "disabled");
            $("#FichaConfVos_FechaFinActividad").attr("disabled", "disabled");

        }
        else {
            $("#ComboModalidadAfip_Selected").removeAttr("disabled");
            $("#FichaPppp_FechaInicioActividad").removeAttr("disabled");
            $("#FichaPppp_FechaFinActividad").removeAttr("disabled");
            $("#FichaVat_FechaInicioActividad").removeAttr("disabled");
            $("#FichaVat_FechaFinActividad").removeAttr("disabled");
            $("#FichaPpp_FechaInicioActividad").removeAttr("disabled");
            $("#FichaPpp_FechaFinActividad").removeAttr("disabled");
            $("#FichaReconversion_FechaInicioActividad").removeAttr("disabled");
            $("#FichaReconversion_FechaFinActividad").removeAttr("disabled");
            $("#FichaEfectores_FechaInicioActividad").removeAttr("disabled");
            $("#FichaEfectores_FechaFinActividad").removeAttr("disabled");
            $("#FichaConfVos_FechaInicioActividad").removeAttr("disabled");
            $("#FichaConfVos_FechaFinActividad").removeAttr("disabled");
        }
    });

    $(".Cursando").click(function () {
        $("#FichaPppCursando").val($('input[name=Cursando]:checked').val());
        $("#FichaVatCursando").val($('input[name=Cursando]:checked').val());
        $("#FichaReconversion").val($('input[name=Cursando]:checked').val());
        $("#FichaEfectores").val($('input[name=Cursando]:checked').val());
        $("#FichaConfVos").val($('input[name=Cursando]:checked').val());
    });



    // 22/04/2014 - DI CAMPLI LEANDRO
    $(".ClassAbandonoEscuela").click(function () {
        $("#FichaConfVos_Abandono_Escuela").val($('input[name=Abandono_Escuela]:checked').val());
    });

    $(".ClassCursa").click(function () {

        $("#FichaConfVos_cursa").val($('input[name=cursa]:checked').val());
    });

    $(".ClassTrabaja_Trabajo").click(function () {

        $("#FichaConfVos_Trabaja_Trabajo").val($('input[name=Trabaja_Trabajo]:checked').val());
    });

    $(".ClassPppAprendiz").click(function () {

        $("#FichaPpp_ppp_aprendiz").val($('input[name=ppp_aprendiz]:checked').val());
    });

    $(".ClassUltimo_Cursado").click(function () {

        $("#FichaConfVos_Ultimo_Cursado").val($('input[name=Ultimo_Cursado]:checked').val());
    });

    $(".Classpit").click(function () {

        $("#FichaConfVos_pit").val($('input[name=pit]:checked').val());
    });

    $(".ClassCentro_Adultos").click(function () {

        $("#FichaConfVos_Centro_Adultos").val($('input[name=Centro_Adultos]:checked').val());
    });

    $(".ClassProgresar").click(function () {

        $("#FichaConfVos_Progresar").val($('input[name=Progresar]:checked').val());
    });

    $(".ClassBenef_progre").click(function () {

        $("#FichaConfVos_Benef_progre").val($('input[name=Benef_progre]:checked').val());
    });

    $(".ClassActividades").click(function () {

        $("#FichaConfVos_Actividades").val($('input[name=Actividades]:checked').val());
    });

    $(".ClassAutoriza_Tutor").click(function () {

        $("#FichaConfVos_Autoriza_Tutor").val($('input[name=Autoriza_Tutor]:checked').val());
    });

    $(".ClassApoderado").click(function () {

        $("#FichaConfVos_Apoderado").val($('input[name=Apoderado]:checked').val());
        if ($("#FichaConfVos_Apoderado").val() == 'S')
        { $("#tbApoConfVos").show(); } else { $("#tbApoConfVos").hide(); }
    });

    $(".ClassApo_Tiene_Hijos").click(function () {

        $("#FichaConfVos_Apo_Tiene_Hijos").val($('input[name=Apo_Tiene_Hijos]:checked').val());

    });

    $(".ClassSENAF").click(function () {

        $("#SENAF").val($('input[name=CHKSENAF]:checked').val());
        //$("#FichaConfVos_SENAF").val($('input[name=SENAF]:checked').val());
    });

    $(".ClassCondicion").click(function () {

        $("#FichaConfVos_TRABAJO_CONDICION").val($('input[name=TRABAJO_CONDICION]:checked').val());
    });

    $(".ClassPerteneceONG").click(function () {

        $("#FichaConfVos_PERTENECE_ONG").val($('input[name=PERTENECE_ONG]:checked').val());
    });

    if ($("#FichaConfVos_Apoderado").val() != 'S') {
        $("#tbApoConfVos").hide();

    }

    $(".ClassHorRot").click(function () {

        $("#FichaPpp_horario_rotativo").val($('input[name=HorarioRotativo]:checked').val());
    });

    $(".ClassSedeRot").click(function () {

        $("#FichaPpp_sede_rotativa").val($('input[name=SedeRotativa]:checked').val());
    });
    // 04/08/2018 - NUEVOS CAMPOS
    $(".ClassCargaHoraria").click(function () {

        $("#FichaPpp_carga_horaria").val($('input[name=carga_horaria]:checked').val());
        if ($("#FichaPpp_carga_horaria").val() == 'S')
        { $("#FichaPpp_ch_cual").removeAttr("disabled"); } else { $("#FichaPpp_ch_cual").attr("disabled", "disabled"); $("#FichaPpp_ch_cual").val(""); }
    });
    if ($("#FichaPpp_carga_horaria").val() != 'S')
    { $("#FichaPpp_ch_cual").attr("disabled", "disabled"); }

    $(".ClassCONSTANCIA_EGRESO").click(function () {

        $("#FichaPpp_CONSTANCIA_EGRESO").val($('input[name=CONSTANCIA_EGRESO]:checked').val());
    });

    $(".ClassMATRICULA").click(function () {

        $("#FichaPpp_MATRICULA").val($('input[name=MATRICULA]:checked').val());
    });
    $(".ClassCONSTANCIA_CURSO").click(function () {

        $("#FichaPpp_CONSTANCIA_CURSO").val($('input[name=CONSTANCIA_CURSO]:checked').val());
    });



    /*************/
    $('#dialogAgregarNuevaEmpresa').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                $('#ABMEmpresaCuit').val("");
                $('#ABMEmpresaRazonSocial').val("");
                $('#LocalidadesEmpresa_Selected').val("");
                $('#ABMEmpresaCodigoActividad').val("");
                $('#ABMEmpresaCantidadEmpleados').val("");
                $('#ABMEmpresaCalle').val("");
                $('#ABMEmpresaNumero').val("");
                $('#ABMEmpresaPiso').val("");
                $('#ABMEmpresaDpto').val("");
                $('#ABMEmpresaCodigoPostal').val("");
                $('#DomicilioLaboralIdem_Selected').val(0);
                $('#ABMEmpresaIdSeleccionado').val(0);
                $('#ABMEmpresaNombreLocalidad').val("");
                $('#ABMEmpresaNombreDomicilioLaboral').val("");
                $('#dialogEmpresa').dialog("open");
                $('#ABMEmpresaAccion').val("Alta");

                $(this).dialog("close");

            },
            "Cancel": function () {
                $(this).dialog("close");
            }
        }
    });


    $('#dialogEmpresaExistente').dialog({
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

    $('#dialogSedes').dialog({
        autoOpen: false,
        width: 1300,
        modal: true,
        resizable: false,
        buttons: {
            "Cerrar": function () {
                $(this).dialog("close");

            }
        }
    });

    $('#dialogProyectos').dialog({
        autoOpen: false,
        width: 700,
        modal: true,
        resizable: false,
        buttons: {
            "Cerrar": function () {
                $(this).dialog("close");

            }
        }
    });

    $('#dialogCursos').dialog({
        autoOpen: false,
        width: 700,
        modal: true,
        resizable: false,
        buttons: {
            "Cerrar": function () {
                $(this).dialog("close");

            }
        }
    });



    $('#dialogActores').dialog({
        autoOpen: false,
        width: 900,
        modal: true,
        resizable: false,
        buttons: {
            "Cerrar": function () {
                $(this).dialog("close");

            }
        }
    });

    $('#dialogONG').dialog({
        autoOpen: false,
        width: 900,
        modal: true,
        resizable: false,
        buttons: {
            "Cerrar": function () {
                $(this).dialog("close");

            }
        }
    });

    $('#dialogSubProg').dialog({
        autoOpen: false,
        width: 700,
        modal: true,
        resizable: false,
        buttons: {
            "Cerrar": function () {
                $(this).dialog("close");

            }
        }
    });

    $('#dialogEscuela').dialog({
        autoOpen: false,
        width: 700,
        modal: true,
        resizable: false,
        buttons: {
            "Cerrar": function () {
                $(this).dialog("close");

            }
        }
    });

    $('#TraerSedes').click('click', function () {
        $('#ABMNombreSede').removeClass("input-validation-error");
        $('#ABMSedeTelefonoContacto').removeClass("input-validation-error");

        $.ajax({
            type: "POST",
            url: "/Fichas/CargarSedesEmpresa",
            data: "{idempresa:" + $('#IdEmpresa').val() + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('#grillaSede').empty();
                $('#grillaSede').append(data);
                $('#dialogSedes').dialog("open");
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });

    $('#AgregarSede').click('click', function () {
        if ($('#ABMNombreSede').val() == "") {
            $('#ABMNombreSede').addClass("input-validation-error");
        }
        else {
            $('#ABMNombreSede').removeClass("input-validation-error");
        }
        if ($('#ABMSedeTelefonoContacto').val() == "") {
            $('#ABMSedeTelefonoContacto').addClass("input-validation-error");
        }
        else {
            $('#ABMSedeTelefonoContacto').removeClass("input-validation-error");
        }
        if ($('#ABMSedeTelefonoContacto').val() != "" && $('#ABMNombreSede').val() != "") {
            $.ajax({
                type: "POST",
                url: "/Fichas/AgregarSedeEmpresa",
                data: "{idempresa:" + $('#IdEmpresa').val() + ",nombre: '" + $('#ABMNombreSede').val() + "',apecon: '" + $('#ABMSedeApellidoContacto').val() + "',nombrecon:'" + $('#ABMSedeNombreContacto').val() + "',localidad:'" + $('#LocalidadesSedes_Selected').val() + "',telefono:'" + $('#ABMSedeTelefonoContacto').val() + "',calle:'" + $('#ABMSedeCalle').val() + "',nro:'" + $('#ABMSedeNumero').val() + "',piso:'" + $('#ABMSedePiso').val() + "',dpto:'" + $('#ABMSedeDpto').val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#grillaSede').empty();
                    $('#grillaSede').append(data);
                    $('#dialogSedes').dialog("open");
                },
                error: function (msg) {
                    alert(msg);
                }
            });
        }

    });

    $('#dialogEmpresa').dialog({
        autoOpen: false,
        width: 950,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                if ($('#ABMEmpresaAccion').val() == "Alta") {
                    if ($('#ABMEmpresaCuit').val() == "") {
                        $('#ABMEmpresaCuit').addClass("input-validation-error");
                        if ($('#ABMEmpresaRazonSocial').val() == "") {
                            $('#ABMEmpresaRazonSocial').addClass("input-validation-error");
                        }
                        else {
                            $('#ABMEmpresaRazonSocial').removeClass("input-validation-error");
                        }
                    }
                    else {
                        $('#ABMEmpresaCuit').removeClass("input-validation-error");
                        $.ajax({
                            type: "POST",
                            url: "/Fichas/ValidarCuit",
                            data: "{cuitEmpresa:'" + $('#ABMEmpresaCuit').val() + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                if (data != null) {
                                    $('#ABMEmpresaCuit').addClass("input-validation-error");
                                    $("#dialogEmpresaExistente").dialog("open");
                                    if ($('#ABMEmpresaRazonSocial').val() == "") {
                                        $('#ABMEmpresaRazonSocial').addClass("input-validation-error");
                                    }
                                    else {
                                        $('#ABMEmpresaRazonSocial').removeClass("input-validation-error");
                                    }
                                }
                                else {
                                    $('#ABMEmpresaCuit').removeClass("input-validation-error");
                                    if ($('#ABMEmpresaRazonSocial').val() == "") {
                                        $('#ABMEmpresaRazonSocial').addClass("input-validation-error");
                                    }
                                    else {
                                        $('#ABMEmpresaRazonSocial').removeClass("input-validation-error");
                                        $.ajax({
                                            type: "POST",
                                            url: "/Fichas/AgregarNuevaEmpresa",
                                            data: "{cuitEmpresa:'" + $("#ABMEmpresaCuit").val() + "',nombreempresa:'" + $('#ABMEmpresaRazonSocial').val() + "',idlocalidad:'" + $('#LocalidadesEmpresa_Selected').val() + "',iddomiciliolaboral:'" + $('#DomicilioLaboralIdem_Selected').val() + "',cantempleados:'" + $('#ABMEmpresaCantidadEmpleados').val() + "',codigoactividad:'" + $('#ABMEmpresaCodigoActividad').val() + "',calle:'" + $('#ABMEmpresaCalle').val() + "',numero:'" + $('#ABMEmpresaNumero').val() + "',piso:'" + $('#ABMEmpresaPiso').val() + "',dpto:'" + $('#ABMEmpresaDpto').val() + "',codigopostal:'" + $('#ABMEmpresaCodigoPostal').val() + "'  }",
                                            contentType: "application/json; charset=utf-8",
                                            dataType: "json",
                                            success: function (data) {
                                                if (data != null) {
                                                    $('#FichaPpp_DescripcionEmpresa').val(data.Descripcion);
                                                    $('#FichaPpp_Empresa_NombreLocalidad').val(data.NombreLocalidad);
                                                    $('#FichaPpp_Empresa_CodigoActividad').val(data.CodigoActividad);
                                                    $('#FichaPpp_Empresa_CantidadEmpleados').val(data.CantidadEmpleados);
                                                    $('#FichaPpp_Empresa_Calle').val(data.Calle);
                                                    $('#FichaPpp_Empresa_Numero').val(data.Numero);
                                                    $('#FichaPpp_Empresa_Piso').val(data.Piso);
                                                    $('#FichaPpp_Empresa_Dpto').val(data.Dpto);
                                                    $('#FichaPpp_Empresa_CodigoPostal').val(data.CodigoPostal);
                                                    $('#IdEmpresa').val(data.Id);
                                                    $('#FichaPpp_Empresa_DomicilioLaboralIdem').val(data.NombreDomicilioLaboralSeleccionado);

                                                    //VAT---------------------------------------------------------------------------------------
                                                    $('#FichaVat_DescripcionEmpresa').val(data.Descripcion);
                                                    $('#FichaVat_Empresa_NombreLocalidad').val(data.NombreLocalidad);
                                                    $('#FichaVat_Empresa_CodigoActividad').val(data.CodigoActividad);
                                                    $('#FichaVat_Empresa_CantidadEmpleados').val(data.CantidadEmpleados);
                                                    $('#FichaVat_Empresa_Calle').val(data.Calle);
                                                    $('#FichaVat_Empresa_Numero').val(data.Numero);
                                                    $('#FichaVat_Empresa_Piso').val(data.Piso);
                                                    $('#FichaVat_Empresa_Dpto').val(data.Dpto);
                                                    $('#FichaVat_Empresa_CodigoPostal').val(data.CodigoPostal);
                                                    $('#IdEmpresa').val(data.Id);
                                                    $('#FichaVat_Empresa_DomicilioLaboralIdem').val(data.NombreDomicilioLaboralSeleccionado);
                                                    //------------------------------------------------------------------------------------------
                                                    //Pppp---------------------------------------------------------------------------------------
                                                    $('#FichaPppp_DescripcionEmpresa').val(data.Descripcion);
                                                    $('#FichaPppp_Empresa_NombreLocalidad').val(data.NombreLocalidad);
                                                    $('#FichaPppp_Empresa_CodigoActividad').val(data.CodigoActividad);
                                                    $('#FichaPppp_Empresa_CantidadEmpleados').val(data.CantidadEmpleados);
                                                    $('#FichaPppp_Empresa_Calle').val(data.Calle);
                                                    $('#FichaPppp_Empresa_Numero').val(data.Numero);
                                                    $('#FichaPppp_Empresa_Piso').val(data.Piso);
                                                    $('#FichaPppp_Empresa_Dpto').val(data.Dpto);
                                                    $('#FichaPppp_Empresa_CodigoPostal').val(data.CodigoPostal);
                                                    $('#IdEmpresa').val(data.Id);
                                                    $('#FichaPppp_Empresa_DomicilioLaboralIdem').val(data.NombreDomicilioLaboralSeleccionado);
                                                    //RECONVERSION------------------------------------------------------------------------------
                                                    $('#FichaReconversion_DescripcionEmpresa').val(data.Descripcion);
                                                    $('#FichaReconversion_Empresa_NombreLocalidad').val(data.NombreLocalidad);
                                                    $('#FichaReconversion_Empresa_CodigoActividad').val(data.CodigoActividad);
                                                    $('#FichaReconversion_Empresa_CantidadEmpleados').val(data.CantidadEmpleados);
                                                    $('#FichaReconversion_Empresa_Calle').val(data.Calle);
                                                    $('#FichaReconversion_Empresa_Numero').val(data.Numero);
                                                    $('#FichaReconversion_Empresa_Piso').val(data.Piso);
                                                    $('#FichaReconversion_Empresa_Dpto').val(data.Dpto);
                                                    $('#FichaReconversion_Empresa_CodigoPostal').val(data.CodigoPostal);
                                                    $('#IdEmpresa').val(data.Id);
                                                    $('#FichaReconversion_Empresa_DomicilioLaboralIdem').val(data.NombreDomicilioLaboralSeleccionado);
                                                    //------------------------------------------------------------------------------------------
                                                    //EFECTORES SOCIALES------------------------------------------------------------------------------
                                                    $('#FichaEfectores_DescripcionEmpresa').val(data.Descripcion);
                                                    $('#FichaEfectores_Empresa_NombreLocalidad').val(data.NombreLocalidad);
                                                    $('#FichaEfectores_Empresa_CodigoActividad').val(data.CodigoActividad);
                                                    $('#FichaEfectores_Empresa_CantidadEmpleados').val(data.CantidadEmpleados);
                                                    $('#FichaEfectores_Empresa_Calle').val(data.Calle);
                                                    $('#FichaEfectores_Empresa_Numero').val(data.Numero);
                                                    $('#FichaEfectores_Empresa_Piso').val(data.Piso);
                                                    $('#FichaEfectores_Empresa_Dpto').val(data.Dpto);
                                                    $('#FichaEfectores_Empresa_CodigoPostal').val(data.CodigoPostal);
                                                    $('#IdEmpresa').val(data.Id);
                                                    $('#FichaEfectores_Empresa_DomicilioLaboralIdem').val(data.NombreDomicilioLaboralSeleccionado);
                                                    //------------------------------------------------------------------------------------------
                                                    //CONFIAMOS EN VOS------------------------------------------------------------------------------
                                                    $('#FichaConfVos_DescripcionEmpresa').val(data.Descripcion);
                                                    $('#FichaConfVos_Empresa_NombreLocalidad').val(data.NombreLocalidad);
                                                    $('#FichaConfVos_Empresa_CodigoActividad').val(data.CodigoActividad);
                                                    $('#FichaConfVos_Empresa_CantidadEmpleados').val(data.CantidadEmpleados);
                                                    $('#FichaConfVos_Empresa_Calle').val(data.Calle);
                                                    $('#FichaConfVos_Empresa_Numero').val(data.Numero);
                                                    $('#FichaConfVos_Empresa_Piso').val(data.Piso);
                                                    $('#FichaConfVos_Empresa_Dpto').val(data.Dpto);
                                                    $('#FichaConfVos_Empresa_CodigoPostal').val(data.CodigoPostal);
                                                    $('#IdEmpresa').val(data.Id);
                                                    $('#FichaConfVos_Empresa_DomicilioLaboralIdem').val(data.NombreDomicilioLaboralSeleccionado);
                                                    //------------------------------------------------------------------------------------------

                                                    $('#dialogEmpresa').dialog("close");

                                                }
                                                else {
                                                    alert('Sucedio un Error verifique los datos!');
                                                }
                                            },
                                            error: function (msg) {
                                                alert(msg);
                                            }
                                        });

                                    }
                                }
                            },
                            error: function (msg) {
                                alert(msg);
                            }
                        });
                    }

                }
                else {
                    $('#FichaPpp_DescripcionEmpresa').val($('#ABMEmpresaRazonSocial').val());
                    $('#FichaPpp_Empresa_NombreLocalidad').val($('#LocalidadesEmpresa_Selected').val());
                    $('#FichaPpp_Empresa_CodigoActividad').val($('#ABMEmpresaCodigoActividad').val());
                    $('#FichaPpp_Empresa_CantidadEmpleados').val($('#ABMEmpresaCantidadEmpleados').val());
                    $('#FichaPpp_Empresa_Calle').val($('#ABMEmpresaCalle').val());
                    $('#FichaPpp_Empresa_Numero').val($('#ABMEmpresaNumero').val());
                    $('#FichaPpp_Empresa_Piso').val($('#ABMEmpresaPiso').val());
                    $('#FichaPpp_Empresa_Dpto').val($('#ABMEmpresaDpto').val());
                    $('#FichaPpp_Empresa_CodigoPostal').val($('#ABMEmpresaCodigoPostal').val());
                    $('#IdEmpresa').val($('#ABMEmpresaIdSeleccionado').val());
                    $('#FichaPpp_Empresa_NombreLocalidad').val($('#ABMEmpresaNombreLocalidad').val());
                    $('#FichaPpp_Empresa_DomicilioLaboralIdem').val($('#ABMEmpresaNombreDomicilioLaboral').val());

                    //VAT ----------------------------------------------------------------------------------------
                    $('#FichaVat_DescripcionEmpresa').val($('#ABMEmpresaRazonSocial').val());
                    $('#FichaVat_Empresa_NombreLocalidad').val($('#LocalidadesEmpresa_Selected').val());
                    $('#FichaVat_Empresa_CodigoActividad').val($('#ABMEmpresaCodigoActividad').val());
                    $('#FichaVat_Empresa_CantidadEmpleados').val($('#ABMEmpresaCantidadEmpleados').val());
                    $('#FichaVat_Empresa_Calle').val($('#ABMEmpresaCalle').val());
                    $('#FichaVat_Empresa_Numero').val($('#ABMEmpresaNumero').val());
                    $('#FichaVat_Empresa_Piso').val($('#ABMEmpresaPiso').val());
                    $('#FichaVat_Empresa_Dpto').val($('#ABMEmpresaDpto').val());
                    $('#FichaVat_Empresa_CodigoPostal').val($('#ABMEmpresaCodigoPostal').val());
                    $('#IdEmpresa').val($('#ABMEmpresaIdSeleccionado').val());
                    $('#FichaVat_Empresa_NombreLocalidad').val($('#ABMEmpresaNombreLocalidad').val());
                    $('#FichaVat_Empresa_DomicilioLaboralIdem').val($('#ABMEmpresaNombreDomicilioLaboral').val());
                    //--------------------------------------------------------------------------------------------
                    //Pppp ----------------------------------------------------------------------------------------
                    $('#FichaPppp_DescripcionEmpresa').val($('#ABMEmpresaRazonSocial').val());
                    $('#FichaPppp_Empresa_NombreLocalidad').val($('#LocalidadesEmpresa_Selected').val());
                    $('#FichaPppp_Empresa_CodigoActividad').val($('#ABMEmpresaCodigoActividad').val());
                    $('#FichaPppp_Empresa_CantidadEmpleados').val($('#ABMEmpresaCantidadEmpleados').val());
                    $('#FichaPppp_Empresa_Calle').val($('#ABMEmpresaCalle').val());
                    $('#FichaPppp_Empresa_Numero').val($('#ABMEmpresaNumero').val());
                    $('#FichaPppp_Empresa_Piso').val($('#ABMEmpresaPiso').val());
                    $('#FichaPppp_Empresa_Dpto').val($('#ABMEmpresaDpto').val());
                    $('#FichaPppp_Empresa_CodigoPostal').val($('#ABMEmpresaCodigoPostal').val());
                    $('#IdEmpresa').val($('#ABMEmpresaIdSeleccionado').val());
                    $('#FichaPppp_Empresa_NombreLocalidad').val($('#ABMEmpresaNombreLocalidad').val());
                    $('#FichaPppp_Empresa_DomicilioLaboralIdem').val($('#ABMEmpresaNombreDomicilioLaboral').val());
                    //RECONVERSION----------------------------------------------------------------------------------
                    $('#FichaReconversion_DescripcionEmpresa').val($('#ABMEmpresaRazonSocial').val());
                    $('#FichaReconversion_Empresa_NombreLocalidad').val($('#LocalidadesEmpresa_Selected').val());
                    $('#FichaReconversion_Empresa_CodigoActividad').val($('#ABMEmpresaCodigoActividad').val());
                    $('#FichaReconversion_Empresa_CantidadEmpleados').val($('#ABMEmpresaCantidadEmpleados').val());
                    $('#FichaReconversion_Empresa_Calle').val($('#ABMEmpresaCalle').val());
                    $('#FichaReconversion_Empresa_Numero').val($('#ABMEmpresaNumero').val());
                    $('#FichaReconversion_Empresa_Piso').val($('#ABMEmpresaPiso').val());
                    $('#FichaReconversion_Empresa_Dpto').val($('#ABMEmpresaDpto').val());
                    $('#FichaReconversion_Empresa_CodigoPostal').val($('#ABMEmpresaCodigoPostal').val());
                    $('#IdEmpresa').val($('#ABMEmpresaIdSeleccionado').val());
                    $('#FichaReconversion_Empresa_NombreLocalidad').val($('#ABMEmpresaNombreLocalidad').val());
                    $('#FichaReconversion_Empresa_DomicilioLaboralIdem').val($('#ABMEmpresaNombreDomicilioLaboral').val());
                    //EFECTORES----------------------------------------------------------------------------------
                    $('#FichaEfectores_DescripcionEmpresa').val($('#ABMEmpresaRazonSocial').val());
                    $('#FichaEfectores_Empresa_NombreLocalidad').val($('#LocalidadesEmpresa_Selected').val());
                    $('#FichaEfectores_Empresa_CodigoActividad').val($('#ABMEmpresaCodigoActividad').val());
                    $('#FichaEfectores_Empresa_CantidadEmpleados').val($('#ABMEmpresaCantidadEmpleados').val());
                    $('#FichaEfectores_Empresa_Calle').val($('#ABMEmpresaCalle').val());
                    $('#FichaEfectores_Empresa_Numero').val($('#ABMEmpresaNumero').val());
                    $('#FichaEfectores_Empresa_Piso').val($('#ABMEmpresaPiso').val());
                    $('#FichaEfectores_Empresa_Dpto').val($('#ABMEmpresaDpto').val());
                    $('#FichaEfectores_Empresa_CodigoPostal').val($('#ABMEmpresaCodigoPostal').val());
                    $('#IdEmpresa').val($('#ABMEmpresaIdSeleccionado').val());
                    $('#FichaEfectores_Empresa_NombreLocalidad').val($('#ABMEmpresaNombreLocalidad').val());
                    $('#FichaEfectores_Empresa_DomicilioLaboralIdem').val($('#ABMEmpresaNombreDomicilioLaboral').val());
                    //--------------------------------------------------------------------------------------------
                    //CONFIAMOS EN VOS----------------------------------------------------------------------------------
                    $('#FichaConfVos_DescripcionEmpresa').val($('#ABMEmpresaRazonSocial').val());
                    $('#FichaConfVos_Empresa_NombreLocalidad').val($('#LocalidadesEmpresa_Selected').val());
                    $('#FichaConfVos_Empresa_CodigoActividad').val($('#ABMEmpresaCodigoActividad').val());
                    $('#FichaConfVos_Empresa_CantidadEmpleados').val($('#ABMEmpresaCantidadEmpleados').val());
                    $('#FichaConfVos_Empresa_Calle').val($('#ABMEmpresaCalle').val());
                    $('#FichaConfVos_Empresa_Numero').val($('#ABMEmpresaNumero').val());
                    $('#FichaConfVos_Empresa_Piso').val($('#ABMEmpresaPiso').val());
                    $('#FichaConfVos_Empresa_Dpto').val($('#ABMEmpresaDpto').val());
                    $('#FichaConfVos_Empresa_CodigoPostal').val($('#ABMEmpresaCodigoPostal').val());
                    $('#IdEmpresa').val($('#ABMEmpresaIdSeleccionado').val());
                    $('#FichaConfVos_Empresa_NombreLocalidad').val($('#ABMEmpresaNombreLocalidad').val());
                    $('#FichaConfVos_Empresa_DomicilioLaboralIdem').val($('#ABMEmpresaNombreDomicilioLaboral').val());
                    //--------------------------------------------------------------------------------------------
                    $(this).dialog("close");
                }
            },
            "Cancel": function () {
                $(this).dialog("close");
            }
        }
    });



    $('#ValidarCuit').click('click', function () {

        $.ajax({
            type: "POST",
            url: "/Fichas/ValidarCuit",
            data: "{cuitEmpresa:'" + $("#FichaPpp_CuitEmpresa").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data != null) {
                    $('#ABMEmpresaCuit').val(data.Cuit);
                    $('#ABMEmpresaRazonSocial').val(data.Descripcion);
                    $('#LocalidadesEmpresa_Selected').val(data.IdLocalidad);
                    $('#ABMEmpresaCodigoActividad').val(data.CodigoActividad);
                    $('#ABMEmpresaCantidadEmpleados').val(data.CantidadEmpleados);
                    $('#ABMEmpresaCalle').val(data.Calle);
                    $('#ABMEmpresaNumero').val(data.Numero);
                    $('#ABMEmpresaPiso').val(data.Piso);
                    $('#ABMEmpresaDpto').val(data.Dpto);
                    $('#ABMEmpresaCodigoPostal').val(data.CodigoPostal);
                    $('#DomicilioLaboralIdem_Selected').val(data.IdDomicilioLaboral);
                    $('#ABMEmpresaIdSeleccionado').val(data.Id);
                    $('#ABMEmpresaNombreLocalidad').val(data.NombreLocalidad);
                    $('#ABMEmpresaNombreDomicilioLaboral').val(data.NombreDomicilioLaboralSeleccionado);
                    $('#ABMEmpresaAccion').val("");
                    $(".ABMEmpresa").attr("disabled", "disabled");
                    $('#LocalidadesEmpresa_Selected').attr("disabled", "disabled");
                    $('#DomicilioLaboralIdem_Selected').attr("disabled", "disabled");
                    $('#dialogEmpresa').dialog("open");
                }
                else {
                    //me da opcion para agregarla
                    $(".ABMEmpresa").removeAttr("disabled");
                    $('#LocalidadesEmpresa_Selected').removeAttr("disabled");
                    $('#DomicilioLaboralIdem_Selected').removeAttr("disabled");
                    $('#dialogAgregarNuevaEmpresa').dialog("open");
                }
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });

    $('#ValidarCuitVat').click('click', function () {

        $.ajax({
            type: "POST",
            url: "/Fichas/ValidarCuit",
            data: "{cuitEmpresa:'" + $("#FichaVat_CuitEmpresa").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data != null) {
                    $('#ABMEmpresaCuit').val(data.Cuit);
                    $('#ABMEmpresaRazonSocial').val(data.Descripcion);
                    $('#LocalidadesEmpresa_Selected').val(data.IdLocalidad);
                    $('#ABMEmpresaCodigoActividad').val(data.CodigoActividad);
                    $('#ABMEmpresaCantidadEmpleados').val(data.CantidadEmpleados);
                    $('#ABMEmpresaCalle').val(data.Calle);
                    $('#ABMEmpresaNumero').val(data.Numero);
                    $('#ABMEmpresaPiso').val(data.Piso);
                    $('#ABMEmpresaDpto').val(data.Dpto);
                    $('#ABMEmpresaCodigoPostal').val(data.CodigoPostal);
                    $('#DomicilioLaboralIdem_Selected').val(data.IdDomicilioLaboral);
                    $('#ABMEmpresaIdSeleccionado').val(data.Id);
                    $('#ABMEmpresaNombreLocalidad').val(data.NombreLocalidad);
                    $('#ABMEmpresaNombreDomicilioLaboral').val(data.NombreDomicilioLaboralSeleccionado);
                    $('#ABMEmpresaAccion').val("");
                    $(".ABMEmpresa").attr("disabled", "disabled");
                    $('#LocalidadesEmpresa_Selected').attr("disabled", "disabled");
                    $('#DomicilioLaboralIdem_Selected').attr("disabled", "disabled");
                    $('#dialogEmpresa').dialog("open");
                }
                else {
                    //me da opcion para agregarla
                    $(".ABMEmpresa").removeAttr("disabled");
                    $('#LocalidadesEmpresa_Selected').removeAttr("disabled");
                    $('#DomicilioLaboralIdem_Selected').removeAttr("disabled");
                    $('#dialogAgregarNuevaEmpresa').dialog("open");
                }
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });

    $('#ValidarCuitPppp').click('click', function () {

        $.ajax({
            type: "POST",
            url: "/Fichas/ValidarCuit",
            data: "{cuitEmpresa:'" + $("#FichaPppp_CuitEmpresa").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data != null) {
                    $('#ABMEmpresaCuit').val(data.Cuit);
                    $('#ABMEmpresaRazonSocial').val(data.Descripcion);
                    $('#LocalidadesEmpresa_Selected').val(data.IdLocalidad);
                    $('#ABMEmpresaCodigoActividad').val(data.CodigoActividad);
                    $('#ABMEmpresaCantidadEmpleados').val(data.CantidadEmpleados);
                    $('#ABMEmpresaCalle').val(data.Calle);
                    $('#ABMEmpresaNumero').val(data.Numero);
                    $('#ABMEmpresaPiso').val(data.Piso);
                    $('#ABMEmpresaDpto').val(data.Dpto);
                    $('#ABMEmpresaCodigoPostal').val(data.CodigoPostal);
                    $('#DomicilioLaboralIdem_Selected').val(data.IdDomicilioLaboral);
                    $('#ABMEmpresaIdSeleccionado').val(data.Id);
                    $('#ABMEmpresaNombreLocalidad').val(data.NombreLocalidad);
                    $('#ABMEmpresaNombreDomicilioLaboral').val(data.NombreDomicilioLaboralSeleccionado);
                    $('#ABMEmpresaAccion').val("");
                    $(".ABMEmpresa").attr("disabled", "disabled");
                    $('#LocalidadesEmpresa_Selected').attr("disabled", "disabled");
                    $('#DomicilioLaboralIdem_Selected').attr("disabled", "disabled");
                    $('#dialogEmpresa').dialog("open");
                }
                else {
                    //me da opcion para agregarla
                    $(".ABMEmpresa").removeAttr("disabled");
                    $('#LocalidadesEmpresa_Selected').removeAttr("disabled");
                    $('#DomicilioLaboralIdem_Selected').removeAttr("disabled");
                    $('#dialogAgregarNuevaEmpresa').dialog("open");
                }
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });

    $('#ValidarCuitReconversion').click('click', function () {

        $.ajax({
            type: "POST",
            url: "/Fichas/ValidarCuit",
            data: "{cuitEmpresa:'" + $("#FichaReconversion_CuitEmpresa").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data != null) {
                    $('#ABMEmpresaCuit').val(data.Cuit);
                    $('#ABMEmpresaRazonSocial').val(data.Descripcion);
                    $('#LocalidadesEmpresa_Selected').val(data.IdLocalidad);
                    $('#ABMEmpresaCodigoActividad').val(data.CodigoActividad);
                    $('#ABMEmpresaCantidadEmpleados').val(data.CantidadEmpleados);
                    $('#ABMEmpresaCalle').val(data.Calle);
                    $('#ABMEmpresaNumero').val(data.Numero);
                    $('#ABMEmpresaPiso').val(data.Piso);
                    $('#ABMEmpresaDpto').val(data.Dpto);
                    $('#ABMEmpresaCodigoPostal').val(data.CodigoPostal);
                    $('#DomicilioLaboralIdem_Selected').val(data.IdDomicilioLaboral);
                    $('#ABMEmpresaIdSeleccionado').val(data.Id);
                    $('#ABMEmpresaNombreLocalidad').val(data.NombreLocalidad);
                    $('#ABMEmpresaNombreDomicilioLaboral').val(data.NombreDomicilioLaboralSeleccionado);
                    $('#ABMEmpresaAccion').val("");
                    $(".ABMEmpresa").attr("disabled", "disabled");
                    $('#LocalidadesEmpresa_Selected').attr("disabled", "disabled");
                    $('#DomicilioLaboralIdem_Selected').attr("disabled", "disabled");
                    $('#dialogEmpresa').dialog("open");
                }
                else {
                    //me da opcion para agregarla
                    $(".ABMEmpresa").removeAttr("disabled");
                    $('#LocalidadesEmpresa_Selected').removeAttr("disabled");
                    $('#DomicilioLaboralIdem_Selected').removeAttr("disabled");
                    $('#dialogAgregarNuevaEmpresa').dialog("open");
                }
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });

    $('#ValidarCuitEfectores').click('click', function () {

        $.ajax({
            type: "POST",
            url: "/Fichas/ValidarCuit",
            data: "{cuitEmpresa:'" + $("#FichaEfectores_CuitEmpresa").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data != null) {
                    $('#ABMEmpresaCuit').val(data.Cuit);
                    $('#ABMEmpresaRazonSocial').val(data.Descripcion);
                    $('#LocalidadesEmpresa_Selected').val(data.IdLocalidad);
                    $('#ABMEmpresaCodigoActividad').val(data.CodigoActividad);
                    $('#ABMEmpresaCantidadEmpleados').val(data.CantidadEmpleados);
                    $('#ABMEmpresaCalle').val(data.Calle);
                    $('#ABMEmpresaNumero').val(data.Numero);
                    $('#ABMEmpresaPiso').val(data.Piso);
                    $('#ABMEmpresaDpto').val(data.Dpto);
                    $('#ABMEmpresaCodigoPostal').val(data.CodigoPostal);
                    $('#DomicilioLaboralIdem_Selected').val(data.IdDomicilioLaboral);
                    $('#ABMEmpresaIdSeleccionado').val(data.Id);
                    $('#ABMEmpresaNombreLocalidad').val(data.NombreLocalidad);
                    $('#ABMEmpresaNombreDomicilioLaboral').val(data.NombreDomicilioLaboralSeleccionado);
                    $('#ABMEmpresaAccion').val("");
                    $(".ABMEmpresa").attr("disabled", "disabled");
                    $('#LocalidadesEmpresa_Selected').attr("disabled", "disabled");
                    $('#DomicilioLaboralIdem_Selected').attr("disabled", "disabled");
                    $('#dialogEmpresa').dialog("open");
                }
                else {
                    //me da opcion para agregarla
                    $(".ABMEmpresa").removeAttr("disabled");
                    $('#LocalidadesEmpresa_Selected').removeAttr("disabled");
                    $('#DomicilioLaboralIdem_Selected').removeAttr("disabled");
                    $('#dialogAgregarNuevaEmpresa').dialog("open");
                }
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });



    $('#ValidarCuitConfVos').click('click', function () {

        $.ajax({
            type: "POST",
            url: "/Fichas/ValidarCuit",
            data: "{cuitEmpresa:'" + $("#FichaConfVos_CuitEmpresa").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data != null) {
                    $('#ABMEmpresaCuit').val(data.Cuit);
                    $('#ABMEmpresaRazonSocial').val(data.Descripcion);
                    $('#LocalidadesEmpresa_Selected').val(data.IdLocalidad);
                    $('#ABMEmpresaCodigoActividad').val(data.CodigoActividad);
                    $('#ABMEmpresaCantidadEmpleados').val(data.CantidadEmpleados);
                    $('#ABMEmpresaCalle').val(data.Calle);
                    $('#ABMEmpresaNumero').val(data.Numero);
                    $('#ABMEmpresaPiso').val(data.Piso);
                    $('#ABMEmpresaDpto').val(data.Dpto);
                    $('#ABMEmpresaCodigoPostal').val(data.CodigoPostal);
                    $('#DomicilioLaboralIdem_Selected').val(data.IdDomicilioLaboral);
                    $('#ABMEmpresaIdSeleccionado').val(data.Id);
                    $('#ABMEmpresaNombreLocalidad').val(data.NombreLocalidad);
                    $('#ABMEmpresaNombreDomicilioLaboral').val(data.NombreDomicilioLaboralSeleccionado);
                    $('#ABMEmpresaAccion').val("");
                    $(".ABMEmpresa").attr("disabled", "disabled");
                    $('#LocalidadesEmpresa_Selected').attr("disabled", "disabled");
                    $('#DomicilioLaboralIdem_Selected').attr("disabled", "disabled");
                    $('#dialogEmpresa').dialog("open");
                }
                else {
                    //me da opcion para agregarla
                    $(".ABMEmpresa").removeAttr("disabled");
                    $('#LocalidadesEmpresa_Selected').removeAttr("disabled");
                    $('#DomicilioLaboralIdem_Selected').removeAttr("disabled");
                    $('#dialogAgregarNuevaEmpresa').dialog("open");
                }
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });




    if ($('input[name=Modalidad]:checked').val() == 1) {
        $(".ClassAltaTemprana").attr("disabled", "disabled");
    }
    else {
        $(".ClassAltaTemprana").removeAttr("disabled");
    }

    if ($('input[name=AltaTemprana]:checked').val() != "S") {

        $("#ComboModalidadAfip_Selected").attr("disabled", "disabled");
        $("#FichaPppp_FechaInicioActividad").attr("disabled", "disabled");
        $("#FichaPppp_FechaFinActividad").attr("disabled", "disabled");
        $("#FichaVat_FechaInicioActividad").attr("disabled", "disabled");
        $("#FichaVat_FechaFinActividad").attr("disabled", "disabled");
        $("#FichaPpp_FechaInicioActividad").attr("disabled", "disabled");
        $("#FichaPpp_FechaFinActividad").attr("disabled", "disabled");
        $("#FichaReconversion_FechaInicioActividad").attr("disabled", "disabled");
        $("#FichaReconversion_FechaFinActividad").attr("disabled", "disabled");
        $("#FichaEfectores_FechaInicioActividad").attr("disabled", "disabled");
        $("#FichaEfectores_FechaFinActividad").attr("disabled", "disabled");
        $("#FichaConfVos_FechaInicioActividad").attr("disabled", "disabled");
        $("#FichaConfVos_FechaFinActividad").attr("disabled", "disabled");


    }
    else {
        $("#ComboModalidadAfip_Selected").removeAttr("disabled");
        $("#FichaPppp_FechaInicioActividad").removeAttr("disabled");
        $("#FichaPppp_FechaFinActividad").removeAttr("disabled");
        $("#FichaVat_FechaInicioActividad").removeAttr("disabled");
        $("#FichaVat_FechaFinActividad").removeAttr("disabled");
        $("#FichaPpp_FechaInicioActividad").removeAttr("disabled");
        $("#FichaPpp_FechaFinActividad").removeAttr("disabled");
        $("#FichaReconversion_FechaInicioActividad").removeAttr("disabled");
        $("#FichaReconversion_FechaFinActividad").removeAttr("disabled");
        $("#FichaEfectores_FechaInicioActividad").removeAttr("disabled");
        $("#FichaEfectores_FechaFinActividad").removeAttr("disabled");
        $("#FichaConfVos_FechaInicioActividad").removeAttr("disabled");
        $("#FichaConfVos_FechaFinActividad").removeAttr("disabled");
    }

    if ($("#Accion").val() == "Ver" || $("#Accion").val() == "CambiarEstado") {
        $("#ComboModalidadAfip_Selected").attr("disabled", "disabled");
        $("#FichaPppp_FechaInicioActividad").attr("disabled", "disabled");
        $("#FichaPppp_FechaFinActividad").attr("disabled", "disabled");
        $("#FichaVat_FechaInicioActividad").attr("disabled", "disabled");
        $("#FichaVat_FechaFinActividad").attr("disabled", "disabled");
        $("#FichaPpp_FechaInicioActividad").attr("disabled", "disabled");
        $("#FichaPpp_FechaFinActividad").attr("disabled", "disabled");
        $("#FichaReconversion_FechaInicioActividad").attr("disabled", "disabled");
        $("#FichaReconversion_FechaFinActividad").attr("disabled", "disabled");
        $("#FichaEfectores_FechaInicioActividad").attr("disabled", "disabled");
        $("#FichaEfectores_FechaFinActividad").attr("disabled", "disabled");

        $("#FichaPpp_CuitEmpresa").attr("disabled", "disabled");
        $("#FichaPppp_CuitEmpresa").attr("disabled", "disabled");
        $("#FichaVat_CuitEmpresa").attr("disabled", "disabled");
        $("#FichaReconversion_CuitEmpresa").attr("disabled", "disabled");
        $("#FichaEfectores_CuitEmpresa").attr("disabled", "disabled");
        $(".ClassModalidad").attr("disabled", "disabled");
        $(".ClassAltaTemprana").attr("disabled", "disabled");

        $("#FichaConfVos_CuitEmpresa").attr("disabled", "disabled");
        $("#FichaConfVos_FechaInicioActividad").attr("disabled", "disabled");
        $("#FichaConfVos_FechaFinActividad").attr("disabled", "disabled");

    }


    // 30/09/2014 - Di Campli Leandro - Traer proyecto

    $('#ValidarProyecto_Efe').click('click', function () {
        //        $('#ABMNombreSede').removeClass("input-validation-error");
        //        $('#ABMSedeTelefonoContacto').removeClass("input-validation-error");

        $.ajax({
            type: "POST",
            url: "/Fichas/ValidarProyecto",
            data: "{NombreProyecto:'" + $('#FichaEfectores_Proyecto_NombreProyecto').val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('#grillaProyecto').empty();
                $('#grillaProyecto').append(data);
                $('#dialogProyectos').dialog("open");
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });

    $('#ValidarProyecto_Conf').click('click', function () {
        //        $('#ABMNombreSede').removeClass("input-validation-error");
        //        $('#ABMSedeTelefonoContacto').removeClass("input-validation-error");

        $.ajax({
            type: "POST",
            url: "/Fichas/ValidarProyecto",
            data: "{NombreProyecto:'" + $('#FichaConfVos_Proyecto_NombreProyecto').val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('#grillaProyecto').empty();
                $('#grillaProyecto').append(data);
                $('#dialogProyectos').dialog("open");
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });
    $('#ValidarProyecto_Reco').click('click', function () {
        //        $('#ABMNombreSede').removeClass("input-validation-error");
        //        $('#ABMSedeTelefonoContacto').removeClass("input-validation-error");

        $.ajax({
            type: "POST",
            url: "/Fichas/ValidarProyecto",
            data: "{NombreProyecto:'" + $('#FichaReconversion_Proyecto_NombreProyecto').val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('#grillaProyecto').empty();
                $('#grillaProyecto').append(data);
                $('#dialogProyectos').dialog("open");
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });

    $('#GuardarFichaRechazada').click('click', function () {
        //$('#FormularioFichas').attr('action', '/Fichas/CambiarCombo');
        $('#FormularioFichas').attr('action', '/Fichas/ModificarFicha');
        $("#FormularioFichas").submit()
    });

    $('#btnGuardarFicha').click('click', function () {
        //alert(ValidarFechaInicioFin());


        if (ValidarFechaInicioFin()) {
            $('#FormularioFichas').attr('action', '/Fichas/ModificarFicha');
            $("#FormularioFichas").submit()
        }
    });

    $('#btnsubir').click('click', function () {
        $('#FormImportarExcel').attr('action', '/Fichas/ImportarExcel');
        $("#FormImportarExcel").submit()
    });

    $('#ExportFichas').click('click', function () {
        $('#FormIndexFichas').attr('action', '/Fichas/ExportFichas');
        $("#FormIndexFichas").submit()
    });


    $('#DesvincularSedes').click('click', function () {
        $('#dialogDesvincularSede').dialog("open");
    })


    $('#dialogDesvincularSede').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                $("#IdSede").val(0);
                //Fichas PPP -------------------------------------
                $("#FichaPpp_Sede_NombreSede").val("");
                $("#FichaPpp_Sede_ApellidoContacto").val("");
                $("#FichaPpp_Sede_NombreContacto").val("");
                $("#FichaPpp_Sede_Telefono").val("");
                $("#FichaPpp_Sede_NombreLocalidad").val("");
                $("#FichaPpp_Sede_Calle").val("");
                $("#FichaPpp_Sede_Numero").val("");
                $("#FichaPpp_Sede_Piso").val("");
                $("#FichaPpp_Sede_Dpto").val("");
                //-------------------------------------------------
                //Fichas PPPp -------------------------------------
                $("#FichaPppp_Sede_NombreSede").val("");
                $("#FichaPppp_Sede_ApellidoContacto").val("");
                $("#FichaPppp_Sede_NombreContacto").val("");
                $("#FichaPppp_Sede_Telefono").val("");
                $("#FichaPppp_Sede_NombreLocalidad").val("");
                $("#FichaPppp_Sede_Calle").val("");
                $("#FichaPppp_Sede_Numero").val("");
                $("#FichaPppp_Sede_Piso").val("");
                $("#FichaPppp_Sede_Dpto").val("");
                //-------------------------------------------------
                //Fichas Vat -------------------------------------
                $("#FichaVat_Sede_NombreSede").val("");
                $("#FichaVat_Sede_ApellidoContacto").val("");
                $("#FichaVat_Sede_NombreContacto").val("");
                $("#FichaVat_Sede_Telefono").val("");
                $("#FichaVat_Sede_NombreLocalidad").val("");
                $("#FichaVat_Sede_Calle").val("");
                $("#FichaVat_Sede_Numero").val("");
                $("#FichaVat_Sede_Piso").val("");
                $("#FichaVat_Sede_Dpto").val("");
                //Fichas RECONVERSION PRODUCTIVA--------------------
                $("#FichaReconversion_Sede_NombreSede").val("");
                $("#FichaReconversion_Sede_ApellidoContacto").val("");
                $("#FichaReconversion_Sede_NombreContacto").val("");
                $("#FichaReconversion_Sede_Telefono").val("");
                $("#FichaReconversion_Sede_NombreLocalidad").val("");
                $("#FichaReconversion_Sede_Calle").val("");
                $("#FichaReconversion_Sede_Numero").val("");
                $("#FichaReconversion_Sede_Piso").val("");
                $("#FichaReconversion_Sede_Dpto").val("");
                //Fichas EFECTORES SOCIALES-------------------------
                $("#FichaEfectores_Sede_NombreSede").val("");
                $("#FichaEfectores_Sede_ApellidoContacto").val("");
                $("#FichaEfectores_Sede_NombreContacto").val("");
                $("#FichaEfectores_Sede_Telefono").val("");
                $("#FichaEfectores_Sede_NombreLocalidad").val("");
                $("#FichaEfectores_Sede_Calle").val("");
                $("#FichaEfectores_Sede_Numero").val("");
                $("#FichaEfectores_Sede_Piso").val("");
                $("#FichaEfectores_Sede_Dpto").val("");
                //-------------------------------------------------

                //Fichas CONFIAMOS EN VOS-------------------------
                $("#FichaConfVos_Sede_NombreSede").val("");
                $("#FichaConfVos_Sede_ApellidoContacto").val("");
                $("#FichaConfVos_Sede_NombreContacto").val("");
                $("#FichaConfVos_Sede_Telefono").val("");
                $("#FichaConfVos_Sede_NombreLocalidad").val("");
                $("#FichaConfVos_Sede_Calle").val("");
                $("#FichaConfVos_Sede_Numero").val("");
                $("#FichaConfVos_Sede_Piso").val("");
                $("#FichaConfVos_Sede_Dpto").val("");
                //-------------------------------------------------
                //-------------------------------------------------
                $(this).dialog("close");
            },
            "Cancelar": function () {
                $(this).dialog("close");
            }
        }
    });

    // Horarios -------------------------------------------
    $('#dialogHorarioAdd').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                $('#FormularioFichas').validate().cancelSubmit = true;
                $('#FormularioFichas').attr('action', '/Fichas/AgregarHorarioEmpresa');
                $("#FormularioFichas").submit()
                $(this).dialog("close");
            },
            "Cancelar": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#AgregarHrLunes').click('click', function () {
        $('#DiaSemanaTitulo').empty();
        $('#DiaSemanaTitulo').append("Lunes");
        $('#DiaSemana').val("01");
        $('#HoraInicio').val("00");
        $('#MinutoInicio').val("00");
        $('#HoraFin').val("00");
        $('#MinutoFin').val("00");
        $('#dialogHorarioAdd').dialog("open");
    });

    $('#AgregarCiLunes').click('click', function () {
        $('#DiaSemana').val("01");
        $('#FormularioFichas').validate().cancelSubmit = true;
        $('#FormularioFichas').attr('action', '/Fichas/AgregarCicloEmpresa');
        $("#FormularioFichas").submit()
    });

    $('#AgregarHrMartes').click('click', function () {
        $('#DiaSemanaTitulo').empty();
        $('#DiaSemanaTitulo').append("Martes");
        $('#DiaSemana').val("02");
        $('#HoraInicio').val("00");
        $('#MinutoInicio').val("00");
        $('#HoraFin').val("00");
        $('#MinutoFin').val("00");
        $('#dialogHorarioAdd').dialog("open");
    });

    $('#AgregarCiMartes').click('click', function () {
        $('#DiaSemana').val("02");
        $('#FormularioFichas').validate().cancelSubmit = true;
        $('#FormularioFichas').attr('action', '/Fichas/AgregarCicloEmpresa');
        $("#FormularioFichas").submit()
    });

    $('#AgregarHrMiercoles').click('click', function () {
        $('#DiaSemanaTitulo').empty();
        $('#DiaSemanaTitulo').append("Miércoles");
        $('#DiaSemana').val("03");
        $('#HoraInicio').val("00");
        $('#MinutoInicio').val("00");
        $('#HoraFin').val("00");
        $('#MinutoFin').val("00");
        $('#dialogHorarioAdd').dialog("open");
    });

    $('#AgregarCiMiercoles').click('click', function () {
        $('#DiaSemana').val("03");
        $('#FormularioFichas').validate().cancelSubmit = true;
        $('#FormularioFichas').attr('action', '/Fichas/AgregarCicloEmpresa');
        $("#FormularioFichas").submit()
    });

    $('#AgregarHrJueves').click('click', function () {
        $('#DiaSemanaTitulo').empty();
        $('#DiaSemanaTitulo').append("Jueves");
        $('#DiaSemana').val("04");
        $('#HoraInicio').val("00");
        $('#MinutoInicio').val("00");
        $('#HoraFin').val("00");
        $('#MinutoFin').val("00");
        $('#dialogHorarioAdd').dialog("open");
    });

    $('#AgregarCiJueves').click('click', function () {
        $('#DiaSemana').val("04");
        $('#FormularioFichas').validate().cancelSubmit = true;
        $('#FormularioFichas').attr('action', '/Fichas/AgregarCicloEmpresa');
        $("#FormularioFichas").submit()
    });

    $('#AgregarHrViernes').click('click', function () {
        $('#DiaSemanaTitulo').empty();
        $('#DiaSemanaTitulo').append("Viernes");
        $('#DiaSemana').val("05");
        $('#HoraInicio').val("00");
        $('#MinutoInicio').val("00");
        $('#HoraFin').val("00");
        $('#MinutoFin').val("00");
        $('#dialogHorarioAdd').dialog("open");
    });

    $('#AgregarCiViernes').click('click', function () {
        $('#DiaSemana').val("05");
        $('#FormularioFichas').validate().cancelSubmit = true;
        $('#FormularioFichas').attr('action', '/Fichas/AgregarCicloEmpresa');
        $("#FormularioFichas").submit()
    });

    $('#AgregarHrSabado').click('click', function () {
        $('#DiaSemanaTitulo').empty();
        $('#DiaSemanaTitulo').append("Sábado");
        $('#DiaSemana').val("06");
        $('#HoraInicio').val("00");
        $('#MinutoInicio').val("00");
        $('#HoraFin').val("00");
        $('#MinutoFin').val("00");
        $('#dialogHorarioAdd').dialog("open");
    });

    $('#AgregarCiSabado').click('click', function () {
        $('#DiaSemana').val("06");
        $('#FormularioFichas').validate().cancelSubmit = true;
        $('#FormularioFichas').attr('action', '/Fichas/AgregarCicloEmpresa');
        $("#FormularioFichas").submit()
    });

    $('#AgregarHrDomingo').click('click', function () {
        $('#DiaSemanaTitulo').empty();
        $('#DiaSemanaTitulo').append("Domingo");
        $('#DiaSemana').val("07");
        $('#HoraInicio').val("00");
        $('#MinutoInicio').val("00");
        $('#HoraFin').val("00");
        $('#MinutoFin').val("00");
        $('#dialogHorarioAdd').dialog("open");
    });

    $('#AgregarCiDomingo').click('click', function () {
        $('#DiaSemana').val("07");
        $('#FormularioFichas').validate().cancelSubmit = true;
        $('#FormularioFichas').attr('action', '/Fichas/AgregarCicloEmpresa');
        $("#FormularioFichas").submit()
    });

    $("#HoraInicioSelect").change(function () {
        $('#HoraInicio').val($("#HoraInicioSelect").val());
    });
    $("#MinutoInicioSelect").change(function () {
        $('#MinutoInicio').val($("#MinutoInicioSelect").val());
    });
    $("#HoraFinSelect").change(function () {
        $('#HoraFin').val($("#HoraFinSelect").val());
    });
    $("#MinutoFinSelect").change(function () {
        $('#MinutoFin').val($("#MinutoFinSelect").val());
    });

    //-----------------------------------------------------



    ///***********VALIDAR SOLO NUMEROS*************

    $(function () {
        $(".validar").keydown(function (event) {
            if (event.shiftKey) {
                event.preventDefault();
            }

            if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9) {
            }
            else {
                if (event.keyCode < 95) {
                    if (event.keyCode < 48 || event.keyCode > 57) {
                        event.preventDefault();
                    }
                }
                else {
                    if (event.keyCode < 96 || event.keyCode > 105) {
                        event.preventDefault();
                    }
                }
            }
        });
    });

    ///********************************************


    // 02/01/2018 - DI CAMPLI LEANDRO - RE EMPADRONAR CON ID FICHAS
    $("#archivoEmpadronar").change(function () {
        var ext = $('#archivoEmpadronar').val().split('.').pop().toLowerCase();
        if ($.inArray(ext, ['xls']) == -1) {
            $('#btnUploadEmpadronar').css('display', 'none');
            alert('Archivo inválido, por favor seleccione otro archivo.');
        }
        else {
            $('#btnUploadEmpadronar').show();
        }
    });
    // 02/01/2018 - DI CAMPLI LEANDRO - RE EMPADRONAR CON ID FICHAS
    $('#btnUploadEmpadronar').click('click', function () {
        var f = $("#FormReEmpadronarFichas");
        $('#FormReEmpadronarFichas').attr('action', '/Fichas/ArchEmpadronar');
        $("#FormReEmpadronarFichas").submit()
    });


    // 02/01/2018 - DI CAMPLI LEANDRO - RE EMPADRONAR CON ID FICHAS
    $('#EmpadronarFichasXLS').click('click', function () {
        var f = $("#FormReEmpadronarFichas");
        $('#FormReEmpadronarFichas').attr('action', '/Fichas/EmpadronarFichasXLS');
        $("#FormReEmpadronarFichas").submit()
    });

    $('#ReEmpadronar').click('click', function () {
        var f = $("#FormReEmpadronarFichas");
        $('#FormReEmpadronarFichas').validate().cancelSubmit = true;
        $('#FormReEmpadronarFichas').attr('action', '/Fichas/ReEmpadronarFichas');
        $("#FormReEmpadronarFichas").submit()
    });

    $('#ExportEmpadronados').click('click', function () {
        var f = $("#FormReEmpadronarFichas");
        $('#FormReEmpadronarFichas').validate().cancelSubmit = true;
        $('#FormReEmpadronarFichas').attr('action', '/Fichas/ExportEmpadronados');
        $("#FormReEmpadronarFichas").submit()
    });


});                // Fin Ready

function SeleccionarSede(idsede, nomsede, apconsede, nomconsede, telsede, locsede, callesede, nrosede, pisosede, dptosede) {
    $("#IdSede").val(idsede);
    $("#dialogSedes").dialog("close");
    $("#IdSede").val(idsede);
    //Fichas PPP -------------------------------------
    $("#FichaPpp_Sede_NombreSede").val(nomsede);
    $("#FichaPpp_Sede_ApellidoContacto").val(apconsede);
    $("#FichaPpp_Sede_NombreContacto").val(nomconsede);
    $("#FichaPpp_Sede_Telefono").val(telsede);
    $("#FichaPpp_Sede_NombreLocalidad").val(locsede);
    $("#FichaPpp_Sede_Calle").val(callesede);
    $("#FichaPpp_Sede_Numero").val(nrosede);
    $("#FichaPpp_Sede_Piso").val(pisosede);
    $("#FichaPpp_Sede_Dpto").val(dptosede);
    //-------------------------------------------------
    //Fichas PPP -------------------------------------
    $("#FichaPppp_Sede_NombreSede").val(nomsede);
    $("#FichaPppp_Sede_ApellidoContacto").val(apconsede);
    $("#FichaPppp_Sede_NombreContacto").val(nomconsede);
    $("#FichaPppp_Sede_Telefono").val(telsede);
    $("#FichaPppp_Sede_NombreLocalidad").val(locsede);
    $("#FichaPppp_Sede_Calle").val(callesede);
    $("#FichaPppp_Sede_Numero").val(nrosede);
    $("#FichaPppp_Sede_Piso").val(pisosede);
    $("#FichaPppp_Sede_Dpto").val(dptosede);
    //-------------------------------------------------
    //Fichas Vat -------------------------------------
    $("#FichaVat_Sede_NombreSede").val(nomsede);
    $("#FichaVat_Sede_ApellidoContacto").val(apconsede);
    $("#FichaVat_Sede_NombreContacto").val(nomconsede);
    $("#FichaVat_Sede_Telefono").val(telsede);
    $("#FichaVat_Sede_NombreLocalidad").val(locsede);
    $("#FichaVat_Sede_Calle").val(callesede);
    $("#FichaVat_Sede_Numero").val(nrosede);
    $("#FichaVat_Sede_Piso").val(pisosede);
    $("#FichaVat_Sede_Dpto").val(dptosede);
    //Fichas RECONVERSION PRODUCTIVA--------------------
    $("#FichaReconversion_Sede_NombreSede").val(nomsede);
    $("#FichaReconversion_Sede_ApellidoContacto").val(apconsede);
    $("#FichaReconversion_Sede_NombreContacto").val(nomconsede);
    $("#FichaReconversion_Sede_Telefono").val(telsede);
    $("#FichaReconversion_Sede_NombreLocalidad").val(locsede);
    $("#FichaReconversion_Sede_Calle").val(callesede);
    $("#FichaReconversion_Sede_Numero").val(nrosede);
    $("#FichaReconversion_Sede_Piso").val(pisosede);
    $("#FichaReconversion_Sede_Dpto").val(dptosede);
    //Fichas EFECTORES SOCIALES------------------------
    $("#FichaEfectores_Sede_NombreSede").val(nomsede);
    $("#FichaEfectores_Sede_ApellidoContacto").val(apconsede);
    $("#FichaEfectores_Sede_NombreContacto").val(nomconsede);
    $("#FichaEfectores_Sede_Telefono").val(telsede);
    $("#FichaEfectores_Sede_NombreLocalidad").val(locsede);
    $("#FichaEfectores_Sede_Calle").val(callesede);
    $("#FichaEfectores_Sede_Numero").val(nrosede);
    $("#FichaEfectores_Sede_Piso").val(pisosede);
    $("#FichaEfectores_Sede_Dpto").val(dptosede);
    //-------------------------------------------------
    //Fichas CONFIAMOS EN VOS------------------------
    $("#FichaConfVos_Sede_NombreSede").val(nomsede);
    $("#FichaConfVos_Sede_ApellidoContacto").val(apconsede);
    $("#FichaConfVos_Sede_NombreContacto").val(nomconsede);
    $("#FichaConfVos_Sede_Telefono").val(telsede);
    $("#FichaConfVos_Sede_NombreLocalidad").val(locsede);
    $("#FichaConfVos_Sede_Calle").val(callesede);
    $("#FichaConfVos_Sede_Numero").val(nrosede);
    $("#FichaConfVos_Sede_Piso").val(pisosede);
    $("#FichaConfVos_Sede_Dpto").val(dptosede);
    //-------------------------------------------------

}


function compare_dates(fecha, fecha2) {
    var xMonth = fecha.substring(3, 5);
    var xDay = fecha.substring(0, 2);
    var xYear = fecha.substring(6, 10);
    var yMonth = fecha2.substring(3, 5);
    var yDay = fecha2.substring(0, 2);
    var yYear = fecha2.substring(6, 10);
    if (xYear > yYear) {
        return (true)
    }
    else {
        if (xYear == yYear) {
            if (xMonth > yMonth) {
                return (true)
            }
            else {
                if (xMonth == yMonth) {
                    if (xDay > yDay)
                        return (true);
                    else
                        return (false);
                }
                else
                    return (false);
            }
        }
        else
            return (false);
    }
}

function ValidarFechaInicioFin() {
    var tipoficha;

    if ($('#TipoFicha').val() != 0) {
        tipoficha = $('#TipoFicha').val();
    }
    else {
        tipoficha = $('#ComboTipoFicha_Selected').val();
    }

    var mreturn = true;
    if (tipoficha == 3 || tipoficha == 4 || tipoficha == 5 || tipoficha == 6) {
        //PPP
        if (tipoficha == 3) {
            $('#FichaPpp_FechaFinActividad').removeClass("input-validation-error");
            $('#FichaPpp_FechaInicioActividad').removeClass("input-validation-error");
            if ($('#FichaPpp_FechaInicioActividad').val() == "" && $('#FichaPpp_FechaFinActividad').val() != "") {
                mreturn = false;
                $("#FichaPpp_FechaInicioActividad").addClass("input-validation-error");
            }
            else {
                mreturn = compare_dates($("#FichaPpp_FechaFinActividad").val(), $("#FichaPpp_FechaInicioActividad").val());
                if (mreturn == false) {
                    $("#FichaPpp_FechaFinActividad").addClass("input-validation-error");
                }
            }
            if ($('#FichaPpp_FechaInicioActividad').val() == "" && $('#FichaPpp_FechaFinActividad').val() == "") {
                mreturn = true;
                $('#FichaPpp_FechaFinActividad').removeClass("input-validation-error");
                $('#FichaPpp_FechaInicioActividad').removeClass("input-validation-error");
            }
            if ($('#FichaPpp_FechaInicioActividad').val() != "" && $('#FichaPpp_FechaFinActividad').val() == "") {
                mreturn = true;
                $('#FichaPpp_FechaFinActividad').removeClass("input-validation-error");
                $('#FichaPpp_FechaInicioActividad').removeClass("input-validation-error");
            }
        }
        //RECONVERSION
        if (tipoficha == 6) {
            $('#FichaReconversion_FechaFinActividad').removeClass("input-validation-error");
            $('#FichaReconversion_FechaInicioActividad').removeClass("input-validation-error");
            if ($('#FichaReconversion_FechaInicioActividad').val() == "" && $('#FichaReconversion_FechaFinActividad').val() != "") {
                mreturn = false;
                $("#FichaReconversion_FechaInicioActividad").addClass("input-validation-error");
            }
            else {
                mreturn = compare_dates($("#FichaReconversion_FechaFinActividad").val(), $("#FichaReconversion_FechaInicioActividad").val());
                if (mreturn == false) {
                    $("#FichaReconversion_FechaFinActividad").addClass("input-validation-error");
                }
            }
            if ($('#FichaReconversion_FechaInicioActividad').val() == "" && $('#FichaReconversion_FechaFinActividad').val() == "") {
                mreturn = true;
                $('#FichaReconversion_FechaFinActividad').removeClass("input-validation-error");
                $('#FichaReconversion_FechaInicioActividad').removeClass("input-validation-error");
            }
            if ($('#FichaReconversion_FechaInicioActividad').val() != "" && $('#FichaReconversion_FechaFinActividad').val() == "") {
                mreturn = true;
                $('#FichaReconversion').removeClass("input-validation-error");
                $('#FichaReconversion_FechaInicioActividad').removeClass("input-validation-error");
            }
        }
        //EFECTORES SOCIALES
        if (tipoficha == 7) {
            $('#FichaEfectores_FechaFinActividad').removeClass("input-validation-error");
            $('#FichaEfectores_FechaInicioActividad').removeClass("input-validation-error");
            if ($('#FichaEfectores_FechaInicioActividad').val() == "" && $('#FichaEfectores_FechaFinActividad').val() != "") {
                mreturn = false;
                $("#FichaEfectores_FechaInicioActividad").addClass("input-validation-error");
            }
            else {
                mreturn = compare_dates($("#FichaEfectores_FechaFinActividad").val(), $("#FichaEfectores_FechaInicioActividad").val());
                if (mreturn == false) {
                    $("#FichaEfectores_FechaFinActividad").addClass("input-validation-error");
                }
            }
            if ($('#FichaEfectores_FechaInicioActividad').val() == "" && $('#FichaEfectores_FechaFinActividad').val() == "") {
                mreturn = true;
                $('#FichaEfectores_FechaFinActividad').removeClass("input-validation-error");
                $('#FichaEfectores_FechaInicioActividad').removeClass("input-validation-error");
            }
            if ($('#FichaEfectores_FechaInicioActividad').val() != "" && $('#FichaEfectores_FechaFinActividad').val() == "") {
                mreturn = true;
                $('#FichaEfectores').removeClass("input-validation-error");
                $('#FichaEfectores_FechaInicioActividad').removeClass("input-validation-error");
            }
        }
        //PPPP
        if (tipoficha == 4) {
            $('#FichaPppp_FechaFinActividad').removeClass("input-validation-error");
            $('#FichaPppp_FechaInicioActividad').removeClass("input-validation-error");
            if ($('#FichaPppp_FechaInicioActividad').val() == "" && $('#FichaPppp_FechaFinActividad').val() != "") {
                mreturn = false;
                $("#FichaPppp_FechaInicioActividad").addClass("input-validation-error");
            }
            else {
                mreturn = compare_dates($("#FichaPppp_FechaFinActividad").val(), $("#FichaPppp_FechaInicioActividad").val());
                if (mreturn == false) {
                    $("#FichaPppp_FechaFinActividad").addClass("input-validation-error");
                }
            }
            if ($('#FichaPppp_FechaInicioActividad').val() == "" && $('#FichaPppp_FechaFinActividad').val() == "") {
                mreturn = true;
                $('#FichaPppp_FechaFinActividad').removeClass("input-validation-error");
                $('#FichaPppp_FechaInicioActividad').removeClass("input-validation-error");
            }
            if ($('#FichaPppp_FechaInicioActividad').val() != "" && $('#FichaPppp_FechaFinActividad').val() == "") {
                mreturn = true;
                $('#FichaPppp_FechaFinActividad').removeClass("input-validation-error");
                $('#FichaPppp_FechaInicioActividad').removeClass("input-validation-error");
            }
        }
        //Vat
        if (tipoficha == 5) {
            $('#FichaVat_FechaFinActividad').removeClass("input-validation-error");
            $('#FichaVat_FechaInicioActividad').removeClass("input-validation-error");
            if ($('#FichaVat_FechaInicioActividad').val() == "" && $('#FichaVat_FechaFinActividad').val() != "") {
                mreturn = false;
                $("#FichaVat_FechaInicioActividad").addClass("input-validation-error");
            }
            else {
                mreturn = compare_dates($("#FichaVat_FechaFinActividad").val(), $("#FichaVat_FechaInicioActividad").val());
                if (mreturn == false) {
                    $("#FichaVat_FechaFinActividad").addClass("input-validation-error");
                }
            }
            if ($('#FichaVat_FechaInicioActividad').val() == "" && $('#FichaVat_FechaFinActividad').val() == "") {
                mreturn = true;
                $('#FichaVat_FechaFinActividad').removeClass("input-validation-error");
                $('#FichaVat_FechaInicioActividad').removeClass("input-validation-error");
            }
            if ($('#FichaVat_FechaInicioActividad').val() != "" && $('#FichaVat_FechaFinActividad').val() == "") {
                mreturn = true;
                $('#FichaVat_FechaFinActividad').removeClass("input-validation-error");
                $('#FichaVat_FechaInicioActividad').removeClass("input-validation-error");
            }
        }
    }

    return mreturn;
}


//************************************ cargar localidades para escuela **********************************

//$("#DepartamentosEscuela_Selected").change(function () {
//    var f = $("#FormularioFichas");
//    if ($('#TipoFicha').val() != 8) {
//        $('#FormularioFichas').validate().cancelSubmit = true;
//        $('#FormularioFichas').attr('action', '/Fichas/CambiarCombo');
//        $("#FormularioFichas").submit()
//    }
//});


$('#DepartamentosEscuela_Selected').change(function () {

    var f = $("#FormularioFichas");
    if (($('#TipoFicha').val() != 8 && $('#Accion').val() != "Agregar") || ($('#ComboTipoFicha_Selected').val() != 8 && $('#Accion').val() == "Agregar")) {
        $('#FormularioFichas').validate().cancelSubmit = true;
        $('#FormularioFichas').attr('action', '/Fichas/CambiarCombo');
        $("#FormularioFichas").submit();
    }
    else {
        $.ajax({
            type: "POST",
            url: "/Fichas/CargarcboLocalidad",
            data: "{idDepartamento:'" + $("#DepartamentosEscuela_Selected").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data != null) {

                    //var cboLocalidad = document.getElementById("LocalidadesEscuela");

                    var options = '<option value="">Elija una localidad...</option>';
                    var cbo = data.Combo;
                    $.each(cbo, function (index, obj) {
                        options += '<option value="' + obj.Id + '">' + obj.Description + '</option>';
                    });
                    // cboLocalidad.appendChild(
                    $("#LocalidadesEscuela_Selected").html(options);
                    // $('#LocalidadesEscuela')
                    //$("#FormularioFichas").submit();

                }
                else {
                    //me da opcion para agregarla

                }
            },
            error: function (msg) {
                alert(msg);
            }
        });
    }
});
//*******************************************************************************************************

//***********************CARGAR LAS ESCUELAS*************************************************************

$("#LocalidadesEscuela_Selected").change(function () {
    var f = $("#FormularioFichas");
    if (($('#TipoFicha').val() != 8 && $('#Accion').val() != "Agregar") || ($('#ComboTipoFicha_Selected').val() != 8 && $('#Accion').val() == "Agregar")) {
        $('#FormularioFichas').validate().cancelSubmit = true;
        $('#FormularioFichas').attr('action', '/Fichas/CambiarCombo');
        $("#FormularioFichas").submit()

    }
    else {
        $.ajax({
            type: "POST",
            url: "/Fichas/CargarcboEscuela",
            data: "{idLocalidad:'" + $("#LocalidadesEscuela_Selected").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data != null) {

                    //var cboLocalidad = document.getElementById("LocalidadesEscuela");

                    var options = '<option value="0">Seleccione ESCUELA...</option>';
                    var cbo = data.Combo;
                    $.each(cbo, function (index, obj) {
                        options += '<option value="' + obj.Id + '">' + obj.Description + '</option>';
                    });
                    // cboLocalidad.appendChild(
                    $("#Escuelas_Selected").html(options);
                    // $('#LocalidadesEscuela')
                    //$("#FormularioFichas").submit();

                }
                else {
                    //me da opcion para agregarla

                }
            },
            error: function (msg) {
                alert(msg);
            }
        });
    }
});

//*******************************************************************************************************

//***Traer los datos de una escuela**********************
$("#Escuelas_Selected").change(function () {

$.ajax({
            type: "POST",
            url: "/Fichas/TraerEscuela",
            data: "{idEscuela:'" + $("#Escuelas_Selected").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data != null) {

                    //var cboLocalidad = document.getElementById("LocalidadesEscuela");



                    $("#FichaConfVos_escuela_CALLE").val(data.CALLE);
                    $("#FichaConfVos_escuela_NUMERO").val(data.NUMERO);
                    $("#FichaConfVos_escuela_CUPO_CONFIAMOS").val(data.CUPO_CONFIAMOS);
                    $("#FichaConfVos_escuela_Cue").val(data.Cue);
                    $("#FichaConfVos_escuela_COOR_APELLIDO").val(data.COOR_APELLIDO);
                    $("#FichaConfVos_escuela_COOR_NOMBRE").val(data.COOR_NOMBRE);
                    $("#FichaConfVos_escuela_COOR_DNI").val(data.COOR_DNI);
                    $("#FichaConfVos_escuela_COOR_CUIL").val(data.COOR_CUIL);
                    $("#FichaConfVos_escuela_COOR_CELULAR").val(data.COOR_CELULAR);
                    $("#FichaConfVos_escuela_COOR_TELEFONO").val(data.COOR_TELEFONO);
                    $("#FichaConfVos_escuela_COOR_MAIL").val(data.COOR_MAIL);
                    $("#FichaConfVos_escuela_COOR_N_LOCALIDAD").val(data.COOR_N_LOCALIDAD);
                    $("#FichaConfVos_escuela_DIR_APELLIDO").val(data.DIR_APELLIDO);
                    $("#FichaConfVos_escuela_DIR_NOMBRE").val(data.DIR_NOMBRE);
                    $("#FichaConfVos_escuela_ENC_APELLIDO").val(data.ENC_APELLIDO);
                    $("#FichaConfVos_escuela_ENC_NOMBRE").val(data.ENC_NOMBRE);
                    $("#FichaConfVos_escuela_ENC_DNI").val(data.ENC_DNI);
                    $("#FichaConfVos_escuela_ENC_CUIL").val(data.ENC_CUIL);
                    $("#FichaConfVos_escuela_ENC_CELULAR").val(data.ENC_CELULAR);
                    $("#FichaConfVos_escuela_ENC_TELEFONO").val(data.ENC_TELEFONO);
                    $("#FichaConfVos_escuela_ENC_MAIL").val(data.ENC_MAIL);
                    $("#FichaConfVos_escuela_ENC_N_LOCALIDAD").val(data.ENC_N_LOCALIDAD);

                    /*

                    */

                }
                else {
                    //me da opcion para agregarla

                }
            },
            error: function (msg) {
                alert(msg);
            }
        });

});
//*******************************************************


//*******************************************************
    $('#BuscarCurso_Conf').click('click', function () {
        //        $('#ABMNombreSede').removeClass("input-validation-error");
        //        $('#ABMSedeTelefonoContacto').removeClass("input-validation-error");

        $.ajax({
            type: "POST",
            url: "/Fichas/TraerCursos",
            data: "{NombreCurso:'" + $('#FichaConfVos_curso_N_CURSO').val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('#grillaCurso').empty();
                $('#grillaCurso').append(data);
                $('#dialogCursos').dialog("open");
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });
    //*********************************************************

    //*******************************************************
    $('#BuscarCursoNro').click('click', function () {
        //        $('#ABMNombreSede').removeClass("input-validation-error");
        //        $('#ABMSedeTelefonoContacto').removeClass("input-validation-error");
        var nroCurso = 0;
        if ($('#FichaConfVos_curso_NRO_COND_CURSO').val() != "") { nroCurso = $('#FichaConfVos_curso_NRO_COND_CURSO').val(); }
        $.ajax({
            type: "POST",
            url: "/Fichas/CursosPorNro",
            data: "{NroCurso:'" + nroCurso + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('#grillaCurso').empty();
                $('#grillaCurso').append(data);
                $('#dialogCursos').dialog("open");
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });
    //*********************************************************

    //*******************************************************
    $('#BuscarCursoEscuela').click('click', function () {
        //        $('#ABMNombreSede').removeClass("input-validation-error");
        //        $('#ABMSedeTelefonoContacto').removeClass("input-validation-error");

        $.ajax({
            type: "POST",
            url: "/Fichas/CursosEscuela",
            data: "{NombreCurso:'" + $('#FichaPpp_cursoPpp_N_CURSO').val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                $('#ABMN_CURSO').val("");
                $('#ABMDURACION').val("");
                $('#ABMMESES').val("");
                $('#ABMCURSOF_INICIO').val("");
                $('#ABMCURSOF_FIN').val("");
                $("#ABMIDNEscuelaCurso").val("");
                $("#ABMNEscuelaCurso").val("");

                $('#grillaCurso').empty();
                $('#grillaCurso').append(data);
                $('#dialogCursos').dialog("open");
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });
    //*********************************************************


    //*******************************************************
    $('#addEscuelaCurso').click('click', function () {

        var nombre = "";
        //if ($('#FichaPpp_escuelaPpp_Nombre_Escuela').val() != "") { nombre = $('#FichaPpp_escuelaPpp_Nombre_Escuela').val(); }
        if ($('#ABMNEscuelaCurso').val() != "") { nombre = $('#ABMNEscuelaCurso').val(); }
        $.ajax({
            type: "POST",
            url: "/Fichas/getEscuelaCurso",
            data: "{nombre:'" + nombre + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('#ABMN_ESCUELA').val("");
                $('#ABMCALLEEsc').val("");
                $('#ABMNro').val("");
                $('#ABMTELEFONOEsc').val("");
                $('#ABMCueEsc').val("");
                $('#ABMRegiseEsc').val("");
                $('#grillaEscuela').empty();
                $('#grillaEscuela').append(data);
                $('#dialogEscuela').dialog("open");
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });

    function SeleEscuelaCurso(IdEscuela, NombreEscuela, calle, numero, telefono, Cue, regise) {


        $("#dialogEscuela").dialog("close");

        //------------------------
        $("#ABMIDNEscuelaCurso").val(IdEscuela);
        $("#ABMNEscuelaCurso").val(NombreEscuela);

        //-------------------------------------------------

    }

    // 19/02/2017
    $('#AgregarCur').click('click', function () {
        if ($('#ABMN_CURSO').val() == "") {
            $('#ABMN_CURSO').addClass("input-validation-error");
        }
        else {
            $('#ABMN_CURSO').removeClass("input-validation-error");
        }
        alert("{nombre: '" + $('#ABMN_CURSO').val() + "',duracion: '" + $('#ABMDURACION').val() + "',meses:'" + $('#ABMMESES').val() + "',inicio:'" + $('#ABMCURSOF_INICIO').val() + "',fin:'" + $('#ABMCURSOF_FIN').val() + "',idEscuela:'" + $('#ABMIDNEscuelaCurso').val() + "'}");
        if ($('#ABMN_CURSO').val() != "") {
            $.ajax({
                type: "POST",
                url: "/Fichas/AgregarCur",
                data: "{nombre: '" + $('#ABMN_CURSO').val() + "',duracion: '" + $('#ABMDURACION').val() + "',meses:'" + $('#ABMMESES').val() + "',inicio:'" + $('#ABMCURSOF_INICIO').val() + "',fin:'" + $('#ABMCURSOF_FIN').val() + "',idEscuela:'" + $('#ABMIDNEscuelaCurso').val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#ABMN_CURSO').val("");
                    $('#ABMDURACION').val("");
                    $('#ABMMESES').val("");
                    $('#ABMCURSOF_INICIO').val("");
                    $('#ABMCURSOF_FIN').val("");
                    $("#ABMIDNEscuelaCurso").val("");
                    $("#ABMNEscuelaCurso").val("");

                    $('#grillaCurso').empty();
                    $('#grillaCurso').append(data);
                    $('#dialogCursos').dialog("open");
                },
                error: function (msg) {
                    alert(msg);
                }
            });
        }

    });


// 18/02/2013 - DI CAMPLI LEANDRO - IMPORTAR EXCEL CON ID FICHAS
$("#archivoFichas").change(function () {
    var ext = $('#archivoFichas').val().split('.').pop().toLowerCase();
    if ($.inArray(ext, ['xls']) == -1) {
        $('#btnsubirArchivoFichas').css('display', 'none');
        alert('Archivo inválido, por favor seleccione otro archivo.');
    }
    else {
        $('#btnsubirArchivoFichas').show();
    }
});
// 18/02/2013 - DI CAMPLI LEANDRO - IMPORTAR EXCEL CON ID FICHAS
$('#btnsubirArchivoFichas').click('click', function () {
    var f = $("#FormAltaMasivaBenef");
    $('#FormAltaMasivaBenef').attr('action', '/Fichas/SubirArchivoFichas');
    $("#FormAltaMasivaBenef").submit()
});


// 18/02/2013 - DI CAMPLI LEANDRO - IMPORTAR EXCEL CON ID FICHAS
$('#ProcAltaMasivaXLS').click('click', function () {
    var f = $("#FormAltaMasivaBenef");
    $('#FormAltaMasivaBenef').attr('action', '/Fichas/CargarMasivaFichasXLS');
    $("#FormAltaMasivaBenef").submit()
});

// 21/02/2013 - DI CAMPLI LEANDRO - CAMBIAR FICHAS A BENEFICIARIO DE MANERA MASIVA
//
$('#AltaMasiva').click('click', function () {
    var f = $("#FormAltaMasivaBenef");
    $('#FormAltaMasivaBenef').validate().cancelSubmit = true;
    $('#FormAltaMasivaBenef').attr('action', '/Fichas/UpdateCargaMasiva');
    $("#FormAltaMasivaBenef").submit()
});


 //10/06/2013 - Trae las etapas para usar de filtro
$('#TraerEtapa').click('click', function () {
    var f = $("#FormIndexFichas");
    //$('#FormularioEtapa').validate().cancelSubmit = true;
    $("#programaSel").val($("#EtapaProgramas_Selected").val());
    $("#filtroEjerc").val($("#ejerc").val());
    $("#BuscarPorEtapa").val("S");
    $('#FormIndexFichas').attr('action', '/Fichas/IndexTraerEtapas');
    $("#FormIndexFichas").submit()
});


//$('#TraerEtapa').click('click', function () {
//    var f = $("#FormIndexFichas");
//    //******************************************************

//    $.ajax({
//        type: "POST",
//        url: "/Fichas/IndexTraerEtapas",
//        data: "{programaSel:" + $("#programaSel").val() + ", filtroEjerc:'" + $("#filtroEjerc").val() + "'}",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (data) {
//            if (data != null) {

//                var options = '';
//                var cbo = data.Combo;
//                $.each(cbo, function (index, obj) {
//                    options += '<option value="' + obj.Id + '">' + obj.Description + '</option>';
//                });

//                $("#etapas_Selected").html(options);
//                $("#BuscarPorEtapa").val("S");
//                $("#etapas_Selected").show();
//                
//            }
//            else {


//            }
//        },
//        error: function (msg) {
//            alert(msg);
//        }
//    });
//    //******************************************************
//});


// 17/06/2013 - Quita el filtro de etapas
$('#QuitarEtapa').click('click', function () {
    var f = $("#FormIndexFichas");
    //$('#FormularioEtapa').validate().cancelSubmit = true;
    $("#BuscarPorEtapa").val("N");
    $('#FormIndexFichas').attr('action', '/Fichas/Index');
    $("#FormIndexFichas").submit()
});


function SeleccionarProyecto(idproyecto, nomproyecto, cantcapacitar, ini, fin, duracion) {


    $("#dialogProyectos").dialog("close");

    //Fichas RECONVERSION PRODUCTIVA--------------------
    $("#FichaReconversion_Proyecto_IdProyecto").val(idproyecto);
    $("#FichaReconversion_Proyecto_NombreProyecto").val(nomproyecto);
    $("#FichaReconversion_Proyecto_CantidadAcapacitar").val(cantcapacitar);
    $("#FichaReconversion_Proyecto_FechaInicioProyecto").val(ini);
    $("#FichaReconversion_Proyecto_FechaFinProyecto").val(fin);
    $("#FichaReconversion_Proyecto_MesesDuracionProyecto").val(duracion);
    //Fichas EFECTORES SOCIALES------------------------
    $("#FichaEfectores_Proyecto_IdProyecto").val(idproyecto);
    $("#FichaEfectores_Proyecto_NombreProyecto").val(nomproyecto);
    $("#FichaEfectores_Proyecto_CantidadAcapacitar").val(cantcapacitar);
    $("#FichaEfectores_Proyecto_FechaInicioProyecto").val(ini);
    $("#FichaEfectores_Proyecto_FechaFinProyecto").val(fin);
    $("#FichaEfectores_Proyecto_MesesDuracionProyecto").val(duracion);
    //-------------------------------------------------
    //Fichas CONFIAMOS EN VOS------------------------
    $("#FichaConfVos_Proyecto_IdProyecto").val(idproyecto);
    $("#FichaConfVos_Proyecto_NombreProyecto").val(nomproyecto);
    $("#FichaConfVos_Proyecto_CantidadAcapacitar").val(cantcapacitar);
    $("#FichaConfVos_Proyecto_FechaInicioProyecto").val(ini);
    $("#FichaConfVos_Proyecto_FechaFinProyecto").val(fin);
    $("#FichaConfVos_Proyecto_MesesDuracionProyecto").val(duracion);

    //-------------------------------------------------

}

function SeleccionarCurso(idcurso, nrocurso, nomcurso, ini, fin, dura, expediente, apedoc, nomdoc, celdoc, ldoc, maildoc, apetut, nomtut, celtut, ltut, mailtut) 
{


    $("#dialogCursos").dialog("close");

    //Fichas CONFIAMOS EN VOS------------------------
    $("#FichaConfVos_ID_CURSO").val(idcurso);
    $("#FichaConfVos_curso_NRO_COND_CURSO").val(nrocurso);
    $("#FichaConfVos_curso_N_CURSO").val(nomcurso);
    $("#FichaConfVos_curso_F_INICIO").val(ini);
    $("#FichaConfVos_curso_F_FIN").val(fin);
    $("#FichaConfVos_curso_DURACION").val(dura);
    $("#FichaConfVos_curso_EXPEDIENTE_LEG").val(expediente);
    $("#FichaConfVos_curso_DOC_APELLIDO").val(apedoc);
    $("#FichaConfVos_curso_DOC_NOMBRE").val(nomdoc);
    $("#FichaConfVos_curso_DOC_CELULAR").val(celdoc);
    $("#FichaConfVos_curso_DOC_N_LOCALIDAD").val(ldoc);
    $("#FichaConfVos_curso_DOC_MAIL").val(maildoc);
    $("#FichaConfVos_curso_TUT_APELLIDO").val(apetut);
    $("#FichaConfVos_curso_TUT_NOMBRE").val(nomtut);
    $("#FichaConfVos_curso_TUT_CELULAR").val(celtut);
    $("#FichaConfVos_curso_TUT_N_LOCALIDAD").val(ltut);
    $("#FichaConfVos_curso_TUT_MAIL").val(mailtut);

    //-------------------------------------------------
    //Fichas Ppp------------------------

    //-------------------------------------------------
}

function SeleCursoEsc(idcurso, nomcurso, ini, fin,dura, meses, escuela, cue, regise) {


    $("#dialogCursos").dialog("close");
    //alert("pepe");
    //Fichas Ppp------------------------
    $("#FichaPpp_ID_CURSO").val(idcurso);
    $("#FichaPpp_cursoPpp_N_CURSO").val(nomcurso);
    $("#FichaPpp_cursoPpp_F_INICIO").val(ini);
    $("#FichaPpp_cursoPpp_F_FIN").val(fin);
    $("#FichaPpp_cursoPpp_DURACION").val(dura);
//    $("#FichaPpp_curso_EXPEDIENTE_LEG").val(expediente);
    $("#FichaPpp_cursoPpp_Meses").val(meses);
    $("#FichaPpp_cursoPpp_NEscuelaCurso").val(escuela);
    $("#FichaPpp_cursoPpp_CueCurso").val(cue);
    $("#FichaPpp_cursoPpp_regiseCurso").val(regise);


    //-------------------------------------------------

}


$('#DesvincularEmpresas').click('click', function () {
    $('#dialogDesvincularEmp').dialog("open");
});




$(function () {
    $(".validar").keydown(function (event) {
        if (event.shiftKey) {
            event.preventDefault();
        }

        if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9) {
        }
        else {
            if (event.keyCode < 95) {
                if (event.keyCode < 48 || event.keyCode > 57) {
                    event.preventDefault();
                }
            }
            else {
                if (event.keyCode < 96 || event.keyCode > 105) {
                    event.preventDefault();
                }
            }
        }
    });
});



//*******************************************************
$('#BuscarFacMonitor').click('click', function () {
    //        $('#ABMNombreSede').removeClass("input-validation-error");
    //        $('#ABMSedeTelefonoContacto').removeClass("input-validation-error");
    
    
    $.ajax({
        type: "POST",
        url: "/Fichas/obtenerFacilitadorMonitor",
        data: "{dni:'" + $('#FichaConfVos_facilitador_monitor_DNI').val() + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#grillaActores').empty();
            $('#grillaActores').append(data);
            $('#dialogActores').dialog("open");
        },
        error: function (msg) {
            alert(msg);
        }
    });
});
//*********************************************************

//*******************************************************
$('#BuscarGestor').click('click', function () {
    //        $('#ABMNombreSede').removeClass("input-validation-error");
    //        $('#ABMSedeTelefonoContacto').removeClass("input-validation-error");


    $.ajax({
        type: "POST",
        url: "/Fichas/obtenerActor",
        data: "{dni:'" + $('#FichaConfVos_gestor_DNI').val() + "',idRolActor:'7'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#grillaActores').empty();
            $('#grillaActores').append(data);
            $('#dialogActores').dialog("open");
        },
        error: function (msg) {
            alert(msg);
        }
    });
});
//*********************************************************

function SeleccionarActor(idActor, dni, ape, nom, cel, mail, barrio, localidad, idRol) {


    $("#dialogActores").dialog("close");

    //Fichas CONFIAMOS EN VOS------------------------
    if (idRol == 5 || idRol == 9) {
        $("#FichaConfVos_ID_FACILITADOR").val(idActor);
        $("#FichaConfVos_facilitador_monitor_DNI").val(dni);
        $("#FichaConfVos_facilitador_monitor_APELLIDO").val(ape);
        $("#FichaConfVos_facilitador_monitor_NOMBRE").val(nom);
        $("#FichaConfVos_facilitador_monitor_CELULAR").val(cel);
        $("#FichaConfVos_facilitador_monitor_MAIL").val(mail);
        $("#FichaConfVos_facilitador_monitor_N_BARRIO").val(barrio);
        $("#FichaConfVos_facilitador_monitor_N_LOCALIDAD").val(localidad);
    }
    if (idRol == 7) {
        $("#FichaConfVos_ID_GESTOR").val(idActor);
        $("#FichaConfVos_gestor_DNI").val(dni);
        $("#FichaConfVos_gestor_APELLIDO").val(ape);
        $("#FichaConfVos_gestor_NOMBRE").val(nom);
        $("#FichaConfVos_gestor_CELULAR").val(cel);
        $("#FichaConfVos_gestor_MAIL").val(mail);
        $("#FichaConfVos_gestor_N_BARRIO").val(barrio);
        $("#FichaConfVos_gestor_N_LOCALIDAD").val(localidad);
    }

    //-------------------------------------------------

}

//*******************************************************
$('#BuscarONG').click('click', function () {
    //        $('#ABMNombreSede').removeClass("input-validation-error");
    //        $('#ABMSedeTelefonoContacto').removeClass("input-validation-error");


    $.ajax({
        type: "POST",
        url: "/Fichas/obtenerONG",
        data: "{cuit:'" + $('#FichaConfVos_ong_CUIT').val() + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#grillaONG').empty();
            $('#grillaONG').append(data);
            $('#dialogONG').dialog("open");
        },
        error: function (msg) {
            alert(msg);
        }
    });
});
//*********************************************************

//*******************************************************
$('#BuscarONGNom').click('click', function () {
    //        $('#ABMNombreSede').removeClass("input-validation-error");
    //        $('#ABMSedeTelefonoContacto').removeClass("input-validation-error");


    $.ajax({
        type: "POST",
        url: "/Fichas/obtenerONGNom",
        data: "{nombre:'" + $('#FichaConfVos_ong_N_NOMBRE').val() + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#grillaONG').empty();
            $('#grillaONG').append(data);
            $('#dialogONG').dialog("open");
        },
        error: function (msg) {
            alert(msg);
        }
    });
});
//*********************************************************
//*******************************************************
$('#BuscarONGPpp').click('click', function () {
    //        $('#ABMNombreSede').removeClass("input-validation-error");
    //        $('#ABMSedeTelefonoContacto').removeClass("input-validation-error");
    $('#ABMNombreONG').val("");
    $('#ABMONGCUIT').val("");
    $('#ABMONGCelular').val("");
    $('#ABMONGTelefono').val("");
    $('#ABMSedeTelefonoContacto').val("");
    $('#ABMONGMail').val("");

    $.ajax({
        type: "POST",
        url: "/Fichas/obtenerONG",
        data: "{cuit:'" + $('#FichaPpp_ong_CUIT').val() + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#grillaONG').empty();
            $('#grillaONG').append(data);
            $('#dialogONG').dialog("open");
        },
        error: function (msg) {
            alert(msg);
        }
    });
});
//*********************************************************

//*******************************************************
$('#BuscarONGNomPpp').click('click', function () {
    //        $('#ABMNombreSede').removeClass("input-validation-error");
    //        $('#ABMSedeTelefonoContacto').removeClass("input-validation-error");
    $('#ABMNombreONG').val("");
    $('#ABMONGCUIT').val("");
    $('#ABMONGCelular').val("");
    $('#ABMONGTelefono').val("");
    $('#ABMSedeTelefonoContacto').val("");
    $('#ABMONGMail').val("");

    $.ajax({
        type: "POST",
        url: "/Fichas/obtenerONGNom",
        data: "{nombre:'" + $('#FichaPpp_ong_N_NOMBRE').val() + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#grillaONG').empty();
            $('#grillaONG').append(data);
            $('#dialogONG').dialog("open");
        },
        error: function (msg) {
            alert(msg);
        }
    });
});
//*********************************************************
$('#AgregarONG').click('click', function () {
    if ($('#ABMNombreONG').val() == "") {
        $('#ABMNombreONG').addClass("input-validation-error");
    }
    else {
        $('#ABMNombreONG').removeClass("input-validation-error");
    }
    //alert("hola");
    if ($('#ABMNombreONG').val() != "") {
        $.ajax({
            type: "POST",
            url: "/Fichas/AddOngFicha",
            data: "{nombre: '" + $('#ABMNombreONG').val() + "',cuit: '" + $('#ABMONGCUIT').val() + "',celular:'" + $('#ABMONGCelular').val() + "',telefono:'" + $('#ABMONGTelefono').val() + "',mail:'" + $('#ABMONGMail').val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('#grillaONG').empty();
                $('#grillaONG').append(data);
                $('#dialogONG').dialog("open");
            },
            error: function (msg) {
                alert(msg);
            }
        });
    }

});

function SeleccionaONG(idong, cuit, nom, cel, tel, mail, aperes, nomres, celres, lres, mailres, apecon, nomcon, celcon, lcon, mailcon) {


    $("#dialogONG").dialog("close");
    //alert(idong);
    //------------------------
    $("#FichaConfVos_ID_ONG").val(idong);
    $("#FichaConfVos_ong_CUIT").val(cuit);
    $("#FichaConfVos_ong_N_NOMBRE").val(nom);
    $("#FichaConfVos_ong_CELULAR").val(cel);
    $("#FichaConfVos_ong_TELEFONO").val(tel);
    $("#FichaConfVos_ong_MAIL_ONG").val(mail);
    $("#FichaConfVos_ong_RES_APELLIDO").val(aperes);
    $("#FichaConfVos_ong_RES_NOMBRE").val(nomres);
    $("#FichaConfVos_ong_RES_CELULAR").val(celres);
    $("#FichaConfVos_ong_RES_N_LOCALIDAD").val(lres);
    $("#FichaConfVos_ong_RES_MAIL").val(mailres);
    $("#FichaConfVos_ong_CON_APELLIDO").val(apecon);
    $("#FichaConfVos_ong_CON_NOMBRE").val(nomcon);
    $("#FichaConfVos_ong_CON_CELULAR").val(celcon);
    $("#FichaConfVos_ong_CON_N_LOCALIDAD").val(lcon);
    $("#FichaConfVos_ong_CON_MAIL").val(mailcon);

    //--PPP-------------------------
    $("#FichaPpp_ID_ONG").val(idong);
    $("#FichaPpp_ong_CUIT").val(cuit);
    $("#FichaPpp_ong_N_NOMBRE").val(nom);
    $("#FichaPpp_ong_CELULAR").val(cel);
    $("#FichaPpp_ong_TELEFONO").val(tel);
    $("#FichaPpp_ong_MAIL_ONG").val(mail);
    $("#FichaPpp_ong_RES_APELLIDO").val(aperes);
    $("#FichaPpp_ong_RES_NOMBRE").val(nomres);
    $("#FichaPpp_ong_RES_CELULAR").val(celres);
    $("#FichaPpp_ong_RES_N_LOCALIDAD").val(lres);
    $("#FichaPpp_ong_RES_MAIL").val(mailres);
    $("#FichaPpp_ong_CON_APELLIDO").val(apecon);
    $("#FichaPpp_ong_CON_NOMBRE").val(nomcon);
    $("#FichaPpp_ong_CON_CELULAR").val(celcon);
    $("#FichaPpp_ong_CON_N_LOCALIDAD").val(lcon);
    $("#FichaPpp_ong_CON_MAIL").val(mailcon);

    //-------------------------------------------------

}

$('#DesvincularONG').click('click', function () {
    $('#dialogDesvincularOng').dialog("open");
});





$('#DesvincularGestor').click('click', function () {
    $('#dialogDesvincularGestor').dialog("open");
});



$('#DesvincularFacMon').click('click', function () {
    $('#dialogDesvincularFacMon').dialog("open");
});



$('#DesvincularCurso').click('click', function () {
    $('#dialogDesvincularCurso').dialog("open");
});



//*******************************************************
$('#BuscarSubProg').click('click', function () {
    //        $('#ABMNombreSede').removeClass("input-validation-error");
    //        $('#ABMSedeTelefonoContacto').removeClass("input-validation-error");
    var nombre = "";
    if ($('#TipoFicha').val() == 3) {
        if ($('#FichaPpp_Subprograma').val() != "") { nombre = $('#FichaPpp_Subprograma').val(); }
        //alert("paso ppp");
    }
    else {
        //alert("paso otros");
        //alert($('#TipoFicha').val());
        if ($('#Subprograma').val() != "") { nombre = $('#Subprograma').val(); }
    }
    $.ajax({
        type: "POST",
        url: "/Fichas/obtenerSubProg",
        data: "{nombre:'" + nombre + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#grillaSubProg').empty();
            $('#grillaSubProg').append(data);
            $('#dialogSubProg').dialog("open");
                        //alert(data);
        },
        error: function (msg) {
            //alert(msg);
        }
    });
});


function SeleccionaSubP(IdSubprograma, NombreSubprograma, Descripcion, monto) {


    $("#dialogSubProg").dialog("close");

    //------------------------
    $("#IdSubprograma").val(IdSubprograma);
    if ($('#TipoFicha').val() == 3) {

        $("#FichaPpp_Subprograma").val(NombreSubprograma);
        $("#FichaPpp_monto").val(monto);
    }
    else {

        $("#Subprograma").val(NombreSubprograma);
        $("#monto").val(monto);
    }

   

    //-------------------------------------------------

}

$('#DesvincularSubP').click('click', function () {
    $('#dialogDesvincularSubP').dialog("open");
});




//*********************************************************

//*******************************************************
$('#BuscarEscuela').click('click', function () {

    var nombre = "";
    //if ($('#FichaPpp_escuelaPpp_Nombre_Escuela').val() != "") { nombre = $('#FichaPpp_escuelaPpp_Nombre_Escuela').val(); }
    if ($('#FichaPpp_N_ESCUELA').val() != "") { nombre = $('#FichaPpp_N_ESCUELA').val(); }
    $.ajax({
        type: "POST",
        url: "/Fichas/obtenerEscuela",
        data: "{nombre:'" + nombre + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#ABMN_ESCUELA').val("");
            $('#ABMCALLEEsc').val("");
            $('#ABMNro').val("");
            $('#ABMTELEFONOEsc').val("");
            $('#ABMCueEsc').val("");
            $('#ABMRegiseEsc').val("");
            $('#grillaEscuela').empty();
            $('#grillaEscuela').append(data);
            $('#dialogEscuela').dialog("open");
        },
        error: function (msg) {
            alert(msg);
        }
    });
});

$('#AgregarEsc').click('click', function () {
    if ($('#ABMN_ESCUELA').val() == "") {
        $('#ABMN_ESCUELA').addClass("input-validation-error");
    }
    else {
        $('#ABMN_ESCUELA').removeClass("input-validation-error");
    }
    if ($('#ABMCueEsc').val() != "") {
        if ($('#ABMCueEsc').val().length > 7) {
            //        $('#ABMCueEsc').attr('data-val-length', 'CUE supera los 7 caracteres.');
            //        $('#ABMCueEsc').attr('data-val-length-max', '7');
            //        $('#ABMCueEsc').attr('data-val', 'true');
            $('#ABMCueEsc').addClass("input-validation-error");
            alert("CUE supera los 7 caracteres.");
        }
        else {

            $('#ABMCueEsc').removeClass("input-validation-error");
        }
    }
    else {

        $('#ABMCueEsc').removeClass("input-validation-error");
    }
    if ($('#ABMN_ESCUELA').val() != "") {


        $.ajax({
            type: "POST",
            url: "/Fichas/AgregarEsc",
            data: "{nombre: '" + $('#ABMN_ESCUELA').val() + "',calle: '" + $('#ABMCALLEEsc').val() + "',nro:'" + $('#ABMNro').val() + "',telefono:'" + $('#ABMTELEFONOEsc').val() + "',cue:'" + $('#ABMCueEsc').val() + "',Regise:'" + $('#ABMRegiseEsc').val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('#ABMN_ESCUELA').val("");
                $('#ABMCALLEEsc').val("");
                $('#ABMNro').val("");
                $('#ABMTELEFONOEsc').val("");
                $('#ABMCueEsc').val("");
                $('#ABMRegiseEsc').val("");
                $('#grillaEscuela').empty();
                $('#grillaEscuela').append(data);
                $('#dialogEscuela').dialog("open");


            },
            error: function (msg) {
                alert(msg);
            }
        });
    }

});

//function SeleccionaEscuela() { $("#dialogEscuela").dialog("close"); }
function SeleccionaEscuela(IdEscuela, NombreEscuela, calle, numero, telefono, Cue, regise) {


    $("#dialogEscuela").dialog("close");

    //------------------------
    $("#FichaPpp_Id_Escuela").val(IdEscuela);
    $("#FichaPpp_N_ESCUELA").val(NombreEscuela);
    $("#FichaPpp_CALLEEsc").val(calle);
    $("#FichaPpp_NUMEROEsc").val(numero);
    $("#FichaPpp_TELEFONOEsc").val(telefono);
    $("#FichaPpp_CueEsc").val(Cue);
    $("#FichaPpp_RegiseEsc").val(regise);

    //-------------------------------------------------

}

$('#DesvincularEscuela').click('click', function () {
    $('#dialogDesvincularEsc').dialog("open");
});




//*********************************************************


if ($("#ProgPorRol").val() == "S") //verificar si tiene la acción activa para consultar los subprogramas
{
    if ($("#idFormulario").val() == 'FormIndexFichas') {

        //******************************************************

        $.ajax({
            type: "POST",
            url: "/Subprogramas/SubprogramasRoles",
            data: "{idPrograma:'" + $("#ComboTipoFicha_Selected").val() + "'}",
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
        if ($('#ComboTipoFicha_Selected').val() == 3) {
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
    if ($("#idFormulario").val() == 'FormIndexFichas') {
        //******************************************************

        $.ajax({
            type: "POST",
            url: "/Subprogramas/SubprogramasProg",
            data: "{idPrograma:'" + $("#ComboTipoFicha_Selected").val() + "'}",
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
        if ($('#ComboTipoFicha_Selected').val() == 3) {
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





$('#ModificarFamilia').click('click', function () {
    $('#FormularioFichas').validate().cancelSubmit = true;
    var url = '/Personas/Familia?idficha=' + $('#IdFicha').val();
    $('#FormularioFichas').attr('action', url);
    $("#FormularioFichas").submit();
});