        let tableMassDelete = [];

        function sidebarColorChange(){
            let sidebarAnchor = document.getElementsByClassName("sidebar-anchor");
            for(let i = 0 ; i < sidebarAnchor.length ; i++){
                sidebarAnchor[i].classList.remove("active-sidebar");
                sidebarAnchor[i].querySelector(".sidebar-name").classList.add("text-dark");
                sidebarAnchor[i].querySelector(".sidebar-name").classList.remove("text-primary");
            }
        }
        sidebarColorChange();
        document.getElementById("TableAndSection-Sidebar").classList.add("active-sidebar");
        document.getElementById("TableAndSection-Sidebar").querySelector(".sidebar-name").classList.remove("text-dark");
        document.getElementById("TableAndSection-Sidebar").querySelector(".sidebar-name").classList.add("text-primary");

        function getSectionId(elem) {
            sectionid = elem.id;
            $.ajax({
                type: "GET",
                url: "/TableAndSection/GetTableForTablePartial",
                data: { SectionId: elem.id },
                success: function (data) {
                    $("#TableSection").empty();
                    let section_name = document.getElementsByClassName("section_name");
                    for(let i = 0 ; i < section_name.length ; i++){
                        section_name[i].classList.remove("text-primary");
                        section_name[i].classList.remove("fs-5");
                    }
                    elem.querySelector(".section_name").classList.add("text-primary");
                    elem.querySelector(".section_name").classList.add("fs-5");
                    $("#TableSection").html(data);
                    tableMassDelete = [];
                    
                },
                error: function (data) {
                    console.log(data);
                }
            });
        }

        function getTables(sectionId) {
            $.ajax({
                type: "GET",
                url: "/TableAndSection/GetTableForTablePartial",
                data: { SectionId: sectionId },
                success: function (data) {
                    $("#TableSection").empty();
                    $("#TableSection").html(data);
                },
                error: function (data) {
                    console.log(data);
                }
            });
        }

        function getSectionEditId(elem) {
            sectionid = elem.id;
            $.ajax({
                type: "GET",
                url: "/TableAndSection/GetSectionDataForEdit",
                data: { SectionId: elem.id },
                success: function (data) {
                    if(data.status == false){
                        notyf.error("Invalid section id");
                        $("#EditSectionModal").modal("hide");
                    }
                    else{
                        $("#EditSectionModal .modal-content").html(data);
                        $("#EditSectionModal").modal("show");
                    }
                }

            });
        }

        function AddTable() {
            $.ajax({
                type: "GET",
                url: "/TableAndSection/GetDataForAddTable",
                success: function (data) {
                    if(data.success === false){
                        notyf.error("You have not permission to add table");
                    }
                    else{
                        $("#AddTableModal .modal-content").html(data);
                        $("#AddTableModalForm").removeAttr("novalidate");
                        $("#AddTableModal").modal("show");
                    }
                },
                error: function (data) {
                    console.log(data);
                }
            });
        }

        function GetSectionForPartial() {
            $.ajax({
                type: "GET",
                url: "/TableAndSection/GetSectionForSectionPartial",
                success: function (data) {
                    $("#SectionList").empty();
                    $("#SectionList").html(data);
                },
                error: function (data) {
                    console.log(data);
                }
            });
        }

        function getSectionDeleteId(elem) {
            $("#deleteSectionId").val(elem.id);
            $("#DeleteSectionModal").modal("show");
        }

        function GetEditTableId(elem) {
            $.ajax({
                type: "GET",
                url: "/TableAndSection/GetEditTable",
                data: { tableId: elem.id },
                success: function (data) {
                    if(data.status == false){
                        notyf.error("Invalid Table Id");
                        $("#EditTableModal").modal("hide");
                    }
                    else{
                        $("#EditTableModal .modal-content").html(data);
                        $("#EditTableModal").modal("show");
                    }
                },
                error: function (data) {
                    console.log(data);
                }
            });
        }

        function GetDeleteTableId(elem) {
            $("#deleteTableId").val(elem.id);
            $("#deleteTableSectionId").val(sectionid);
            $("#DeleteTableModal").modal("show");
        }

        function TablepagginationArrowControll(Page, PageSize, TotalRecord) {
            if (Page >= 1 && Page <= TotalRecord) {
                $.ajax({
                    type: "GET",
                    url: "/TableAndSection/GetTableForTablePartial",
                    data: { SectionId: sectionid, page: Page, pageSize: PageSize },
                    success: function (data) {
                        $("#TableSection").empty();
                        $("#TableSection").html(data);
                        checkbox = document.getElementsByClassName('TableCheckBox');
                        for (let i = 0; i < checkbox.length; i++) {
                            if (tableMassDelete.includes(checkbox[i].getAttribute("id"))) {
                                checkbox[i].checked = true;
                            }
                        }
                        CheckTableCheckBoxSelectAll();
                    },
                    error: function (data) {
                        console.log(data);
                    }
                })
            }
        }

        function TablePageSizeDropDownControl() {
            let PageSize = $("#TablePageSizeDropDown").val();
            $.ajax({
                type: "GET",
                url: "/TableAndSection/GetTableForTablePartial",
                data: { SectionId: sectionid, pageSize: PageSize },
                success: function (data) {
                    $("#TableSection").empty();
                    $("#TableSection").html(data);
                    checkbox = document.getElementsByClassName('TableCheckBox');
                    for (let i = 0; i < checkbox.length; i++) {
                        if (tableMassDelete.includes(checkbox[i].getAttribute("id"))) {
                            checkbox[i].checked = true;
                        }
                    }
                    CheckTableCheckBoxSelectAll();
                },
                error: function (data) {
                    console.log(data);
                }
            })
        }

        function TableMassDelete(){
            $.ajax({
                type : "POST",
                url : "/TableAndSection/Massdelete",
                data : {TableIds : tableMassDelete},
                success : function(data){
                    if (data.status == true) {
                        notyf.success(`${tableMassDelete.length} Tables Deleted Succesfully`);
                        tableMassDelete = [];
                        getTables(sectionid);
                    }
                    else if(data.access === false){
                        notyf.error("You can't delete table.");
                        tableMassDelete = [];
                        getTables(sectionid);
                    }
                    else {
                        notyf.error("Something Went Wrong");
                        tableMassDelete = [];
                        getTables(sectionid);
                    }
                }
            });
        }

        function GetAllselectedTable(elem){
            let TableId = elem.id;
            if(!tableMassDelete.includes(TableId)){
                tableMassDelete.push(TableId);
            }
            else{
                tableMassDelete.splice(tableMassDelete.indexOf(TableId) , 1);
            }
            CheckTableCheckBoxSelectAll();
        }

        function SelectAllTableForMassDelete(elem){
            if(elem.checked){
                checkbox = document.getElementsByClassName('TableCheckBox');
                for (let i = 0; i < checkbox.length; i++) {
                    if (!tableMassDelete.includes(checkbox[i].id)) {
                        checkbox[i].checked = true;
                        tableMassDelete.push(checkbox[i].id);
                    }
                }
            }
            else{
                checkbox = document.getElementsByClassName('TableCheckBox');
                for (let i = 0; i < checkbox.length; i++) {
                    if (tableMassDelete.includes(checkbox[i].id)){
                        checkbox[i].checked = false;
                        tableMassDelete.splice(tableMassDelete.indexOf(checkbox[i].id) , 1);
                    }
                }
            }
        }
        
        function CheckTableCheckBoxSelectAll(){
            checkbox = document.getElementsByClassName('TableCheckBox');
            let c = false;
            for (let i = 0; i < checkbox.length; i++){
                if(checkbox[i].checked == true){
                    c = true;
                }
                else{
                    c = false;
                    break;
                }
            }
            if(c == true){
                document.getElementById("TableCheckBoxSelectAll").checked = true;
            }
            else{
                document.getElementById("TableCheckBoxSelectAll").checked = false;
            }
        }

        $(document).off("input", "#TableSearchBox").on("input", "#TableSearchBox", function (event) {
            let searchData = $("#TableSearchBox").val().trim();
            $.ajax({
                type: "GET",
                url: "/TableAndSection/GetTableForTablePartial",
                data: { SectionId: sectionid, SearchCriteria: searchData },
                success: function (data) {
                    $("#TableSection").empty();
                    $("#TableSection").html(data);
                    checkbox = document.getElementsByClassName('TableCheckBox');
                    for (let i = 0; i < checkbox.length; i++) {
                        if (tableMassDelete.includes(checkbox[i].getAttribute("id"))) {
                            checkbox[i].checked = true;
                        }
                    }
                    CheckTableCheckBoxSelectAll();
                    $("#TableSearchBox").val(searchData);
                    $("#TableSearchBox").focus();
                },
                error: function (data) {
                    console.log(data);
                }
            });
        })

        $(document).off("submit", "#AddTableModalForm").on("submit", "#AddTableModalForm", function (e) {
            e.preventDefault();
            $("#AddTableModalForm").removeAttr("novalidate");
            $.validator.unobtrusive.parse($("#AddTableModalForm"));
            if (!$("#AddTableModalForm").valid()) {
                return;
            }
            $.ajax({
                type: "POST",
                url: "/TableAndSection/AddTable",
                data: $("#AddTableModalForm").serialize(),
                success: function (data) {
                    if (data.status == true) {
                        notyf.success("Table Added Succesfully");
                    }
                    else if(data.access === false){
                        notyf.error("You can't add table.");
                    }
                    else {
                        notyf.error("Something Went mnj Wrong");
                    }
                    $("#AddTableModal").modal("hide");
                    getTables(sectionid);
                },
                error: function (data) {
                    console.log(data);
                }
            });
        });

        $(document).off("click", "#AddSectionModalSubmit").on("click", "#AddSectionModalSubmit", function (event) {
            event.preventDefault();
            $("#AddSectionForm").removeAttr("novalidate");
            $.validator.unobtrusive.parse($("#AddSectionForm"));
            if (!$("#AddSectionForm").valid()) {
                return;
            }
            $.ajax({
                type: "POST",
                url: "/TableAndSection/AddSection",
                data: $("#AddSectionForm").serialize(),
                success: function (data) {
                    if (data.status == true) {
                        notyf.success("Section Added Succesfully");
                    }
                    else if(data.access === false){
                        notyf.error("You can't add section.");
                    }
                    else {
                        notyf.error("Something Went Wrong");
                    }
                    $("#AddSectionForm")[0].reset();
                    $("#AddSectionModal").modal("hide");
                    GetSectionForPartial();
                },
                error: function (data) {
                    console.log(data);
                }
            });
        })

        $(document).off("click", "#EditSectionModalSubmit").on("click", "#EditSectionModalSubmit", function (event) {
            event.preventDefault();
            $("#EditSectionForm").removeAttr("novalidate");
            $.validator.unobtrusive.parse($("#EditSectionForm"));
            if (!$("#EditSectionForm").valid()) {
                return;
            }
            $.ajax({
                type: "POST",
                url: "/TableAndSection/EditSection",
                data: $("#EditSectionForm").serialize(),
                success: function (data) {
                    if (data.status == true) {
                        notyf.success("Section Edited Succesfully");
                    }
                    else if(data.access === false){
                        notyf.error("You can't edit section.");
                    }
                    else {
                        notyf.error("Something Went Wrong");
                    }
                    $("#EditSectionModal").modal("hide");
                    $("#EditSectionForm")[0].reset();
                    GetSectionForPartial();
                },
                error: function (data) {
                    console.log(data);
                }
            });
        })

        $(document).off("click", "#DeleteSectionModalSubmit").on("click", "#DeleteSectionModalSubmit", function (event) {
            event.preventDefault();
            $.ajax({
                type: "POST",
                url: "/TableAndSection/DeleteSection",
                data: $("#DeleteSectionForm").serialize(),
                success: function (data) {
                    if (data.status == true) {
                        notyf.success("Section Deleted Succesfully");
                        $("#DeleteSectionModal").modal("hide");
                        GetSectionForPartial();
                    }
                    else if(data.access === false){
                        notyf.error("You can't delete section.");
                    }
                    else {
                        notyf.error("Something Went Wrong");
                        $("#DeleteSectionModal").modal("hide");
                        GetSectionForPartial();
                    }
                },
                error: function (data) {
                    console.log(data);
                }
            });
        })

        $(document).off("submit", "#EditTableModalForm").on("submit", "#EditTableModalForm", function (event) {
            event.preventDefault();
            $("#EditTableModalForm").removeAttr("novalidate");
            $.validator.unobtrusive.parse($("#EditTableModalForm"));
            if (!$("#EditTableModalForm").valid()) {
                return;
            }
            $.ajax({
                type: "POST",
                url: "/TableAndSection/EditTable",
                data: $("#EditTableModalForm").serialize(),
                success: function (data) {
                    if (data.status == true) {
                        notyf.success("Table Edited Succesfully");
                        $("#EditTableModal").modal("hide");
                        $("#EditTableModalForm")[0].reset();
                        getTables(sectionid);
                    }
                    else if(data.access === false){
                        notyf.error("You can't edit table.");
                        $("#EditTableModal").modal("hide");
                        $("#EditTableModalForm")[0].reset();
                        getTables(sectionid);
                    }
                    else {
                        notyf.error("Something Went Wrong");
                        $("#EditTableModal").modal("hide");
                        $("#EditTableModalForm")[0].reset();
                        getTables(sectionid);
                    }
                }
            });
        })

        $(document).off("submit", "#DeleteTableForm").on("submit", "#DeleteTableForm", function (event) {
            event.preventDefault();
            $.ajax({
                type: "POST",
                url: "/TableAndSection/DeleteTable",
                data: $("#DeleteTableForm").serialize(),
                success: function (data) {
                    if (data.status == true) {
                        notyf.success("Table Deleted Succesfully");
                        $("#DeleteTableModal").modal("hide");
                        getTables(sectionid);
                    }
                    else if(data.access === false){
                        notyf.error("You can't delete table.");
                        $("#DeleteTableModal").modal("hide");
                        getTables(sectionid);
                    }
                    else {
                        notyf.error("Something Went Wrong");
                        $("#DeleteTableModal").modal("hide");
                        getTables(sectionid);
                    }

                },
                error: function (data) {
                    console.log(data);
                }
            });
        })
