$(document).ready(function () {
    PopulateMonths();

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



$('#btnsearch').click(function () {
    var month = $('#Month').val();

    $.ajax({
        url: "/SalarySlip/PdfSalarySlip",
        type: "GET",
        data: { month: month },
        success: function (response) {
            if (response.exists) {
                var data = response.data;

                var tableHtml = ` <div class="container mt-5" id="salarySlipContent">
                     <h2 class="text-center mb-4" >Salary Slip</h2>
                     <div class="row mb-4">
                     <div class="col-md-6">
                     <h5 class="bg-success text-white p-2">Employee Details</h5>
                    <table class="table table-bordered">
                    <thead>
                   
                    </thead>
                     <tbody>
                      <tr><td>Year | Month</td><td>${data.Year} | ${data.Month}</td></tr>
                     <tr><td>Employee Id</td><td>${data.EmployeeId}</td></tr>
                     <tr><td>Employee Name</td><td>${data.EmployeeName}</td></tr>
                     <tr><td>Contact No</td><td>${data.ContactNo}</td></tr>
                     <tr><td>Email</td><td>${data.Email}</td></tr>
                     <tr><td>Department Name</td><td>${data.DepartmentName}</td></tr>
                    <tr><td>Designation Name</td><td>${data.DesignationName}</td></tr>
                    </tbody>
                    </table>
                    </div>


                        <div class="col-md-6">
                            <h5 class="bg-danger text-white p-2">Salary Details</h5>
                           <table class="table table-bordered">
                               <thead>
                                  <tr>
                               
                                   </tr>
                               </thead>
                              <tbody>
                                    <tr><td>Basic</td><td>${data.Basic}</td></tr>
                                   <tr><td>HRA</td><td>${data.HRA}</td></tr>
                                    <tr><td>Allowance</td><td>${data.Allowances}</td></tr>
                                    <tr><td>Deducations</td><td>${data.Deductions}</td></tr>
                                                 <tr style="background-color: Yellow;">
                                            <td><b>Total Salary (Amt.)</b></td>
                                            <td>&#8377; ${data.NetSalary}</td>
                                        </tr>
                                        </tbody>
                            </table>
                        </div>  
                        </div>
                    </div>
                     <div class="row">
                            <button type="button" id="btndownload" class="btn btn-primary mb-5"><i class="fa-solid fa-download"></i> Download</button>
                        </div>
                    `;

                $('#displayDiv').html(tableHtml);

                //download html table in .pdf file 
                $('#btndownload').click(function () {
                    var element = document.getElementById('salarySlipContent');
                    var opt = {
                        margin: [0.5, 0.5, 0.5, 0.5], // top, left, bottom, right
                        /*filename: 'SalarySlip.pdf',*/
                        filename: `SalarySlip_${data.Month}${data.Year}.pdf`,
                        image: { type: 'jpeg', quality: 1 },
                        html2canvas: {
                            scale: 2,
                            useCORS: true,
                            allowTaint: false,
                            scrollY: 0
                        },
                        jsPDF: { unit: 'in', format: 'A4', orientation: 'portrait' }
                    };
                    html2pdf().set(opt).from(element).save();
                });

            } else {
                $('#displayDiv').html('<div class="alert alert-warning">Salary Slip Not Available for selected Month.</div>');
            }
        },
        error: function () {
            $('#displayDiv').html('<div class="alert alert-danger">Error retrieving salary slip.</div>');
        }
    });
});