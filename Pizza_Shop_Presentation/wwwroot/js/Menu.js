var checkbox;
var itemMassDelete = [];
var modifierMassDelete = [];
var deleteMgModifier = [];
var selectModifierFromAddEx = [];
var nameOfModifer = [];
var selectModifierGroupFromDropDown = [];
var deleteExistingModifierGroupEditItem = [];
var previousSelectedModifier = [];
var previousSelectedModifierName = [];


function sidebarColorChange() {
    var sidebarAnchor = document.getElementsByClassName("sidebar-anchor");
    for (var i = 0; i < sidebarAnchor.length; i++) {
        sidebarAnchor[i].classList.remove("active-sidebar");
        sidebarAnchor[i].querySelector(".sidebar-name").classList.add("text-dark");
        sidebarAnchor[i].querySelector(".sidebar-name").classList.remove("text-primary");
    }
}
sidebarColorChange();
document.getElementById("MenuItems-Sidebar").classList.add("active-sidebar");
document.getElementById("MenuItems-Sidebar").querySelector(".sidebar-name").classList.remove("text-dark");
document.getElementById("MenuItems-Sidebar").querySelector(".sidebar-name").classList.add("text-primary");

function removeselectedMgId() {
    selectModifierGroupFromDropDown = [];
    document.getElementById("SelectModifireGroupSection").innerHTML = "";
    $("#AddItemModalForm")[0].reset();
    if ($("#CategoryDropDown").find(`option[value = "${cid}"]`).length > 0) {
        $("#CategoryDropDown").val(cid).change();
    }
    $("#AddNewMenuItemModal").modal("show");
}

function DeleteMGFromAddItems(Mgid) {
    var cardId = `#modifierGroup-${Mgid}`;
    selectModifierGroupFromDropDown.splice(selectModifierGroupFromDropDown.indexOf(Mgid), 1);
    $(cardId).remove();
}

function DeleteMgFromEditItem(ModifierGroupid) {
    var cardId = `#EditmodifierGroup-${ModifierGroupid}`;
    selectModifierGroupFromDropDown.splice(selectModifierGroupFromDropDown.indexOf(ModifierGroupid), 1);
    $(cardId).remove();
}

function DeleteExistingMGFromEditItems(ModifierGroupid) {
    var cardId = `#EditmodifierGroup-${ModifierGroupid}`;
    deleteExistingModifierGroupEditItem.push(ModifierGroupid);
    if (selectModifierGroupFromDropDown.includes(ModifierGroupid)) {
        selectModifierGroupFromDropDown.splice(selectModifierGroupFromDropDown.indexOf(ModifierGroupid), 1);
    }
    $(cardId).remove();
}

function ItempagginationArrowControll(currentPage, pageSize, totalpage) {
    var searchData = $("#SearchBox").val().trim();
    if (currentPage >= 1 && currentPage <= totalpage) {
        $.ajax({
            url: "/Menu/GetItemByCategoryId",
            method: "GET",
            data: { categoryId: cid, page: currentPage, pagesize: pageSize , searchcriteria: searchData},
            success: function (data) {
                $("#ItemSection").empty();
                $("#ItemSection").html(data);
                checkbox = document.getElementsByClassName('ItemCheckBox');
                for (var i = 0; i < checkbox.length; i++) {
                    if (itemMassDelete.includes(checkbox[i].getAttribute("id"))) {
                        //console.log("asfadvsdv");
                        checkbox[i].checked = true;
                    }
                }
                checkItemCheckBoxSelectAll();
            },
            error: function (response) {
                console.log(response);
            }
        });
    }
}

function ModifierpagginationArrowControll(currentPage, pageSize, totalpage) {
    var searchData = $("#ModifierSearchBox").val().trim();
    if (currentPage >= 1 && currentPage <= totalpage) {
        $.ajax({
            url: "/Menu/GetModifiersByMGId",
            method: "GET",
            data: { Mgid: mgid, page: currentPage, pagesize: pageSize , searchcriteria: searchData},
            success: function (data) {
                $("#ModifierSection").empty();
                $("#ModifierSection").html(data);
                checkbox = document.getElementsByClassName('ModifierCheckbox');
                for (var i = 0; i < checkbox.length; i++) {
                    if (modifierMassDelete.includes(checkbox[i].getAttribute("id"))) {
                        checkbox[i].checked = true;
                    }
                }
                checkModifierCheckBoxSelectAll();
            },
            error: function (data) {
                console.log(data);
            }
        });
    }
}

function EXModifierpagginationArrowControll(currentPage, pageSize, totalpage) {
    if (currentPage >= 1 && currentPage <= totalpage) {
        $.ajax({
            url: "/Menu/GetDataForExistingModifiers",
            method: "GET",
            data: { page: currentPage, pagesize: pageSize },
            success: function (data) {
                $("#AddExistingModiferGroupModal .modal-content").html(data);
                $("#SelectExModifierId").val(selectModifierFromAddEx);
                var selectedCheckBox = $("#SelectExModifierId").val();
                checkbox = document.getElementsByClassName('EXModiferCheckBox');
                for (var i = 0; i < checkbox.length; i++) {
                    if (selectModifierFromAddEx.includes(checkbox[i].getAttribute("id"))) {
                        checkbox[i].checked = true;
                    }
                }
            },
            error: function (data) {
                console.log(data);
            }
        });
    }
}

function getEditID(elem) {
    $.ajax({
        url: "/Menu/EditCategory",
        type: "GET",
        data: { categoryId: elem.id },
        success: function (data) {
            $("#EditCategoryIdModal").val(data.categoryid);
            $("#EditCategoryNameModal").val(data.categoryname);
            $("#EditCategoryDescModal").val(data.categoryDesc);
            $("#EditCategoryModal").modal("show");
        }
    })
}

function getMGEditID(elem) {
    selectModifierFromAddEx = [];
    deleteMgModifier = [];
    nameOfModifer = [];
    $.ajax({
        url: "/Menu/EditModifierGroup",
        type: "GET",
        data: { MGid: elem.id },
        success: function (data) {
            // console.log(data);
            $("#EditModiferGroupModal .modal-content").html(data);
            if (document.getElementById("ModifierGroupFromDB").hasChildNodes() == true) {
                var alreadyExistModifierInEditMg = document.getElementById("ModifierGroupFromDB").childNodes;
                for (var i = 0; i < alreadyExistModifierInEditMg.length; i++) {
                    var id = alreadyExistModifierInEditMg[i].id;
                    var modifierName = alreadyExistModifierInEditMg[i].innerText;
                    if (!selectModifierFromAddEx.includes(id)) {
                        selectModifierFromAddEx.push(id);
                        if (!nameOfModifer.includes(modifierName)) {
                            nameOfModifer.push(modifierName);
                        }
                    }
                }
            }
            $("#EditModiferGroupModal").modal("show");
        }
    });
}

