$(document).ready(function () {
    $('.input-validation-error').removeClass("input-validation-error");
    $('.field-validation-error').addClass("field-validation-valid");

    $('#Buscar').click('click', function () {
        $('#FormIndexLocalidad').attr('action', '/Localidades/BuscarLocalidad');
        $("#FormIndexLocalidad").submit()
    });

    $('#Agregar').click('click', function () {
        $('#FormularioLocalidad').attr('action', '/Localidades/AgregarLocalidad');
        $("#FormularioLocalidad").submit()
    });

    $('#Modificar').click('click', function () {
        $('#FormularioLocalidad').attr('action', '/Localidades/ModificarLocalidad');
        $("#FormularioLocalidad").submit()
    });
});