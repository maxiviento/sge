
$(document).ready(function () {

    $("#Espace").mousemove(function (event) {
        var x = event.clientX;
        var y = event.clientY;

        if ($("#x").val() != x && $("#x").val() != y) {
            $.ajax({
                type: "POST",
                url: "/Cookie/UltimaActividad",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $("#UltimaActividad").val(data);
                },
                error: function (msg) {
                }
            });
        }

        $("#x").val(x);
        $("#y").val(y);
    });

    $.timer(10000, function () {
        $.ajax({
            type: "POST",
            url: "/Cookie/ControlSeccion",
            data: "{date:'" + $("#UltimaActividad").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data == "60s") {
                    $('#dialog60s').dialog("open");
                }

                if (data == "Expiro") {
                    $('#dialog60s').dialog("close");
                    $('#dialogExpiracion').dialog("open");
                }

            },
            error: function (msg) {
            }
        });
    });

});