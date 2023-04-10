
function togglePassword() {

    let element = document.getElementById("Password");
    let img = document.getElementById("img");

    if (element.type == "password") {
        element.type = "text";
        img.src = "/icons/visible.png";
    } else {
        element.type = "password";
        img.src = "/icons/unvisible.png";

    }

    
}