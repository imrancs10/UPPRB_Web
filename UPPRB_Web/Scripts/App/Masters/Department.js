/// <reference path="../../jquery-1.10.2.js" />
/// <reference path="../Global/App.js" />
/// <reference path="../Global/Utility.js" />
'use strict'
var department = {};
$(document).ready(function () {
    department.getData();
});

department.getData = function () {
    var url = app.urls.departmentList;
    utility.ajax.helper(url, function (data) {
        var table = $('#deptTable');
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
            var urlDepartment = app.urls.departmentById;
            var depImage = null;
            utility.ajax.helperWithData(urlDepartment, { deptId: ele["DepartmentId"] }, function (dataDepartment) {
                depImage = dataDepartment.ImageUrl;
                var tr = '<tr>';
                var td = '<td>' + (ind + 1) + '</td>';
                $(binderArray).each(function (ind1, ele1) {
                    var text = ele[ele1];
                    if (ele1 == "Image" && depImage !== null && typeof depImage !== typeof undefined) {
                        td = td + "<td><img src='" + depImage + "' alt='' class='img-responsive galimg' style='height:100px;width:100px;'/></td>";
                    }
                    else if (ele1 == "ImageUrl") {
                        td = td + '<td class="hidden">' + depImage + '</td>';
                    }
                    else if (text !== null && typeof text !== typeof undefined) {
                        td = td + '<td>' + text + '</td>';
                    }
                    else {
                        td = td + '<td></td>';
                    }
                });
                td = td + '<td><div class="btn-group" role="group" aria-label="Basic example">' +
                    '<button type="button" id="btnEdit" class="btn btn-secondary" data-id=' + ele["DepartmentId"] + ' onclick="department.edit(this)">Edit</button>' +
                    '<button type="button" class="btn btn-danger" data-id=' + ele["DepartmentId"] + ' onclick="department.delete(this)">Delete</button>' +
                    '</div></td>';
                tr = tr + td + '</tr>';
                $(tbody).append(tr);
            }, undefined, undefined, false);
        });
    });
}

department.RemoveFile = function (_this) {
    var html = '<input type="file" name="Image" id="Image" accept=".gif,.jpg,.jpeg,.png"/>';
    $(html).insertAfter(_this);
    $(_this).parent().find('a').remove();
    return false;
}

department.addNew = function () {
    if ($('[name="txtDepartment"]').length > 0) {
        utility.alert.setAlert(utility.alert.alertType.warning, 'One row is already in add mode');
    }
    else {
        var table = $('#deptTable');
        var tbody = $(table).find('tbody');
        var trLen = $(tbody).find('tr').length;
        var tr = '<tr>';
        var td = '<td>' + (trLen + 1) + '</td>';
        td = td + '<td> <input type="text" class="form-control" name="txtDepartment" value="" /></td>';
        td = td + '<td> <input type="text" class="form-control" name="txtDepartmentUrl" value="" /></td>';
        td = td + '<td> <input type="text" class="form-control" name="Description" value="" /></td>';
        td = td + '<td> <input type="file" name="Image" id="Image" accept=".gif,.jpg,.jpeg,.png"/></td>';
        td = td + '<td> </td>';
        td = td + '<td><div class="btn-group" role="group" aria-label="Basic example">' +
            '<button type="button" class="btn btn-secondary" onclick="department.save(this)">Save</button>' +
            '<button type="button" class="btn btn-secondary" onclick="department.cancel(this)">Cancel</button>' +
            '</div></td>';
        tr = tr + td + '</tr>';
        $("#deptTable tr:first").after(tr);
        //$(tbody).append(tr);
    }
}

department.cancel = function (row) {
    $(row).parent().parent().parent().remove();
}



