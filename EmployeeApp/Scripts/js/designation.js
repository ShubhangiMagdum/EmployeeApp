var dataTable;

$(document).ready(function () {

    loadDesignationList();
});

function loadDesignationList() {

    if ($.fn.DataTable.isDataTable("#tblDesignation")) {
        $('#tblDesignation').DataTable().destroy();
    }

    var j = 1;

    window.dataTableInstance = $('#tblDesignation').DataTable({
        "ajax": {
            url: "/Designation/GetAll",
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
                "data": "DesignationName",
                "width": "35%"
            },
            {
                "data": "DesignationDescription",
                "width": "35%"
            },
            {
                "data": "DesignationId",
                "render": function (data) {
                    return `
                                <div class="btn-group" role="group">
                                   <button class="btn btn-warning btn-sm depteditbtn mx-2" data-id="${data}">Edit</button>
                                    <button class="btn btn-info btn-sm deptsavebtn mx-2" data-id="${data}" style="display:none;">Save</button>
                                    <a onClick=DeleteDesignation(${data}) class="btn btn-danger btn-sm statesavebtn">Delete</a>
                                </div>`;

                },
                "width": "40%"
            }
        ]
    });
}

function DeleteDesignation(id) {
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
                url: '/Designation/Delete/' + id,
                type: 'POST',
                success: function (res) {
                    if (res.success) {
                        $('#tblDesignation').DataTable().ajax.reload();
                        Swal.fire(
                            'Deleted!',
                            'Designation has been deleted.',
                            'success'
                        );
                    } else {
                        Swal.fire('Error', 'Something went wrong!', 'error');
                    }
                },
                error: function () {
                    Swal.fire('Error', 'Unable to delete Designation.', 'error');
                }
            });
        }
    });
}


// Handle Edit button click
$('#tblDesignation tbody').on('click', '.depteditbtn', function () {

    var row = $(this).closest('tr');
    var dataTable = window.dataTableInstance;

    if (!dataTable) {
        console.error("DataTable instance not found!");
        return;
    }

    var rowData = dataTable.row(row).data(); // Retrieve row data
    if (!rowData) {
        console.error("Row data not found!");
        return;
    }

    // Convert state name to an editable input field
    row.find('td:eq(1)').html(`<input type="text" class="edit-designname form-control" value="${rowData.DesignationName}">`);
    row.find('td:eq(2)').html(`<input type="text" class="edit-designdesc form-control" value="${rowData.DesignationDescription}">`);

    // Hide Edit button & Show Save button
    row.find('.depteditbtn').hide();
    row.find('.deptsavebtn').show();
});



// Handle Save Button Click
$('#tblDesignation tbody').on('click', '.deptsavebtn', function () {
    var row = $(this).closest('tr');
    var designId = $(this).data('id');
    var newDesignationName = row.find('.edit-designname').val();
    var newDescription = row.find('.edit-designdesc').val();

    $.ajax({
        url: '/Designation/AddOrEdit',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ DesignationId: designId, DesignationName : newDesignationName, DesignationDescription : newDescription }),
        success: function (response) {
            if (response.success) {

                toastr.success("Designation updated successfully!");
                loadDesignationList();
            } else {
                toastr.error("Failed to update Designation.");
            }
        },
        error: function (xhr, status, error) {
            toastr.error("Error updating Designation. Please try again.");
        }
    });
});


//partial page
function showModal(id) {
    $.get('/Designation/AddOrEdit/' + id, function (data) {
        $('#modal-body').html(data);
        var myModal = new bootstrap.Modal(document.getElementById('desModal'), {});
        myModal.show();
    });
}

function submitForm() {
    $("#designForm").validate({
        rules: {
            DesignationName: {
                required: true
            },
            DesignationDescription: {
                required: true
            }
        },
        messages: {
            DesignationName: "Designation Name is required.",
            DesignationDescription: "Description is required."
        },
        submitHandler: function (form) {
            var form = $(form);

            $.ajax({
                url: '/Designation/AddOrEdit',
                type: 'POST',
                data: form.serialize(),
                success: function (res) {
                    if (res.success) {
                        $('#desModal').modal('hide');
                        toastr.success("Designation saved successfully!");
                        loadDesignationList(); 
                    } else {
                        toastr.error(res.message || "Failed to save Designation.");
                    }
                },
                error: function (xhr, status, error) {
                    toastr.error("An error occurred while saving the Designation.");
                }
            });

            return false; 
        }
    });

}



