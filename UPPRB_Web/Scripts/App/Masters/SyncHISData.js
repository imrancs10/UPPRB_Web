/// <reference path="../../jquery-1.10.2.js" />
/// <reference path="../Global/App.js" />
/// <reference path="../Global/Utility.js" />

var synHISData = {};

$(document).ready(function () {

});

synHISData.SyncHISRenewal = function (row) {
    var patientId = $(row).parent().parent().find('td').eq(0).html();
    this.SyncData(patientId, 1);
};

synHISData.SyncHISRegistration = function (row) {
    var patientId = $(row).parent().parent().find('td').eq(0).html();
    this.SyncData(patientId, 0);
};

synHISData.SyncAlready = function (row) {
    var patientId = $(row).parent().parent().find('td').eq(0).html();
    var result = confirm('are you sure want to proceed?');
    if (result)
        this.SyncDataAlready(patientId);
};

synHISData.SyncData = function (patientId, transactionType) {
    $.ajax({
        dataType: 'json',
        type: 'POST',
        url: '/Masters/SyncHISData',
        data: '{patientId: "' + patientId + '" ,transactionType: "' + transactionType + '" }',
        async: true,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data == true) {
                utility.alert.setAlert("Sync Data", "Patient has been marked as Sync.");
                window.location.reload();
            }
        },
        failure: function (response) {
            alert(response);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
};

synHISData.SyncDataAlready = function (patientId) {
    $.ajax({
        dataType: 'json',
        type: 'POST',
        url: '/Masters/SyncHISAlreadyData',
        data: '{patientId: "' + patientId + '"}',
        async: true,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data == true) {
                utility.alert.setAlert("Sync Data", "Patient has been marked as Sync.");
                window.location.reload();
            }
        },
        failure: function (response) {
            alert(response);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
};