function ShowAddModiferGroupModal() {
    selectModifierFromAddEx = [];
    deleteMgModifier = [];
    nameOfModifer = [];
    previousSelectedModifier = [];
    previousSelectedModifierName = [];
    var data2 = document.getElementById("existingModiferLableforAddMG");
    data2.innerHTML = "";
    $("#AddModiferGroupModal").modal("show");
}

function OpenAddModifierModal() {
    $("#AddModifier")[0].reset();
    if ($("#ModifierGroupDropDown").find(`option[value = "${mgid}"]`).length > 0) {
        $("#ModifierGroupDropDown").val(mgid).change();
    }
    $("#AddModifierModal").modal("show");
}

function GetAddExistingModifers(check) {
    $.ajax({
        url: "/Menu/GetDataForExistingModifiers",
        type: "GET",
        success: function (data) {
            $("#AddExistingModiferGroupModal .modal-content").html(data);
            $("#SelectExModifierId").val(selectModifierFromAddEx);

            var selectedCheckBox = $("#SelectExModifierId").val();

            checkbox = document.getElementsByClassName('EXModiferCheckBox');

            console.log("previousSelectedModifier - ",previousSelectedModifier);
            previousSelectedModifierName = nameOfModifer.slice();

            for (var i = 0; i < checkbox.length; i++) {
                if (selectModifierFromAddEx.includes(checkbox[i].getAttribute("id"))) {
                    checkbox[i].checked = true;
                }
            }

            $("#AddExistingModiferGroupModal").modal("show");
        },
        error: function (data) {
            console.log(data);
        }
    });
}

function getModifierEditID(elem) {
    $.ajax({
        url: "/Menu/EditModifer",
        type: "GET",
        data: { mid: elem.id },
        success: function (data) {
            $("#EditModifierModal .modal-content").html(data);
            $("#EditModifierModal").modal("show");
        },
        error: function (data) {
            console.log(data);
        }
    });
}


//change
function MassDelete() {
    $.ajax({
        url: "/Menu/MassDelete",
        type: "POST",
        data: { itemid: itemMassDelete, categoryId: cid },
        success: function (response) {
            notyf.success(`${itemMassDelete.length} Items Deleted Successfully`);
            itemMassDelete = [];
            $("#ItemSection").empty();
            $("#ItemSection").html(response);
        },
        error: function (response) { }
    });
}

function MassDeleteModifiers() {
    $.ajax({
        url: "/Menu/MassDeleteModifier",
        type: "POST",
        data: { modifierIds: modifierMassDelete, modifierGroupId: mgid },
        success: function (data) {
            if (data.status == true) {
                notyf.success(`${modifierMassDelete.length} Modifiers Deleted Succesfully`);
            }
            else {
                notyf.error("Something Went Wrong");
            }
            GetModifierForPartialView(mgid);
        },
        error: function (data) {
            console.log(data);
        }
    });
}

function GetAllSelectedItem(elem) {
    var ItemId = elem.id;
    if (!itemMassDelete.includes(ItemId)) {
        itemMassDelete.push(ItemId);
    }
    else {
        itemMassDelete.splice(itemMassDelete.indexOf(ItemId), 1);
    }
    checkItemCheckBoxSelectAll();
    // console.log("itemMassDelete - ", itemMassDelete);
}

function GetAllSelectedModifier(elem) {
    var modifierId = elem.id;
    if (!modifierMassDelete.includes(modifierId)) {
        modifierMassDelete.push(modifierId);
    }
    else {
        modifierMassDelete.splice(modifierMassDelete.indexOf(modifierId), 1);
    }
    checkModifierCheckBoxSelectAll();
    // console.log("GetAllSelectedModifier modifierMassDelete - ", modifierMassDelete);
}

function SelectAllItemForMassDelete(elem) {
    if (elem.checked) {
        checkbox = document.getElementsByClassName('ItemCheckBox');
        // console.log("checkbox - ", checkbox);
        for (var i = 0; i < checkbox.length; i++) {
            if (!itemMassDelete.includes(checkbox[i].id)) {
                checkbox[i].checked = true;
                itemMassDelete.push(checkbox[i].id);
            }
        }
    }
    else {
        checkbox = document.getElementsByClassName('ItemCheckBox');
        // console.log("checkbox - ", checkbox);
        for (var i = 0; i < checkbox.length; i++) {
            if (itemMassDelete.includes(checkbox[i].id)) {
                checkbox[i].checked = false;
                itemMassDelete.splice(itemMassDelete.indexOf(checkbox[i].id), 1);
            }
        }
    }
    // console.log("SelectAllItemForMassDelete itemMassDelete - ", itemMassDelete);
}

function SelectAllModifierForMassDelete(elem) {
    if (elem.checked) {
        checkbox = document.getElementsByClassName('ModifierCheckbox');
        // console.log("checkbox - ", checkbox);
        for (var i = 0; i < checkbox.length; i++) {
            if (!modifierMassDelete.includes(checkbox[i].id)) {
                checkbox[i].checked = true;
                modifierMassDelete.push(checkbox[i].id);
            }
        }
    }
    else {
        checkbox = document.getElementsByClassName('ModifierCheckbox');
        // console.log("checkbox - ", checkbox);
        for (var i = 0; i < checkbox.length; i++) {
            if (modifierMassDelete.includes(checkbox[i].id)) {
                checkbox[i].checked = false;
                modifierMassDelete.splice(modifierMassDelete.indexOf(checkbox[i].id), 1);
            }
        }
    }
    // console.log("SelectAllModifierForMassDelete modifierMassDelete - ", modifierMassDelete);
}

function checkItemCheckBoxSelectAll() {
    checkbox = document.getElementsByClassName('ItemCheckBox');
    // console.log("checkbox - ", checkbox);
    var c = false;
    for (var i = 0; i < checkbox.length; i++) {
        if (checkbox[i].checked == true) {
            c = true;
        }
        else {
            c = false;
            break;
        }
    }
    if (c == true) {
        document.getElementById("ItemCheckBoxSelectAll").checked = true;
        //console.log("aasascccs - ",c);
    }
    else {
        document.getElementById("ItemCheckBoxSelectAll").checked = false;
    }
    // console.log("checkItemCheckBoxSelectAll itemMassDelete - ", itemMassDelete);
}

