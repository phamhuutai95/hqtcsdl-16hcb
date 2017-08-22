var IsLoadServer = false;

///////////////////////////////////////////////////THÊM/////////////////////////////////////////////////
$(document).ready(function () {
    //Khởi tạo datetime
    $('#dtmBirthDate').datepicker({
        yearRange: "-50:+0",
        dateFormat: 'dd-mm-yy',
        changeMonth: true,
        changeYear: true
    });

    if ($('#hdFormType').val() == "Insert") {
        //Khoi tao su kien sau khi insert
        $('#frmTeacherInsert').ajaxForm(function (data) {
            ajaxInsertSuccess(data);
        });
    } else {
        //Khoi tao su kien sau khi update
        $('#frmEmloyeeRolesUpdate').ajaxForm(function (data) {
            ajaxUpdateSuccess(data);
        });
        InitAddPermission();
        InitRemovePermission();
    }

});

function SearchPermission() {
    //Nạp tham số
    var param = {};
    param.strKeyWord = $('#txtKeyWord').val();

    $.ajax({
        url: "/Teacher/SearchPermission",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(param),
        dataType: 'json',
        beforeSend: function () {
            showLoading("btnSearch");
        },
        success: function (data) {
            if (data.iserror == false) {
                $('#ContentModelPermission').html(data.content);
                Paging(data.totalpages, data.totalrows, 1);
                InitAddPermission();
            } else {
                $.notify("Đã có lỗi xảy ra! Vui lòng nhấn F5 để thử lại.", "error");
            }

            hideLoading("btnSearch");
        },
        error: function (xhr, status, error) {
            $.notify("Đã có lỗi xảy ra! Vui lòng nhấn F5 để thử lại.", "error");
            hideLoading("btnSearch");
        }
    });
}

function Paging(totalPages, totalRows, startpage) {
    if (totalRows > parseInt(visiblepages)) {
        $('#paging').remove();
        $('#divPagingIndex').html("<ul id='paging' class='pagination'></ul>");
        $('#paging').twbsPagination({
            startPage: startpage,
            totalPages: totalPages,
            visiblePages: visiblepages,
            onPageClick: function (event, page) {
                //Ghi nhận chỉ số trang hiện tại
                $('#hdCurrentPage').val(page - 1); //Vì store bắt đầu từ 0 nên -1

                //Load data ứng với chỉ số page từ server
                if (IsLoadServer == true)
                    SearchAjaxPaging();

                //Thể hiện chỉ số record đang hiển thị trên giao diện
                var visiblePages = parseInt(visiblepages);
                var ToIndextemp = page * visiblePages;
                var FromIndex = (page - 1) * visiblePages + 1;
                //Lấy chỉ số đến trang (Nếu không là trang cuối)
                if (ToIndextemp < totalRows)
                    ToIndex = ToIndextemp;
                else
                    ToIndex = totalRows;

                $('#page-from-index').text(FromIndex);
                $('#page-to-index').text(ToIndex);
                $('#total-record').text(totalRows);

                IsLoadServer = true;
            }
        });
        $('#divPaging').css('display', 'block');
    } else {
        $('#divPaging').css('display', 'none');
    }
}

function SearchAjaxPaging() {
    //Nạp tham số
    var param = {};
    param.strKeyWord = $('#txtKeyWord').attr('data-value');
    param.intPageIndex = $('#hdCurrentPage').val();

    $.ajax({
        url: "/Teacher/SearchPermission",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(param),
        dataType: 'json',
        beforeSend: function () {
        },
        success: function (data) {
            if (data.iserror == false) {
                $('#ContentModelPermission').html(data.content);
                InitAddPermission();
            } else {
                $.notify("Đã có lỗi xảy ra! Vui lòng nhấn F5 để thử lại.", "error");
            }
        },
        error: function (xhr, status, error) {
            $.notify("Đã có lỗi xảy ra! Vui lòng nhấn F5 để thử lại.", "error");
        }
    });
}

function InitAddPermission() {
    $('span[data-name="btnAddPermission"]').each(function () {
        $(this).click(function () {
            //Lấy thông tin
            var idkey = $(this).attr('data-idkey');
            var description = $('#tr_' + idkey).find('td[data-name="description"]').html();

            if (checkIDKeyMainTable(idkey) == true) {
                //Gen HTML để thể hiện ra bảng chính
                var strHTMLPermission = "";

                strHTMLPermission += "<tr id='tr_" + idkey + "'>";
                strHTMLPermission += "<td data-name='idkey'>" + idkey + "</td>";
                strHTMLPermission += "<td data-name='description'>" + description + "</td>";
                strHTMLPermission += "<td><span class='glyphicon glyphicon-remove table-icon' data-name='btnRemovePermission' data-idkey='" + idkey + "'></span></td>";
                strHTMLPermission += "</tr>";

                //Đưa tr vào bảng trên giao diện chính
                if ($('#ContentPermission tr td').length == 1) { //Hiện không có nội dung
                    $('#ContentPermission').empty();
                    $('#ContentPermission').append(strHTMLPermission);
                } else {
                    $('#ContentPermission').append(strHTMLPermission);
                }
            } else {
                $.notify("Quyền đã tồn tại.", "warn");
            }
        });
    });
}

