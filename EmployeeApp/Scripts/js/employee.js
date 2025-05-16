$(document).ready(function () {

    loadDepartment();
    loadDesignation();
    loadEmployeeList();
});

function dropdown() {
    // First, hide all dropdown contents initially
    $(".custom-dropdown-content").hide(); // Ensure hidden

    // Handle click on Actions button
    $(document).off('click', '.custom-dropdown .dr').on('click', '.custom-dropdown .dr', function (e) {
        e.stopPropagation(); // Prevent bubbling
        var $dropdownContent = $(this).siblings(".custom-dropdown-content");

        // Close all other open dropdowns first
        $(".custom-dropdown-content").not($dropdownContent).slideUp();

        // Toggle clicked dropdown
        $dropdownContent.stop(true, true).slideToggle(200);
    });

    // Hide dropdown when clicking outside
    $(document).off('click.dropdownClose').on('click.dropdownClose', function () {
        $(".custom-dropdown-content").slideUp();
    });

    // Prevent closing when clicking inside the dropdown
    $(document).off('click', '.custom-dropdown-content').on('click', '.custom-dropdown-content', function (e) {
        e.stopPropagation();
    });
}

$('#addemp').on('click', function () {
    window.location.href = '/Employee/AddEmployee';

   
});

function submitForm() {
    var form = $('#empForm');
    $.ajax({
        url: form.attr('action'),
        type: 'POST',
        data: form.serialize(),
        success: function (res) {
            console.log("Response:", res);

            if (res.success) {
                $('#empModal').modal('hide');
                toastr.success("Department saved successfully!");

                // loadDepartmentList(); // if you have any AJAX reload function
            } else {
                toastr.error(res.message || "Failed to save department.");
            }

        },
        error: function (xhr, status, error) {
            toastr.error("An error occurred while saving the department.");
        }
    });
    return false;
}



function loadDepartment() {

    $.ajax({
        url: "/Department/GetDepartment",
        type: "GET",
        dataType: "json",
        success: function (data) {
            populateDepartmentDropdown(data);
        },
        error: function () {
            alert("Error loading Department");
        }
    });
}

// Function to populate Department dropdown
function populateDepartmentDropdown(data) {

    var dropdown = $("#DrpDepartment");
    dropdown.empty();
    dropdown.append('<option value="" selected>Select Department</option>');


    // Hidden field for existing Department ID
    var existingDepartmentId = $("#ExistingDepartmentId").val();

    $.each(data, function (index, item) {
        var option = $('<option></option>')
            .val(item.DepartmentId)
            .text(item.DepartmentName);

        if (item.DepartmentId == existingDepartmentId) {
            option.prop('selected', true); // Correct way to select
            $('#existingDepartment').text(`Existing Department: ${item.DepartmentName}`).css('color', 'red');
        }
        dropdown.append(option);
    });
}


// Function to load Designation
function loadDesignation() {
    $.ajax({
        url: "/Designation/GetDesignation",
        type: "GET",
        dataType: "json",
        success: function (data) {
            populateDesignationDropdown(data);
        },
        error: function () {
            alert("Error loading Designation");
        }
    });
}

// Function to populate Designation dropdown
function populateDesignationDropdown(data) {
    var dropdown = $("#DrpDesignation");
    dropdown.empty();
    dropdown.append('<option value="" selected>Select Designation</option>');

    // Hidden field for existing Department ID
    var existingDesignationId = $("#ExistingDesignationId").val();

    $.each(data, function (index, item) {
        var option = $('<option></option>')
            .val(item.DesignationId)
            .text(item.DesignationName);

        if (item.DesignationId == existingDesignationId) {
            option.prop('selected', true); // Correct way to select
            $('#existingDesignation').text(`Existing Designation: ${item.DesignationName}`).css('color', 'red');
        }


        dropdown.append(option);
    });
}

