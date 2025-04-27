function TaxFeepagginationArrowControll(Page, PageSize, TotalRecord){
    if (Page >= 1 && Page <= TotalRecord) {
        $.ajax({
            type: "GET",
            url: "/TaxesAndFees/GetDataForTaxlistPartialView",
            data: {page: Page, pageSize: PageSize },
            success: function (data) {
                $("#TaxesListTable").empty();
                $("#TaxesListTable").html(data);
            },
            error: function (data) {
                console.log(data);
            }
        })
    }
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
document.getElementById("TaxesAndFees-Sidebar").classList.add("active-sidebar");
document.getElementById("TaxesAndFees-Sidebar").querySelector(".sidebar-name").classList.remove("text-dark");
document.getElementById("TaxesAndFees-Sidebar").querySelector(".sidebar-name").classList.add("text-primary");

function ChangeTaxEnabled(elem){
    let taxid = elem.id;
    let status = elem.checked;
    $.ajax({
        type : "POST",
        url : "/TaxesAndFees/TaxIsEnabled",
        data : {TaxId : taxid , IsEnable : status},
        success : function(data){
            if(data.status == true){
                if(status){
                    notyf.success("Tax Enabled Succesfully");
                }
                else{
                    notyf.success("Tax Disabled Succesfully");
                }
                GetTaxForPartialView();
            }
            else{
                notyf.error("Something Went Wrong");
                GetTaxForPartialView();
            }
        },
        error : function(data){
            console.log(data);
        }
    });
}

function ChangeDefaultTax(elem){
    let taxid = elem.id;
    let status = elem.checked;
    $.ajax({
        type : "POST",
        url : "/TaxesAndFees/DefaultTaxChange",
        data : {TaxId : taxid , Default : status},
        success : function(data){
            if(data.status == true){
                if(status){
                    notyf.success("Tax Default");
                }
                else{
                    notyf.success("Tax Is Not Default");
                }
                GetTaxForPartialView();
            }
            else{
                notyf.error("Something Went Wrong");
                GetTaxForPartialView();
            }
        },
        error : function(data){
            console.log(data);
        }
    });
}

function AddTax(){
    $("#AddTaxForm")[0].reset();
    $("#AddTaxModal").modal("show");
}

function EditTax(elem){
    let taxId = elem.id;
    $.ajax({
        type : "GET",
        url : "/TaxesAndFees/EditTax",
        data : {TaxId : taxId},
        success : function(data){
            if(data.status == false){
                notyf.error("Invalid tax id");
                $("#EditTaxModal").modal("hide");
            }
            else{
                $("#EditTaxModal .modal-content").html(data);
                $("#EditTaxModal").modal("show");
            }
        },
        error : function(data){
            console.log(data);
        }
    });
}

function DeleteTax(elem){
    $("#DeleteTaxForm")[0].reset();
    $("#deleteTaxId").val(elem.id);
    $("#DeleteTaxModal").modal("show");
};

function GetTaxForPartialView(){
    $.ajax({
        type: "GET",
        url: "/TaxesAndFees/GetDataForTaxlistPartialView",
        success: function (data) {
            $("#TaxesListTable").empty();
            $("#TaxesListTable").html(data);
        },
        error: function (data) {
            console.log(data);
        }
    });
};

$(document).off("submit" , "#AddTaxForm").on("submit" , "#AddTaxForm" , function(event){
    event.preventDefault();
    $("#AddTaxForm").removeAttr("novalidate");
    $.validator.unobtrusive.parse($("#AddTaxForm"));
    if (!$("#AddTaxForm").valid()) {
        return;
    }
    $.ajax({
        type : "POST",
        url : "/TaxesAndFees/AddTax",
        data : $("#AddTaxForm").serialize(),
        success : function(data){
            if(data.status == true){
                notyf.success("Tax Added Succesfully");
                $("#AddTaxModal").modal("hide");
                $("#AddTaxForm")[0].reset();
                GetTaxForPartialView();
            }
            else{
                notyf.error("Something Went Wrong");
                $("#AddTaxModal").modal("hide");
                $("#AddTaxForm")[0].reset();
                GetTaxForPartialView();
            }
        },
        error : function(data){
            console.log(data);
        }
    });
});

$(document).off("submit" , "#EditTaxForm").on("submit" , "#EditTaxForm" , function(event){
    event.preventDefault();
    $("#EditTaxForm").removeAttr("novalidate");
    $.validator.unobtrusive.parse($("#EditTaxForm"));
    if (!$("#EditTaxForm").valid()) {
        return;
    }
    /*if($("#EditTaxTypeDropDown").val() == null){
        $("#TaxTypeValidation").removeClass("d-none");
        notyf.error("Select Tax Type");
        return;
    }*/
    $.ajax({
        type : "POST",
        url : "/TaxesAndFees/EditTax",
        data : $("#EditTaxForm").serialize(),
        success : function(data){
            if(data.status == true){
                notyf.success("Tax Editted Succesfully");
                $("#EditTaxModal").modal("hide");
                $("#EditTaxForm")[0].reset();
                GetTaxForPartialView();
            }
            else{
                notyf.error("Something Went Wrong");
                $("#EditTaxModal").modal("hide");
                $("#EditTaxForm")[0].reset();
                GetTaxForPartialView();
            }
        },
        error : function(data){
            console.log(data);
        }
    });
});

$(document).off("submit" , "#DeleteTaxForm").on("submit" , "#DeleteTaxForm" , function(event){
    event.preventDefault();
    $.ajax({
        type : "POST",
        url : "/TaxesAndFees/DeleteTax",
        data : $("#DeleteTaxForm").serialize(),
        success : function(data){
            if(data.status == true){
                notyf.success("Tax Deleted Succesfully");
                $("#DeleteTaxForm")[0].reset();
                $("#DeleteTaxModal").modal("hide");
                GetTaxForPartialView();
            }
            else{
                notyf.error("Something Went Wrong");
                $("#DeleteTaxForm")[0].reset();
                $("#DeleteTaxModal").modal("hide");
                GetTaxForPartialView();
            }
        },
        error : function(data){
            console.log(data);
        }
    });
});

$(document).ready(function(){
    
    $("#TaxAndFeeSearchBox").on("input" , function(){
        let serachData = $("#TaxAndFeeSearchBox").val().trim();
        $.ajax({
            type : "GET",
            url : "/TaxesAndFees/GetDataForTaxlistPartialView",
            data : {SearchCriteria : serachData},
            success : function(data){
                $("#TaxesListTable").empty();
                $("#TaxesListTable").html(data);
                $("#TaxAndFeeSearchBox").val(serachData);
                $("#TaxAndFeeSearchBox").focus();
            },
            error : function(data){
                console.log(data);
            }
        });
    })

    $(document).on("change" , "#ItemPageSizeDropDown" ,function(){
        let PageSize = $("#ItemPageSizeDropDown").val().trim();
        $.ajax({
            type: "GET",
            url: "/TaxesAndFees/GetDataForTaxlistPartialView",
            data: {pageSize: PageSize },
            success: function (data) {
                $("#TaxesListTable").empty();
                $("#TaxesListTable").html(data);
            },
            error: function (data) {
                console.log(data);
            }
        })
    })

})