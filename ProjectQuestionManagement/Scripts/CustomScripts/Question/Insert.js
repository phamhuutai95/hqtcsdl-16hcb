var IsLoadServer = false;
//Bien so luong Answer
var _numbAnswer = 0;
var _numbincreased = 0;
///////////////////////////////////////////////////THÊM/////////////////////////////////////////////////
$(document).ready(function () {
    //Khoi tao su kien sau khi insert
    $('#frmInsert').ajaxForm(function (data) {
        ajaxInsertSuccess(data);
    });

});



function Insert() {
    var test1 = $("#taContent").val();
    var test2 = $("#cboHMH").val();
    if (test1.trim() === "") {
        $.notify("Nội dung câu hỏi không thể rỗng.", "error");
    }
    else if (test2 === null) {
        $.notify("Bạn chưa chọn hệ-môn học.", "error");
    }
    else {
        $('#createAnswerhdnumberanswer').val(_numbAnswer);
        showLoading("btnInsert");


        $('#frmInsert').submit();
    }
   
    return false;
}


function ajaxInsertSuccess(data) {
    hideLoading("btnInsert");

    if (data.iserror == false) {
        $.notify("Thêm câu hỏi thành công.", "success");
        setTimeout(function () {
            window.location.href = "/quan-ly-cau-hoi";
        }, 500);
    }
    else {
        $.notify("Đã có lỗi xảy ra! Vui lòng nhấn F5 để thử lại.", "error");
    }
}


function BackManage() {
    setTimeout(function () { window.location.href = "/quan-ly-cau-hoi/" }, 500);
}

function CreateAnswer() {
    if (!$('#createAnswertaContentA').val()) {
        $.notify("Nội dung câu trả lời không thể rỗng.", "error");
    }
    else {
        _numbAnswer++;
        _numbincreased++;
        var html = [];
        html.push(
          " <textarea id='HDInserttaAnswer_" + _numbincreased + "' name='HDInserttaAnswer_" + _numbincreased + "' style='display:none;'>" + $('#createAnswertaContentA').val() + "</textarea> \n",
          " <input id='HDInsertipTrueAnswer_" + _numbincreased + "' name='HDInsertipTrueAnswer_" + _numbincreased + "' type='hidden' value='" + $('#createAnswercbRightA').is(":checked") + "'>  \n",
          ' <button type="button" id="btnCheckAnswer' + _numbincreased + '" class="btn btn-primary" onclick="CheckThisAnswer(' + _numbincreased + ');">Xem/Sửa câu trả lời ' + _numbincreased + '</button> \n'
        );
        $('#containerCreateAnswer').prepend(html.join(""));
    }
}


function CheckThisAnswer(numb) {
    var slector = "#HDInserttaAnswer_" + numb;
    var value = $(slector).val();
    $('#ViewAnswertaContentA').val(value);
    slector = "#HDInsertipTrueAnswer_" + numb;
    value = $(slector).val();
    if (value == "true") {
        $('#ViewAnswercbRightA').prop('checked', true);
    }
    else {
        $('#ViewAnswercbRightA').prop('checked', false);
    }

    $('#ViewAnswerhdnumbAnswer').val(numb);
    $('#ModalViewAnswer').modal('toggle');
}


function openModalCA() {
    $('#createAnswertaContentA').val("");
    $('#createAnswercbRightA').prop('checked', false);
    $('#ModalCreateAnswer').modal('toggle');
}

function EditAnswer() {
    var numb = $('#ViewAnswerhdnumbAnswer').val();
    var slector = "#HDInserttaAnswer_" + numb;
    $(slector).val($('#ViewAnswertaContentA').val());
    slector = "#HDInsertipTrueAnswer_" + numb;
    $(slector).val($('#ViewAnswercbRightA').is(":checked"));
}


function DeleteAnswer() {
    var numb = $('#ViewAnswerhdnumbAnswer').val();
    var slector = "#HDInserttaAnswer_" + numb;
    $(slector).remove();
    slector = "#HDInsertipTrueAnswer_" + numb;
    $(slector).remove();
    slector = "#btnCheckAnswer" + numb;
    $(slector).remove();
    _numbAnswer--;
}