function checkModifierCheckBoxSelectAll() {
    checkbox = document.getElementsByClassName('ModifierCheckbox');
    // console.log("checkbox - ", checkbox);
    var c = false;
    for (var i = 0; i < checkbox.length; i++) {
        if (checkbox[i].checked == true) {
            c = true;
        }
        else {
            c = false;
            break;
        }
    }
    if (c == true) {
        document.getElementById("ModifierCheckBoxSelectAll").checked = true;
    }
    else {
        document.getElementById("ModifierCheckBoxSelectAll").checked = false;
    }
    // console.log("checkModifierCheckBoxSelectAll modifierMassDelete - ", modifierMassDelete);
}

function getAllSelectExModifier(elem) {
    let modifierId = elem.id;
    let Mname = elem.closest("div").id;
    // console.log("ModifierID - ", modifierId);
    // console.log("Mname - ", Mname);

    if (!selectModifierFromAddEx.includes(modifierId)) {
        selectModifierFromAddEx.push(modifierId);
        if (!nameOfModifer.includes(Mname)) {
            nameOfModifer.push(Mname);
        }
    }

    else {
        selectModifierFromAddEx.splice(selectModifierFromAddEx.indexOf(modifierId), 1);
        nameOfModifer.splice(nameOfModifer.indexOf(Mname), 1);
        deleteMgModifier.push(modifierId);
    }
}

function migrateListOfModifer() {
    var data = document.getElementById("existingModiferLable");
    var data2 = document.getElementById("existingModiferLableforAddMG");
    //console.log("data - ", data);
    //console.log("data2 - ", data);
    if (data != null) {
        data.innerHTML = "";
        for (i = 1; i < nameOfModifer.length; i++) {
            //console.log("i - ", i);
            data.innerHTML += `<span class="badge badge-pill badge-light border p-1 ps-2 pe-2 fs-6 mb-2 me-2" id="${selectModifierFromAddEx[i]}" >${nameOfModifer[i]} &nbsp; <a onclick="MGmodifierBadge(this)" class="p-1"><i class="fa-solid fa-xmark"></i></a></span>`;
        }
    }
    if (data2 != null) {
        data2.innerHTML = "";
        for (i = 0; i < nameOfModifer.length; i++) {
            // console.log("i - ", i);
            data2.innerHTML += `<span class="badge badge-pill badge-light border p-1 ps-2 pe-2 fs-6 mb-2 me-2" id="${selectModifierFromAddEx[i]}" >${nameOfModifer[i]} &nbsp; <a onclick="MGmodifierBadge(this)" class="p-1"><i class="fa-solid fa-xmark"></i></a></span>`;
        }
    }
}

function MGmodifierBadge(elem) {
    let modifierId = elem.closest("span").id;
    let modifierName = elem.closest("span").textContent.trim();
    // console.log("MGmodifierBadge = ", modifierId, modifierName);
    if (selectModifierFromAddEx.includes(modifierId)) {
        selectModifierFromAddEx.splice(selectModifierFromAddEx.indexOf(modifierId), 1);
        nameOfModifer.splice(nameOfModifer.indexOf(modifierName), 1);
    }
    if (!deleteMgModifier.includes(modifierId)) {
        deleteMgModifier.push(modifierId);
    }
    // console.log("deleteMgModifier = ", deleteMgModifier);
    // console.log("MGmodifierBadge selectModifierFromAddEx - ", selectModifierFromAddEx);
    // console.log("MGmodifierBadge nameOfModifer - ", nameOfModifer);
    elem.closest("span").classList.add("d-none");
}

function getEditItemID(elem) {
    // console.log("Cid - ", cid);
    selectModifierGroupFromDropDown = [];
    deleteExistingModifierGroupEditItem = [];
    $.ajax({
        url: "/Menu/EditItem",
        type: "GET",
        data: { itemid: elem.id },
        success: function (data) {
            if(data.status == false){
                notyf.error("Invalid Item Id");
                $("#EditMenuItemModal").modal("hide");
            }
            else{
                // console.log(data);
                $("#EditMenuItemModal .modal-content").html(data);
                $("#EditMenuItemModal").modal("show");
            }
        },
        error : function(data){
            console.log(data);
        }
    });
}

function getDeleteID(elem) {
    // console.log("Cid - ", elem.id);
    $.ajax({
        url: "/Menu/EditCategory",
        type: "GET",
        data: { categoryId: elem.id },
        success: function (data) {
            // console.log(data);
            $("#DeleteCategoryIdModal").val(data.categoryid);
            $("#DeleteCategoryNameModal").val(data.categoryname);
            $("#DeleteCategoryDescModal").val(data.categoryDesc);
            $("#DeleteCategoryModal").modal("show");
        },
        error : function(data){
            console.log(data);
        }
    })
}

function getModifierDeleteID(elem) {
    // console.log("Delete Modifier id - ", elem.id);
    // console.log("Delete MGid - ", mgid);
    $("#DeleteModifierIdModal").val(elem.id);
    $("#DeleteModifierGroupIdModal").val(mgid);
    $("#DeleteModifierModal").modal("show");
}

function getMGDeleteID(elem) {
    // console.log("MGid - ", elem.id);
    $.ajax({
        url: "/Menu/DeleteMG",
        type: "GET",
        data: { MGid: elem.id },
        success: function (data) {
            // console.log(data);
            $("#DeleteModiferGroupModal .modal-content").html(data);
            $("#DeleteModiferGroupModal").modal("show");
        },
        error : function(data){
            console.log(data);
        }
    });
}

function getDeleteItemID(elem) {
    $("#DeleteItemId").val(elem.id);
    $("#DeleteItemCategoryId").val(cid);
    $("#DeleteItemModal").modal("show");
}

