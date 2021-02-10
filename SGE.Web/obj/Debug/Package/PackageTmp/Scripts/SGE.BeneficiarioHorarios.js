$(document).ready(function () {
//    $("#ExportBeneficiario").hide();


//    $("#Estados_Selected").change(function () {
//        $("#Estado").val($("#Estados_Selected").val());
//        if ($("#Estados_Selected").val() == "0")
//        { $("#ExportBeneficiario").hide(); }
//        else
//        { $("#ExportBeneficiario").show(); }
//    });


    $('#RptBeneficiarioHorariosPpp').click('click', function () {
        $('#FormIndexBeneficiarioHorarios').attr('action', '/Beneficiarios/RptBeneficiarioHorarios');
        $("#FormIndexBeneficiarioHorarios").submit()
    });



});