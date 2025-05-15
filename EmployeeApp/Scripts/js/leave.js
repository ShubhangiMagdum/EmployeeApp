$(document).ready(function () {
    var employeeId = $("#UserId").val(); 
        loadEmployeeLeaveList(employeeId);
 
    populateLeaveDropdown();
    
});

function populateLeaveDropdown() {
    var leaveDropdown = document.getElementById("LeaveType");

    if (!leaveDropdown) {
        console.warn("LeaveType dropdown not found!");
        return;
    }

    var leaveTypes = [
        { value: "Casual", text: "Casual Leave" },
        { value: "Sick", text: "Sick Leave" },
        { value: "Paid", text: "Paid Leave" },
        { value: "Unpaid", text: "Unpaid Leave" }
    ];

    // Clear any existing options (optional)
    leaveDropdown.innerHTML = '<option value="">-- Select Leave Type --</option>';

    // Add options dynamically
    leaveTypes.forEach(function (leave) {
        var option = document.createElement("option");
        option.value = leave.value;
        option.textContent = leave.text;
        leaveDropdown.appendChild(option);
    });
}


// Function to calculate total days between start and end date
function calculateDays() {
    // Get the values of the start and end date inputs
    var startDate = document.getElementById("LeaveStartDate").value;
    var endDate = document.getElementById("LeaveEndDate").value;

    // Ensure both dates are selected
    if (startDate && endDate) {
        var start = new Date(startDate);
        var end = new Date(endDate);

        // Calculate the difference in time
        var timeDiff = end.getTime() - start.getTime();
        var daysDiff = Math.ceil(timeDiff / (1000 * 3600 * 24)); // Convert milliseconds to days

        // Update the TotalDays input with the calculated difference
        document.getElementById("TotalDays").value = daysDiff;
    } else {
        // Handle the case where one or both of the dates are not selected
        document.getElementById("TotalDays").value = '';
    }
}


$('#addleave').on('click', function () {
    window.location.href = '/Leave/AddLeave';
    $("#LeaveStartDate, #LeaveEndDate").on('change', function () {
        calculateDays();
    });

});


//employee leave list in datatable

function loadEmployeeLeaveList() {

    var j = 1;
    $('#tblLeave').DataTable({
        "ajax": {
            url: "/Leave/GetAll",
            type: "GET",
            datatype: "json"
        },
        "columns": [
            {
                "data": null,
                "render": function (data, type, row, meta) {
                    return meta.row + 1;
                },
                "width": "5%"
            },
            { "data": "leaveType", "width": "10%" },
            { "data": "reason", "width": "20%" },
            {
                "data": "leaveStartDate",
                "render": function (data) {
                    if (data) {
                        var timestamp = data.match(/\/Date\((\d+)\)\//);
                        if (timestamp) {
                            var date = new Date(parseInt(timestamp[1]));
                            return date.toLocaleDateString('en-GB');
                        }
                    }
                    return "";
                },
                "width": "15%"
            },
            {
                "data": "leaveEndDate",
                "render": function (data) {
                    if (data) {
                        var timestamp = data.match(/\/Date\((\d+)\)\//);
                        if (timestamp) {
                            var date = new Date(parseInt(timestamp[1]));
                            return date.toLocaleDateString('en-GB');
                        }
                    }
                    return "";
                },
                "width": "15%"
            },
            { "data": "totalDays", "width": "10%" },
            { "data": "leaveStatus", "width": "10%" },
            {
                data: "leaveId",
                render: function (data, type, row) {
                    if (row.role === 'Admin' && row.leaveStatus === "Pending") {
                        return `<a onClick="ApprovedLeave(${data})" class="btn btn-warning btn-sm">Approve</a>`;
                    } else if (row.role === 'Admin' && row.leaveStatus === "Approved") {
                        return `<a onClick="DeleteLeave(${data})" class="btn btn-danger btn-sm">Cancel</a>`;
                    } else if (row.role === 'Admin' && row.leaveStatus === "Cancel") {
                        return `<p>Leave Cancelled</p>`;
                    } else {
                        return `
                            
                            <a onClick="DeleteLeave(${data})" class="btn btn-danger btn-sm">Cancel</a>`;
                    }
                },
                "width": "15%"
            }
        ]
    });
}

// Filter the DataTable based on leaveStatus when tab is clicked
$(document).on("click", "#leaveTabs a.nav-link", function () {
    let status = $(this).data("status");
    let table = $('#tblLeave').DataTable();

    if (status === "All") {
        // Clear filter on "Status" column
        table.column(6).search('').draw();  
    } else {
        table.column(6).search(status).draw();
    }
});
function DeleteLeave(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, cancel it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Leave/Delete/' + id,
                type: 'POST',
                success: function (res) {
                    if (res.success) {
                        $('#tblLeave').DataTable().ajax.reload();
                        Swal.fire(
                            'Canceled!',
                            'Employee leave has been canceled.',
                            'success'
                        );
                    } else {
                        Swal.fire('Error', 'Something went wrong!', 'error');
                    }
                },
                error: function () {
                    Swal.fire('Error', 'Unable to cancel employee Leave.', 'error');
                }
            });
        }
    });
}


function ApprovedLeave(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Approved it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Leave/Approved/' + id,
                type: 'POST',
                success: function (res) {
                    if (res.success) {
                        $('#tblLeave').DataTable().ajax.reload();
                        Swal.fire(
                            'Approved!',
                            'Employee leave has been Approved.',
                            'success'
                        );
                    } else {
                        Swal.fire('Error', 'Something went wrong!', 'error');
                    }
                },
                error: function () {
                    Swal.fire('Error', 'Unable to Approved employee Leave.', 'error');
                }
            });
        }
    });
}



