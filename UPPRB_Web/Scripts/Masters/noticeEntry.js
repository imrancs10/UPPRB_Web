'use strict';
$(document).ready(function () {
    //$('#btnAddNotice').click(function () {
    //    $('#addEditNoticeModel').modal('show');
    //});
    //FillNoticeCategory();
    //FillNoticeSubCategory();
    FillEntryType();
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
                //get user role
                $.ajax({
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    type: 'POST',
                    url: '/Master/GetUserPermission',
                    success: function (permissionData) {
                        $.each(data, function (key, entry) {
                            if (permissionData.includes(entry.LookupName))
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
        if (valueSelected == 'Notice' || valueSelected == 'RecruitmentRules' || valueSelected == 'Court') {
            FillNoticeType(this.value);
            $('#divNotice').css('display', '');
            $('#divNoticeCategory').css('display', '');
        }
        else if (valueSelected == 'GO' || valueSelected == 'PhotoGalary' || valueSelected == 'Syllabus') {
            FillNoticeType(this.value);
            $('#divNotice').css('display', '');
            $('#divNoticeCategory').css('display', 'none');
        }
        else {
            $('#divNotice').css('display', 'none');
            $('#divNoticeCategory').css('display', 'none');
            $('#NoticeCategory').val("");
            $('#NoticeType').val("");
        }
        if (valueSelected == 'Court')
            $('#lblPublishDate').text('Judgement Date')
        else
            $('#lblPublishDate').text('Publish Date')
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

        const urlParams = new URLSearchParams(location.search);
        const noticeId = urlParams.get('noticeId');
        if (noticeId != null && noticeId != undefined) {
            $('[name*=postedFile]').prop("disabled", true);
        }

    });
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

function FillNoticeType(entryTypeId = null, selectedNoticeTypeId = null) {
    let dropdown = $('#NoticeType');
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        data: '{lookupTypeId: "' + entryTypeId + '",lookupType: "NoticeType" }',
        url: '/Master/GetLookupDetail',
        success: function (data) {
            dropdown.empty();
            dropdown.append('<option value="">Select</option>');
            dropdown.prop('selectedIndex', 0);
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
//, EntryTypeId, NoticeType, NoticeCategoryId, Subject, NoticeDate, fileURL, filename, IsNew, EntryTypeName
function EditNotice(Id) {
    window.location.href = '/Admin/NoticeEntry?noticeId=' + Id;
    //$('#hiddenId').val(Id);
    //$('[name*=EntryTypeName]').val(EntryTypeName);
    //$('[name*=hiddenNoticeID]').val(Id);
    ////$('#btnSave').val('Update');
    //$('#EntryType').val(EntryTypeId);
    ////$('#NoticeType').val(NoticeType);
    //FillNoticeType(EntryTypeId, NoticeType)
    //FillNoticeCategory(NoticeType, NoticeCategoryId);
    ////$('#NoticeCategory').val(NoticeCategoryId);
    //$('#Subject').val(Subject);
    //$('#NoticeDate').val(formatDateyyyyMMdd(NoticeDate));
    //$('#fileURL').val(fileURL != "null" ? fileURL : "");
    ////$('#customFile').val(filename);
    //$('#EntryType').change();
    ////if (EntryTypeName == 'Notice')
    ////    $('#divNotice').css('display', '');
    ////$('#btnAddNotice')[0].click();
    //$('#myModal').modal('show');
    //$('#highlightNew').prop('checked', IsNew)
    //if (filename != null && filename != "" && filename != undefined && filename != "")
    //    $('[id*=customRadioInline2]').prop("checked", true);
    //else
    //    $('[id*=customRadioInline1]').prop("checked", true);
    //$('[name*=customRadioInline1]').change();
}
function formatDate(noticeDate) {
    var milli = noticeDate.replace(/\/Date\((-?\d+)\)\//, '$1');
    var now = new Date(parseInt(milli));

    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);

    var today = (day) + "-" + (month) + "-" + now.getFullYear();
    return today;
}
function formatDateyyyyMMdd(noticeDate) {
    var milli = 0;
    if (noticeDate.source) {
        milli = noticeDate.source.replace("Date(", "").replace(")", "");
    }
    else {
        milli = noticeDate.replace("Date(", "").replace(")", "").replaceAll("/", "");
    }
    var now = new Date(parseInt(milli));

    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);

    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    return today;
}