$(document).ready(function () {
    $.ajax({
        url: "/Employee/RecentSalarySlip",
        type: "GET",
        success: function (response) {
            if (response.length > 0) { // Ensure data exists
                var tableHtml = `<table class="table table-bordered mt-2">
                            <thead>
                                <tr>
                                    <th>Employee</th>
                                    <th>Date</th>
                                    <th>Amount</th>
                                </tr>
                            </thead>
                            <tbody>`;

                response.forEach(function (item) { // Loop over response data
                    var date = "N/A"; // Default value in case conversion fails
                    var timestamp = item.GeneratedDate.match(/\/Date\((\d+)\)\//);
                    if (timestamp) {
                        date = new Date(parseInt(timestamp[1])).toLocaleDateString('en-GB'); // Convert timestamp to readable date
                    }

                    tableHtml += `<tr>
                                <td>${item.EmployeeName}</td>
                                <td>${date}</td>
                                <td>&#8377; ${item.NetSalary}</td>
                              </tr>`;
                });

                tableHtml += `</tbody></table>`;

                $('#tbldashbaord').html(tableHtml);
            } else {
                $('#tbldashbaord').html('<div class="alert alert-warning">Salary Slip Not Available.</div>');
            }
        },
        error: function () {
            $('#tbldashbaord').html('<div class="alert alert-danger">Error retrieving salary slip.</div>');
        }
    });

    fetchLineChart();
    FetchPiechart();
    loadEmployeeCount();
    loadSalarySlipCount();
    loadpendingRequestCount();

});

//display total employee
function loadEmployeeCount() {
    $.ajax({
        url: '/Employee/GetEmployeeCount',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            $('#employeeCount').text("Total Employees: " + response.count);
            $('#employeeCount').text(response);

        },
        error: function () {
            alert("Failed to fetch employee count.");
        }
    });
}

//display salary slip 
function loadSalarySlipCount() {
    $.ajax({
        url: '/Salary/GetSalarySlipCount',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            $('#salarySlipCount').text("Salary Slip: " + response.count);
            $('#salarySlipCount').text(response);

        },
        error: function () {
            alert("Failed to fetch salary slip count.");
        }
    });
}

//display pending Request 
function loadpendingRequestCount() {
    $.ajax({
        url: '/Leave/GetPendingCount',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            $('#leaveCount').text("Pending Request: " + response.count);
            $('#leaveCount').text(response);

        },
        error: function () {
            alert("Failed to fetch request count.");
        }
    });
}


function fetchLineChart() {
    $.ajax({
        url: '/Salary/SalaryBarChart',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            const labels = data.map(d => d.EmployeeName);
            const salaries = data.map(d => d.NetSalary);

            const ctx = document.getElementById('salaryChart').getContext('2d');

            new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Net Salary',
                        data: salaries,
                        backgroundColor: 'rgba(54, 162, 235, 0.6)',
                        borderColor: 'rgba(54, 162, 235, 1)',
                        borderWidth: 1
                    }]
                },
                options: {
                    indexAxis: 'y',
                    responsive: true,
                    plugins: {
                        tooltip: {
                            callbacks: {
                                label: function (tooltipItem) {
                                    var index = tooltipItem.dataIndex;
                                    var employeeData = data[index]; // Get employee data by index
                                    return [
                                        `Net Salary: ${employeeData.NetSalary}`,
                                        `Designation: ${employeeData.Designation}`,
                                        `Department: ${employeeData.Department}`
                                    ];
                                }
                            }
                        }
                    },
                    scales: {
                        x: {
                            beginAtZero: true
                        }
                    }
                }
            });
        },
        error: function (xhr, status, error) {
            console.log("Error loading chart data: " + error);
        }
    });
}

function FetchPiechart() {
    $.ajax({
        url: '/Employee/EmployeePieChart',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            const labels = data.map(d => `${d.Department} - ${d.Designation}`);
            const employeeCounts = data.map(d => d.EmployeeCount);
            const backgroundColors = [
                'rgba(75, 192, 192, 0.6)',
                'rgba(153, 102, 255, 0.6)',
                'rgba(255, 159, 64, 0.6)',
                'rgba(255, 99, 132, 0.6)',
                'rgba(54, 162, 235, 0.6)',
                'rgba(255, 206, 86, 0.6)'
            ];

            const ctx = document.getElementById('employeeChart').getContext('2d');

            new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Employee Count',
                        data: employeeCounts,
                        backgroundColor: backgroundColors,
                        borderColor: backgroundColors.map(c => c.replace('0.6', '1')),
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true,
                    plugins: {
                        legend: {
                            display: false // Use custom legend
                        },
                        tooltip: {
                            callbacks: {
                                label: function (context) {
                                    return `${context.label}: ${context.raw} employees`;
                                }
                            }
                        }
                    }
                }
            });

            // Generate custom legend
            const legendContainer = document.getElementById('custom-legend');
            legendContainer.innerHTML = '';
            labels.forEach((label, index) => {
                const li = document.createElement('li');
                li.style.marginBottom = '8px';
                li.innerHTML = `
                    <span style="display:inline-block;width:12px;height:12px;background:${backgroundColors[index]};margin-right:8px;"></span>
                    ${label}
                `;
                legendContainer.appendChild(li);
            });
        },
        error: function (xhr, status, error) {
            console.error("Error loading chart data: " + error);
        }
    });
}