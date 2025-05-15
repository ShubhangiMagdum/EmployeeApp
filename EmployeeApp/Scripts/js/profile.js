
$(document).ready(function () {

    loadDepartment();
    loadDesignation();

    $('#btnEdit').click(function () {
        $('#profileView').hide();
        $('#profileEditForm').show();
        $('#heading').text('Edit Profile');
        $('#btnEdit').hide();
    });

    $('#btnCancel').click(function () {
        $('#profileEditForm').hide();
        $('#profileView').show();
        $('#heading').text('Profile');
        $('#btnEdit').show();
    });


    $('#btnpassCancel').click(function () {
        $('#passwordformcontainer').hide();
        $('#profileView').show();
        $('#heading').text('Profile');

    });

    $('#btnChangePassword').click(function () {
        $('#profileView').hide();
        $('#passwordformcontainer').show();
        $('#heading').text('Change Passward');
        $('#btnEdit').show();
        $('#btnChangePassword').hide();
    });

});


$("#changePasswordForm").submit(function (e) {
    e.preventDefault();

    $("#currentPasswordError, #newPasswordError, #confirmPasswordError").text("");

    var isValid = true;
    var currentPassword = $("#CurrentPassword").val();
    var newPassword = $("#NewPassword").val();
    var confirmPassword = $("#ConfirmPassword").val();

    if (currentPassword === "") {
        $("#currentPasswordError").text("Current password is required.");
        isValid = false;
    }

    if (newPassword === "") {
        $("#newPasswordError").text("New password is required.");
        isValid = false;
    } else if (newPassword.length < 6) {
        $("#newPasswordError").text("New password must be at least 6 characters.");
        isValid = false;
    }

    if (confirmPassword !== newPassword) {
        $("#confirmPasswordError").text("Confirm password doesn't match.");
        isValid = false;
    }

    if (!isValid) return;

    $.ajax({
        url: '/Employee/ChangePassword',
        type: 'POST',
        data: {
            CurrentPassword: currentPassword,
            NewPassword: newPassword,
            ConfirmPassword: confirmPassword
        },
        success: function (response) {
            if (response.success) {
                $('#changePasswordForm')[0].reset();
                $('#passwordformcontainer').hide();
                $('#profileView').show();
            } else {
                alert(response.message);
            }
        },
        error: function (xhr) {
            alert("Error occurred: " + xhr.status + " - " + xhr.statusText);
        }
    });

});

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







 
