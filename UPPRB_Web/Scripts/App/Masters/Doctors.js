﻿/// <reference path="../../jquery-1.10.2.js" />
/// <reference path="../Global/App.js" />
/// <reference path="../Global/Utility.js" />

var doctor = {};

$(document).ready(function () {
    utility.bindDdlByAjax(app.urls.commonDepartmentList, 'ddlDepartment', 'DeparmentName', 'DepartmentId', function () {
        doctor.getData();
    });
});

doctor.addNew = function () {
    if ($('#txtDoctor').length > 0) {
        utility.alert.setAlert(utility.alert.alertType.warning, 'One row is already in add mode');
    }
    else {
        var table = $('#doctorTable');
        var tbody = $(table).find('tbody');
        var trLen = $(tbody).find('tr').length;
        var tr = '<tr>';
        var td = '<td>' + (trLen + 1) + '</td>';
        td = td + '<td> <select class="form-control" required="required">' + $('#ddlDepartment').clone().html() + '</select></td>';
        td = td + '<td> <input type="text" id="txtDoctor" placeholder="Doctor name" class="form-control" name="txtDoctor" value="" /></td>';
        td = td + '<td> <input type="text" id="txtDesignation" placeholder="Designation" class="form-control" name="txtDesignation" value="" /></td>';
        td = td + '<td> <input type="text" id="txtDegree" class="form-control" placeholder="Degree" name="txtDegree" value="" /></td>';
        td = td + '<td> <input type="text" class="form-control" name="Description" value="" /></td>';
        td = td + '<td> <input type="file" name="Image" id="Image" accept=".gif,.jpg,.jpeg,.png"/></td>';
        td = td + '<td> </td>';
        td = td + '<td><div class="btn-group" role="group" aria-label="Basic example">' +
            '<button type="button" class="btn btn-secondary" onclick="doctor.save(this)">Save</button>' +
            '<button type="button" class="btn btn-secondary" onclick="doctor.cancel(this)">Cancel</button>' +
            '</div></td>';
        tr = tr + td + '</tr>';
        //$(tbody).append(tr);
        $("#doctorTable tr:first").after(tr);
    }
}

doctor.cancel = function (row) {
    $(row).parent().parent().parent().remove();
}

doctor.getData = function () {
    var url = app.urls.doctorList;
    utility.ajax.helper(url, function (data) {
        var table = $('#doctorTable');
        if (table.length == 0)
            throw new Error('Table not found');
        var tbody = $(table).find('tbody');
        tbody.empty();
        var binderArray = [];

        $(table).find('thead tr th').each(function (ind, ele) {
            if ($(ele).attr('name') !== undefined) {
                binderArray.push($(ele).attr('name'));
            }
        });

        $(data).each(function (ind, ele) {
            var urlDoctor = app.urls.doctorById;
            var doctorImage = null;
            utility.ajax.helperWithData(urlDoctor, { doctorId: ele["DoctorId"] }, function (dataDoctor) {
                doctorImage = dataDoctor.ImageUrl;
                var tr = '<tr>';
                var td = '<td>' + (ind + 1) + '</td>';
                $(binderArray).each(function (ind1, ele1) {
                    var text = ele[ele1];
                    if (ele1 == "Image" && doctorImage !== null && typeof doctorImage !== typeof undefined) {
                        td = td + "<td data-deptid='" + ele["DepartmentId"] + "'><img src='" + doctorImage + "' alt='' class='img-responsive galimg' style='height:100px;width:100px;'/></td>";
                    }
                    else if (ele1 == "ImageUrl") {
                        td = td + '<td class="hidden" data-deptid="' + ele["DepartmentId"] + '">' + doctorImage + '</td>';
                    }
                    else if (text !== null && typeof text !== typeof undefined) {
                        td = td + '<td data-deptid="' + ele["DepartmentId"] + '">' + text + '</td>';
                    }
                    else {
                        td = td + '<td data-deptid="' + ele["DepartmentId"] + '"></td>';
                    }
                    //td = td + '<td data-deptid="' + ele["DepartmentId"] + '">' + ele[ele1] + '</td>';
                });
                td = td + '<td><div class="btn-group" role="group" aria-label="Basic example">' +
                    '<button type="button" id="btnEdit" class="btn btn-secondary" data-docid="' + ele["DoctorId"] + '" data-deptid="' + ele["DepartmentId"] + '" onclick="doctor.edit(this)">Edit</button>' +
                    '<button type="button" class="btn btn-danger" data-docid="' + ele["DoctorId"] + '" data-deptid="' + ele["DepartmentId"] + '" onclick="doctor.delete(this)">Delete</button>' +
                    '</div></td>';
                tr = tr + td + '</tr>';
                $(tbody).append(tr);
            }, undefined, undefined, false);

        });
    });
}