function getCategoryId(elem) {
    cid = elem.id;
    if ($("#CategoryDropDown").find(`option[value = "${cid}"]`).length > 0) {
        $("#CategoryDropDown").val(cid).change();
        // console.log("Val - ", $("#CategoryDropDown").val());
        // console.log("zsvzv")
    }
    // console.log("New cid - ", cid);
    $.ajax({
        url: "/Menu/GetItemByCategoryId",
        type: "GET",
        data: { categoryId: elem.id },
        success: function (data) {
            $("#ItemSection").empty();
            $("#ItemSection").html(data);
            var category_name = document.getElementsByClassName("category_name");
            for (var i = 0; i < category_name.length; i++) {
                category_name[i].classList.remove("text-primary");
                category_name[i].classList.remove("fs-5");
            }
            elem.querySelector(".category_name").classList.add("text-primary");
            elem.querySelector(".category_name").classList.add("fs-5");
            itemMassDelete = [];
        }
    })
}

function GetDataForItemList(categoryid) {
    $.ajax({
        url: "/Menu/GetItemByCategoryId",
        type: "GET",
        data: { categoryId: categoryid },
        success: function (data) {
            $("#ItemSection").empty();
            $("#ItemSection").html(data);
        },
        error : function(data){
            console.log(data);
        }
    })
}

function GetModifierGroupList() {
    $.ajax({
        url: "/Menu/GetAllMgforPartialView",
        type: "GET",
        success: function (data) {
            $("#modifierGroupList").empty();
            $("#modifierGroupList").html(data);
        },
        error: function (data) {
            console.log(data);
        }
    })
}

function GetModifierForPartialView(modifiergroupid) {
    $.ajax({
        url: "/Menu/GetModifiersByMGId",
        type: "GET",
        data: { Mgid: modifiergroupid },
        success: function (data) {
            // console.log(data);
            $("#ModifierSection").empty();
            $("#ModifierSection").html(data);
        },
        error: function (data) {
            console.log(data);
        }
    });
}

function getMGId(elem) {
    // console.log("MGid - ", elem.id);
    mgid = elem.id;
    if ($("#ModifierGroupDropDown").find(`option[value = "${mgid}"]`).length > 0) {
        $("#ModifierGroupDropDown").val(mgid).change();
    }
    $.ajax({
        url: "/Menu/GetModifiersByMGId",
        type: "GET",
        data: { Mgid: elem.id },
        success: function (data) {
            // console.log(data);
            $("#ModifierSection").empty();
            $("#ModifierSection").html(data);
            modifierMassDelete = [];
            var modifier_group_name = document.getElementsByClassName("modifier_group_name");
            for (var i = 0; i < modifier_group_name.length; i++) {
                modifier_group_name[i].classList.remove("text-primary");
                modifier_group_name[i].classList.remove("fs-5");
            }
            elem.querySelector(".modifier_group_name").classList.add("text-primary");
            elem.querySelector(".modifier_group_name").classList.add("fs-5");
        },
        error: function (data) {
            console.log(data);
        }
    });
}

function openTabs(event, tabId) {
    document.querySelectorAll(".tab-content").forEach(tab => {
        tab.style.display = "none";
    });
    document.querySelectorAll(".tab-btn").forEach(tab => {
        tab.classList.remove("active-menu-option");
        tab.classList.remove("text-primary");
    });
    document.getElementById(tabId).style.display = "block";
    event.currentTarget.classList.add("active-menu-option");
    event.currentTarget.classList.add("text-primary");
}

function PageSizeDropDown() {
    var selectedpagesize = document.getElementById("ItemPageSizeDropDown").value;
    // console.log("New Pagesize - ", selectedpagesize);
    var searchData = $("#SearchBox").val().trim();
    $.ajax({
        url: "/Menu/GetItemByCategoryId",
        method: "GET",
        data: { categoryId: cid, pagesize: selectedpagesize , searchcriteria: searchData},
        success: function (data) {
            $("#ItemSection").empty();
            $("#ItemSection").html(data);
            checkbox = document.getElementsByClassName('ItemCheckBox');
            //console.log("length - ",checkbox.length);
            for (var i = 0; i < checkbox.length; i++) {
                // console.log("CheckBod id - ", checkbox[i].getAttribute("id"));
                // console.log(itemMassDelete.includes(checkbox[i].getAttribute("id")));
                if (itemMassDelete.includes(checkbox[i].getAttribute("id"))) {
                    checkbox[i].checked = true;
                }
            }
            checkItemCheckBoxSelectAll();
        },
        error: function (response) {
            console.log(response);
        }
    });
}

function ModifierPageSizeDropDown() {
    var selectedpagesize = document.getElementById("ModifierPageSizeDropDown").value;
    // console.log("New Pagesize - ", selectedpagesize);
    var searchData = $("#ModifierSearchBox").val().trim();
    $.ajax({
        url: "/Menu/GetModifiersByMGId",
        method: "GET",
        data: { Mgid: mgid, pagesize: selectedpagesize , searchcriteria: searchData},
        success: function (data) {
            $("#ModifierSection").empty();
            $("#ModifierSection").html(data);
            checkbox = document.getElementsByClassName('ModifierCheckbox');
            for (var i = 0; i < checkbox.length; i++) {
                // console.log("CheckBod id - ", checkbox[i].getAttribute("id"));
                // console.log(modifierMassDelete.includes(checkbox[i].getAttribute("id")));
                if (modifierMassDelete.includes(checkbox[i].getAttribute("id"))) {
                    checkbox[i].checked = true;
                }
            }
            checkModifierCheckBoxSelectAll();
        },
        error: function (data) {
            console.log(data);
        }
    });
}

function EXModifierPageSizeDropDownchange() {
    var selectedpagesize = document.getElementById("EXModifierPageSizeDropDown").value;
    // console.log("New Pagesize - ", selectedpagesize);
    $.ajax({
        url: "/Menu/GetDataForExistingModifiers",
        method: "GET",
        data: { pagesize: selectedpagesize },
        success: function (data) {
            $("#AddExistingModiferGroupModal .modal-content").html(data);
            $("#SelectExModifierId").val(selectModifierFromAddEx);
            var selectedCheckBox = $("#SelectExModifierId").val();
            checkbox = document.getElementsByClassName('EXModiferCheckBox');
            //console.log("length - ",checkbox.length);
            for (var i = 0; i < checkbox.length; i++) {
                // console.log("CheckBod id - ", checkbox[i].getAttribute("id"));
                // console.log(selectModifierFromAddEx.includes(checkbox[i].getAttribute("id")));
                if (selectModifierFromAddEx.includes(checkbox[i].getAttribute("id"))) {
                    checkbox[i].checked = true;
                }
            }
        },
        error: function (data) {
            console.log(data);
        }
    });
}