department.edit = function (row) {
    if ($('[name="txtDepartment"]').length > 0) {
        utility.alert.setAlert(utility.alert.alertType.warning, 'One row is already in editable mode');
    }
    else {
        //var td = $(row).parent().parent().parent().find('td:eq(1)');
        var departmentNameTd = $(row).parent().parent().parent().find('td:eq(1)');
        var departmentUrlTd = $(row).parent().parent().parent().find('td:eq(2)');
        var DescriptionTd = $(row).parent().parent().parent().find('td:eq(3)');
        var ImageTd = $(row).parent().parent().parent().find('td:eq(4)');
        var ImageUrlTd = $(row).parent().parent().parent().find('td:eq(5)');

        var departmentName = $(departmentNameTd).text();
        var departmentUrl = $(departmentUrlTd).text();
        var Description = $(DescriptionTd).text();
        var ImageUrl = $(ImageUrlTd).text();

        $(departmentNameTd).empty().append('<input type="text" name="txtDepartment" class="form-control" value="' + departmentName + '" />');
        $(departmentUrlTd).empty().append('<input type="text" name="txtDepartmentUrl" class="form-control" value="' + departmentUrl + '" />');
        $(DescriptionTd).empty().append('<input type="text" name="Description" class="form-control" value="' + Description + '" />');
        if (ImageUrl != '' && ImageUrl != "null") {
            $(ImageTd).empty().append("<a><img src='" + ImageUrl + "' alt='' class='img-responsive galimg' style='height:100px;width:100px;'/></a><a href='javascript:void(0)' onclick='javascript:return department.RemoveFile(this)' style='color:red;padding-left:7px;'>X</a>");
        }
        else {
            $(ImageTd).empty().append('<input type="file" name="Image" id="Image" accept=".gif,.jpg,.jpeg,.png"/>');
        }

        $(ImageUrlTd).empty().append(ImageUrl);
        $(row).parent().prepend('<button type="button" id="btnUpdate" class="btn btn-secondary" data-id="' + $(row).data('id') + '" onclick="department.update(this)">Update</button>');
        $(row).parent().append('<button type="button" class="btn btn-secondary" onclick="department.cancelEdit(this,' + $(row).data('id') + ')">cancel</button>');
        $(row).remove();

    }
}

department.update = function (row) {
    var deptName = $(row).parent().parent().parent().find('input[name="txtDepartment"]').val();
    var deptUrl = $(row).parent().parent().parent().find('input[name="txtDepartmentUrl"]').val();
    var deptDesc = $(row).parent().parent().parent().find('input[name="Description"]').val();
    var deptId = $(row).data('id');
    var url = app.urls.departmentEdit;
    utility.ajax.helperWithData(url, { deptName: deptName, deptUrl: deptUrl, deptDesc: deptDesc, deptId: deptId }, function (data) {
        if (data == 'Data has been updated') {
            department.saveFiles(row);
            utility.alert.setAlert(utility.alert.alertType.success, 'Data has been updated');
        }
    });
}

department.cancelEdit = function (row, id) {
    //var preText = $(row).parent().parent().parent().find('td:eq(1) input[type="text"]').val();
    //$(row).parent().parent().parent().find('td:eq(1)').empty().text(preText);
    //$(row).parent().prepend('<button type="button" class="btn btn-secondary" data-id="' + id + '" onclick="department.edit(this)">Edit</button>');
    //$('#btnUpdate').remove();
    //$(row).remove();
    department.getData();
}

department.delete = function (row) {
    var deptId = $(row).data('id');
    var url = app.urls.departmentDelete;
    utility.confirmBox('Are you sure..!\n\n You wanr to delete', 'Confirmation', function () {
        utility.ajax.helperWithData(url, { deptId: deptId }, function (data) {
            if (data == 'Data delete from database') {
                utility.alert.setAlert(utility.alert.alertType.success, 'Data delete from database');
                department.getData();
            }
        });
        $(this).dialog("close");
    }, function () {
        $(this).dialog("close");
    });


}

department.save = function (row) {
    var deptName = $(row).parent().parent().parent().find('input[name="txtDepartment"]').val();
    var deptUrl = $(row).parent().parent().parent().find('input[name="txtDepartmentUrl"]').val();
    var deptDesc = $(row).parent().parent().parent().find('input[name="Description"]').val();
    var url = app.urls.departmentSave;
    utility.ajax.helperWithData(url, { deptName: deptName, deptUrl: deptUrl, deptDesc: deptDesc }, function (data) {
        if (data == 'Data has been saved') {
            department.saveFiles(row);
            utility.alert.setAlert(utility.alert.alertType.success, 'Data has been saved');
        }
    });
}
department.saveFiles = function (row) {
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
                url: '/Masters/DepartmentImageSave',
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                data: fileData,
                success: function (data) {
                    $(row).parent().parent().parent()[0].remove();
                    department.getData();
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
            department.getData();
        }
    }
}