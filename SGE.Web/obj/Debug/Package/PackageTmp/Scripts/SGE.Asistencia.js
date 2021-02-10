

function CargarFecha(fecha) {
    var inicio = fecha.replace('+', '');
    var fin = inicio.replace('+', '');
    $('#FechaLimpieza').val(fin);
    $('#dialogConfirmar').dialog("open");
}


$(document).ready(function () {


    $('#Paginador').smartpaginator({ totalrecords: $('#ContBenef').val()
, recordsperpage: 10, datacontainer: 'grilla', dataelement: 'tr',
        initval: 0, next: 'Next', prev: 'Prev', first: 'First', last: 'Last', theme: 'green'
    });


    $("#Cuil").mask("99-99999999-9");
    $("#Numero_Documento").mask("99999999");
    $("#CuilBusqueda").mask("99-99999999-9");
    $("#NumeroDocumentoBusqueda").mask("99999999");


    $('#BuscarAsistenciasRpt').click('click', function () {
        var f = $("#FormAsistenciaRpt");

        $('#FormAsistenciaRpt').attr('action', '/Reportes/BuscarAsistencias');
        $("#FormAsistenciaRpt").submit()
    });


    $('#ExportAsistencias').click('click', function () {
        var f = $("#FormAsistenciaRpt");
        $('#FormAsistenciaRpt').attr('action', '/Reportes/AsistenciasXls');
        $("#FormAsistenciaRpt").submit();
    });


    
    $('#dialog').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                $(this).dialog("close");
            }

        }
    });

    $('#dialogErrorDatos').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        buttons: {
            "Ok": function () {
                $('#ErrorDatos').val('False');
                $(this).dialog("close");
            }

        }
    });

    
    if ($('#ErrorDatos').val() == "True")
    { $('#dialogErrorDatos').dialog("open"); }

    $('#procesando').dialog({
        autoOpen: false,
        width: 450,
        modal: true,
        resizable: false,
        //        buttons: {
        //            "Ok": function () {
        //                $('#ErrorDatos').val('False');
        //                $(this).dialog("close");
        //            }
        //        }
    closeOnEscape: false,
    beforeclose: function (event, ui) { return false; }//,
    //dialogClass: "noclose"

    });


$('#dialogCursos').dialog({
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

$('#dialogEscuela').dialog({
    autoOpen: false,
    width: 900,
    modal: true,
    resizable: false,
    buttons: {
        "Cerrar": function () {
            $(this).dialog("close");

        }
    }
});

$('#dialogCursos').dialog({
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
//FIN READY
});




$(function () {
    $(".validar").keydown(function (event) {
        if (event.shiftKey) {
            event.preventDefault();
        }

        if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9) {
        }
        else {
            if (event.keyCode < 95) {
                if (event.keyCode < 48 || event.keyCode > 57) {
                    event.preventDefault();
                }
            }
            else {
                if (event.keyCode < 96 || event.keyCode > 105) {
                    event.preventDefault();
                }
            }
        }
    });
});


//*******************************************************
$('#BuscarEscuela').click('click', function () {
    //        $('#ABMNombreSede').removeClass("input-validation-error");
    //        $('#ABMSedeTelefonoContacto').removeClass("input-validation-error");


    $.ajax({
        type: "POST",
        url: "/Cursos/obtenerEscuela",
        data: "{nombre:'" + $('#N_ESCUELA').val() + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#grillaEscuela').empty();
            $('#grillaEscuela').append(data);
            $('#dialogEscuela').dialog("open");
        },
        error: function (msg) {
            alert(msg);
        }
    });
});
//*********************************************************





