$(document).ready(function () {
    $("#btnLogout").click(function () {
        $('#ModalLogout').modal('toggle');
    });
    $("#btnLogoutTrue").click(function () {
        $.ajax({
            type: "POST",
            url: "/Home/Logout",
            dataType: "text",
            beforeSend: function () {
                $("#btnLogoutTrue").button('loading');
            },
            success: function (data) {
                window.location.href = "/";
            },
            error: function (xhr, status, error) {
                $.notify("Lỗi #Logout01, xin thông báo với quản trị viên!", "error");
            }
        });
    });

});