function ChangeItemAvailableStatus(elem) {
    var itemid = elem.id;
    var status = elem.checked;
    // console.log("status - ", status);
    $.ajax({
        type: "POST",
        url: "/Menu/ChangeItemAvailable",
        data: { ItemId: itemid, Status: status },
        success: function (data) {
            if (data.status == true) {
                if(status){
                    notyf.success("Item Is Available");
                }
                else{
                    notyf.success("Item Is Not Available");
                }
            }
            else {
                notyf.error("Something Went Wrong");
            }
            GetDataForItemList(cid);
        },
        error: function (data) {
            notyf.error("Something Went Wrong");
            console.log(data);
        }
    });
}

function imgPreview(elem){
    const input = elem.target;
    const preview = document.getElementById("add-item-img-preview");

    if(input && input.files[0]){
        const reader = new FileReader();
        reader.onload = function(e){
            preview.src = e.target.result;
            preview.style.display = 'block';
            document.getElementById("add-item-img-preview").style.display = 'none';
        }
    }
    else{
        preview.src = "#";
        preview.style.display = 'none';
        document.getElementById("add-item-img-preview").style.display = 'block';
    }
}

$(document).off("click", "#AddModifierSubmit").on("click", "#AddModifierSubmit", function (event) {
    event.preventDefault();
    $("#AddModifier").removeAttr("novalidate");
    $.validator.unobtrusive.parse($("#AddModifier"));
    if (!$("#AddModifier").valid()) {
        return;
    }
    $.ajax({
        type: "POST",
        url: "/Menu/AddModifier",
        data: $("#AddModifier").serialize(),
        success: function (data) {
            previousSelectedModifierName = [];
            previousSelectedModifier = [];
            $("#AddModifier")[0].reset();
            $("#AddModifierModal").modal("hide");
            if (data.status == true) {
                notyf.success("Modifier Added Succesfully");
            }
            else {
                notyf.error("Something Went Wrong");
            }
            GetModifierForPartialView(mgid);
        },
        error: function (data) {
            console.log(data);
        }
    });
})

$(document).off("click", "#EditModifierGroupSubmit").on("click", "#EditModifierGroupSubmit", function (event) {
    event.preventDefault();
    $("#EditModifierGroup").removeAttr("novalidate");
    $.validator.unobtrusive.parse($("#EditModifierGroup"));
    if (!$("#EditModifierGroup").valid()) {
        return;
    }
    $.ajax({
        type: "POST",
        url: "/Menu/EditModifierGroup",
        data: $("#EditModifierGroup").serialize(),
        success: function (data) {
            console.log(data);
            $.ajax({
                type: "POST",
                url: "/Menu/AddAllModifiersFromExModifier",
                data: { mid: selectModifierFromAddEx, MgId: mgid },
                success: function (data) {
                    // console.log(data);
                    $.ajax({
                        type: "POST",
                        url: "/Menu/DeleteModifierMG",
                        data: { modifier: deleteMgModifier, MgId: mgid },
                        success: function (data) {
                            // console.log(data);
                            $("#ModifierSection").empty();
                            $("#ModifierSection").html(data);
                            $("#EditModifierGroup")[0].reset();
                            $("#EditModiferGroupModal").modal("hide");
                        },
                        error: function (data) {
                            console.log(data);
                        }
                    });
                },
                error: function (data) {
                    console.log(data);
                }
            });
            if (data.status == true) {
                notyf.success("Modifier Group Added Succesfully");
            }
            else {
                notyf.error("Something Went Wrong");
            }
            GetModifierGroupList();
        },
        error: function (data) {
            console.log(data);
            notyf.error("Something Went Wrong");
        }
    });

})

$(document).off("click", "#AddModifierGroupModalSubmit").on("click", "#AddModifierGroupModalSubmit", function (event) {
    event.preventDefault();
    $("#AddModfierGroupForm").removeAttr("novalidate");
    $.validator.unobtrusive.parse($("#AddModfierGroupForm"));
    if (!$("#AddModfierGroupForm").valid()) {
        return;
    }
    let name = $("#AddMgName").val();
    let desc = $("#AddMgDesc").val();
    // console.log("AddModifierGroupModalSubmit selectModifierFromAddEx - ", selectModifierFromAddEx);
    $.ajax({
        type: "POST",
        url: "/Menu/AddModifierGroup",
        data: { mgName: name, mgDesc: desc, mids: selectModifierFromAddEx },
        success: function (data) {
            // console.log(data);
            if (data.status == true) {
                notyf.success("Modifier Group Added Succesfully");
            }
            else {
                notyf.error("Something Went Wrong");
            }
            $("#AddModfierGroupForm")[0].reset();
            $("#AddModiferGroupModal").modal("hide");
            GetModifierGroupList();
        },
        error: function (data) {
            console.log(data);
        }
    });
})

$(document).off("click", "#AddExModifierModalSubmit").on("click", "#AddExModifierModalSubmit", function () {
    migrateListOfModifer();
    $("#AddExistingModiferGroupModal").modal("hide");
})

$(document).off("click", "#DeleteModifierGroupSubmit").on("click", "#DeleteModifierGroupSubmit", function (event) {
    event.preventDefault();
    $.ajax({
        type: "POST",
        url: "/Menu/DeleteMG",
        data: $("#DeleteModifierGroup").serialize(),
        success: function (data) {
            // console.log(data);
            if (data.status == true) {
                notyf.success("Modifier Group Deleted Succesfully");
            }
            else {
                notyf.error("Something Went Wrong");
            }
            $("#DeleteModiferGroupModal").modal("hide");
            GetModifierGroupList();
        },
        error: function (data) {
            console.log(data);
        }
    });
})

$(document).off("click", "#DeleteModifierSubmit").on("click", "#DeleteModifierSubmit", function (event) {
    event.preventDefault();
    $.ajax({
        type: "POST",
        url: "/Menu/DeleteModifier",
        data: $("#DeleteModifier").serialize(),
        success: function (data) {
            if (data.status == true) {
                notyf.success("Modifier Editted Succesfully");
            }
            else {
                notyf.error("Something Went Wrong");
            }
            $("#DeleteModifier")[0].reset();
            $("#DeleteModifier").closest(".modal").modal("hide");
            GetModifierForPartialView(mgid);
        },
        error: function (data) {
            console.log(data);
        }
    });
})

