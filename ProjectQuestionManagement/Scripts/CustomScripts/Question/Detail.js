$(document).ready(function () {


    //Khoi tao su kien sau khi sửa
    $('#frmUpdate').ajaxForm(function (data) {
        ajaxInsertSuccess(data);
    });
    $('#cbochoicetype').val(bg_CTID)
    $('#cbolevel').val(bg_LVID)
    $('#cboHMH').val(bg_TTSID)
});


function ajaxInsertSuccess(data) {
    hideLoading("btnUpdateConfirm");

    if (data.iserror == false) {
        $.notify("Sửa câu hỏi thành công.", "success");
        setTimeout(function () {
            window.location.href = "/quan-ly-cau-hoi/chi-tiet/" + $("#txtQID").val();
        }, 500);
    }
    else {
        $.notify("Đã có lỗi xảy ra! Vui lòng nhấn F5 để thử lại.", "error");
    }
}


function CheckThisAnswer(numb) {
    var slector = "#HDInserttaAnswer_" + numb;
    var value = $(slector).val();
    $('#ViewAnswertaContentA').val(value);
    slector = "#HDInsertipTrueAnswer_" + numb;
    value = $(slector).val();
    if (value == "true" || value == "True") {
        $('#ViewAnswercbRightA').prop('checked', true);
    }
    else {
        $('#ViewAnswercbRightA').prop('checked', false);
    }

    $('#ViewAnswerhdnumbAnswer').val(numb);
    $('#ModalViewAnswer').modal('toggle');
}


function UpdateShow() {
    $("#btnUpdateShow").hide();
    $("#btnNoAnswer").hide();
    $("#btnUpdateConfirm").show();
    $("div.modal-footer > button").show();
    $("div#containerCreateAnswer > button.btnCreateAnswer").show();
    $("textarea").prop('disabled', false);
    $("input").prop('disabled', false);
    $("select").prop('disabled', false);
    $("#txtQID").prop('disabled', true);
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
    slector = "#HDInserttaAnswer_" + numb;
    var datatest = $(slector).data("qid");
    var slector2 = "#HDUpdateAnswer_" + numb;
    if (!$(slector2).length && !(!datatest))
    {
        var html = [];
        html.push(
          " <input id='HDUpdateAnswer_" + numb + "' name='HDUpdateAnswer_" + numb + "' type='hidden' value='" + $(slector).data("qid") + "'>  \n"
        );
        $('#containerCreateAnswer').prepend(html.join(""));
    }
    


}

function DeleteAnswer() {
    var numb = $('#ViewAnswerhdnumbAnswer').val();
    var slector = "#HDInserttaAnswer_" + numb;
    var datatest = $(slector).data("qid");
    $(slector).remove();
    slector = "#HDInsertipTrueAnswer_" + numb;
    $(slector).remove();
    slector = "#btnCheckAnswer" + numb;
    $(slector).remove();
    slector = "#HDUpdateAnswer_" + numb;
    $(slector).remove();
    _numbAnswer--;
    _numdelete++;
    if (!(!datatest)) {
        var html = [];
        html.push(
          " <input id='HDDeleteAnswer_" + _numdelete + "' name='HDDeleteAnswer_" + _numdelete + "' type='hidden' value='" + datatest + "'>  \n"
        );
        $('#containerCreateAnswer').prepend(html.join(""));
    }

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
          " <textarea id='HDInserttaAnswer_" + _numbincreased + "' name='HDInserttaAnswer_" + _numbincreased + "' style='display:none;' data-qid=''>" + $('#createAnswertaContentA').val() + "</textarea> \n",
          " <input id='HDInsertipTrueAnswer_" + _numbincreased + "' name='HDInsertipTrueAnswer_" + _numbincreased + "' type='hidden' value='" + $('#createAnswercbRightA').is(":checked") + "'>  \n",
          ' <button type="button" id="btnCheckAnswer' + _numbincreased + '" class="btn btn-primary" onclick="CheckThisAnswer(' + _numbincreased + ');">Xem/Sửa câu trả lời ' + _numbincreased + '</button> \n'
        );
        $('#containerCreateAnswer').prepend(html.join(""));
    }
}




function Update() {
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
        showLoading("btnUpdateConfirm");


        $('#frmUpdate').submit();
    }

    return false;
}

function BackManage() {
    setTimeout(function () { window.location.href = "/quan-ly-cau-hoi/" }, 500);
}