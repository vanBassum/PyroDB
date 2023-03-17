// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    AfterLoad($(this));
});

function AfterLoad(loadedObj) {
    loadedObj.find("*.ChemInfo").each(function (index, value) {
        var obj = $(this);
        var uri = obj.attr('data-uri');
        var id = obj.attr('data-id');

        obj.change(function () {

            var data = {
                "key": id,
                "value": this.checked
            };

            $.ajax({
                type: "POST",
                url: uri,
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(data) ,
                success: function (data) {
                    
                },
                error: function (response) {
                    alert("error");
                }
            });
        });
    })
};
