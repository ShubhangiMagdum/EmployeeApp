
function Login(event) {
    event.preventDefault();
    $(".error").text("");
    // Get form values
    const email = $("#username").val().trim();
    const password = $("#password").val().trim();

    // Validation flag
    let isValid = true;

    // Validate Email
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (email === "") {
        $("#emailError").text("Email is required.");
        isValid = false;
    } else if (!emailRegex.test(email)) {
        $("#emailError").text("Please enter a valid email address.");
        isValid = false;
    }

    // Validate Password
    if (password === "") {
        $("#passwordError").text("Password is required.");
        isValid = false;
    } else if (password.length < 6) {
        $("#passwordError").text("Password must be at least 6 characters long.");
        isValid = false;
    }

    // Submit the form if valid
    if (isValid) {
        $.ajax({

            url: "/Account/LoginSubmit",
            type: "POST",
            data: { email: email, password: password },
            
            success: function (response) {
                console.log(response);
                if (response.success) {
                    
                    toastr.success("Login successfully!");

                    window.location.href = response.redirectUrl;
                } else {
                    toastr.warning("No changes detected.");
                }
            },
            error: function () {
                toastr.error("Error updating employee.");
            }
        });
        return true; // allow form submission
    }

    return false; // prevent form submission
}



document.getElementById("togglePassword").addEventListener("click", function () {
    const passwordInput = document.getElementById("password");
    const toggleIcon = document.getElementById("toggleIcon");

    // Toggle password visibility
    if (passwordInput.type === "password") {
        passwordInput.type = "text"; // Show password
        toggleIcon.classList.remove("bi-eye-slash");
        toggleIcon.classList.add("bi-eye");
    } else {
        passwordInput.type = "password"; // Hide password
        toggleIcon.classList.remove("bi-eye");
        toggleIcon.classList.add("bi-eye-slash");
    }
});