$(document).off("click", "#EditModifierModalSubmit").on("click", "#EditModifierModalSubmit", function (event) {
    event.preventDefault();
    $("#EditModifierForm").removeAttr("novalidate");
    $.validator.unobtrusive.parse($("#EditModifierForm "));
    if (!$("#EditModifierForm").valid()) {
        return;
    }
    $.ajax({
        type: "POST",
        url: "/Menu/EditModifer",
        data: $("#EditModifierForm").serialize(),
        success: function (data) {
            if (data.status == true) {
                notyf.success("Modifier Editted Succesfully");
            }
            else {
                notyf.error("Something Went Wrong");
            }
            $("#EditModifierForm")[0].reset();
            $("#EditModifierModal").modal("hide");
            GetModifierForPartialView(mgid);
        },
        error: function (data) {
            console.log(data);
        }
    });
})

$(document).off("change", "#add-item-upload-file").on("change", "#add-item-upload-file", function (event) {

    var img_path = $("#add-item-upload-file").val().split("\\");
    var img = img_path[img_path.length - 1];

    // console.log("add-item-upload-file - ", img_path);
    // console.log("add-item-upload-file - ", img);

    document.getElementById("add-item-drag-drop").style.display = 'none';
    $("#Add-Item-Img-Name").text(img);

    const input = event.target;
    const preview = document.getElementById("add-item-img-preview");

    if(input && input.files[0]){
        const reader = new FileReader();
        reader.onload = function(e){
            preview.src = e.target.result;
            preview.style.display = 'block';
        }
        reader.readAsDataURL(input.files[0]);
        document.getElementById("add-item-file-upload").style.display = 'none';
    }
    else{
        preview.src = "#";
        preview.style.display = 'none';
        document.getElementById("add-item-file-upload").style.display = 'block';
    }
})

$(document).off("change", "#edit-upload-file").on("change", "#edit-upload-file", function (event) {

    var img_path = $("#edit-upload-file").val().split("\\");
    var img = img_path[img_path.length - 1];
    // console.log("edit-upload-file - ", img_path);
    // console.log("edit-upload-file - ", img);
    document.getElementById("edit-item-drag-drop").style.display = 'none';
    $("#edit-item-img-name").text(img);

    const input = event.target;
    const preview = document.getElementById("edit-item-img-preview");

    if(input && input.files[0]){
        // console.log("Input - ",input.files[0]);
        const reader = new FileReader();
        reader.onload = function(e){
            preview.src = e.target.result;
            preview.style.display = 'block';
        }
        // document.getElementById("edit-item-file-upload").style.display = 'none';
        reader.readAsDataURL(input.files[0]);
    }
    else{
        preview.src = "#";
        preview.style.display = 'none';
        document.getElementById("edit-item-file-upload").style.display = 'none';
    }
})

$(document).off("click", "#AddItemModalSubmit").on("click", "#AddItemModalSubmit", function (event) {
    event.preventDefault();
    $("#AddItemModalForm").removeAttr("novalidate");
    $.validator.unobtrusive.parse($("#AddItemModalForm"));
    if (!$("#AddItemModalForm").valid()) {
        return;
    }
    var modifierGroupdata = [];
    var break1 = false;
    var formData = new FormData($("#AddItemModalForm")[0]);
    selectModifierGroupFromDropDown.forEach(function (id) {
        var minid = `#min-mod-${id}`;
        var maxid = `#max-mod-${id}`;
        if ($(minid).val() == "") {
            break1 = true;
            notyf.error("Select min value");
            return;
        }
        if ($(maxid).val() == "") {
            break1 = true;
            notyf.error("Select max value");
            return;
        }
        if ($(minid).val() > $(maxid).val()) {
            break1 = true;
            notyf.error("Select valid min max value");
            return;
        }
        modifierGroupdata.push({
            ItemId: 0,
            ModifierGroupId: parseInt(id),
            ModifierGroupMinVal: parseInt($(minid).val()),
            ModifierGroupMaxVal: parseInt($(maxid).val())
        });
    });
    // console.log("modifierGroupdata - ", modifierGroupdata);
    if (break1 == true) {
        return;
    }
    formData.append("ModiferGroupData", JSON.stringify(modifierGroupdata));
    $.ajax({
        type: "POST",
        url: "/Menu/AddItems",
        data: formData,
        contentType: false,
        processData: false,
        success: function (data) {
            // console.log(data);
            if (data.status == true) {
                notyf.success("Item Added Succesfully");
            }
            else {
                notyf.error("Something Went Wrong");
            }
            selectModifierGroupFromDropDown = [];
            $("#AddItemModalForm")[0].reset();
            $("#AddNewMenuItemModal").modal("hide");
            GetDataForItemList(cid);
        },
        error: function (data) {
            console.log(data);
        }
    });
})

$(document).off("click", "#EditItemModalSubmit").on("click", "#EditItemModalSubmit", function (event) {
    event.preventDefault();
    $("#EditItemModalForm").removeAttr("novalidate");
    $.validator.unobtrusive.parse($("#EditItemModalForm"));
    if (!$("#EditItemModalForm").valid()) {
        return;
    }
    var modifierGroupdata = [];
    var break1 = false;
    var formData = new FormData($("#EditItemModalForm")[0]);
    $("#ExistingModifireGroupSectionEditItem > div").each(function () {
        var itemId = $("#Edit-item-Id").val();
        var modifierGroupId = $(this).find("input[id ^= 'edit-modifier-id']").val();
        var minid = `#edit-min-mod-${modifierGroupId}`;
        var maxid = `#edit-max-mod-${modifierGroupId}`;
        if ($(minid).val() == "") {
            break1 = true;
            notyf.error("Select min value");
            return;
        }
        if ($(maxid).val() == "") {
            break1 = true;
            notyf.error("Select max value");
            return;
        }
        if ($(minid).val() > $(maxid).val()) {
            break1 = true;
            notyf.error("Select valid min max value");
            return;
        }
        modifierGroupdata.push({
            ItemId: itemId,
            ModifierGroupId: modifierGroupId,
            ModifierGroupMinVal: parseInt($(minid).val()),
            ModifierGroupMaxVal: parseInt($(maxid).val())
        });
    });
    // console.log("modifierGroupdata - ", modifierGroupdata);
    if (break1 == true) {
        return;
    }
    formData.append("ModifierGroupData", JSON.stringify(modifierGroupdata));
    var ItemId = $("#Edit-item-Id").val();
    var categoryId = $("#EditItemCategoryId").val();
    $.ajax({
        type: "POST",
        url: "/Menu/EditItem",
        data: formData,
        contentType: false,
        processData: false,
        success: function (data) {
            selectModifierGroupFromDropDown = [];
            $.ajax({
                type: "POST",
                url: "/Menu/DeleteModifierGroupForEditItem",
                data: { modifierIdList: deleteExistingModifierGroupEditItem, itemId: ItemId, CategoryId: categoryId },
                success: function (data) {
                    deleteExistingModifierGroupEditItem = [];
                    $("#EditItemModalForm")[0].reset();
                    $("#EditMenuItemModal").modal("hide");
                },
                error: function (data) {
                    console.log(data);
                }
            });
            if (data.status == true) {
                notyf.success("Item Editted Succesfully");
            }
            else {
                notyf.error("Something Went Wrong");
            }
            GetDataForItemList(cid);
        },
        error: function (data) {
            console.log(data);
        }
    });
})

