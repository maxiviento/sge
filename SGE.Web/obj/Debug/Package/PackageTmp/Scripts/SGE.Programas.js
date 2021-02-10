$(document).ready(function () {
    $('.input-validation-error').removeClass("input-validation-error");
    $('.field-validation-error').addClass("field-validation-valid");

    $('#Buscar').click('click', function () {
        $('#FormIndexPrograma').attr('action', '/Programas/BuscarPrograma');
        $("#FormIndexPrograma").submit()
    });

    $('#Agregar').click('click', function () {
        $('#FormularioPrograma').attr('action', '/Programas/AgregarPrograma');
        $("#FormularioPrograma").submit()
    });

    $('#Modificar').click('click', function () {
        $('#FormularioPrograma').attr('action', '/Programas/ModificarPrograma');
        $("#FormularioPrograma").submit()
    });
});