function checkIDKeyMainTable(idkey) {
    var check = true;

    $('#ContentPermission tr').each(function () {
        var idkeytemp = $(this).find('td span[data-name="btnRemovePermission"]').attr('data-idkey');

        if (idkeytemp == idkey)
            check = false;
    });
    return check;
}

function InitRemovePermission() {
    $('#ContentPermission tr').each(function () {
        $(this).find("td span[data-name='btnRemovePermission']").click(function () {
            $(this).parent().parent().remove();

            //Nếu không còn dòng dữ liệu nào
            if ($('#ContentPermission tr').length == 0)
                $('#ContentPermission').append('<tr><td colspan="12" class="table-content-null">Hiện không có nội dung</td></tr>');
        });
    });
}

function InsertPermission() {
    $('#ModalPermission').modal('toggle');
}

function CloseInsertPermission() {
    InitRemovePermission();
    $('#ModalPermission').modal('toggle');
}

function InsertPermissionAll() {
    //Nạp tham số
    var param = {};
    param.strKeyWord = $('#txtKeyWord').val();

    $.ajax({
        url: "/Teacher/SearchPermissionAll",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(param),
        dataType: 'json',
        beforeSend: function () {
            showLoading("btnInsertPermissionAll");
        },
        success: function (data) {
            if (data.iserror == false) {
                $('#ContentPermission').html(data.content);
                //Paging(data.totalpages, data.totalrows, 1);
                InitRemovePermission();
            } else {
                $.notify("Đã có lỗi xảy ra! Vui lòng nhấn F5 để thử lại.", "error");
            }

            hideLoading("btnInsertPermissionAll");
        },
        error: function (xhr, status, error) {
            $.notify("Đã có lỗi xảy ra! Vui lòng nhấn F5 để thử lại.", "error");
            hideLoading("btnInsertPermissionAll");
        }
    });
}

function BackManage() {
    setTimeout(function () { window.location.href = "/quan-ly-giao-vien/" }, 500);
}

function InsertTeacher() {
    var arrPermission = [];
    showLoading("btnInsert");

    //Lấy danh sách quyền
    if ($('#ContentPermission tr td').length > 1) {
        $('#ContentPermission tr').each(function () {
            var idkey = $(this).find('td[data-name="idkey"]').html();

            var objPermission = {
                IDKey: idkey
            };

            arrPermission.push(objPermission);
        });
        $('#hdListPermission').val(JSON.stringify(arrPermission));
    }

    $('#frmTeacherInsert').submit();
    return false;
}

//Ham thuc hien sau khi insert
function ajaxInsertSuccess(data) {
    hideLoading("btnInsert");

    if (data.iserror == false) {
        $.notify("Thêm nhóm người dùng thành công.", "success");
        setTimeout(function () {
            window.location.href = "/quan-ly-nhom-nguoi-dung";
        }, 500);
    }
    else {
        $.notify("Đã có lỗi xảy ra! Vui lòng nhấn F5 để thử lại.", "error");
    }
}

///////////////////////////////////////////////////CẬP NHẬT/////////////////////////////////////////////////
function UpdateEmloyeeRoles() {
    var arrPermission = [];
    showLoading("btnUpdate");

    //Lấy danh sách quyền
    if ($('#ContentPermission tr td').length > 1) {
        $('#ContentPermission tr').each(function () {
            var idkey = $(this).find('td[data-name="idkey"]').html();

            var objPermission = {
                IDKey: idkey
            };

            arrPermission.push(objPermission);
        });
        $('#hdListPermission').val(JSON.stringify(arrPermission));
    }
    debugger;
    $('#frmEmloyeeRolesUpdate').submit();
    return false;
}

//Ham thuc hien sau khi update
function ajaxUpdateSuccess(data) {
    hideLoading("btnUpdate");

    if (data.iserror == false) {
        $.notify("Cập nhật nhóm người dùng thành công.", "success");
        setTimeout(function () {
            var strRoleID = $('#hdRoleID').val();
            setTimeout(function () { window.location.href = "/quan-ly-nhom-nguoi-dung/chi-tiet/" + strRoleID }, 500);
        }, 500);
    }
    else {
        $.notify("Đã có lỗi xảy ra! Vui lòng nhấn F5 để thử lại.", "error");
    }
}