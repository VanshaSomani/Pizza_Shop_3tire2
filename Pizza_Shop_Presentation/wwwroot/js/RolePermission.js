function IsSelectedCheckBoxClicked(elem){
    let PermissionName = elem.id;
    if(elem.checked){
        let viewId = `#CanView-${PermissionName}`;
        let addEditId = `#CanAddEdit-${PermissionName}`;
        let deleteId = `#CanDeleted-${PermissionName}`;
        $(viewId).attr('disabled' , false);
        $(addEditId).attr('disabled' , false);
        $(deleteId).attr('disabled' , false);
    }
    else{
        let viewId = `#CanView-${PermissionName}`;
        let addEditId = `#CanAddEdit-${PermissionName}`;
        let deleteId = `#CanDeleted-${PermissionName}`;
        $(viewId).attr('disabled' , true);
        $(addEditId).attr('disabled' , true);
        $(deleteId).attr('disabled' , true);
    }  
}

function getID(elem){
    // console.log(row.id);
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
document.getElementById("RoleAndPermission-Sidebar").classList.add("active-sidebar");
document.getElementById("RoleAndPermission-Sidebar").querySelector(".sidebar-name").classList.remove("text-dark");
document.getElementById("RoleAndPermission-Sidebar").querySelector(".sidebar-name").classList.add("text-primary");

function CanViewChecked(elem){
    let viewId = elem.id;
    let PermissionName = viewId.split('-')[1];
    let addEditId = `#CanAddEdit-${PermissionName}`;
    let deleteId = `#CanDeleted-${PermissionName}`;
    if(elem.checked){
        $(addEditId).attr('disabled' , false);
        $(deleteId).attr('disabled' , false);
    }
    else{
        $(addEditId).prop('checked' , false);
        $(deleteId).attr('checked' , false);
        $(addEditId).attr('disabled' , true);
        $(deleteId).attr('disabled' , true);
    }
}
