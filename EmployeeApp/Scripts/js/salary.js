$(document).ready(function () {
    PopulateMonths();
   
    loadSalaryTable();
    const currentYear = new Date().getFullYear();

    // Set the current year in the input field
    document.getElementById('Year').value = currentYear;

        //Validation 
        $("#addSalary").validate({
            rules: {
                Basic: {
                    required: true,
                    number: true,
                    min: 0
                },
                HRA: {
                    required: true,
                    number: true,
                    min: 0
                },
                Allowances: {
                    required: true,
                    number: true,
                    min: 0
                },
                Deductions: {
                    required: true,
                    number: true,
                    min: 0
                }

            },
            messages: {
                Basic: "Basic Salary is required and must be a positive number.",
                HRA: "HRA must be a valid positive number.",
                Allowances: "Allowances must be entered correctly.",
                Deductions: "Deductions cannot be negative."
               
            },
            submitHandler: function (form) {
               // alert("Form submitted successfully!");
                form.submit();
            }
        });
});

function PopulateMonths() {
    var monthDropdown = document.getElementById("Month");
    var months = [
        "January", "February", "March", "April",
        "May", "June", "July", "August",
        "September", "October", "November", "December"
    ];

    // Hidden field for existing Month
    var existingMonth = $("#ExistingMonth").val(); // Trim to avoid whitespace issues

    // Clear previous values to avoid duplication
    monthDropdown.innerHTML = "";

    // Add month options dynamically
    months.forEach((month, index) => {
        var option = document.createElement("option");
        option.value = month; 
        option.textContent = month;
        monthDropdown.appendChild(option);

        // Corrected logic for selecting existing month
        if (month === existingMonth) {
            option.selected = true; // Correct way to select an option
            $('#existingMon').text(`Existing Month: ${month}`).css('color', 'red');
        }
    });
}

function loadSalaryTable() {

    if ($.fn.DataTable.isDataTable("#tblSalary")) {
        $('#tblSalary').DataTable().destroy();
    }

    var j = 1;
    var empId = $("#EmployeeId").val();

    window.dataTableInstance = $('#tblSalary').DataTable({
        "ajax": {
            url: "/Salary/GetAll",
            type: "GET",
            data: { empId: empId },
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
                "data": "basic",
                "width": "35%"
            },
            {
                "data": "hra",
                "width": "35%"
            },
            {
                "data": "allowances",
                "width": "35%"
            },
            {
                "data": "deductions",
                "width": "35%"
            },
            {
                "data": "month",
                "width": "35%"
            },
            {
                "data": "year",
                "width": "35%"
            },
            {
                "data": "netSalary",
                "width": "35%"
            },
            {
                "data": "salaryId",
                "render": function (data) {
                    return `
                            <div class="btn-group" role="group">
                               <a class="btn btn-warning btn-sm mx-2 editsalary" data-id="${data}" >Edit</a>
                                  <a class="btn btn-warning btn-sm mx-2 savesalary" data-id="${data}" style="display:none;" >Save</a>
                              <a onClick=DeleteSalary(${data}) class="btn btn-danger btn-sm statesavebtn">Delete</a>
                                 </div>`;

                },

                "width": "40%"
            }
        ]
    });
}

function DeleteSalary(id) {
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
                url: '/Salary/Delete/' + id,
                type: 'POST',
                success: function (res) {
                    if (res.success) {
                        $('#tblSalary').DataTable().ajax.reload();
                        Swal.fire(
                            'Deleted!',
                            'Salary has been deleted.',
                            'success'
                        );
                    } else {
                        Swal.fire('Error', 'Something went wrong!', 'error');
                    }
                },
                error: function () {
                    Swal.fire('Error', 'Unable to delete Salary.', 'error');
                }
            });
        }
    });
}


$("#Basic, #HRA, #Allowances, #Deductions").on("change", function () {
    calculateNetSalary();
});

function calculateNetSalary() {

    var Basic = parseFloat(document.getElementById("Basic").value) || 0;
    var HRA = parseFloat(document.getElementById("HRA").value) || 0;
    var Allowances = parseFloat(document.getElementById("Allowances").value) || 0;
    var Deductions = parseFloat(document.getElementById("Deductions").value) || 0;


    if (Basic && HRA && Allowances && Deductions) {
        // Calculate tax amount
        var netSalary = Basic + HRA + Allowances - Deductions;

        // Display the calculated net salary in the textbox
        document.getElementById("NetSalary").value = netSalary;

    }
    else {
        document.getElementById("NetSalary").value = '';
    }

}

$('#addSalary').submit(function (e) {
    e.preventDefault();
    var formData = $(this).serialize();
    $.ajax({
        url: '/Salary/Create',
        type: 'POST',
        data: formData,
        success: function () {
            $('#addSalary')[0].reset(); // Clear form
            window.location.href = "/Employee/Index";
        },
        error: function () {
           // showAlert("An error occurred while saving.", "danger");
        }
    });
});

function showAlert(message, type) {
    $('#alert-placeholder').html(
        `<div class="alert alert-${type} alert-dismissible fade show" role="alert">
                    ${message}
                    <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>`
    );
}


// Handle Edit button click
$('#tblSalary tbody').on('click', '.editsalary', function () {

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
    row.find('td:eq(1)').html(`<input type="text" class="edit-basic form-control" value="${rowData.basic}">`);
    row.find('td:eq(2)').html(`<input type="text" class="edit-hra form-control" value="${rowData.hra}">`);
    row.find('td:eq(3)').html(`<input type="text" class="edit-allowances form-control" value="${rowData.allowances}">`);
    row.find('td:eq(4)').html(`<input type="text" class="edit-deductions form-control" value="${rowData.deductions}">`);
    row.find('td:eq(5)').html(`<input type="text" class="edit-month form-control" value="${rowData.month}" disabled>`);
    row.find('td:eq(6)').html(`<input type="text" class="edit-year form-control" value="${rowData.year}" disabled>`);

    // Hide Edit button & Show Save button
    row.find('.editsalary').hide();
    row.find('.savesalary').show();
});


// Handle Save Button Click
$('#tblSalary tbody').on('click', '.savesalary', function () {
    var row = $(this).closest('tr');
    var salaryId = $(this).data('id');
    var newBasic = row.find('.edit-basic').val();
    var newHra = row.find('.edit-hra').val();
    var newallowances = row.find('.edit-allowances').val();
    var newdeductions = row.find('.edit-deductions').val();
    var month = row.find('.edit-month').val();
    var year = row.find('.edit-year').val();

    var newempId = $("#EmployeeId").val();


    $.ajax({
        url: '/Salary/EditSalaryByEmployee',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ SalaryId: salaryId, Basic: newBasic, HRA: newHra, Allowances: newallowances, deductions: newdeductions, EmployeeId: newempId, Year: year, Month:month }),
        success: function () {
            window.location.href = '/Employee/Index';
            //console.log(response);
            //if (response.success) {

            //    window.location.href = '/Employee/Index';
            //  //  loadSalaryTable();
            //} else {
            //    toastr.error("Failed to update salary.");
            //}
        },
        error: function (xhr, status, error) {
            console.error("Error updating salary:", error);
            toastr.error("Error updating salary. Please try again.");
        }
    });
});








 





