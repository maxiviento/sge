$(document).ready(function () {
    $('.input-validation-error').removeClass("input-validation-error");
    $('.field-validation-error').addClass("field-validation-valid");

    $('#Buscar').click('click', function () {
        $('#FormIndexCarrera').attr('action', '/Carreras/BuscarCarrera');
        $("#FormIndexCarrera").submit()
    });

    $('#Agregar').click('click', function () {
        $('#FormularioCarrera').attr('action', '/Carreras/AgregarCarrera');
        $("#FormularioCarrera").submit()
    });

    $('#Modificar').click('click', function () {
        $('#FormularioCarrera').attr('action', '/Carreras/ModificarCarrera');
        $("#FormularioCarrera").submit()
    });

    $('#Eliminar').click('click', function () {

        $.ajax({
            type: "POST",
            url: "/Carreras/ValidarEliminar",
            data: "{idCarrera:'" + $("#Id").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data == true) {
                    $('#dialog').dialog("open");
                }
                else {
                    $('#dialogConfirmarEliminar').dialog("open");
                }
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });

    $('#dialogConfirmarEliminar').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                $('#FormularioCarrera').attr('action', '/Carreras/EliminarCarrera');
                $("#FormularioCarrera").submit();
                $(this).dialog("close");
            },
            "Cancel": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialog').dialog({
        autoOpen: false,
        width: 350,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#NombreInstitucion_Selected').change('change', function () {
        $('#FormularioCarrera').attr('action', '/Carreras/FormularioCarreraCambiaInstitucion');
        $("#FormularioCarrera").submit()
    });


    $('#Paginador').smartpaginator({ totalrecords: $('#ContCarreras').val()
, recordsperpage: 25, datacontainer: 'tblCarreras', dataelement: 'tr',
        initval: 0, next: 'Next', prev: 'Prev', first: 'First', last: 'Last', theme: 'green'
    });


    $('#impSecInstCarr').click('click', function () {
        $('#FormSectorInstCarrera').attr('action', '/Carreras/ReporteSectorInstCarrera');
        $("#FormSectorInstCarrera").submit()
    });



    //Fin document.ready
});

