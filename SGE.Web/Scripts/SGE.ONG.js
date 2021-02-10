$(document).ready(function () {
    $('.input-validation-error').removeClass("input-validation-error");
    $('.field-validation-error').addClass("field-validation-valid");

    $("#CUIT").mask("99-99999999-9");
    $("#dni").mask("99999999");


    $("#RES_DNI").mask("99999999");
    $("#CON_DNI").mask("99999999");


    $('#Buscar').click('click', function () {
        $('#FormIndexOng').attr('action', '/Ong/BuscarOng');
        $("#FormIndexOng").submit()
    });

    $('#Agregar').click('click', function () {
        $('#FormularioOng').attr('action', '/Ong/AgregarOng');
        $("#FormularioOng").submit()
    });



    $('#Modificar').click('click', function () {
        $('#FormularioOng').attr('action', '/Ong/ModificarOng');
        $("#FormularioOng").submit()
    });


//    $('#Eliminar').click('click', function () {

//        $.ajax({
//            type: "POST",
//            url: "/Carreras/ValidarEliminar",
//            data: "{idCarrera:'" + $("#Id").val() + "'}",
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            success: function (data) {
//                if (data == true) {
//                    $('#dialog').dialog("open");
//                }
//                else {
//                    $('#dialogConfirmarEliminar').dialog("open");
//                }
//            },
//            error: function (msg) {
//                alert(msg);
//            }
//        });
//    });

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
    $('#BuscarResponsable').click('click', function () {
        //        $('#ABMNombreSede').removeClass("input-validation-error");
        //        $('#ABMSedeTelefonoContacto').removeClass("input-validation-error");


        $.ajax({
            type: "POST",
            url: "/Ong/obtenerActor",
            data: "{dni:'" + $('#RES_DNI').val() + "',idRolActor:'8', rubro:'RES'}",
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
    $('#BuscarContacto').click('click', function () {
        //        $('#ABMNombreSede').removeClass("input-validation-error");
        //        $('#ABMSedeTelefonoContacto').removeClass("input-validation-error");


        $.ajax({
            type: "POST",
            url: "/Ong/obtenerActor",
            data: "{dni:'" + $('#CON_DNI').val() + "',idRolActor:'8', rubro:'CON'}",
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


function SeleccionarActor(idActor, dni, ape, nom, cel, mail, barrio, localidad, rubro, idRol) {


    $("#dialogActores").dialog("close");

    //Fichas CONFIAMOS EN VOS------------------------
    if (rubro == "RES") {
        $("#ID_RESPONSABLE").val(idActor);
        $("#RES_DNI").val(dni);
        $("#RES_APELLIDO").val(ape);
        $("#RES_NOMBRE").val(nom);
        $("#RES_CELULAR").val(cel);
        $("#RES_MAIL").val(mail);
        $("#RES_N_BARRIO").val(barrio);
        $("#RES_N_LOCALIDAD").val(localidad);
    }
    if (rubro == "CON") {
        $("#ID_CONTACTO").val(idActor);
        $("#CON_DNI").val(dni);
        $("#CON_APELLIDO").val(ape);
        $("#CON_NOMBRE").val(nom);
        $("#CON_CELULAR").val(cel);
        $("#CON_MAIL").val(mail);
        $("#CON_N_BARRIO").val(barrio);
        $("#CON_N_LOCALIDAD").val(localidad);
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
