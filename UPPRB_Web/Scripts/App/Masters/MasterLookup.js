/// <reference path="../../jquery-1.10.2.js" />
/// <reference path="../Global/App.js" />
/// <reference path="../Global/Utility.js" />
var department = {};
$(document).ready(function () {
    department.getData();
});

department.getData = function () {
    var url = app.urls.masterLookupList;
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
            var tr = '<tr>';
            var td = '<td>' + (ind + 1) + '</td>';
            $(binderArray).each(function (ind1, ele1) {
                td = td + '<td>' + ele[ele1] + '</td>';
            });
            td = td + '<td><div class="btn-group" role="group" aria-label="Basic example">' +
                                '<button type="button" id="btnEdit" class="btn btn-secondary" data-id=' + ele["Id"] + ' onclick="department.edit(this)">Edit</button>' +
                                '<button type="button" class="btn btn-danger" data-id=' + ele["Id"] + ' onclick="department.delete(this)">Delete</button>' +
                            '</div></td>';
            tr = tr + td + '</tr>';
            $(tbody).append(tr);
        });
    });
}

department.addNew = function () {
    if ($('[name="txtName"]').length > 0) {
        utility.alert.setAlert(utility.alert.alertType.warning, 'One row is already in add mode');
    }
    else {
        var table = $('#deptTable');
        var tbody = $(table).find('tbody');
        var trLen = $(tbody).find('tr').length;
        var tr = '<tr>';
        var td = '<td>' + (trLen + 1) + '</td>';
        td = td + '<td> <input type="text" class="form-control" name="txtName" value="" placeholder="Name Without Space/Special character" /></td>';
        td = td + '<td> <input type="text" class="form-control" name="txtValue" value="" /></td>';
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
    if ($('[name="txtName"]').length > 0) {
        utility.alert.setAlert(utility.alert.alertType.warning, 'One row is already in editable mode');
    }
    else {
        var td = $(row).parent().parent().parent().find('td:eq(1)');
        var preText = $(td).text();
        var tdValue = $(row).parent().parent().parent().find('td:eq(2)');
        var preTextValue = $(tdValue).text();
        $(td).empty().append('<input type="text" name="txtName" class="form-control" value="' + preText + '" />');
        $(tdValue).empty().append('<input type="text" name="txtValue" class="form-control" value="' + preTextValue + '" />');
        $(row).parent().prepend('<button type="button" id="btnUpdate" class="btn btn-secondary" data-id="' + $(row).data('id') + '" onclick="department.update(this)">Update</button>');
        $(row).parent().append('<button type="button" class="btn btn-secondary" onclick="department.cancelEdit(this,' + $(row).data('id') + ')">cancel</button>');
        $(row).remove();
    }
}

department.update = function (row) {
    var Name = $(row).parent().parent().parent().find('input[type="text"]').eq(0).val();
    var Value = $(row).parent().parent().parent().find('input[type="text"]').eq(1).val();
    var Id = $(row).data('id');
    var url = app.urls.masterLookupEdit;
    utility.ajax.helperWithData(url, { name: Name, value: Value, deptId: Id }, function (data) {
        if (data == 'Data has been updated') {
            utility.alert.setAlert(utility.alert.alertType.success, 'Data has been updated');
            department.getData();
        }
    });
}

department.cancelEdit = function (row,id) {
    var preText = $(row).parent().parent().parent().find('td:eq(1) input[type="text"]').eq(0).val();
    $(row).parent().parent().parent().find('td:eq(1)').empty().text(preText);
    var preTextValue = $(row).parent().parent().parent().find('td:eq(1) input[type="text"]').eq(1).val();
    $(row).parent().parent().parent().find('td:eq(2)').empty().text(preTextValue);
    $(row).parent().prepend('<button type="button" class="btn btn-secondary" data-id="' + id + '" onclick="department.edit(this)">Edit</button>');
    $('#btnUpdate').remove();
    $(row).remove();
}

department.delete = function (row) {
    var deptId = $(row).data('id');
    var url = app.urls.masterLookupDelete;
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
    var name = $(row).parent().parent().parent().find('input[type="text"]').eq(0).val();
    var value = $(row).parent().parent().parent().find('input[type="text"]').eq(1).val();
    var url = app.urls.masterLookupSave;
    utility.ajax.helperWithData(url, { name: name, value: value  }, function (data) {
        if (data == 'Data has been saved') {
            $(row).parent().parent().parent()[0].remove();
            utility.alert.setAlert(utility.alert.alertType.success, 'Data has been saved');
            department.getData();
        }
    });
}
