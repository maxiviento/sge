$(document).ready(function () {
    $('.input-validation-error').removeClass("input-validation-error");
    $('.field-validation-error').addClass("field-validation-valid");

    $('#Buscar').click('click', function () {
        $('#FormIndexFichasPPP').attr('action', '/FichaPPP/BuscarFichaPPP');
        $("#FormIndexFichasPPP").submit()
    });


});