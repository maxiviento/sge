
$(document).ready(function () {

    var dates = $("#from,#to").datepicker({
        changeMonth: true,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 2,
        onSelect: function (selectedDate) {
            var option = this.id == "from" ? "maxDate" : "minDate",
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
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                $(this).dialog("close");
            }

        }
    });

    $('#dialogCambioEstado').dialog({
        //open: function (event, ui) { $('.ui-draggable').attr("style", "outline: 0px; height: auto; width: 450px; top: 350px; left: 444px; display: block; z-index: 1002;"); },
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                var f = $("#FormularioLiquidacion");
                $('#FormularioLiquidacion').attr('action', '/Liquidacion/CambiarEstado');
                $("#FormularioLiquidacion").submit()
                $(this).dialog("close");
            },
            "Cancel": function () {
                document.execCommand('SaveAs', '1', 'give img location here');
                $(this).dialog("close");
            }
        }
    });





    $('#Buscar').click('click', function () {
        var f = $("#FormIndexLiquidacion");
        $('#FormIndexLiquidacion').attr('action', '/Liquidacion/BuscarBeneficiario');
        $("#FormIndexLiquidacion").submit()
    });

    $('#BuscarLiquidacion').click('click', function () {
        var f = $("#FormIndexLiquidacion");
        $('#FormIndexLiquidacion').attr('action', '/Liquidacion/BuscarLiquidaciones');
        $("#FormIndexLiquidacion").submit()
    });

    $('#btnBuscarBeneficiario').click('click', function () {
        var f = $("#FormularioLiquidacion");
        $('#FormularioLiquidacion').attr('action', '/Liquidacion/BuscarBeneficiario');
        $("#FormularioLiquidacion").submit()
    });

    $('#btnVolverFormulario').click('click', function () {
        var f = $("#FormularioLiquidacion");
        $('#FormularioLiquidacion').attr('action', '/Liquidacion/VolverFormulario');
        $("#FormularioLiquidacion").submit()
    });


    $('#btnGuardarEstado').click('click', function () {
        $('#dialogCambioEstado').dialog("open");
    });



    $('#btnVolverFormBeneficiario').click('click', function () {
        if ($('#CountConceptos').val() > 0) {
            $('#FormularioLiquidacion').attr('action', '/Liquidacion/VolverFormBeneficiario');
            $("#FormularioLiquidacion").submit()
        }
        else {
            $("#dialog").dialog("open");
        }

    });

    $('#btnVolverFormBeneficiarioDesdeImporte').click('click', function () {

        $('#FormularioModificacionMontoLiquidar').attr('action', '/Liquidacion/VolverFormBeneficiarioDesdeImporte');
        $("#FormularioModificacionMontoLiquidar").submit()


    });


    $('#btnCargarConceptosFormulario').click('click', function () {
        var f = $("#FormularioLiquidacion");
        $("#Convenios_Selected").val();
        $('#FormularioLiquidacion').attr('action', '/Liquidacion/CargarConceptos');
        $("#FormularioLiquidacion").submit()
    });

    $('#GenerarFile').click('click', function () {
        var f = $("#FormIndexLiquidacion");
        $('#FormIndexLiquidacion').attr('action', '/Liquidacion/GenerarFile');
        $("#FormIndexLiquidacion").submit()
    });

    $('#btnGuardar').click('click', function () {
        var f = $("#FormularioLiquidacion");
        $('#FormularioLiquidacion').attr('action', '/Liquidacion/GuardaLiquidacion');
        $("#FormularioLiquidacion").submit()
    });


    $('#btnAgregarConcepto').click('click', function () {
        var f = $("#FormularioLiquidacion");
        $('#FormularioLiquidacion').attr('action', '/Liquidacion/AgregarConcepto');
        $("#FormularioLiquidacion").submit()
    });

    $('#btnVerImporte').click('click', function () {
        var f = $("#FormularioLiquidacion");
        $('#FormularioLiquidacion').attr('action', '/Liquidacion/VerMontoBeneficiario');
        $("#FormularioLiquidacion").submit()
    });

    $('#btnEditarImporte').click('click', function () {




        $('#FormularioModificacionMontoLiquidar').attr('action', '/Liquidacion/EditarMontoLiquidarBeneficiario');
        $("#FormularioModificacionMontoLiquidar").submit()
    });

    $("#archivoliquidacion").change(function () {
        var ext = $('#archivoliquidacion').val().split('.').pop().toLowerCase();
        if ($.inArray(ext, ['hab']) == -1) {
            $('#btnsubir').css('display', 'none');
            alert('Archivo inválido, por favor seleccione otro archivo.');
        }
        else {
            $('#btnsubir').show();
        }
    });

    $('#btnsubir').click('click', function () {
        var f = $("#FormImportar");
        $('#FormImportar').attr('action', '/Liquidacion/SubirArchivo');
        $("#FormImportar").submit()
    });

    $('#btnProcesarLiquidacion').click('click', function () {
        var f = $("#FormImportar");
        $('#FormImportar').attr('action', '/Liquidacion/ProcesarLiquidacion');
        $("#FormImportar").submit()
    });

    $('#btnExportarLiquidacion').click('click', function () {
        $('#FormularioLiquidacion').attr('action', '/Liquidacion/ExportarLiquidacion');
        $("#FormularioLiquidacion").submit()
    });

    $('#btnCargarConceptosFormularioRepago').click('click', function () {
        $('#FormularioLiquidacion').attr('action', '/Liquidacion/PantallaImporatdoraBeneficiarioRepago');
        $("#FormularioLiquidacion").submit()
    });


    $("#archivorepago").change(function () {
        var ext = $('#archivorepago').val().split('.').pop().toLowerCase();
        if ($.inArray(ext, ['xls']) == -1) {
            $('#btnsubirArchivoRepago').css('display', 'none');
            alert('Archivo inválido, por favor seleccione otro archivo.');
        }
        else {
            $('#btnsubirArchivoRepago').show();
        }
    });

    $('#btnsubirArchivoRepago').click('click', function () {
        var f = $("#FormImportar");
        $('#FormImportar').attr('action', '/Liquidacion/SubirArchivoRepago');
        $("#FormImportar").submit()
    });

    $('#btnProcesarLiquidacionRepago').click('click', function () {
        var f = $("#FormImportar");
        $('#FormImportar').attr('action', '/Liquidacion/CargarConceptosRepago');
        $("#FormImportar").submit()
    });






});