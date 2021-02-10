$(document).ready(function () {
    //    $("#ExportBeneficiario").hide();

    $("#BusquedaSectorProductivo_Selected").change(function () {
        var f = $("#FormIndexRBecasAcademicas");
        $('#FormIndexRBecasAcademicas').validate().cancelSubmit = true;
        $('#FormIndexRBecasAcademicas').attr('action', '/Reportes/CambiarComboRBecas');
        $("#FormIndexRBecasAcademicas").submit()
    });

    $("#BusquedaInstituciones_Selected").change(function () {
        var f = $("#FormIndexRBecasAcademicas");
        $('#FormIndexRBecasAcademicas').validate().cancelSubmit = true;
        $('#FormIndexRBecasAcademicas').attr('action', '/Reportes/CambiarComboRBecas');
        $("#FormIndexRBecasAcademicas").submit()
    });

    $('#Buscar').click('click', function () {
        $('#FormIndexRBecasAcademicas').attr('action', '/Reportes/GenerarRBecasAcademicas');
        $("#FormIndexRBecasAcademicas").submit()
    });

    $('#ExportBeneficiario').click('click', function () {
        $('#FormIndexRBecasAcademicas').attr('action', '/Reportes/ExportarRBecasAcademicas');
        $("#FormIndexRBecasAcademicas").submit()
    });
    
});