$(document).ready(function () {
    $('.input-validation-error').removeClass("input-validation-error");
    $('.field-validation-error').addClass("field-validation-valid");

    $('#Buscar').click('click', function () {
        $('#FormIndexUniversidad').attr('action', '/Universidades/BuscarUniversidad');
        $("#FormIndexUniversidad").submit()
    });

    $('#Agregar').click('click', function () {
        $('#FormularioUniversidad').attr('action', '/Universidades/AgregarUniversidad');
        $("#FormularioUniversidad").submit()
    });

    $('#Modificar').click('click', function () {
        $('#FormularioUniversidad').attr('action', '/Universidades/ModificarUniversidad');
        $("#FormularioUniversidad").submit()
    });
});