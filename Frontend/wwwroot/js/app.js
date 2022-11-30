const forms = document.querySelector(".wrapper"),
        pwShowHide = document.querySelectorAll(".eye-icon"),
        links = document.querySelectorAll(".link");

pwShowHide.forEach(eyeIcon => {
    eyeIcon.addEventListener("click", () => {
        let pwFields = eyeIcon.parentElement.parentElement.querySelectorAll(".password");
        pwFields.forEach(password => {
            if(password.type === "password"){
                password.type = "text";
            }
            else{
                password.type = "password";
            }
        })
    })
})
