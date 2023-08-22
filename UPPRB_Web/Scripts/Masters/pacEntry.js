'use strict';
$(document).ready(function () {
    //$('#btnAddNotice').click(function () {
    //    $('#addEditNoticeModel').modal('show');
    //});
    //FillNoticeCategory();
    //FillNoticeSubCategory();
    FillState();
    FillDistrict(0);
    FillRecruitementType();
    FillCenterStatus();
    $('[id*=customRadioInline2]').prop("checked", true);
    /*$('[name*=customRadioInline1]').change();*/
    $('[name*=fileURL]').prop("disabled", true);
    $('[name*=postedFile]').removeAttr('disabled');

    function FillRecruitementType(selectedRecruitementTypeId = null) {
        let dropdown = $('#RecruitementType');
        dropdown.empty();
        dropdown.append('<option value="">Select</option>');
        dropdown.prop('selectedIndex', 0);
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{lookupTypeId: 2,lookupType: "NoticeCategory" }',
            url: '/Master/GetLookupDetail',
            success: function (data) {
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').attr('value', entry.LookupId).text(entry.LookupName));
                });
                dropdown.append('<option value="">Select All</option>');
                if (selectedRecruitementTypeId != null) {
                    dropdown.val(selectedRecruitementTypeId);
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
    function FillState(selectedStateId = null) {
        let dropdown = $('#State');
        dropdown.empty();
        dropdown.append('<option value="">Select</option>');
        dropdown.prop('selectedIndex', 0);
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '',
            url: '/Master/GetStateDetail',
            success: function (data) {
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').attr('value', entry.StateId).text(entry.StateName));
                });
                if (selectedStateId != null) {
                    dropdown.val(selectedStateId);
                }
                dropdown.val(1);
                FillZone(1);
            },
            failure: function (response) {
                console.log(response);
            },
            error: function (response) {
                console.log(response.responseText);
            }
        });
    }
    $('#State').on('change', function (e) {
        var valueSelected = this.value;
        FillZone(valueSelected);
    });

    $('#Zone').on('change', function (e) {
        var valueSelected = this.value;
        FillRange(valueSelected);
    });

    $('#Range').on('change', function (e) {
        var valueSelected = this.value;
        if (valueSelected == "")
            valueSelected = 0;
        FillDistrict(valueSelected);
    });

    $('#District').on('change', function (e) {
        var valueSelected = this.value;
        FillPoliceStation(valueSelected);
    });

    $('#PoliceStation').on('change', function (e) {
        var valueSelected = this.value;
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{psId: ' + valueSelected + '}',
            url: '/Master/GetPACDetailByPSId',
            success: function (data) {
                $('#tableCSPACNo tbody').empty();
                var dom = "<tr>";
                $.each(data, function (key, entry) {
                    dom += "<td><a style='color: blue;white-space: nowrap;' href='/Admin/PACEntry?Id=" + entry.Id + "'>" + entry.PACNumber + "</a></td>";
                });
                dom += "</tr>";
                $('#tableCSPACNo tbody').append(dom)
            },
            failure: function (response) {
                console.log(response);
            },
            error: function (response) {
                console.log(response.responseText);
            }
        });
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
});

function FillZone(StateId, selectedZoneId = null) {
    let dropdown = $('#Zone');
    dropdown.empty();
    dropdown.append('<option value="">Select</option>');
    dropdown.prop('selectedIndex', 0);
    if (StateId != null) {
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{stateId: "' + StateId + '" }',
            url: '/Master/GetZoneDetail',
            success: function (data) {
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').attr('value', entry.ZoneId).text(entry.ZoneName));
                });
                if (selectedZoneId != null) {
                    dropdown.val(selectedZoneId);
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

}

function FillRange(ZoneId, selectedRangeId = null) {
    let dropdown = $('#Range');
    dropdown.empty();
    dropdown.append('<option value="">Select</option>');
    dropdown.prop('selectedIndex', 0);
    if (ZoneId != null) {
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{zoneId: "' + ZoneId + '" }',
            url: '/Master/GetRangeDetail',
            success: function (data) {
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').attr('value', entry.RangeId).text(entry.RangeName));
                });
                if (selectedRangeId != null) {
                    dropdown.val(selectedRangeId);
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

}

function FillDistrict(rangeId, selectedDisctrictId = null) {
    let dropdown = $('#District');
    dropdown.empty();
    dropdown.append('<option value="">Select</option>');
    dropdown.prop('selectedIndex', 0);
    rangeId = rangeId == null ? 0 : rangeId;
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        data: '{rangeId: "' + rangeId + '" }',
        url: '/Master/GetDistrictDetail',
        success: function (data) {
            $.each(data, function (key, entry) {
                dropdown.append($('<option></option>').attr('value', entry.DistrictId).text(entry.DistrictName));
            });
            if (selectedDisctrictId != null) {
                dropdown.val(selectedDisctrictId);
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

function FillPoliceStation(districtId, selectedPoliceStationId = null) {
    let dropdown = $('#PoliceStation');
    dropdown.empty();
    dropdown.append('<option value="">Select</option>');
    dropdown.prop('selectedIndex', 0);
    if (districtId != null) {
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            data: '{districtId: "' + districtId + '" }',
            url: '/Master/GetPoliceStationDetail',
            success: function (data) {
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').attr('value', entry.PSId).text(entry.PSName));
                });
                if (selectedPoliceStationId != null) {
                    dropdown.val(selectedPoliceStationId);
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
}

function FillCenterStatus(selectedCenterStatusId = null) {
    let dropdown = $('#CenterStatus');
    dropdown.empty();
    dropdown.append('<option value="">Select</option>');
    dropdown.prop('selectedIndex', 0);
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        data: '{lookupTypeId: 0,lookupType: "CenterStatus" }',
        url: '/Master/GetLookupDetail',
        success: function (data) {
            $.each(data, function (key, entry) {
                dropdown.append($('<option></option>').attr('value', entry.LookupId).text(entry.LookupName));
            });
            if (selectedCenterStatusId != null) {
                dropdown.val(selectedCenterStatusId);
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

function formatDate(noticeDate) {
    var milli = noticeDate.replace(/\/Date\((-?\d+)\)\//, '$1');
    var now = new Date(parseInt(milli));

    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);

    var today = (day) + "-" + (month) + "-" + now.getFullYear();
    return today;
}
function formatDateyyyyMMdd(noticeDate) {
    if (noticeDate != null) {
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
    return '';
}