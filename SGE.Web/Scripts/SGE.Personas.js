$(document).ready(function () {
    $('.input-validation-error').removeClass("input-validation-error");
    $('.field-validation-error').addClass("field-validation-valid");

    $("#cuil").mask("99-99999999-9");
    $("#dni").mask("99999999");
    $("#numero_documento").mask("99999999");

    $(".ClassEmpleado").click(function () {

        $("#empleado").val($('input[name=empleado]:checked').val());
    });

    $('#Buscar').click('click', function () {
        $('#FormIndexPersona').attr('action', '/Personas/BuscarPersona');
        $("#FormIndexPersona").submit()
    });

    $('#Agregar').click('click', function () {
        $('#FormularioPersona').attr('action', '/Personas/AgregarPersona');
        $("#FormularioPersona").submit()
    });

    $('#AgregarRol').click('click', function () {

        //$('#apellido').removeClass("input-validation-error");
        $('#FormularioActor').attr('action', '/Personas/AgregarActorRol');
        $("#FormularioActor").submit()
    });


    $('#Modificar').click('click', function () {
        $('#FormularioPersona').attr('action', '/Personas/ModificarPersona');
        $("#FormularioPersona").submit()
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

    $('#dialogFamiExiste').dialog({
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



    $("#departamentos_Selected").change(function () {
        //        var f = $("#FormularioPersonas");
        //        $('#FormularioPersonas').validate().cancelSubmit = true;
        //        $('#FormularioPersonas').attr('action', '/Persona/CambiarCombo');
        //        $("#FormularioPersonas").submit()

        $.ajax({
            type: "POST",
            url: "/Personas/CargarLocalidades",
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
            url: "/Personas/BuscarBarrios",
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


    $('#VerificarPer').click('click', function () {

        $("#Apellido").val("");
        $('#Nombres').val("");
        $('#idPersona').val("0");

        $.ajax({
            type: "POST",
            url: "/Personas/GetDNI",
            data: "{dni:'" + $("#numero_documento").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {


                if (data != null) {

                    $("#Apellido").val(data.apellido);
                    $('#Nombres').val(data.nombre);
                    $('#idPersona').val(data.id_persona);

                    //alert(data.id_persona);

                }
                else {
                    //me da opcion para agregarla
                    alert("Ningun resultado");
                    $("#numero_documento").focus();
                }
            },
            error: function (msg) {
                alert(msg);
            }
        });

    });


    $('#AddFamiliar').click('click', function () {

        if ($('#idPersona').val() == "0") {
            alert("Debe cargar una persona");
        }
        else {


            var cant = $("#cantFami").val();
            for (i = 0; i < cant; i++) {
                id = $("#IDPersona_" + i).val();
                if (id == $('#idPersona').val()) {
                    $('#dialogFamiExiste').dialog("open");
                    return;
                }
            }

            var f = $("#FormularioFamilia");
            //$('#FormularioEtapa').validate().cancelSubmit = true;

            $('#FormularioFamilia').attr('action', '/Personas/AddFamiliar');
            $("#FormularioFamilia").submit();
        }

    });


    $('#GuardarPer').click('click', function () {

        if ($('#idPersona').val() != "0") {
            alert("Esta persona ya se encuentra cargada");
            return;
        }


        if ($("#Apellido").val() == "") {
            alert("Debe cargar el apellido");
            $("#Apellido").focus();
            return;
        }

        if ($("#Nombres").val() == "") {
            alert("Debe cargar el nombre");
            $("#Nombres").focus();
            return;
        }

        //alert($("#numero_documento").val().length);
        if ($("#numero_documento").val().length != 8) {
            alert("Debe cargar el documento correctamente");
            $("#numero_documento").focus();
            return;
        }


        $.ajax({
            type: "POST",
            url: "/Personas/addPersona",
            data: "{dni:'" + $("#numero_documento").val() + "',apellido:'" + $("#Apellido").val() + "',nombre:'" + $("#Nombres").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {


                if (data != null) {
                    $('#idPersona').val(data);
                    if (data == 0) {
                        alert("Ya existe una persona con ese DNI");
                        $('#idPersona').val("0");
                    }

                    if (data == -2) {
                        alert("No se ha guardado el registro");
                        $('#idPersona').val("0");
                    }

                    if (data > 0) {
                        alert("La persona se registró correctamente");
                        $('#idPersona').val(data);
                    }

                }
                else {
                    //me da opcion para agregarla
                    alert("No se ha guardado el registro");
                    $("#numero_documento").focus();
                }
            },
            error: function (msg) {
                alert(msg);
            }
        });




        //        var f = $("#FormularioFamilia");
        //        //$('#FormularioEtapa').validate().cancelSubmit = true;

        //        $('#FormularioFamilia').attr('action', '/Personas/AgregarPersona');
        //                    $("#FormularioFamilia").submit();


    });

    $('#limpiarPer').click('click', function () {

        $("#Apellido").val("");
        $('#Nombres').val("");
        $('#numero_documento').val("");
        $('#idPersona').val("0");
        $("#numero_documento").focus();


    });




    //Fin document.ready
});

