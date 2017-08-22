$(document).ready(function () {
    $.ajax({
        type: "POST",
        url: "/Home/craftXmlmenu",
        dataType: "text",
        success: function (data) {
            var htmlstring = $(data);
            $("#side-menu").html(htmlstring);
            $('#side-menu').metisMenu();
        },
        error: function (xhr, status, error) {
            $.notify("Lỗi #XML01, xin thông báo với quản trị viên!", "error");
        }
    });
});