$(document).off("click" , ".AddCategoryModalDismiss").on("click" , ".AddCategoryModalDismiss" , function(event){
    event.preventDefault();
    console.log("Remove data");
    $("#AddCategoryModalForm")[0].reset();
})

$(document).off("click", "#AddExModifierModalCancle").on("click", "#AddExModifierModalCancle", function () {
    //event.preventDefault();
    selectModifierFromAddEx = [];
    nameOfModifer = [];
    selectModifierFromAddEx = previousSelectedModifier.slice();
    nameOfModifer = previousSelectedModifierName.slice();
    if (document.getElementById("ModifierGroupFromDB") != null) {
        if (document.getElementById("ModifierGroupFromDB").hasChildNodes() == true) {
            var alreadyExistModifierInEditMg = document.getElementById("ModifierGroupFromDB").childNodes;
            //console.log("getMGEditID alreadyExistModifierInEditMg - ", alreadyExistModifierInEditMg);
            for (var i = 0; i < alreadyExistModifierInEditMg.length; i++) {
                var id = alreadyExistModifierInEditMg[i].id;
                var modifierName = alreadyExistModifierInEditMg[i].innerText;
                //modifierName = modifierName.trim();
                //console.log("modifierName", modifierName);
                if (!selectModifierFromAddEx.includes(id)) {
                    selectModifierFromAddEx.push(id);
                    if (!nameOfModifer.includes(modifierName)) {
                        nameOfModifer.push(modifierName);
                    }
                }
            }
            //console.log("getMGEditID selectModifierFromAddEx - ", selectModifierFromAddEx);
            //console.log("getMGEditID nameOfModifer - ", nameOfModifer);
        }
    }
    $("#AddExistingModiferGroupModal").modal("hide");
})

$(document).off("click", "#AddExModifierModalDismiss").on("click", "#AddExModifierModalDismiss", function () {
    //event.preventDefault();
    selectModifierFromAddEx = [];
    nameOfModifer = [];
    selectModifierFromAddEx = previousSelectedModifier.slice();
    nameOfModifer = previousSelectedModifierName.slice();
    if (document.getElementById("ModifierGroupFromDB").hasChildNodes() == true) {
        var alreadyExistModifierInEditMg = document.getElementById("ModifierGroupFromDB").childNodes;
        //console.log("getMGEditID alreadyExistModifierInEditMg - ", alreadyExistModifierInEditMg);
        for (var i = 0; i < alreadyExistModifierInEditMg.length; i++) {
            var id = alreadyExistModifierInEditMg[i].id;
            var modifierName = alreadyExistModifierInEditMg[i].innerText;
            //modifierName = modifierName.trim();
            //console.log("modifierName", modifierName);
            if (!selectModifierFromAddEx.includes(id)) {
                selectModifierFromAddEx.push(id);
                if (!nameOfModifer.includes(modifierName)) {
                    nameOfModifer.push(modifierName);
                }
            }
        }
        //console.log("getMGEditID selectModifierFromAddEx - ", selectModifierFromAddEx);
        //console.log("getMGEditID nameOfModifer - ", nameOfModifer);
    }
    $("#AddExistingModiferGroupModal").modal("hide");
})

