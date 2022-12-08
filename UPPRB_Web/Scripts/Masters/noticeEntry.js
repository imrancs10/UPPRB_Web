'use strict';
$(document).ready(function () {
    $('#btnAddNotice').click(function () {
        $('#addEditNoticeModel').modal('show');
    });
    //FillNoticeCategory();
    //FillNoticeSubCategory();
    FillEntryType();
    FillNoticeType();
    function FillEntryType(selectedEntryTypeId = null) {
        let dropdown = $('#EntryType');
        dropdown.empty();
        dropdown.append('<option value="">Select</option>');
        dropdown.prop('selectedIndex', 0);
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{lookupTypeId: 0,lookupType: "UploadType" }',
            url: '/Master/GetLookupDetail',
            success: function (data) {
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').attr('value', entry.LookupId).text(entry.LookupName));
                });
                if (selectedEntryTypeId != null) {
                    dropdown.val(selectedEntryTypeId);
                }
            },
            failure: function (response) {
                console.log(response);
            },
            error: function (response) {
                console.log(response.responseText);
            }
        });
    }
    $('#EntryType').on('change', function (e) {
        var valueSelected = $("#EntryType option:selected").text();
        if (valueSelected == 'Notice')
            $('#divNotice').css('display', '');
        else {
            $('#divNotice').css('display', 'none');
            $('#NoticeCategory').val("");
            $('#NoticeType').val("");
        }
        $('[name*=EntryTypeName]').val(valueSelected);
    });
    $('[name*=customRadioInline1]').on('change', function (e) {
        var valueSelected = this.value;
        if (valueSelected == 1) {
            $('[name*=fileURL]').removeAttr('disabled');
            $('[name*=postedFile]').prop("disabled", true);
            $('[name*=postedFile]').val(null);
        }
        else {
            $('[name*=fileURL]').prop("disabled", true);
            $('[name*=postedFile]').removeAttr('disabled');
        }
    });
    function FillNoticeType(selectedNoticeTypeId = null) {
        let dropdown = $('#NoticeType');
        dropdown.empty();
        dropdown.append('<option value="">Select</option>');
        dropdown.prop('selectedIndex', 0);
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{lookupTypeId: 0,lookupType: "NoticeType" }',
            url: '/Master/GetLookupDetail',
            success: function (data) {
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').attr('value', entry.LookupId).text(entry.LookupName));
                });
                if (selectedNoticeTypeId != null) {
                    dropdown.val(selectedNoticeTypeId);
                }
            },
            failure: function (response) {
                console.log(response);
            },
            error: function (response) {
                console.log(response.responseText);
            }
        });
    }
    $('#NoticeType').on('change', function (e) {
        var valueSelected = this.value;
        FillNoticeCategory(valueSelected);
    });
   
    $('#NoticeCategory').on('change', function (e) {
        var valueSelected = this.value;
        FillNoticeSubCategory(valueSelected);
    });
    function FillNoticeSubCategory(NoticeCategoryId, selectedNoticeCategoryId = null) {
        let dropdown = $('#NoticeSubCategory');
        dropdown.empty();
        dropdown.append('<option value="">Select</option>');
        dropdown.prop('selectedIndex', 0);
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{lookupTypeId: 0,lookupType: "NoticeSubCategory" }',
            url: '/Master/GetLookupDetail',
            success: function (data) {
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').attr('value', entry.LookupId).text(entry.LookupName));
                });
                if (selectedNoticeCategoryId != null) {
                    dropdown.val(selectedNoticeCategoryId);
                }
            },
            failure: function (response) {
                console.log(response);
            },
            error: function (response) {
                console.log(response.responseText);
            }
        });
    }
});

function FillNoticeCategory(NoticeTypeId, selectedNoticeCategoryId = null) {
    let dropdown = $('#NoticeCategory');
    dropdown.empty();
    dropdown.append('<option value="">Select</option>');
    dropdown.prop('selectedIndex', 0);
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        data: '{lookupTypeId: "' + NoticeTypeId + '",lookupType: "NoticeCategory" }',
        url: '/Master/GetLookupDetail',
        success: function (data) {
            $.each(data, function (key, entry) {
                dropdown.append($('<option></option>').attr('value', entry.LookupId).text(entry.LookupName));
            });
            if (selectedNoticeCategoryId != null) {
                dropdown.val(selectedNoticeCategoryId);
            }
        },
        failure: function (response) {
            console.log(response);
        },
        error: function (response) {
            console.log(response.responseText);
        }
    });
}

function EditNotice(Id, EntryTypeId, NoticeType, NoticeCategoryId, Subject, NoticeDate, fileURL, filename, IsNew, EntryTypeName) {
    $('#hiddenId').val(Id);
    //$('#btnSave').val('Update');
    $('#EntryType').val(EntryTypeId);
    $('#NoticeType').val(NoticeType);
    FillNoticeCategory(NoticeType, NoticeCategoryId);
    //$('#NoticeCategory').val(NoticeCategoryId);
    $('#Subject').val(Subject);
    $('#NoticeDate').val(NoticeDate);
    $('#fileURL').val(fileURL);
    //$('#customFile').val(filename);
    $('#EntryType').change();
    //if (EntryTypeName == 'Notice')
    //    $('#divNotice').css('display', '');
    //$('#btnAddNotice')[0].click();
    $('#myModal').modal('show');
}
function formatDate(noticeDate) {
    var milli = noticeDate.replace(/\/Date\((-?\d+)\)\//, '$1');
    var now = new Date(parseInt(milli));

    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);

    var today = (day) + "-" + (month) + "-" + now.getFullYear();
    return today;
}