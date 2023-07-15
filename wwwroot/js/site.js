// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const validateEmail = (email) => {
  return String(email)
    .toLowerCase()
    .match(
      /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|.(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
    );
};

const validatePhoneNumber = (phoneNumber) => {
  return String(phoneNumber)
    .toLowerCase()
    .match(/(84|0[3|5|7|8|9])+([0-9]{8})\b/g);
};

// register account
function eventValidFormRegisterAccount() {
  const registerForm = document.getElementById("register-form");

  const validForm = () => {
    const firstnameRegister = document.getElementById("firstname");
    const lastnameRegister = document.getElementById("lastname");
    const birthdayRegister = document.getElementById("birthday");
    const passwordRegister = document.getElementById("password");
    const rePasswordRegister = document.getElementById("re-password");
    const emailRegister = document.getElementById("email");
    const phoneNumberRegister = document.getElementById("phone-number");

    const emailError = document.getElementById("error-email");
    const phoneNumberError = document.getElementById("error-phonenumber");
    const firstNameError = document.getElementById("error-firstname");
    const lastNameError = document.getElementById("error-lastname");
    const birthdayError = document.getElementById("error-birthday");
    const passwordError = document.getElementById("error-password");
    const rePasswordError = document.getElementById("error-repassword");

    const passwordValue = passwordRegister.value;
    const rePasswordValue = rePasswordRegister.value;
    const emailValue = emailRegister.value;
    const phoneNumberValue = phoneNumberRegister.value;

    if (firstnameRegister.value.trim() == "") {
      firstNameError.innerText = "Tên không được để trống";
      firstNameError.hidden = false;
      return false;
    }
    firstNameError.hidden = true;

    if (lastnameRegister.value.trim() == "") {
      lastNameError.innerText = "Tên đệm không được để trống";
      lastNameError.hidden = false;
      return false;
    }
    firstNameError.hidden = true;

    if (birthdayRegister.value.trim() == "") {
      birthdayError.innerText = "Ngày sinh không được để trống";
      birthdayError.hidden = false;
      return false;
    }
    birthdayError.hidden = true;

    if (emailValue.trim() == "" && phoneNumberValue.trim() == "") {
      phoneNumberError.innerText = "Nhập số điện thoại hoặc email";
      emailError.innerText = "Nhập số điện thoại hoặc email";

      phoneNumberError.hidden = false;
      emailError.hidden = false;
      return false;
    }
    emailError.hidden = true;
    phoneNumberError.hidden = true;

    if (passwordValue.trim() == "") {
      passwordError.innerText = "Vui lòng nhập mật khẩu";

      passwordError.hidden = false;
      return false;
    }
    passwordError.hidden = true;

    if (passwordValue.trim() != rePasswordValue.trim()) {
      passwordError.innerText = "Vui lòng nhập mật khẩu";
      rePasswordError.innerText = "Vui lòng nhập đúng mật khẩu vừa nhập";

      passwordError.hidden = false;
      rePasswordError.hidden = false;
      return false;
    }
    passwordError.hidden = true;
    rePasswordRegister.hidden = true;

    if (passwordValue.length <= 5) {
      passwordError.innerText = "Vui lòng nhập mật khẩu lớn hơn 4 ký tự";

      passwordError.hidden = false;
      return false;
    }
    passwordError.hidden = true;

    if (!validateEmail(emailValue.trim())) {
      emailError.innerText = "Email phải nhập đúng định dạng";
      emailError.hidden = false;
      return false;
    }
    emailError.hidden = true;

    if (!validatePhoneNumber(phoneNumberValue.trim())) {
      phoneNumberError.innerText = "Số điện thoại phải đúng định dạng";
      phoneNumberError.hidden = false;
      return false;
    }
    phoneNumberError.hidden = true;

    return true;
  };

  registerForm.addEventListener("submit", (e) => {
    e.preventDefault();
    const isValidForm = validForm();
    console.log(isValidForm);
    if (!isValidForm) {
    }
  });
}

eventValidFormRegisterAccount();
