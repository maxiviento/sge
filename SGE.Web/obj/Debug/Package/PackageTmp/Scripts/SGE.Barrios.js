$(document).ready(function () {
    $('.input-validation-error').removeClass("input-validation-error");
    $('.field-validation-error').addClass("field-validation-valid");

//    $("#cuil").mask("99-99999999-9");
//    $("#dni").mask("99999999");


    $('#Buscar').click('click', function () {
        $('#FormIndexBarrio').attr('action', '/Barrios/BuscarBarrio');
        $("#FormIndexBarrio").submit()
    });

    $('#Agregar').click('click', function () {
        $('#FormularioBarrio').attr('action', '/Barrios/AgregarBarrio');
        $("#FormularioBarrio").submit()
    });



    $('#Modificar').click('click', function () {
        $('#FormularioBarrio').attr('action', '/Barrios/ModificarBarrio');
        $("#FormularioBarrio").submit()
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


//    //Esto sirve para smartpaginator -- Todo el resultado se carga en memoria
//    $('#Paginador').smartpaginator({ totalrecords: $('#ContBarrios').val()
//, recordsperpage: 25, datacontainer: 'tblBarrios', dataelement: 'tr',
//        initval: 0, next: 'Next', prev: 'Prev', first: 'First', last: 'Last', theme: 'green'
//    });

if ($("#Localidades_Selected").val() != "25") {
    $('#Seccionales_Selected').attr("disabled", "disabled");
}

    $('#Localidades_Selected').change(function () {

        if ($('#Localidades_Selected').val() == "25") {

            $('#Seccionales_Selected').removeAttr("disabled");
            //alert("enabled");
        }
        else {

            $('#Seccionales_Selected').attr("disabled", "disabled");
            //alert("disabled");
        }
    });
   
    //Fin document.ready
});


