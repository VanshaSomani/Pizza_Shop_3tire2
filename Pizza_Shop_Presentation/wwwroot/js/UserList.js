let sortby;
let Desc;

function getID(elem){
    let row = elem.parentElement.parentElement;
    let ModalUserId = document.getElementById("modalUserId");
    ModalUserId.value = row.id;
}

function GetDeteletIdAccord(elem){
    let ModalUserId = document.getElementById("modalUserId");
    ModalUserId.value = elem.id;
}

function sidebarColorChange(){
    let sidebarAnchor = document.getElementsByClassName("sidebar-anchor");
    for(let i = 0 ; i < sidebarAnchor.length ; i++){
        sidebarAnchor[i].classList.remove("active-sidebar");
        sidebarAnchor[i].querySelector(".sidebar-name").classList.add("text-dark");
        sidebarAnchor[i].querySelector(".sidebar-name").classList.remove("text-primary");
    }
}
sidebarColorChange();
document.getElementById("User-Sidebar").classList.add("active-sidebar");
document.getElementById("User-Sidebar").querySelector(".sidebar-name").classList.remove("text-dark");
document.getElementById("User-Sidebar").querySelector(".sidebar-name").classList.add("text-primary");

function pagginationArrowControll( currentPage ,  pageSize , totalpage){
    if(currentPage >= 1 && currentPage <= totalpage){
        $.ajax({
            url : "/User/UserListPartialData",
            method : "GET",
            data : {page : currentPage , pagesize : pageSize , searchcriteria : $("#SearchBox").val().trim() , sortBy : sortby , desc : Desc },
            success : function(data){
                $("#userListTable").empty();
                $("#userListTable").html(data);
            },
            error : function(response){
                console.log(response);
            }
        });
    }
}

function UserPageSizeDropDown() {
    let selectedpagesize = document.getElementById("PageSizeDropDown").value;
    $.ajax({
        url: "/User/UserListPartialData",
        method: "GET",
        data: {pagesize: selectedpagesize , sortBy : sortby , desc : Desc , searchcriteria : $("#SearchBox").val().trim()},
        success: function (data) {
            $("#userListTable").empty();
            $("#userListTable").html(data);
        },
        error: function (response) {
            console.log(response);
        }
    });
}

$(document).off("click",".sortIcon").on("click",".sortIcon",function(){
    sortby = $(this).siblings("span").text();
    Desc = $(this).hasClass("desc");
    $.ajax({
        url: "/User/UserListPartialData",
        method: "GET",
        data: {sortBy : sortby , desc : Desc , searchcriteria : $("#SearchBox").val().trim() , pagesize :$("#PageSizeDropDown").val().trim()},
        success: function (data) {
            $("#userListTable").empty();
            $("#userListTable").html(data);
        },
        error: function (response) {
            console.log(response);
        }
    });
})

$(document).off("click",".AccordsortIcon").on("click",".AccordsortIcon", function(){
    sortby = $("#AccordSortDropDown").val();
    Desc = $(this).hasClass("desc");
    if(sortby != ""){
        $.ajax({
            url: "/User/UserListPartialData",
            method: "GET",
            data: {sortBy : sortby , desc : Desc , searchcriteria : $("#SearchBox").val().trim() , pagesize :$("#PageSizeDropDown").val().trim()},
            success: function (data) {
                $("#userListTable").empty();
                $("#userListTable").html(data);
                $("#AccordSortDropDown").val(sortby);
            },
            error: function (response) {
                console.log(response);
            }
        });
    }
})

$(document).ready(function(){
    //Search
    $("#SearchBox").on("input",function(){
        let selectedpagesize = $("#PageSizeDropDown").val().trim();
        let searchData = $("#SearchBox").val().trim();
        $.ajax({
            type : "POST",
            url: "/User/UserListPartialData",
            data : {sortBy : sortby , desc : Desc , pagesize : selectedpagesize , searchcriteria : searchData},
            success : function(response){
                $("#userListTable").empty();
                $("#userListTable").html(response);
                $("#SearchBox").val(searchData);
                $("#SearchBox").focus();
            }
        })
    })

})