doctor.RemoveFile = function (_this) {
    var html = '<input type="file" name="Image" id="Image" accept=".gif,.jpg,.jpeg,.png"/>';
    $(html).insertAfter(_this);
    $(_this).parent().find('a').remove();
    return false;
}

doctor.save = function (row) {
    var mainContainer = $(row).parent().parent().parent();
    let docName = $(mainContainer).find('input[id="txtDoctor"]').val();
    let designation = $(mainContainer).find('input[id="txtDesignation"]').val();
    let degree = $(mainContainer).find('input[id="txtDegree"]').val();
    let deptId = $(mainContainer).find('select').find(':selected').val();
    var deptDesc = $(mainContainer).find('input[name="Description"]').val();
    if (deptId != null && typeof deptId !== undefined && deptId !== '') {

        if (docName != null && typeof docName !== undefined && docName !== '') {
            var url = app.urls.doctorSave;
            var param = {};
            param.doctorName = docName;
            param.deptId = deptId;
            param.designation = designation;
            param.degree = degree;
            param.doctorDesc = deptDesc;
            utility.ajax.helperWithData(url, param, function (data) {
                if (data == 'Data has been saved') {
                    //$(row).parent().parent().parent()[0].remove();
                    utility.alert.setAlert(utility.alert.alertType.success, 'Data has been saved');
                    //doctor.getData();
                    doctor.saveFiles(row);
                }
            });

        }
        else {
            utility.alert.setAlert(utility.alert.alertType.warning, 'Doctor name is required');
            $(mainContainer).find('input[type="text"]').focus();
        }
    }
    else {
        utility.alert.setAlert(utility.alert.alertType.warning, 'Please select department');
        $(mainContainer).find('select').focus();
    }
}


doctor.edit = function (row) {
    if ($('#txtDoctor').length > 0) {
        utility.alert.setAlert(utility.alert.alertType.warning, 'One row is already in editable mode');
    }
    else {
        var mainContainer = $(row).parent().parent().parent();
        var doctortd = $(mainContainer).find('td:eq(2)');
        var depttd = $(mainContainer).find('td:eq(1)');
        var desigtd = $(mainContainer).find('td:eq(3)');
        var degreetd = $(mainContainer).find('td:eq(4)');
        var DescriptionTd = $(mainContainer).find('td:eq(5)');
        var ImageTd = $(mainContainer).find('td:eq(6)');
        var ImageUrlTd = $(mainContainer).find('td:eq(7)');
        var docName = $(doctortd).text();
        let designatoin = $(desigtd).text();
        let degree = $(degreetd).text();
        var Description = $(DescriptionTd).text();
        var ImageUrl = $(ImageUrlTd).text();
        $(doctortd).empty().append('<input id="txtDoctor" placeholder="Doctor Name" type="text" class="form-control" value="' + docName + '" />');
        $(desigtd).empty().append('<input id="txtDesignation" placeholder="Designation" type="text" class="form-control" value="' + designatoin + '" />');
        $(degreetd).empty().append('<input id="txtDegree" placeholder="Degree" type="text" class="form-control" value="' + degree + '" />');
        $(depttd).empty().append('<select class="form-control" required="required">' + $('#ddlDepartment').clone().html() + '</select>');
        $(depttd).find('select').val($(depttd).data('deptid'));
        $(DescriptionTd).empty().append('<input type="text" name="Description" class="form-control" value="' + Description + '" />');
        if (ImageUrl != '' && ImageUrl != "null") {
            $(ImageTd).empty().append("<a><img src='" + ImageUrl + "' alt='' class='img-responsive galimg' style='height:100px;width:100px;'/></a><a href='javascript:void(0)' onclick='javascript:return doctor.RemoveFile(this)' style='color:red;padding-left:7px;'>X</a>");
        }
        else {
            $(ImageTd).empty().append('<input type="file" name="Image" id="Image" accept=".gif,.jpg,.jpeg,.png"/>');
        }

        $(ImageUrlTd).empty().append(ImageUrl);
        $(row).parent().prepend('<button type="button" id="btnUpdate" class="btn btn-secondary" data-docid="' + $(row).data('docid') + '" data-deptid="' + $(row).data('deptid') + '" onclick="doctor.update(this)">Update</button>');
        $(row).parent().append('<button type="button" class="btn btn-secondary" onclick="doctor.cancelEdit(this,' + $(row).data('deptid') + ')">cancel</button>');
        $(row).remove();
    }
}

