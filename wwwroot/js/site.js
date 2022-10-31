// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $("#brands").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/api/cars/searchbrands',
                headers: {
                    "RequestVerificationToken":
                        $('input[name="__RequestVerificationToken"]').val()
                },
                data: { "term": request.term },
                dataType: "json",
                success: function (data) {
                    response($.map(data, function (item) {
                        return item;
                    }))
                },
                error: function (xhr, textStatus, error) {
                    alert(xhr.statusText);
                },
                failure: function (response) {
                    alert("failure " + response.responseText);
                }
            });
        },
        select: function (e, i) {
            $("#lblVendorName").text(i.item.value);
        },
        minLength: 2
    });
});

$(document).ready(function () {
    $("#models").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/api/cars/searchmodels',
                headers: {
                    "RequestVerificationToken":
                        $('input[name="__RequestVerificationToken"]').val()
                },
                data: { "term": request.term },
                dataType: "json",
                success: function (data) {
                    response($.map(data, function (item) {
                        return item;
                    }))
                },
                error: function (xhr, textStatus, error) {
                    alert(xhr.statusText);
                },
                failure: function (response) {
                    alert("failure " + response.responseText);
                }
            });
        },
        select: function (e, i) {
            $("#lblVendorName").text(i.item.value);
        },
        minLength: 2
    });
});

$(document).ready(function () {
    $("#teams").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/api/teams/searchteams',
                headers: {
                    "RequestVerificationToken":
                        $('input[name="__RequestVerificationToken"]').val()
                },
                data: { "term": request.term },
                dataType: "json",
                success: function (data) {
                    response($.map(data, function (item) {
                        return item;
                    }))
                },
                error: function (xhr, textStatus, error) {
                    alert(xhr.statusText);
                },
                failure: function (response) {
                    alert("failure " + response.responseText);
                }
            });
        },
        select: function (e, i) {
            $("#lblVendorName").text(i.item.value);
        },
        minLength: 3
    });
});

$(document).ready(function () {
    $("#tournaments").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/api/tournaments/searchtournaments',
                headers: {
                    "RequestVerificationToken":
                        $('input[name="__RequestVerificationToken"]').val()
                },
                data: { "term": request.term },
                dataType: "json",
                success: function (data) {
                    response($.map(data, function (item) {
                        return item;
                    }))
                },
                error: function (xhr, textStatus, error) {
                    alert(xhr.statusText);
                },
                failure: function (response) {
                    alert("failure " + response.responseText);
                }
            });
        },
        select: function (e, i) {
            $("#lblVendorName").text(i.item.value);
        },
        minLength: 3
    });
});