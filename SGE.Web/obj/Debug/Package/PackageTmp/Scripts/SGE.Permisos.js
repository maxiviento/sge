$(document).ready(function () {
    $('.input-validation-error').removeClass("input-validation-error");
    $('.field-validation-error').addClass("field-validation-valid");

    $('#Buscar').click('click', function () {
        $('#FormIndexPermisos').attr('action', '/Permisos/BuscarRoles');
        $("#FormIndexPermisos").submit()
    });

    $('#Modificar').click('click', function () {
        $('#FormularioPermisos').attr('action', '/Permisos/ModificarPermiso');
        $("#FormularioPermisos").submit()
    });

});