doctor.update = function (row) {
    var mainContainer = $(row).parent().parent().parent();
    var docName = $(mainContainer).find('input[type="text"]').val();
    var deptId = $(mainContainer).find('select').find(':selected').val();
    var doctorId = $(row).data('docid');
    let designation = $(mainContainer).find('input[id="txtDesignation"]').val();
    let degree = $(mainContainer).find('input[id="txtDegree"]').val();
    var deptDesc = $(row).parent().parent().parent().find('input[name="Description"]').val();
    if (deptId != null && typeof deptId !== undefined && deptId !== '') {

        if (docName != null && typeof docName !== undefined && docName !== '') {

            var url = app.urls.doctorEdit;
            var param = {};
            param.doctorName = docName;
            param.deptId = deptId;
            param.docId = doctorId;
            param.designation = designation;
            param.degree = degree;
            param.doctorDesc = deptDesc;
            utility.ajax.helperWithData(url, param, function (data) {
                if (data == 'Data has been updated') {
                    utility.alert.setAlert(utility.alert.alertType.success, 'Data has been updated');
                    //doctor.getData();
                    doctor.saveFiles(row);
                }
            });

        }
        else {
            utility.alert.setAlert(utility.alert.alertType.warning, 'Doctor name is required');
            $(mainContainer).find('input[type="text"]').focus();
        }
    }
    else {
        utility.alert.setAlert(utility.alert.alertType.warning, 'Please select department');
        $(mainContainer).find('select').focus();
    }
}

doctor.cancelEdit = function (row, id) {
    //var mainContainer = $(row).parent().parent().parent();
    //var degree = $(mainContainer).find('td:eq(4) input[id="txtDegree"]').val();
    //var designation = $(mainContainer).find('td:eq(3) input[id="txtDesignation"]').val();
    //var doctorname = $(mainContainer).find('td:eq(2) input[id="txtDoctor"]').val();
    //var department = $(mainContainer).find('td:eq(1) select').val(id).find(':selected').text();
    //$(mainContainer).find('td:eq(2)').empty().text(doctorname);
    //$(mainContainer).find('td:eq(1)').empty().text(department);
    //$(mainContainer).find('td:eq(3)').empty().text(designation);
    //$(mainContainer).find('td:eq(4)').empty().text(degree);
    //$(row).parent().prepend('<button type="button" class="btn btn-secondary" data-id="' + id + '" onclick="doctor.edit(this)">Edit</button>');
    //$('#btnUpdate').remove();
    //$(row).remove();
    doctor.getData();
}

doctor.delete = function (row) {
    var docId = $(row).data('docid');
    var url = app.urls.doctorDelete;
    utility.confirmBox('Are you sure..!\n\n You wanr to delete', 'Confirmation', function () {
        utility.ajax.helperWithData(url, { docId: docId }, function (data) {
            if (data == 'Data delete from database') {
                utility.alert.setAlert(utility.alert.alertType.success, 'Data delete from database');
                doctor.getData();
            }
        });
        $(this).dialog("close");
    }, function () {
        $(this).dialog("close");
    });

}

doctor.saveFiles = function (row) {
    //save files
    if (window.FormData !== undefined) {
        var fileData = new FormData();
        //image
        var fileUpload = $(row).parent().parent().parent().find('input[type="file"]').get(0);
        if (fileUpload != undefined) {
            var files = fileUpload.files;
            fileData.append(files[0].name, files[0]);
            $.ajax({
                dataType: 'json',
                type: 'POST',
                url: '/Masters/DoctorImageSave',
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                data: fileData,
                success: function (data) {
                    $(row).parent().parent().parent()[0].remove();
                    doctor.getData();
                },
                failure: function (response) {
                    alert(response);
                },
                error: function (response) {
                    alert(response.responseText);
                }
            });
        }
        else {
            $(row).parent().parent().parent()[0].remove();
            doctor.getData();
        }
    }
}

