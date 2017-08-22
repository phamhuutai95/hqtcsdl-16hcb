$(document).ready(function () {
    //Khởi tạo sự kiện nút enter
    $('input').keypress(function (e) {
        var key = e.which;
        if (key == 13) {
            $('#btnXacNhan').click();
            return false;
        }
    });



    $("#btnXacNhan").click(function () {
        var oldpassword = $("#tbxMKCu").val();
        var newpassword = $("#tbxMKMoi").val();
        var checkpassword = $("#tbxMKXNMoi").val();

        //Kiểm tra dữ liệu
        if (oldpassword == "") {
            $.notify("Vui lòng nhập mật khẩu cũ", "info");
            $("#tbxMKCu").focus();
            return false;
        }

        if (newpassword == "") {
            $.notify("Vui lòng nhập mật khẩu mới", "info");
            $("#tbxMKMoi").focus();
            return false;
        }

        if (checkpassword == "") {
            $.notify("Vui lòng nhập xác nhận mật khẩu", "info");
            $("#tbxMKXNMoi").focus();
            return false;
        }


        if (oldpassword.length < 4) {
            $.notify("Mật khẩu phải lớn hơn 4 kí tự", "info");
            $("#tbxMKCu").focus();
            return false;
        }


        if (newpassword.length < 4) {
            $.notify("Mật khẩu phải lớn hơn 4 kí tự", "info");
            $("#tbxMKMoi").focus();
            return false;
        }

        if (checkpassword.length < 4) {
            $.notify("Mật khẩu phải lớn hơn 4 kí tự", "info");
            $("#tbxMKXNMoi").focus();
            return false;
        }

        if (newpassword != checkpassword) {
            $.notify("Mật khẩu mới không trùng với xác nhận mật khẩu", "info");
            $("#tbxMKXNMoi").focus();
            return false;
        }

        var param = {};
        param.stroldpass = oldpassword;
        param.strnewpass = newpassword;
        $.ajax({
            url: "/Home/CheckPass",
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(param),
            dataType: 'json',
            beforeSend: function () {
                $("#tbxMKCu").attr('readonly', true);
                $("#tbxMKMoi").attr('readonly', true);
                $("#tbxMKXNMoi").attr('readonly', true);
                $("#btnXacNhan").button('loading');
            },
            success: function (data) {
                if (data.iserror == false) {
                    if (data.chkUser == false) {
                        $.notify("Mật khẩu cũ không chính xác!", "error");
                        $("#tbxMKCu").val("");
                        $("#tbxMKCu").focus();
                    }
                    else {
                        $.ajax({
                            url: "/Home/ChangePassProgress",
                            type: "POST",
                            contentType: 'application/json; charset=utf-8',
                            data: JSON.stringify(param),
                            dataType: 'json',
                            success: function (data) {
                                if (data.iserror == false) {
                                    if (data.successchange == false) {
                                        $.notify("Đã có lỗi xảy ra! Vui lòng nhấn F5 để thử lại.", "error"); 
                                    }
                                    else {
                                        $('#ModalConfirmPass').modal('toggle');
                                    }
                                }
                                else {
                                    $.notify("Đã có lỗi xảy ra! Vui lòng nhấn F5 để thử lại.", "error");
                                }
                            },
                            error: function (xhr, status, error) {
                                $.notify("Đã có lỗi xảy ra! Vui lòng nhấn F5 để thử lại.", "error");
                            }
                        });
                    }
                }
                $("#tbxMKCu").attr('readonly', false);
                $("#tbxMKMoi").attr('readonly', false);
                $("#tbxMKXNMoi").attr('readonly', false);
                $("#btnXacNhan").button('reset');
            },
            error: function (xhr, status, error) {
                $.notify("Đã có lỗi xảy ra! Vui lòng nhấn F5 để thử lại.", "error");
            }
        });
    });
});

function RedirectToIndex() {
    window.location.href = "/Home/Index";
}