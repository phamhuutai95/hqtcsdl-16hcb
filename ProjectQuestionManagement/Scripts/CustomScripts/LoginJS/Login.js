$(document).ready(function () {
    //Khởi tạo sự kiện nút enter
    $('input').keypress(function (e) {
        var key = e.which;
        if (key == 13) {
            $('#btnlogin').click();
            return false;
        }
    });

    //Khởi tạo sự kiện cho nút đăng nhập
    $("#btnlogin").click(function () {
        var username = $("#txtUsername").val();
        var password = $("#txtPassword").val();

        //Kiểm tra dữ liệu
        if (username == "") {
            $.notify("Vui lòng nhập Tên đăng nhập!", "info");
            $("#txtUsername").focus();
            return false;
        }

        if (password == "") {
            $.notify("Vui lòng nhập Mật khẩu!", "info");
            $("#txtPassword").focus();
            return false;
        }

        var param = {};
        param.strUserName = username;
        param.strPassword = password;
        $.ajax({
            url: "/Home/LoginCheck",
            type: "POST",
            contentType: 'application/json; charset=UTF-8',
            data: JSON.stringify(param),
            dataType: 'json',
            beforeSend: function () {
                $("#btnlogin").button('loading');
            },
            success: function (data) {
                if (data.iserror == false) {
                    if (data.chkUser == true) {
                        window.location.href = "/Home/Index";
                    }
                    else {
                        $("#btnlogin").button('reset');
                        $("#txtPassword").val("");
                        $.notify("Đăng nhập không thành công, xin kiểm tra lại tài khoản và mật khẩu!", "error");
                    }
                } else {
                    $("#btnlogin").button('reset');
                    $.notify("Lỗi #Login01, xin thông báo với quản trị viên!", "error");
                }
            },
            error: function (xhr, status, error) {
                $("#btnlogin").button('reset');
                $.notify("Lỗi #Login02, xin thông báo với quản trị viên!", "error");
            }
        });
    });
})