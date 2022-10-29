/// <reference path="../../jquery-1.10.2.js" />
/// <reference path="../Global/App.js" />
/// <reference path="../Global/Utility.js" />

function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

$(function () {
    $("#dialog").dialog({
        resizable: false,
        height: "auto",
        width: 400,
        modal: true,
        buttons: {
            "OK": function () {
                $(this).dialog("close");
            }
        }
    });
});

$(document).ready(function () {
    GetDepartmentAndOPDDetail();
    function GetDepartmentAndOPDDetail() {
        $.ajax({
            dataType: 'json',
            type: 'POST',
            async: true,
            url: '/Home/GetDepartmentAndOPDDetail',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                //set text for OPD detail
                var totOPD = "0000";
                var totIPD = "0000";
                var total = "0000";
                if (data.OPDModel != null) {
                    var pdmodel = data.OPDModel;
                    totOPD = pdmodel.totOPD;
                    totIPD = pdmodel.totIPD;
                    total = parseInt(pdmodel.totOPD) + parseInt(pdmodel.totIPD);

                    $('#TotalOPDText').html(totOPD);
                    $('#TotalIPDText').html(totIPD);
                    $('#TotalPatientText').html(total);
                }

                var html = '';

                if (data.Departments != null) {
                    var departments = data.Departments;
                    var rowCount = Math.ceil(departments.length / 3);
                    var row = 0;
                    var index = 0;
                    html = html + "<div class='col-sm-4 lists'>";
                    var i = 0;
                    for (i = 0; i < rowCount; i++) {
                        html = html + "<p><a href='" + departments[i].DepartmentUrl + "' target='_blank'><img src='../img/arrow.png' style='height:20px;width:20px;' /><span class='num'>" + (++index) + "</span> "
                            + departments[i].DeparmentName + "</a></p>";
                        row = row + 1;
                    }
                    html = html + "</div><div class='col-sm-4 lists'>";
                    for (i = 0; i < rowCount; i++) {
                        html = html + "<p><a href='" + departments[row].DepartmentUrl + "' target='_blank'><img src='../img/arrow.png' style='height:20px;width:20px;' /><span class='num'>" + (++index) + "</span> " +
                            departments[row].DeparmentName + "</a></p>";
                        row = row + 1;
                    }
                    html = html + "</div><div class='col-sm-4 lists'>";
                    for (i = 0; i < (departments.length - 2 * rowCount); i++) {
                        html = html + "<p><a href='" + departments[row].DepartmentUrl + "' target='_blank'><img src='../img/arrow.png' style='height:20px;width:20px;' /><span class='num'>" + (++index) + "</span> " +
                            departments[row].DeparmentName + "</a></p>";
                        row = row + 1;
                    }
                    html = html + "</div>";
                    $('#divDepartments').html(html);
                }

            },
            failure: function (response) {
                alert(response);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }
});