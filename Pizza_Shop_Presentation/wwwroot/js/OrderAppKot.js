let KOTcatrgoryId = -1;
let KOTstatus = "In Progress";

$(".navbar-badges-container span").removeClass('selected_badge');
$("#OrderAppKOTBadge").find('span').addClass('selected_badge');

function DateTimerKOT() {
    $(".KotDateCounter").each(function () {
        let orderDatestr = $(this).data("datecounter");
        let orderDate = new Date(orderDatestr).getTime();
        let current = new Date().getTime();
        let diff = current - orderDate;

        let days = Math.floor(diff / (1000 * 60 * 60 * 24));
        let hours = Math.floor((diff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        let minutes = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));
        let seconds = Math.floor((diff % (1000 * 60)) / 1000);

        $(this).find(".KotDateCounterText").text(`${days} days ${hours} hours ${minutes} min ${seconds} sec`);
    })
}
DateTimerKOT();
setInterval(DateTimerKOT, 1000);

function GetKotForCategory(elem) {
    KOTcatrgoryId = elem.id;
    let categoryName = elem.textContent.trim();
        $.ajax({
            type: "GET",
            url: "/OrderAppKOT/GetCategorywiseDataForKotCards",
            data: { CategoryID: KOTcatrgoryId , Status: "In Progress"},
            success: function (data) {
                KOTstatus = "In Progress";
                $("#KOTCardsContainer").empty();
                $("#KOTCardsContainer").html(data);
                let fiter_btn = document.getElementById("In Progress");
                SetFilterBtn(fiter_btn);
                document.getElementById("SelectedCategoryName").innerHTML = categoryName;
                document.querySelectorAll(".tab-btn").forEach(tab => {
                    tab.classList.remove("active-menu-option");
                    tab.classList.remove("text-primary");
                    tab.classList.add("text-secondary");
                });
                elem.classList.add("active-menu-option");
                elem.classList.add("text-primary");
                elem.classList.remove("text-secondary");
            },
            error : function(data){
                console.log(data);
            }
        });
}

function GetDataForKOTModal(elem){
    let orderId = elem.id;
    $.ajax({
        type : "GET",
        url : "/OrderAppKOT/GetKotModalInfo",
        data : {OrderId : orderId , Status : KOTstatus , CategoryID : KOTcatrgoryId},
        success : function(data){
            $("#KOTModalContainer").empty();
            $("#KOTModalContainer").html(data);
            $("#KOTDetailsModal").modal("show");
        },
        error : function(data){
            console.log(data);
        }
    });
}

$(document).off("click" , "#KOTDetailsModalSubmit").on("click" , "#KOTDetailsModalSubmit" , function(){
    let updatedItem = [];
    $("input[type=checkbox]:checked").each(function(){
        let ItemId = $(this).data("itemid");
        let Quantity = document.getElementById(`item-quantity-${ItemId}`);
        let ItemQuantity = parseInt(Quantity.innerText);
        updatedItem.push({
            itemId : ItemId,
            itemQuantity : ItemQuantity 
        });
    });

    let orderId = $("#orderId").data("orderid");
    let status = $(this).data("status");

    $.ajax({
        type : "POST",
        url : "/OrderAppKOT/UpdateKOTItemInfo",
        data : {OrderId : orderId , Status : status , UpdatedItem : updatedItem },
        success : function(data){
            if(data.status == true){
                notyf.success("Item quantity updated succesfully");
            }
            else{
                notyf.error("Something went wrong");
            }
            $("#KOTDetailsModal").modal("hide");
            $("#KOTModalContainer").empty();
            GetDataAfterKotModalSubmit();
        },
        error : function(data){
            console.log(data);
        }
    });
})

$(document).off("click" , ".rigth-scroll").on("click" , ".right-scroll" , function(){
    $("#KOTCardsContainer").animate({
        scrollLeft: $("#KOTCardsContainer").scrollLeft() + 500
    } , 500);
});

$(document).off("click" , ".left-scroll").on("click" , ".left-scroll" , function(){
    $("#KOTCardsContainer").animate({
        scrollLeft: $("#KOTCardsContainer").scrollLeft() - 500
    } , 500);
});

function MinusQuantity(itemid){
    let Quantity = document.getElementById(`item-quantity-${itemid}`);
    let ItemQuantity = parseInt(Quantity.innerText);
    if(ItemQuantity == 0){
        notyf.error("Cannot further decrease the quantity.");
    }
    else{
        ItemQuantity = ItemQuantity - 1;
        Quantity.innerText = ItemQuantity;
    }
}

function PlusQuantity(itemid , maxQuantity){
    let Quantity = document.getElementById(`item-quantity-${itemid}`);
    let ItemQuantity = parseInt(Quantity.innerText);
    if(ItemQuantity >= maxQuantity){
        notyf.error("Cannot further increase the quantity.");
    }
    else{
        ItemQuantity = ItemQuantity + 1;
        Quantity.innerText = ItemQuantity;
    }   
}

function GetDataAfterKotModalSubmit(){
    $.ajax({
        type: "GET",
        url: "/OrderAppKOT/GetCategorywiseDataForKotCards",
        data: { CategoryID: KOTcatrgoryId, Status: KOTstatus },
        success: function (data) {
            $("#KOTCardsContainer").empty();
            $("#KOTCardsContainer").html(data);
            let elem = document.getElementById(KOTstatus);
            SetFilterBtn(elem);
        },
        error : function(data){
            console.log(data);
        }
    });
}

function SetFilterBtn(elem){
    document.querySelectorAll(".Kot_filter_btn").forEach(b => {
        b.classList.remove("btn-primary");
        b.classList.remove("modal_submit_btn");
        b.classList.add("btn-outline-primary");
        b.classList.add("modal_close_btn");
    });
    elem.classList.remove("btn-outline-primary");
    elem.classList.remove("modal_close_btn");
    elem.classList.add("btn-primary");
    elem.classList.add("modal_submit_btn");
}

function KotStatusFilter(elem) {
    let status = elem.id;
    KOTstatus = status;
    $.ajax({
        type: "GET",
        url: "/OrderAppKOT/GetCategorywiseDataForKotCards",
        data: { CategoryID: KOTcatrgoryId, Status: status },
        success: function (data) {
            $("#KOTCardsContainer").empty();
            $("#KOTCardsContainer").html(data);
            SetFilterBtn(elem);
        },
        error : function(data){
            console.log(data);
        }
    });
}
