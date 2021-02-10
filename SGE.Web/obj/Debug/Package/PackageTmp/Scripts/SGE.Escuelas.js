$(document).ready(function () {
    $('.input-validation-error').removeClass("input-validation-error");
    $('.field-validation-error').addClass("field-validation-valid");


    $("#DIR_DNI").mask("99999999");
    $("#COOR_DNI").mask("99999999");
    $("#ENC_DNI").mask("99999999");
    

    $('#Buscar').click('click', function () {
        $('#FormIndexEscuela').attr('action', '/Escuelas/BuscarEscuela');
        $("#FormIndexEscuela").submit()
    });

    $('#Agregar').click('click', function () {
        $('#FormularioEscuela').attr('action', '/Escuelas/AgregarEscuela');
        $("#FormularioEscuela").submit()
    });



    $('#Modificar').click('click', function () {
        $('#FormularioEscuela').attr('action', '/Escuelas/ModificarEscuela');
        $("#FormularioEscuela").submit()
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



    $('#Paginador').smartpaginator({ totalrecords: $('#ContEscuelas').val()
, recordsperpage: 25, datacontainer: 'tblEscuelas', dataelement: 'tr',
        initval: 0, next: 'Next', prev: 'Prev', first: 'First', last: 'Last', theme: 'green'
    });



    $("#departamentos_Selected").change(function () {
        //        var f = $("#FormularioEscuelas");
        //        $('#FormularioEscuelas').validate().cancelSubmit = true;
        //        $('#FormularioEscuelas').attr('action', '/Escuela/CambiarCombo');
        //        $("#FormularioEscuelas").submit()

        $.ajax({
            type: "POST",
            url: "/Escuelas/CargarLocalidades",
            data: "{idDep:'" + $("#departamentos_Selected").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data != null) {
                    var options = '<option value="0">Seleccione Localidad...</option>';
                    var cbo = data;
                    $.each(cbo, function (index, obj) {
                        options += '<option value="' + obj.IdLocalidad + '">' + obj.NombreLocalidad + '</option>';
                    });

                    $("#Localidades_Selected").html(options);
                }
                else {

                }
            },
            error: function (msg) {
                alert(msg);
            }
        });

    });


    $("#Localidades_Selected").change(function () {

        $.ajax({
            type: "POST",
            url: "/Escuelas/BuscarBarrios",
            data: "{idlocalidad:'" + $("#Localidades_Selected").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data != null) {
                    var options = '<option value="0">Seleccione Barrio...</option>';
                    var cbo = data;
                    if (cbo[0] != null) {
                        $.each(cbo, function (index, obj) {
                            options += '<option value="' + obj.ID_BARRIO + '">' + obj.N_BARRIO + '</option>';
                        });
                    }
                    $("#barrios_Selected").html(options);
                }
                else {

                }
            },
            error: function (msg) {
                alert(msg);
            }
        });

    });


    //*******************************************************
    $('#BuscarDirector').click('click', function () {
        //        $('#ABMNombreSede').removeClass("input-validation-error");
        //        $('#ABMSedeTelefonoContacto').removeClass("input-validation-error");


        $.ajax({
            type: "POST",
            url: "/Cursos/obtenerActor",
            data: "{dni:'" + $('#DIR_DNI').val() + "',idRolActor:'3'}",
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
    $('#BuscarCoordinador').click('click', function () {
        //        $('#ABMNombreSede').removeClass("input-validation-error");
        //        $('#ABMSedeTelefonoContacto').removeClass("input-validation-error");


        $.ajax({
            type: "POST",
            url: "/Cursos/obtenerActor",
            data: "{dni:'" + $('#COOR_DNI').val() + "',idRolActor:'2'}",
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
    $('#BuscarEncargado').click('click', function () {
        //        $('#ABMNombreSede').removeClass("input-validation-error");
        //        $('#ABMSedeTelefonoContacto').removeClass("input-validation-error");


        $.ajax({
            type: "POST",
            url: "/Cursos/obtenerActor",
            data: "{dni:'" + $('#ENC_DNI').val() + "',idRolActor:'4'}",
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

    //Fin document.ready
});


function SeleccionarActor(idActor, dni, ape, nom, cel, mail, barrio, localidad, idRol) {


    $("#dialogActores").dialog("close");

    //Fichas CONFIAMOS EN VOS------------------------
    if (idRol == 3) {
        $("#ID_DIRECTOR").val(idActor);
        $("#DIR_DNI").val(dni);
        $("#DIR_APELLIDO").val(ape);
        $("#DIR_NOMBRE").val(nom);
        $("#DIR_CELULAR").val(cel);
        $("#DIR_MAIL").val(mail);
        $("#DIR_N_BARRIO").val(barrio);
        $("#DIR_N_LOCALIDAD").val(localidad);
    }
    if (idRol == 2) {
        $("#ID_COORDINADOR").val(idActor);
        $("#COOR_DNI").val(dni);
        $("#COOR_APELLIDO").val(ape);
        $("#COOR_NOMBRE").val(nom);
        $("#COOR_CELULAR").val(cel);
        $("#COOR_MAIL").val(mail);
        $("#COOR_N_BARRIO").val(barrio);
        $("#COOR_N_LOCALIDAD").val(localidad);
    }

    if (idRol == 4) {
        $("#ID_ENCARGADO").val(idActor);
        $("#ENC_DNI").val(dni);
        $("#ENC_APELLIDO").val(ape);
        $("#ENC_NOMBRE").val(nom);
        $("#ENC_CELULAR").val(cel);
        $("#ENC_MAIL").val(mail);
        $("#ENC_N_BARRIO").val(barrio);
        $("#ENC_N_LOCALIDAD").val(localidad);
    }
    //-------------------------------------------------

}