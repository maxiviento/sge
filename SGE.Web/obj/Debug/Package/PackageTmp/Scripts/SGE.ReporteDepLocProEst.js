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


    $("#BusquedaProgramas_Selected").change(function () {

        if ($('#BusquedaProgramas_Selected').val() == 3) {
            $("#ComboTiposPpp_Selected").show();
            $("#lblTipoPpp").show();
        }
        else {
            $("#ComboTiposPpp_Selected").hide();
            $("#lblTipoPpp").hide();
        }

    });



}); // Fin Ready

if ($('#BusquedaProgramas_Selected').val() == 3) {
    $("#ComboTiposPpp_Selected").show();
    $("#lblTipoPpp").show();
}
else {
    $("#ComboTiposPpp_Selected").hide();
    $("#lblTipoPpp").hide();
}