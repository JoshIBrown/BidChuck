﻿$(document).ready(function () {
    $('#Architect').autocomplete({
        source: function (request, response) {
            $.getJSON(
                '/api/Companies',
                { query: request.term, type: "Architect" }
            ).success(function (data) {
                response($.map(data, function (item) {
                    return {
                        label: item.Text,
                        value: item.Id
                    }
                }));
            });
        },
        minLength: 2,
        focus: function (event, ui) {
            this.value = ui.item.label;
            //$('#Architect').val(ui.item.label);
            event.preventDefault();
        },
        select: function (event, ui) {
            this.value = ui.item.label;
            $('#ArchitectId').val(ui.item.value);
            event.preventDefault();
        }
    });
});