//***Traer los datos de una escuela**********************
function SeleccionaEscuela(idEscuela) {


    $.ajax({
        type: "POST",
        url: "/Cursos/TraerEscuela",
        data: "{idEscuela:'" + idEscuela + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data != null) {

                //var cboLocalidad = document.getElementById("LocalidadesEscuela");


                $("#dialogEscuela").dialog("close");
                $("#idEscuela").val(data.Id_Escuela);
                $("#N_ESCUELA").val(data.Nombre_Escuela);
//                $("#CALLE").val(data.CALLE);
//                $("#NUMERO").val(data.NUMERO);
//                $("#CUPO_CONFIAMOS").val(data.CUPO_CONFIAMOS);
//                $("#Cue").val(data.Cue);
//                $("#COOR_APELLIDO").val(data.COOR_APELLIDO);
//                $("#COOR_NOMBRE").val(data.COOR_NOMBRE);
//                $("#COOR_DNI").val(data.COOR_DNI);
//                $("#COOR_CUIL").val(data.COOR_CUIL);
//                $("#COOR_CELULAR").val(data.COOR_CELULAR);
//                $("#COOR_TELEFONO").val(data.COOR_TELEFONO);
//                $("#COOR_MAIL").val(data.COOR_MAIL);
//                $("#COOR_N_LOCALIDAD").val(data.COOR_N_LOCALIDAD);
//                $("#DIR_APELLIDO").val(data.DIR_APELLIDO);
//                $("#DIR_NOMBRE").val(data.DIR_NOMBRE);
//                $("#ENC_APELLIDO").val(data.ENC_APELLIDO);
//                $("#ENC_NOMBRE").val(data.ENC_NOMBRE);
//                $("#ENC_DNI").val(data.ENC_DNI);
//                $("#ENC_CUIL").val(data.ENC_CUIL);
//                $("#ENC_CELULAR").val(data.ENC_CELULAR);
//                $("#ENC_TELEFONO").val(data.ENC_TELEFONO);
//                $("#ENC_MAIL").val(data.ENC_MAIL);
//                $("#ENC_N_LOCALIDAD").val(data.ENC_N_LOCALIDAD);

                /*

                */

            }
            else {
                //me da opcion para agregarla

            }
        },
        error: function (msg) {
            alert(msg);
        }


    });

}
//*******************************************************
//*******************************************************
$('#BuscarCurso_Conf').click('click', function () {
    //        $('#ABMNombreSede').removeClass("input-validation-error");
    //        $('#ABMSedeTelefonoContacto').removeClass("input-validation-error");

    $.ajax({
        type: "POST",
        url: "/Fichas/TraerCursos",
        data: "{NombreCurso:'" + $('#N_CURSO').val() + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#grillaCurso').empty();
            $('#grillaCurso').append(data);
            $('#dialogCursos').dialog("open");
        },
        error: function (msg) {
            alert(msg);
        }
    });
});
//*********************************************************

//*******************************************************
$('#BuscarCursoNro').click('click', function () {
    //        $('#ABMNombreSede').removeClass("input-validation-error");
    //        $('#ABMSedeTelefonoContacto').removeClass("input-validation-error");
    var nroCurso = 0;
    if ($('#NRO_COND_CURSO').val() != "") { nroCurso = $('#NRO_COND_CURSO').val(); }
    $.ajax({
        type: "POST",
        url: "/Fichas/CursosPorNro",
        data: "{NroCurso:'" + nroCurso + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#grillaCurso').empty();
            $('#grillaCurso').append(data);
            $('#dialogCursos').dialog("open");
        },
        error: function (msg) {
            alert(msg);
        }
    });
});
//*********************************************************

function SeleccionarCurso(idcurso, nrocurso, nomcurso, ini, fin, dura, expediente, apedoc, nomdoc, celdoc, ldoc, maildoc, apetut, nomtut, celtut, ltut, mailtut) {


    $("#dialogCursos").dialog("close");

    //Fichas CONFIAMOS EN VOS------------------------
    $("#idCurso").val(idcurso);
    $("#NRO_COND_CURSO").val(nrocurso);
    $("#N_CURSO").val(nomcurso);
//    $("#F_INICIO").val(ini);
//    $("#F_FIN").val(fin);
//    $("#DURACION").val(dura);
//    $("#EXPEDIENTE_LEG").val(expediente);
//    $("#DOC_APELLIDO").val(apedoc);
//    $("#DOC_NOMBRE").val(nomdoc);
//    $("#DOC_CELULAR").val(celdoc);
//    $("#DOC_N_LOCALIDAD").val(ldoc);
//    $("#DOC_MAIL").val(maildoc);
//    $("#TUT_APELLIDO").val(apetut);
//    $("#TUT_NOMBRE").val(nomtut);
//    $("#TUT_CELULAR").val(celtut);
//    $("#TUT_N_LOCALIDAD").val(ltut);
//    $("#TUT_MAIL").val(mailtut);

    //-------------------------------------------------

}


$('#TraerEtapa').click('click', function () {
    var f = $("#FormAsistenciaRpt");
    //$('#FormularioEtapa').validate().cancelSubmit = true;
    $("#programaSel").val($("#EtapaProgramas_Selected").val());
    $("#filtroEjerc").val($("#ejerc").val());
    $("#BuscarPorEtapa").val("S");
    $('#FormAsistenciaRpt').attr('action', '/Reportes/IndexTraerEtapas');
    $("#FormAsistenciaRpt").submit()
});

// 17/06/2013 - Quita el filtro de etapas
$('#QuitarEtapa').click('click', function () {
    var f = $("#FormAsistenciaRpt");
    //$('#FormularioEtapa').validate().cancelSubmit = true;
    $("#BuscarPorEtapa").val("N");
    $('#FormAsistenciaRpt').attr('action', '/Reportes/AsistenciasRpt');
    $("#FormAsistenciaRpt").submit()
});
