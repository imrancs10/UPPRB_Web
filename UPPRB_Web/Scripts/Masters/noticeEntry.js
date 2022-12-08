'use strict';
$(document).ready(function () {

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



    function FillSchemeName(SchemeTypeId, selectedSchemeNameId = null) {
        let dropdown = $('#SchemeName');
        dropdown.empty();
        dropdown.append('<option value="">Select</option>');
        dropdown.prop('selectedIndex', 0);
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{lookupTypeId: "' + SchemeTypeId + '",lookupType: "SchemeName" }',
            url: '/Masters/GetLookupDetail',
            success: function (data) {
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').attr('value', entry.LookupId).text(entry.LookupName));
                });
                if (selectedSchemeNameId != null) {
                    dropdown.val(selectedSchemeNameId);
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
    $('#SchemeName').on('change', function (e) {
        var valueSelected = this.value;
        FillSector(valueSelected);
    });
});

function editUpload(id) {
    alert(id);
}
