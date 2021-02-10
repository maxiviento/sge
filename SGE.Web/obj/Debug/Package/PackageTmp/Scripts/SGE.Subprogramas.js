$(document).ready(function () {
    $('.input-validation-error').removeClass("input-validation-error");
    $('.field-validation-error').addClass("field-validation-valid");

    $('#Paginador').smartpaginator({ totalrecords: $('#ContFichas').val()
, recordsperpage: 10, datacontainer: 'tblFichas', dataelement: 'tr',
        initval: 0, next: 'Siguiente', prev: 'Anterior', first: 'Primero', last: 'Ultimo', theme: 'green'
    });

    $('#Buscar').click('click', function () {
        $('#FormIndexSubprograma').attr('action', '/Subprogramas/BuscarSubprograma');
        $("#FormIndexSubprograma").submit()
    });

    $('#Agregar').click('click', function () {
        $('#FormularioSubprograma').attr('action', '/Subprogramas/AgregarSubprograma');
        $("#FormularioSubprograma").submit()
    });

    $('#Modificar').click('click', function () {
        $('#FormularioSubprograma').attr('action', '/Subprogramas/ModificarSubprograma');
        $("#FormularioSubprograma").submit()
    });


    $('#dialogPrograma').dialog({
        autoOpen: false,
        width: 700,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                $(this).dialog("close");
            }

        }
    });

    $('#dialogDesvincularPro').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {

                $("#IdPrograma").val("");
                $("#Programa").val("");
                $(this).dialog("close");
            },
            "Cancelar": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogSubProg').dialog({
        autoOpen: false,
        width: 700,
        modal: true,
        resizable: false,
        buttons: {
            "Cerrar": function () {
                $(this).dialog("close");

            }
        }
    });

    //*******************************************************
    $('#BuscarProg').click('click', function () {

        $.ajax({
            type: "POST",
            url: "/Subprogramas/Programas",
            data: "{programa:'" + $('#Programa').val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('#grillaPrograma').empty();
                $('#grillaPrograma').append(data);
                $('#dialogPrograma').dialog("open");
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });
    //*********************************************************


}); //Fin Ready


function SeleccionarPrograma(idprograma, programa) {

    $("#dialogPrograma").dialog("close");
    $("#IdPrograma").val(idprograma);
    $("#Programa").val(programa);

}

$('#DesvincularPro').click('click', function () {
    $('#dialogDesvincularPro').dialog("open");
});


//*******************************************************
$('#BuscarSubProg').click('click', function () {
    
    var nombre = "";
    if ($('#subprograma').val() != "") { nombre = $('#subprograma').val(); }

    $.ajax({
        type: "POST",
        url: "/Subprogramas/obtenerSubProg",
        data: "{nombre:'" + nombre + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#grillaSubProg').empty();
            $('#grillaSubProg').append(data);
            $('#dialogSubProg').dialog("open");
            //alert(data);
        },
        error: function (msg) {
            //alert(msg);
        }
    });
});

function SeleccionaSubP(IdSubprograma, NombreSubprograma) {


    $("#dialogSubProg").dialog("close");

    //------------------------
    $("#idsubprograma").val(IdSubprograma);
    $("#subprograma").val(NombreSubprograma);
    $("#Selsubprograma").val(NombreSubprograma);


    //-------------------------------------------------

}

$('#Paso2').click('click', function () {
    var f = $("#FormAsociarSubprograma");
    var idsub = "0";
    if ($("#idsubprograma").val() != "") {
        idsub = $("#idsubprograma").val();
    }
    if (idsub == "0") {
        alert("No ha cargado ningun subprograma");
        return;
    }


    $('#FormAsociarSubprograma').attr('action', '/Subprogramas/AsociarSubprogramaPaso2');
    $("#FormAsociarSubprograma").submit()
});

$('#Paso3').click('click', function () {
    $('#FormAsociarSubprograma').attr('action', '/Subprogramas/AsociarSubprogramaPaso3');
    $("#FormAsociarSubprograma").submit()
});


$("#archivoFichas").change(function () {
    var ext = $('#archivoFichas').val().split('.').pop().toLowerCase();
    if ($.inArray(ext, ['xls']) == -1) {
        $('#btnsubirArchivoFichas').css('display', 'none');
        alert('Archivo inválido, por favor seleccione otro archivo.');
    }
    else {
        $('#btnsubirArchivoFichas').show();
    }
});

//    // 20/02/2013 - DI CAMPLI LEANDRO - IMPORTAR EXCEL CON ID FICHAS
$('#btnsubirArchivoFichas').click('click', function () {
    var f = $("#FormAsociarSubprograma");
    $('#FormAsociarSubprograma').attr('action', '/Subprogramas/SubirArchivoFichas');
    $("#FormAsociarSubprograma").submit()
});

$('#AsignarSubprograma').click('click', function () {
    var f = $("#FormAsociarSubprograma");
    $('#FormAsociarSubprograma').validate().cancelSubmit = true;
    $('#FormAsociarSubprograma').attr('action', '/Subprogramas/udpSubprogramaFichas');
    $("#FormAsociarSubprograma").submit()
});
