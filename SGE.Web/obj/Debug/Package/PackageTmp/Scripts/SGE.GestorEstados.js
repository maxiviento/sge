$(document).ready(function () {




    $("#EJERCICIO").mask("9999");




    $('#Paginador').smartpaginator({ totalrecords: $('#ContFichas').val()
, recordsperpage: 10, datacontainer: 'tblFichas', dataelement: 'tr',
        initval: 0, next: 'Siguiente', prev: 'Anterior', first: 'Primero', last: 'Ultimo', theme: 'green'
    });

    var dates = $("#FEC_INICIO,#FEC_FIN").datepicker({
        changeMonth: true,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 2,
        onSelect: function (selectedDate) {
            var option = this.id == "FEC_FIN" ? "maxDate" : "minDate",
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




    $('#AgregarEtapa').click('click', function () {
        //$('#FormularioEtapa').validate().cancelSubmit = true;
        $('#FormularioEtapa').attr('action', '/Etapa/AgregarEtapa');
        $("#FormularioEtapa").submit()
    });

    $('#ModificarEtapa').click('click', function () {
        //$('#FormularioEtapa').validate().cancelSubmit = true;
        $('#FormularioEtapa').attr('action', '/Etapa/modificarEtapa');
        $("#FormularioEtapa").submit()
    });

    $('#CancelarEtapa').click('click', function () {
        $('#FormularioEtapa').validate().cancelSubmit = true;
        $('#FormularioEtapa').attr('action', '/Etapa/Index');
        $("#FormularioEtapa").submit()
    });



    $('#Buscar').click('click', function () {
        var f = $("#FormIndexEtapa");
        $('#FormIndexEtapa').attr('action', '/Etapa/GetEtapas');
        $("#FormIndexEtapa").submit()
    });


    $('#TraerEtapas').click('click', function () {
        var f = $("#FormAsociarEtapa");
        //$('#FormularioEtapa').validate().cancelSubmit = true;
        $("#programaSel").val($("#Programas_Selected").val());
        $("#filtroEjerc").val($("#ejerc").val());
        $('#FormAsociarEtapa').attr('action', '/Etapa/GetEtapasAsignar');
        $("#FormAsociarEtapa").submit()
    });


    $("#etapas_Selected").change(function () {
        var f = $("#FormAsociarEtapa");
        //$('#FormAsociarEtapa').validate().cancelSubmit = true;
        $('#FormAsociarEtapa').attr('action', '/Etapa/GetEtapaAsignar');
        $("#FormAsociarEtapa").submit()
    });


    $('#Paso3').click('click', function () {
        var f = $("#FormAsociarEtapa");
        $("#filtroEtapa").val($("#etapas_Selected").val());
        $('#FormAsociarEtapa').attr('action', '/Etapa/EtapaAsignarPaso3');
        $("#FormAsociarEtapa").submit()
    });


    //    // 20/05/2013 - DI CAMPLI LEANDRO - IMPORTAR EXCEL CON ID FICHAS
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

    //    // 20/02/2013 - DI CAMPLI LEANDRO - IMPORTAR EXCEL CON ID FICHAS
    $('#btnsubirArchivoFichas').click('click', function () {
        var f = $("#FormGestorEstadosFicha");
        $("#idEstadoFicha").val($("#ComboEstadoFicha_Selected").val());
        $('#FormGestorEstadosFicha').attr('action', '/Fichas/SubirArchivoGestor');
        $("#FormGestorEstadosFicha").submit()
    });



    $('#AsignarEtapa').click('click', function () {
        var f = $("#FormAsociarEtapa");
        $('#FormAsociarEtapa').validate().cancelSubmit = true;
        $('#FormAsociarEtapa').attr('action', '/Etapa/udpEtapaFichas');
        $("#FormAsociarEtapa").submit()
        });




    //FIN READY
});


