var dataTable;

$(document).ready(function () {
    
    loadDepartmentList();
});

function loadDepartmentList() {

    if ($.fn.DataTable.isDataTable("#tblDepartment")) {
        $('#tblDepartment').DataTable().destroy();
    }
    var j = 1;

    window.dataTableInstance = $('#tblDepartment').DataTable({
        "ajax": {
            url: "/Department/GetAll",
            type: "GET",
            datatype: "json"

        },
        "columns": [
            {
                "data": null,
                "render": function (data, type, row, meta) {
                    return meta.row + 1;
                },
                "width": "10%"
            },
            {
                "data": "DepartmentName",
                "width": "60%"
            },
            {
                "data": "DepartmentId",
                "render": function (data) {
                    return `
                                <div class="btn-group" role="group">
                                   <button class="btn btn-warning btn-sm depteditbtn mx-2" data-id="${data}">Edit</button>
                                    <button class="btn btn-info btn-sm deptsavebtn mx-2" data-id="${data}" style="display:none;">Save</button>
                                    <a onClick=DeleteDepartment(${data}) class="btn btn-danger btn-sm statesavebtn">Delete</a>
                                </div>`;
                   
                },
                "width": "30%"
            }
        ]
    });
}

function DeleteDepartment(id) {
    
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            
            $.ajax({
                url: '/Department/Delete/' + id,
                type: 'POST',
                success: function (res) {
                    if (res.success) {
                        $('#tblDepartment').DataTable().ajax.reload();
                        Swal.fire(
                            'Deleted!',
                            'Department has been deleted.',
                            'success'
                        );
                    } else {
                        Swal.fire('Error', 'Something went wrong!', 'error');
                    }
                },
                error: function () {
                    Swal.fire('Error', 'Unable to delete department.', 'error');
                }
            });
        }
    });
}


// Handle Edit button click
$('#tblDepartment tbody').on('click', '.depteditbtn', function () {
  
    var row = $(this).closest('tr');

    // Use the global DataTable instance
    var dataTable = window.dataTableInstance;

    if (!dataTable) {
        console.error("DataTable instance not found!");
        return;
    }

    var rowData = dataTable.row(row).data(); // Retrieve row data
    console.log(rowData);
    if (!rowData) {
        console.error("Row data not found!");
        return;
    }

    // Convert state name to an editable input field
    row.find('td:eq(1)').html(`<input type="text" class="edit-deptname form-control" value="${rowData.DepartmentName}">`);

    // Hide Edit button & Show Save button
    row.find('.depteditbtn').hide();
    row.find('.deptsavebtn').show();
});



// Handle Save Button Click
$('#tblDepartment tbody').on('click', '.deptsavebtn', function () {
    var row = $(this).closest('tr');
    var deptId = $(this).data('id');
    var newDepartmentName = row.find('.edit-deptname').val();

    $.ajax({
        url: '/Department/AddOrEdit',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ DepartmentId: deptId, DepartmentName: newDepartmentName }),
        success: function (response) {
            console.log(response);
            if (response.success) {
                
                toastr.success("Department updated successfully!");
                loadDepartmentList();
            } else {
                toastr.error("Failed to update state.");
            }
        },
        error: function (xhr, status, error) {
            console.error("Error updating state:", error);
            toastr.error("Error updating state. Please try again.");
        }
    });
});


//partial page
    function showModal(id) {
        $.get('/Department/AddOrEdit/' + id, function (data) {
            $('#modal-body').html(data);
            var myModal = new bootstrap.Modal(document.getElementById('depModal'), {});
            myModal.show();
        });
    }

function submitForm() {

    var form = $('#depForm');
    $.ajax({
        url: '/Department/AddOrEdit',
        type: 'POST',
        data: form.serialize(),
        success: function (res) {
            if (res.success) {
                $('#depModal').modal('hide');
                toastr.success("Department saved successfully!");
                loadDepartmentList();
            } else {
            }
        },
        error: function (xhr, status, error) {
            toastr.error("An error occurred while saving the department.");
        }
    });
    return false;
    }


    
