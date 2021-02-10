/// <reference path="jquery-1.5.1-vsdoc.js" />
/// <reference path="jquery-ui-1.8.11.js" />

$(document).ready(function () {
    SGESetFront.init();
});

SGESetFront = function () {

    function init() {
        setJqueyUi();
        quitLastRowZebra();
        //setCommonFieldSet();
        setCommonGrilla();
        setPager();
    }

    function setJqueyUi() {

        // tabs
        $("#tabs").tabs();

        // AutoComplete
        $(":input[data-autocomplete]").each(function () {
            $(this).autocomplete({ source: $(this).attr("data-autocomplete") });
        });

        // Color Readonly
        $(":input[readonly]").attr('style', 'color:#808080');
        $(":input[set-readonly]").attr('style', 'color:#808080');

        // datepicker

        var allDatepicker = $(":input[data-datepicker]");
        allDatepicker.each(function () {
            if ($(this).val() == '01/01/0001 12:00:00 a.m.') {
                $(this).val($("#CurrentDate").val());
            }
        });

        $(":input[data-datepicker]").datepicker({ dateFormat: 'dd/mm/yy' });
        $(":input[data-datepicker]").mask("99/99/9999");
    }

    function quitLastRowZebra() {
        $('table[id="grilla"] tr:last').each(function () {
            var quit = false;
            $(this).find("td").each(function () {
                var result = $(this).attr('colspan');
                if (result != "undefined " && result != "1") {
                    quit = true;
                    return;
                }

            });

            if (quit) {
                $(this).find("td").each(function () {
                    $(this).attr('style', 'border-bottom:#ccc 0px solid;');

                });
                $(this).hover(function () {
                    $(this).css({ 'background': '#fff' });
                });
            }
        });
    }

    function setCommonFieldSet() {
        $("fieldset").attr('style', 'font-family: Arial, Helvetica, sans-serif; font-size: 12px; width: 700px;margin-left: 5px; margin-top: 5px;"');
    }

    function setCommonGrilla() {
        $("#grilla thead th").attr('height', '30');
        //$("#grilla").attr('align', 'center');
        //$("#grilla").attr('width', '722px');
        //$("#grilla").attr('cellpadding', '0');
        //$("#grilla").attr('cellspacing','0');

    }

    function setPager() {

        var PageNumber = parseInt($("#PageNumber").val());
        var PageCount = parseInt($("#TotalPages").val());

        PageClick = function (pageclickednumber) {
            
            $("#PageNumber").val(pageclickednumber);
            $("#pager").pager({ pagenumber: pageclickednumber, pagecount: PageCount, buttonClickCallback: PageClick });
            var formName = $('#FormName').val();

            $('#' + formName).attr('action', $("#ActionName").val());
            $('#' + formName).submit();

        };

        if (parseInt($("#TotalPages").val()) > 1) {
            $("#pager").pager({ pagenumber: PageNumber, pagecount: PageCount, buttonClickCallback: PageClick });
            $("#pager").show();
        }
        else {
            $("#pager").hide();
        }
    }

    return {
        init: init
    };
} ();

