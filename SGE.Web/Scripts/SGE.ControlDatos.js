$(document).ready(function () {
    $('.input-validation-error').removeClass("input-validation-error");
    $('.field-validation-error').addClass("field-validation-valid");

    $('#Buscar').click('click', function () {
        $('#FormIndexControlDatos').attr('action', '/ControlDatos/BuscarControlDatos');
        $("#FormIndexControlDatos").submit()
    });


    $('#TraerEtapa').click('click', function () {
        $("#programaSel").val($("#EtapaProgramas_Selected").val());
        $("#filtroEjerc").val($("#ejerc").val());
        $("#BuscarPorEtapa").val("S");
        $('#FormIndexControlDatos').attr('action', '/ControlDatos/IndexTraerEtapas');
        $("#FormIndexControlDatos").submit()
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


    $('#QuitarEtapa').click('click', function () {
        var f = $("#FormIndexControlDatos");
        //$('#FormularioEtapa').validate().cancelSubmit = true;
        $("#BuscarPorEtapa").val("N");
        $('#FormIndexControlDatos').attr('action', '/ControlDatos/Index');
        $("#FormIndexControlDatos").submit()
    });


    $('#ExportControlDatos').click('click', function () {
        $('#FormIndexControlDatos').attr('action', '/ControlDatos/ExportarControlDatos');
        $("#FormIndexControlDatos").submit()
    });

    //Fin document.ready
});

