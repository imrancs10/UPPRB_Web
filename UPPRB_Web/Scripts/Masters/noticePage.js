'use strict';
function onClickNotice(noticeId, CategoryId) {
    //alert(noticeId + " " + CategoryId);
    fillNotice(noticeId, CategoryId);
}

function fillNotice(noticeId, CategoryId) {
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        data: '{noticeId: ' + noticeId + ',CategoryId: ' + CategoryId + ' }',
        url: '/Master/GetNoticeDetail',
        success: function (data) {
            var element = '';
            $('#noticeList').empty();
            $.each(data, function (key, item) {
                //dropdown.append($('<option></option>').attr('value', entry.LookupId).text(entry.LookupName));
                if (item.filename != '' && item.filename != null) {
                    element += '<li>';
                    if (item.IsNew == true) {
                        element += '<img id="Image1_0" class="w_fl_ico" src="/Content/images/new.gif">';
                    }
                    element += "<a href='/FilesUploaded/Notice/" + item.filename + "' title='Click to view more' target='_blank'>";
                    element += item.Subject + "<span class='whatsnew' style='color:brown'> [ Notice Board ]</span></a>";
                    element += '<p class="date"> Date :' + formatDate(item.NoticeDate) + '</p></li>';
                }
                else {
                    element += '<li>';
                    if (item.IsNew == true) {
                        element += '<img id="Image1_0" class="w_fl_ico" src="/Content/images/new.gif">';
                    }
                    element += "<a href='" + item.fileURL + "' title='Click to view more' target='_blank'>";
                    element += item.Subject + "<span class='whatsnew' style='color:brown'> [ Notice Board ]</span></a>";
                    element += '<p class="date"> Date :' + formatDate(item.NoticeDate) + '</p></li>';
                }
            });
            $('#noticeList').append(element);
        },
        failure: function (response) {
            console.log(response);
        },
        error: function (response) {
            console.log(response.responseText);
        }
    });
}

function formatDate(noticeDate) {
    var milli = noticeDate.replace(/\/Date\((-?\d+)\)\//, '$1');
    var now = new Date(parseInt(milli));

    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);

    var today = (day) + "-" + (month) + "-" + now.getFullYear();
    return today;
}

$(document).ready(function () {
});

