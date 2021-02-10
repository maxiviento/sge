$(document).ready(function () {
    $('.input-validation-error').removeClass("input-validation-error");
    $('.field-validation-error').addClass("field-validation-valid");

    $('#Buscar').click('click', function () {
        $('#FormIndexSucursalCobertura').attr('action', '/SucursalCobertura/BuscarSucursalCobertura');
        $("#FormIndexSucursalCobertura").submit()
    });

    $('#Modificar').click('click', function () {
        $('#FormularioSucursalCobertura').attr('action', '/SucursalCobertura/ModificarSucursalCobertura');
        $("#FormularioSucursalCobertura").submit()
    });

    $("#BusquedaDepartamentos_Selected").change(function () {
        var f = $("#FormIndexSucursalCobertura");
        $('#FormIndexSucursalCobertura').validate().cancelSubmit = true;
        $('#FormIndexSucursalCobertura').attr('action', '/SucursalCobertura/CambiarCombo');
        $("#FormIndexSucursalCobertura").submit()
    });




});