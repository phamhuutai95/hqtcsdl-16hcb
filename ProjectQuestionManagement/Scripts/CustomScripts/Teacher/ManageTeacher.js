var IsLoadServer = false;

$(document).ready(function () {
    $("#menu-toggle").click(function () {
        $("#sidebar-hidden-div").toggle();
        if ($("#sidebar-hidden-div").css('display') == "none")
            $("#page-wrapper").prop('id', 'page-wrapper-disable');
        else
            $("#page-wrapper-disable").prop('id', 'page-wrapper');
    });
});

function SearchTeacher() {
    //Nạp tham số
    var param = {};
    param.strKeyWord = $('#txtKeyWord').val();

    $.ajax({
        url: "/Teacher/SearchTeacher",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(param),
        dataType: 'json',
        beforeSend: function () {
            $("#btnSearch").button('loading');
        },
        success: function (data) {
            if (data.iserror == false) {
                $('#ContentTeacher').html(data.content);
                Paging(data.totalpages, data.totalrows, 1);
                InitProcessInTable();

                //Gán data để xóa có data load lại trang
                $('#txtKeyWord').attr('data-value', $('#txtKeyWord').val());
                $('#cboSubject').attr('data-value', $('#cboSubject').val());
                $('#chkIsBlock').attr('data-value', $('#chkIsBlock').val());
            } else {
                $.notify("Đã có lỗi xảy ra! Vui lòng nhấn F5 để thử lại.", "error");
            }

            $("#btnSearch").button('reset');
        },
        error: function (xhr, status, error) {
            $.notify("Đã có lỗi xảy ra! Vui lòng nhấn F5 để thử lại.", "error");
            $("#btnSearch").button('reset');
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
    param.intSubjectID = $('#cboSubject').attr('data-value');
    param.bolIsBlock = $('#chkIsBlock').attr('data-value');
    param.intPageIndex = $('#hdCurrentPage').val();

    $.ajax({
        url: "/Teacher/SearchTeacher",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(param),
        dataType: 'json',
        beforeSend: function () {
        },
        success: function (data) {
            if (data.iserror == false) {
                $('#ContentTeacher').html(data.content);
                //Paging(data.totalpages, data.totalrows, parseInt($('#hdCurrentPage').val()));
                InitProcessInTable();
            } else {
                $.notify("Đã có lỗi xảy ra! Vui lòng nhấn F5 để thử lại.", "error");
            }
        },
        error: function (xhr, status, error) {
            $.notify("Đã có lỗi xảy ra! Vui lòng nhấn F5 để thử lại.", "error");
        }
    });
}

function InitProcessInTable() {
    $('#ContentTeacher tr').each(function () {
        $(this).find('td span[data-name="Detail"]').click(function () {
            var strTeacherID = $(this).closest('td').attr('data-teacherid');

            setTimeout(function () { window.location.href = "/quan-ly-giao-vien/chi-tiet/" + strTeacherID }, 500);
        });

        $(this).find('td span[data-name="Delete"]').click(function () {
            var strTeacherID = $(this).closest('td').attr('data-teacherid');
            var strMessage = "Bạn có muốn xóa giáo viên " + strTeacherID + "?"

            $('#divMessage').html(strMessage);
            $('#ModalConfirmDelete').modal('toggle');
            $('#divMessage').attr('data-id', strTeacherID);
        });
    });
}

function InsertTeacher() {
    setTimeout(function () { window.location.href = "/quan-ly-giao-vien/them/" }, 500);
}

function DeleleTeacher() {
    //Ghi nhận lại số dòng hiện trên màn hình trước khi xóa
    $('#hdCurRowBeforeDelOnScreen').val($('#ContentTeacher tr').length);

    param = {};
    param.strTeacherID = $('#divMessage').attr('data-id');

    $.ajax({
        url: "/Teacher/DeleteTeacher",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(param),
        dataType: 'json',
        beforeSend: function () {
        },
        success: function (data) {
            if (data.iserror == false) {
                SearchTeacherAfterDelete(parseInt($('#hdCurRowBeforeDelOnScreen').val()) - 1);
            } else {
                if (data.message != "") {
                    $.notify(data.message, "error");
                } else {
                    $.notify("Đã có lỗi xảy ra! Vui lòng nhấn F5 để thử lại.", "error");
                }
            }
        },
        error: function (xhr, status, error) {
            $.notify("Đã có lỗi xảy ra! Vui lòng nhấn F5 để thử lại.", "error");
        }
    });
}

function SearchTeacherAfterDelete(CurRowAfterDelOnScreen) {
    //Nếu xóa mà chia hết nghĩa là không còn bất cứ trang nào chưa full phần tử
    if (CurRowAfterDelOnScreen == 0)
        $('#hdCurrentPage').val(parseInt($('#hdCurrentPage').val()) - 1);
    else
        $('#hdCurrentPage').val(parseInt($('#hdCurrentPage').val()));

    //Nạp tham số
    var param = {};
    param.strKeyWord = $('#txtKeyWord').attr('data-value');
    param.intPageIndex = $('#hdCurrentPage').val();

    $.ajax({
        url: "/EmloyeeRoles/SearchEmloyeeRoles",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(param),
        dataType: 'json',
        beforeSend: function () {
        },
        success: function (data) {
            if (data.iserror == false) {
                $('#ContentEmloyeeRoles').html(data.content);
                Paging(data.totalpages, data.totalrows, parseInt($('#hdCurrentPage').val()) + 1);
                InitProcessInTable();
            } else {
                $.notify("Đã có lỗi xảy ra! Vui lòng nhấn F5 để thử lại.", "error");
            }
        },
        error: function (xhr, status, error) {
            $.notify("Đã có lỗi xảy ra! Vui lòng nhấn F5 để thử lại.", "error");
        }
    });
}