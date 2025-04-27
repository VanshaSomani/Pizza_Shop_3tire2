// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function showPassword(inp , icon){
    const passicon = document.getElementById(icon);
    const input = document.getElementById(inp);
    // console.log(input.type);
    if(input.type === "password"){
        input.type = "text";
        passicon.classList.remove("fa-eye-slash");
        passicon.classList.add("fa-eye");
    }
    else{
        input.type = "password";
        passicon.classList.remove("fa-eye");
        passicon.classList.add("fa-eye-slash");
    }
}

function sidebarColorChange(elem){
    let sidebarAnchor = document.getElementsByClassName("sidebar-anchor");
    for(let i = 0 ; i < sidebarAnchor.length ; i++){
        sidebarAnchor[i].classList.remove("active-sidebar");
    }
    elem.classList.add("active-sidebar");
}