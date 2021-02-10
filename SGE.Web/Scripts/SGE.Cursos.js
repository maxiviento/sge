$(document).ready(function () {
    $('.input-validation-error').removeClass("input-validation-error");
    $('.field-validation-error').addClass("field-validation-valid");

    $("#cuil").mask("99-99999999-9");
    $("#dni").mask("99999999");


    $("#DOC_DNI").mask("99999999");
    $("#TUT_DNI").mask("99999999");


    $('#Buscar').click('click', function () {
        $('#FormIndexCurso').attr('action', '/Cursos/BuscarCurso');
        $("#FormIndexCurso").submit()
    });

    $('#Agregar').click('click', function () {
        $('#FormularioCurso').attr('action', '/Cursos/AgregarCurso');
        $("#FormularioCurso").submit()
    });



    $('#Modificar').click('click', function () {
        $('#FormularioCurso').attr('action', '/Cursos/ModificarCurso');
        $("#FormularioCurso").submit()
    });

    $('#BuscarCursos').click('click', function () {
        $('#ListIndexCurso').attr('action', '/Cursos/ListarCursos');
        $("#ListIndexCurso").submit()
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



    $('#Paginador').smartpaginator({ totalrecords: $('#ContPersonas').val()
, recordsperpage: 25, datacontainer: 'tblPersonas', dataelement: 'tr',
        initval: 0, next: 'Next', prev: 'Prev', first: 'First', last: 'Last', theme: 'green'
    });



    //*******************************************************
    $('#BuscarDocente').click('click', function () {
        //        $('#ABMNombreSede').removeClass("input-validation-error");
        //        $('#ABMSedeTelefonoContacto').removeClass("input-validation-error");


        $.ajax({
            type: "POST",
            url: "/Cursos/obtenerActor",
            data: "{dni:'" + $('#DOC_DNI').val() + "',idRolActor:'1'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('#grillaActores').empty();
                $('#grillaActores').append(data);
                $('#dialogActores').dialog("open");
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });
    //*********************************************************

    //*******************************************************
    $('#BuscarTutor').click('click', function () {
        //        $('#ABMNombreSede').removeClass("input-validation-error");
        //        $('#ABMSedeTelefonoContacto').removeClass("input-validation-error");


        $.ajax({
            type: "POST",
            url: "/Cursos/obtenerActor",
            data: "{dni:'" + $('#TUT_DNI').val() + "',idRolActor:'6'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('#grillaActores').empty();
                $('#grillaActores').append(data);
                $('#dialogActores').dialog("open");
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });
    //*********************************************************


    $('#dialogActores').dialog({
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


    //*******************************************************
    $('#BuscarONG').click('click', function () {
        //        $('#ABMNombreSede').removeClass("input-validation-error");
        //        $('#ABMSedeTelefonoContacto').removeClass("input-validation-error");


        $.ajax({
            type: "POST",
            url: "/Cursos/obtenerONG",
            data: "{cuit:'" + $('#ONG_CUIT').val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('#grillaONG').empty();
                $('#grillaONG').append(data);
                $('#dialogONG').dialog("open");
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });
    //*********************************************************


    $('#dialogONG').dialog({
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


    $('#AgregarAsistencia').click('click', function () {
        muestra_oculta('divFichas', 'block');
        //$('#divFichas').style.display == 'block';
    });
    $('#CancelarAsistencia').click('click', function () {
        muestra_oculta('divFichas', 'none');
        //$('#divFichas').style.display == 'none';
    });

    $('#GuardarAsistencia').click('click', function () {
        if ($('#cboperiodos_Selected').val() == "0") {
            alert("Seleccione un período válido");
        } else {
            $('#FormularioCargaAsis').attr('action', '/Cursos/GuardarAsistencias');
            $("#FormularioCargaAsis").submit()
        }
    });

    $('#CargarAsistencia').click('click', function () {
        $('#FormularioAsistencias').attr('action', '/Cursos/CargarAsistencias');
        $("#FormularioAsistencias").submit()
    });


    $('#cboperiodos_Selected').change(function () {
        var a = $('#totalFichas').val();
        //var el = document.getElementsByName('[0].MES');
        for (i = 0; i < a; i++) {
            //var mes = 'input[name=[' + i + '].MES]';
            var mes = document.getElementsByName('[' + i + '].MES');  //'[' + i + '].MES';
            mes[0].value = $('#cboperiodos_Selected').val();
        }

    });

    $('#ModificarAsistencias').click('click', function () {
        $('#FormularioAsistencias').attr('action', '/Cursos/UpdateAsistencias');
        $("#FormularioAsistencias").submit()
    });

    $('#AddAsistencia').click('click', function () {

        var a = $('#totalFichas').val();
        var resu = "N";
        for (i = 0; i < a; i++) {
            //var mes = 'input[name=[' + i + '].MES]';
            //var r = document.getElementsByName('[' + i + '].registrar');  //'[' + i + '].MES';
            var rr = $("#chk" + i + "").val(); //r[0].value;
            if (rr == "S") {
                resu = "S";
            }
        }
        if (resu == "S") {
            $('#FormCargarAsis').attr('action', '/Cursos/insFichaSinAsistencias');
            $("#FormCargarAsis").submit()
        }
        else {
            alert("No ha seleccionado ningun registro");
        }

    });

    ///***********VALIDAR SOLO NUMEROS*************

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

    ///********************************************

    //Fin document.ready
});


function SeleccionarActor(idActor, dni, ape, nom, cel, mail, barrio, localidad, idRol) {


    $("#dialogActores").dialog("close");

    //Fichas CONFIAMOS EN VOS------------------------
    if (idRol == 1) {
        $("#ID_DOCENTE").val(idActor);
        $("#DOC_DNI").val(dni);
        $("#DOC_APELLIDO").val(ape);
        $("#DOC_NOMBRE").val(nom);
        $("#DOC_CELULAR").val(cel);
        $("#DOC_MAIL").val(mail);
        $("#DOC_N_BARRIO").val(barrio);
        $("#DOC_N_LOCALIDAD").val(localidad);
    }
    if (idRol == 6) {
        $("#ID_TUTOR").val(idActor);
        $("#TUT_DNI").val(dni);
        $("#TUT_APELLIDO").val(ape);
        $("#TUT_NOMBRE").val(nom);
        $("#TUT_CELULAR").val(cel);
        $("#TUT_MAIL").val(mail);
        $("#TUT_N_BARRIO").val(barrio);
        $("#TUT_N_LOCALIDAD").val(localidad);
    }

    //-------------------------------------------------

}


function SeleccionaONG(idong, cuit, nom, cel, tel, mail, aperes, nomres, celres, lres, mailres, apecon, nomcon, celcon, lcon, mailcon) {


    $("#dialogONG").dialog("close");

    //------------------------
    $("#ID_ONG").val(idong);
    $("#ONG_CUIT").val(cuit);
    $("#ONG_N_NOMBRE").val(nom);
    $("#ONG_CELULAR").val(cel);
    $("#ONG_TELEFONO").val(tel);
    $("#ONG_MAIL").val(mail);
    $("#ONG_RES_APELLIDO").val(aperes);
    $("#ONG_RES_NOMBRE").val(nomres);
    $("#ONG_RES_CELULAR").val(celres);
    $("#ONG_RES_N_LOCALIDAD").val(lres);
    $("#ONG_RES_MAIL").val(mailres);
    $("#ONG_CON_APELLIDO").val(apecon);
    $("#ONG_CON_NOMBRE").val(nomcon);
    $("#ONG_CON_CELULAR").val(celcon);
    $("#ONG_CON_N_LOCALIDAD").val(lcon);
    $("#ONG_CON_MAIL").val(mailcon);

    //-------------------------------------------------

}



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
function SeleccionaEscuela(idEscuela)
{


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
                $("#ID_ESCUELA").val(data.Id_Escuela);
                $("#N_ESCUELA").val(data.Nombre_Escuela);
                $("#CALLE").val(data.CALLE);
                $("#NUMERO").val(data.NUMERO);
                $("#CUPO_CONFIAMOS").val(data.CUPO_CONFIAMOS);
                $("#Cue").val(data.Cue);
                $("#COOR_APELLIDO").val(data.COOR_APELLIDO);
                $("#COOR_NOMBRE").val(data.COOR_NOMBRE);
                $("#COOR_DNI").val(data.COOR_DNI);
                $("#COOR_CUIL").val(data.COOR_CUIL);
                $("#COOR_CELULAR").val(data.COOR_CELULAR);
                $("#COOR_TELEFONO").val(data.COOR_TELEFONO);
                $("#COOR_MAIL").val(data.COOR_MAIL);
                $("#COOR_N_LOCALIDAD").val(data.COOR_N_LOCALIDAD);
                $("#DIR_APELLIDO").val(data.DIR_APELLIDO);
                $("#DIR_NOMBRE").val(data.DIR_NOMBRE);
                $("#ENC_APELLIDO").val(data.ENC_APELLIDO);
                $("#ENC_NOMBRE").val(data.ENC_NOMBRE);
                $("#ENC_DNI").val(data.ENC_DNI);
                $("#ENC_CUIL").val(data.ENC_CUIL);
                $("#ENC_CELULAR").val(data.ENC_CELULAR);
                $("#ENC_TELEFONO").val(data.ENC_TELEFONO);
                $("#ENC_MAIL").val(data.ENC_MAIL);
                $("#ENC_N_LOCALIDAD").val(data.ENC_N_LOCALIDAD);

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
function ChangeRegistrar(id, obj) {


    //$("#[" + id + "].registrar").val($("input[name=[" + id + "].registrar]:checked").val());
    if ($("#chk" + id + "").val() == "N")
    { $("#chk" + id + "").val("S"); return true; }
    else { 
    $("#chk" + id + "").val("N"); return false; }
    
}