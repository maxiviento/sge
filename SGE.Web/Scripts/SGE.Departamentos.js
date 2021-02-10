$(document).ready(function () {

    $('#dialogConfirmarEliminacion').dialog({
        open: function (event, ui) { $('.ui-draggable').attr("style", "outline: 0px; height: auto; width: 450px; top: 350px; left: 444px; display: block; z-index: 1002;"); },
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                var f = $("#FormularioDepartamento");
                $('#FormularioDepartamento').attr('action', '/Departamentos/EliminarDepartamento');
                $("#FormularioDepartamento").submit()
                $(this).dialog("close");
            },
            "Cancel": function () {
                document.execCommand('SaveAs', '1', 'give img location here');
                $(this).dialog("close");
            }
        }
    });

    $('.input-validation-error').removeClass("input-validation-error");
    $('.field-validation-error').addClass("field-validation-valid");

    $('#Buscar').click('click', function () {
        $('#FormIndexDepartamento').attr('action', '/Departamentos/BuscarDepartamento');
        $("#FormIndexDepartamento").submit()
    });

    $('#Agregar').click('click', function () {
        $('#FormularioDepartamento').attr('action', '/Departamentos/AgregarDepartamento');
        $("#FormularioDepartamento").submit()
    });

    $('#Modificar').click('click', function () {
        $('#FormularioDepartamento').attr('action', '/Departamentos/ModificarDepartamento');
        $("#FormularioDepartamento").submit()
    });

   
    $('#Eliminar').click('click', function () {
        $('#dialogConfirmarEliminacion').dialog("open");
    });


});