$(document).ready(function () {
    $('.input-validation-error').removeClass("input-validation-error");
    $('.field-validation-error').addClass("field-validation-valid");

    $('#Buscar').click('click', function () {
        $('#FormIndexRoles').attr('action', '/Roles/BuscarRoles');
        $("#FormIndexRoles").submit()
    });

    $('#Agregar').click('click', function () {
        $('#FormularioRoles').attr('action', '/Roles/AgregarRol');
        $("#FormularioRoles").submit()
    });

    $('#Modificar').click('click', function () {
        $('#FormularioRoles').attr('action', '/Roles/ModificarRol');
        $("#FormularioRoles").submit()
    });

    $('#Eliminar').click('click', function () {
        $('#FormularioRoles').attr('action', '/Roles/EliminarRol');
        $("#FormularioRoles").submit()
    });

});