//employee list in datatable 
function loadEmployeeList() {

    if ($.fn.DataTable.isDataTable("#tblEmployee")) {
        $('#tblEmployee').DataTable().destroy();
    }

    var j = 1;

    window.dataTableInstance = $('#tblEmployee').DataTable({
        "ajax": {
            url: "/Employee/GetAll",
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
                "data": "ProfileImg",
                "render": function (data, type, row) {
                    console.log("Image URL:", data);
                    return `<img src="/Images/profile/${data}" width="50" height="50"
                              onerror="this.onerror=null; this.src='/Images/Login.png';" />`;
                }
            },
            {
                "data": "employeeName",
                "width": "35%"
            },
            {
                "data": "email",
                "width": "35%"
            },
            {
                "data": "contactNo",
                "width": "35%"
            },
            {
                "data": "passward",
                "width": "35%"
            },
            {
                "data": "designationName",
                "width": "35%"
            },
            {
                "data": "departmentName",
                "width": "35%"
            },
            {
                "data": "employeeId",
                "render": function (data, type, row, meta) {
                    return `
                        <div class="custom-dropdown">
                            <button class="btn dr" type="button">Actions </button>
                            <ul class="custom-dropdown-content">
                            <li><a class="dropdown-item" href="/Employee/Edit/${data}"><i class="fa-solid fa-pen-to-square"></i>Edit</a></li>
                                <li><a onClick="DeleteEmployee(${data})"><i class="fa-solid fa-trash px-1"></i>Delete</a></li>
                                <li><a class="dropdown-item btndec" data-id="${data}" href="/Salary/AddSalary/${data}"><i class="fa-solid fa-file px-1"></i>Salary</a></li>
                            </ul>
                        </div>
                    `;
                },
               
                "width": "40%"
            }
        ]
    });

    dropdown();

}


function DeleteEmployee(id) {
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
                url: '/Employee/Delete/' + id,
                type: 'POST',
                success: function (res) {
                    if (res.success) {
                        $('#tblEmployee').DataTable().ajax.reload();
                        Swal.fire(
                            'Deleted!',
                            'Employee has been deleted.',
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

function editEmployee(id) {

    var form = $('#empForm')[0];
    if (!form) {
        alert("Form not found.");
        return;
    }

    var formData = new FormData(form);

    var id = $("input[name='EmployeeId']").val();
    if (!id) {
        alert("Employee ID is missing.");
        return;
    }

    var fileInput = $("#editprofileImg")[0];

    if (!fileInput || fileInput.files.length === 0) {
        alert("Please select at least one image.");
        return;
    }

    var images = fileInput.files;
    for (var i = 0; i < images.length; i++) {
        formData.append("Images", images[i]);
    }

    $.ajax({
        url: "/Employee/EditEmployee?id=" + encodeURIComponent(id), // Encoding to prevent issues
        type: "POST",
        data: formData,
        processData: false,
        contentType: false,
        success: function () {
            window.location.href = "/Employee/Index";

        },
        error: function () {
            toastr.error("Error updating employee.");
        }
    });
}



function AddEmployee() {
    $("#empforms").validate({
        rules: {
            EmployeeName: {
                required: true
            },
            Email: {
                required: true,
                email: true
            },
            ContactNo: {
                required: true,
                number: true,
                min: 0
            },
            file: {
                required: true

            },
            PasswordHash: {
                required: true
            },
            DepartmentName: {
                required: true
            },
            DesignationName: {
                required: true
            }
        },
        messages: {
            EmployeeName: "Employee Name is required.",
            Email: "Please enter a valid email address.",
            ContactNo: "Contact number must be a valid number.",
            file: "Please upload a valid image file (jpg, jpeg, png, gif).",
            PasswordHash: "PasswordHash is required.",
            DepartmentName: "Department Name  is required.",
            DesignationName: "Designation Name  is required."
        },
        submitHandler: function (form) {
            var formData = new FormData(form);
            //for (var pair of formData.entries()) {
            //    console.log(pair[0] + ':', pair[1]);
            //}
           
            $.ajax({
                url: '/Employee/Create',
                type: 'POST',
                data: formData,
                contentType: false,
                processData: false,
                success: function () {
                    window.location.href = '/Employee/Index';
                },
                error: function () {
                    alert('An unexpected error occurred.');
                }
            });
            return false;


        }
    });
}


