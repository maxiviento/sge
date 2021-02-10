$(document).ready(function () {



    $('#Paginador').smartpaginator({ totalrecords: $('#ContRegistros').val()
, recordsperpage: 10, datacontainer: 'tblCupones', dataelement: 'tr',
        initval: 0, next: 'Siguiente', prev: 'Anterior', first: 'Primero', last: 'Ultimo', theme: 'green'
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



    $("#archivoRecupero").change(function () {
        var ext = $('#archivoRecupero').val().split('.').pop().toLowerCase();
        if ($.inArray(ext, ['dat']) == -1) {
            $('#btnsubirArchivoRecupero').css('display', 'none');
            alert('Archivo inválido, por favor seleccione otro archivo.');
        }
        else {
            $('#btnsubirArchivoRecupero').show();
        }
    });

    $('#btnsubirArchivoRecupero').click('click', function () {
        var f = $("#FormRecupero");
        $('#FormRecupero').attr('action', '/Empresas/SubirArchivoRecupero');
        $("#FormRecupero").submit();
    });

    $('#btnExportarCupones').click('click', function () {
        $('#FormRecupero').attr('action', '/Empresas/ExportarCupones');
        $("#FormRecupero").submit();
    });

    //FIN READY
});


