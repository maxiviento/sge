$(document).ready(function () {
//    $("#ExportBeneficiario").hide();

    $("#BusquedaDepartamentos_Selected").change(function () {
        var f = $("#FormIndexSucursalCobertura");
        $('#FormIndexRDepLocProEst').validate().cancelSubmit = true;
        $('#FormIndexRDepLocProEst').attr('action', '/Reportes/CambiarComboRDepLocProgEst');
        $("#FormIndexRDepLocProEst").submit()
    });

//    $("#Estados_Selected").change(function () {
//        $("#Estado").val($("#Estados_Selected").val());
//        if ($("#Estados_Selected").val() == "0")
//        { $("#ExportBeneficiario").hide(); }
//        else
//        { $("#ExportBeneficiario").show(); }
//    });

    $('#Buscar').click('click', function () {
        $('#FormIndexRDepLocProEst').attr('action', '/Reportes/GenerarRDepLocProgEst');
        $("#FormIndexRDepLocProEst").submit()
    });

    if ($("#Estado").val() == "3") {
        $("#Estados_Selected").val(3);
        $("#ExportBeneficiario").show();
    }
    


    $('#ExportBeneficiario').click('click', function () {
        $('#FormIndexRDepLocProEst').attr('action', '/Reportes/ExportarRDepLocProgEst');
        $("#FormIndexRDepLocProEst").submit()
    });

    $('#RptArtPpp').click('click', function () {
        $('#FormIndexReportesArt').attr('action', '/Beneficiarios/ReporteArtPpp');
        $("#FormIndexReportesArt").submit()
    });

    $('#RptArtReconv').click('click', function () {
        $('#FormIndexReportesArt').attr('action', '/Beneficiarios/ReporteArtReconversion');
        $("#FormIndexReportesArt").submit()
    });

    $('#RptArtEfect').click('click', function () {
        $('#FormIndexReportesArt').attr('action', '/Beneficiarios/ReporteArtEfectores');
        $("#FormIndexReportesArt").submit()
    });

    $('#RptArtVat').click('click', function () {
        $('#FormIndexReportesArt').attr('action', '/Beneficiarios/ReporteArtVat');
        $("#FormIndexReportesArt").submit()
    });

    $('#RptArtPprof').click('click', function () {
        $('#FormIndexReportesArt').attr('action', '/Beneficiarios/ReporteArtPppp');
        $("#FormIndexReportesArt").submit()
    });

    $('#RptArtConfVos').click('click', function () {
        $('#FormIndexReportesArt').attr('action', '/Beneficiarios/ReporteArtConfVos');
        $("#FormIndexReportesArt").submit()
    });

});