$(document).ready(function () {

    $("#AddItemModierDropDown").on("change", function () {
        var ModifireGroupId = $("#AddItemModierDropDown").val();
        var ModifireGroupName = $("#AddItemModierDropDown").find("option:selected").text();
        //console.log("Modifer Group - ", ModifireGroupId);
        //console.log("ModiferGroupid - ", ModifireGroupId);
        if (!selectModifierGroupFromDropDown.includes(ModifireGroupId)) {
            selectModifierGroupFromDropDown.push(ModifireGroupId);
        }
        //console.log("AddModifierGroupAddItem selectModifierGroupFromDropDown - ", selectModifierGroupFromDropDown);
        $.ajax({
            type: "GET",
            url: "/Menu/GetModForAddItem",
            data: { MgId: ModifireGroupId },
            success: function (data) {
                console.log(data);
                if ($(`#modifierGroup-` + ModifireGroupId).length == 0) {
                    var MgTable = data.map(function (modifiers) {
                        return `
                                        <tr>
                                            <td>
                                                <li>${modifiers.modifiername}</li>
                                            </td>
                                            <td class="text-end">${modifiers.rate}</td>
                                        </tr>
                                    `;
                    }).join('');
                    var newModGroup = `
                                    <div class="row px-3" id="modifierGroup-${ModifireGroupId}">
                                        <div class="col-12 d-flex justify-content-between align-items-center">
                                            <div class="h5" id="${ModifireGroupId}">${ModifireGroupName}</div>
                                            <div class="h5"><i class="fa-solid fa-trash" onClick="DeleteMGFromAddItems(${ModifireGroupId})"></i></div>
                                        </div>
                                        <div class="col-12">
                                            <div class="">
                                                <div class="col-12 d-flex justify-content-center align-items-center p-0">
                                                    <div class="col-6 p-1 pe-2 d-flex justify-content-start">
                                                        <input type="number" class="form-control rounded-pill mx-1 min-modifier"
                                                            placeholder="Min Modifiers" id="min-mod-${ModifireGroupId}">
                                                    </div>
                                                    <div class="col-6 p-1 ps-2 d-flex justify-content-end">
                                                        <input type="number" class="form-control rounded-pill mx-1 max-modifier"
                                                            placeholder="Max Modifiers" id="max-mod-${ModifireGroupId}">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-12 d-flex justify-content-center">
                                            <table class="mx-2 w-100">
                                                ${MgTable}
                                            </table>
                                        </div>
                                    </div>
                                `;
                    //console.log(newModGroup);
                    $("#SelectModifireGroupSection").append(newModGroup);
                }
            },
            error: function (data) {
                console.log(data);
            }
        });
    })

    $(document).on("change", "#EditItemModifierGroupDropDown", function () {
        var modifierGroupid = $("#EditItemModifierGroupDropDown").val();
        var ModifireGroupName = $("#EditItemModifierGroupDropDown").find("option:selected").text();

        $.ajax({
            type: "GET",
            url: "/Menu/GetModForAddItem",
            data: { MgId: modifierGroupid },
            success: function (data) {
                if ($(`#EditmodifierGroup-` + modifierGroupid).length == 0) {
                    if (!selectModifierGroupFromDropDown.includes(modifierGroupid)) {
                        selectModifierGroupFromDropDown.push(modifierGroupid);
                    }
                    // console.log("EditItemModifierGroupDropDown selectModifierGroupFromDropDown - ", selectModifierGroupFromDropDown);
                    var MgTable = data.map(function (modifiers) {
                        return `
                                        <tr>
                                            <td>
                                                <li>${modifiers.modifiername}</li>
                                            </td>
                                            <td class="text-end">${modifiers.rate}</td>
                                        </tr>
                                    `;
                    }).join('');
                    var newModGroup = `
                                    <div class="row px-3" id="EditmodifierGroup-${modifierGroupid}">
                                        <div class="col-12 d-flex justify-content-between align-items-center">
                                            <div class="h5" id="${modifierGroupid}">${ModifireGroupName}</div>
                                            <div class="h5"><i class="fa-solid fa-trash" onClick="DeleteMgFromEditItem(${modifierGroupid})"></i></div>
                                        </div>
                                        <div class="col-12">
                                            <div class="">
                                                <div class="col-12 d-flex justify-content-center align-items-center p-0">
                                                    <div class="col-6 p-1 pe-2 d-flex justify-content-start">
                                                    <input type="hidden" name="" id="edit-modifier-id-${modifierGroupid}" value="${modifierGroupid}">
                                                        <input type="number" class="form-control rounded-pill mx-1 min-modifier"
                                                            placeholder="Min Modifiers" min="0" id="edit-min-mod-${modifierGroupid}">
                                                    </div>
                                                    <div class="col-6 p-1 ps-2 d-flex justify-content-end">
                                                        <input type="number" class="form-control rounded-pill mx-1 max-modifier"
                                                            placeholder="Max Modifiers" min="0" id="edit-max-mod-${modifierGroupid}">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-12 d-flex justify-content-center">
                                            <table class="mx-2 w-100">
                                                ${MgTable}
                                            </table>
                                        </div>
                                    </div>
                                `;
                    $("#ExistingModifireGroupSectionEditItem").append(newModGroup);
                }
            },
            error: function (data) {
                console.log(data);
            }
        });
    })

    //searchcriteria
    $("#SearchBox").on("input", function () {
        var searchData = $("#SearchBox").val().trim();
        var selectedpagesize = document.getElementById("ItemPageSizeDropDown").value;
        //console.log("Cid - ", cid);
        $.ajax({
            type: "Get",
            url: "/Menu/GetItemByCategoryId",
            data: { categoryId: cid, searchcriteria: searchData , pagesize: selectedpagesize},
            success: function (data) {
                $("#ItemSection").empty();
                $("#ItemSection").html(data);
                $("#SearchBox").val(searchData);
                $("#SearchBox").focus();
                checkbox = document.getElementsByClassName('ItemCheckBox');
                //console.log("length - ",checkbox.length);
                for (var i = 0; i < checkbox.length; i++) {
                    // console.log("CheckBod id - ", checkbox[i].getAttribute("id"));
                    // console.log(itemMassDelete.includes(checkbox[i].getAttribute("id")));
                    if (itemMassDelete.includes(checkbox[i].getAttribute("id"))) {
                        checkbox[i].checked = true;
                    }
                }
                checkItemCheckBoxSelectAll();
            }
        })
    });

    //modifier search
    $("#ModifierSearchBox").on("input", function () {
        var searchData = $("#ModifierSearchBox").val().trim();
        var selectedpagesize = document.getElementById("ModifierPageSizeDropDown").value;
        //console.log("MGid - ", mgid);
        //console.log("searchData - ", searchData);
        $.ajax({
            url:"/Menu/GetModifiersByMGId",
            type: "GET",
            data: { Mgid: mgid, searchcriteria: searchData , pagesize: selectedpagesize },
            success: function (data) {
                $("#ModifierSection").empty();
                $("#ModifierSection").html(data);
                $("#ModifierSearchBox").val(searchData);
                $("#ModifierSearchBox").focus();
                checkbox = document.getElementsByClassName('ModifierCheckbox');
                for (var i = 0; i < checkbox.length; i++) {
                    // console.log("CheckBod id - ", checkbox[i].getAttribute("id"));
                    // console.log(modifierMassDelete.includes(checkbox[i].getAttribute("id")));
                    if (modifierMassDelete.includes(checkbox[i].getAttribute("id"))) {
                        checkbox[i].checked = true;
                    }
                }
                checkModifierCheckBoxSelectAll();
            },
            error: function (data) {
                console.log(data);
            }
        });
    });

    //add exisiting search
    $(document).on("input", "#AddExistingModifierSearchBox", function () {
        var searchData = $("#AddExistingModifierSearchBox").val().trim();
        var pageSize = $("#EXModifierPageSizeDropDown").val();
        $.ajax({
            url: "/Menu/GetDataForExistingModifiers",
            type: "GET",
            data: { searchcriteria: searchData, pagesize: pageSize },
            success: function (data) {
                $("#AddExistingModiferGroupModal .modal-content").html(data);
                checkbox = document.getElementsByClassName('EXModiferCheckBox');
                //console.log("length - ",checkbox.length);
                for (var i = 0; i < checkbox.length; i++) {
                    // console.log("CheckBod id - ", checkbox[i].getAttribute("id"));
                    // console.log(selectModifierFromAddEx.includes(checkbox[i].getAttribute("id")));
                    if (selectModifierFromAddEx.includes(checkbox[i].getAttribute("id"))) {
                        checkbox[i].checked = true;
                    }
                }
                $("#AddExistingModifierSearchBox").val(searchData);
                $("#AddExistingModifierSearchBox").focus();
            },
            error: function (data) {
                console.log(data);
            }
        });
    });

})