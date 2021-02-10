$(document).ready(function () {
    $('.input-validation-error').removeClass("input-validation-error");
    $('.field-validation-error').addClass("field-validation-valid");

    $('#Buscar').click('click', function () {
        $('#FormIndexUsuarios').attr('action', '/Usuarios/BuscarUsuarios');
        $("#FormIndexUsuarios").submit()
    });

    $('#Agregar').click('click', function () {
        $('#FormularioUsuarios').attr('action', '/Usuarios/AgregarUsuario');
        $("#FormularioUsuarios").submit()
    });

    $('#Modificar').click('click', function () {
        $('#FormularioUsuarios').attr('action', '/Usuarios/ModificarUsuario');
        $("#FormularioUsuarios").submit()
    });

    $('#Eliminar').click('click', function () {
        $('#FormularioUsuarios').attr('action', '/Usuarios/EliminarUsuario');
        $("#FormularioUsuarios").submit()
    });

    $('#Iniciar').click('click', function () {
        $('#FormularioLogin').attr('action', '/Login/Login');
        $("#FormularioLogin").submit()
    });

    $("#login").bind("keypress", function (e) {
        if (e.keyCode == 13) {
            $("#login").submit();
        }
    });

    $("#Login").focus();

    $('#btnMofificarPsw').click('click', function () {
        $('#FormularioPassword').attr('action', '/Password/Modificar');
        $("#FormularioPassword").submit()
    }); 

});