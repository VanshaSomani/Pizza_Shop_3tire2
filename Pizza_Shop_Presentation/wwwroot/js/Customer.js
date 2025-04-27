
        let fromDate;
        let toDate;

        function sidebarColorChange() {
            let sidebarAnchor = document.getElementsByClassName("sidebar-anchor");
            for (let i = 0; i < sidebarAnchor.length; i++) {
                sidebarAnchor[i].classList.remove("active-sidebar");
                sidebarAnchor[i].querySelector(".sidebar-name").classList.add("text-dark");
                sidebarAnchor[i].querySelector(".sidebar-name").classList.remove("text-primary");  
            }
        }
        sidebarColorChange();
        document.getElementById("Customer-Sidebar").classList.add("active-sidebar");
        document.getElementById("Customer-Sidebar").querySelector(".sidebar-name").classList.remove("text-dark");
        document.getElementById("Customer-Sidebar").querySelector(".sidebar-name").classList.add("text-primary");
        

        $("#StartDateCustomer").attr("max", new Date().toISOString().split('T')[0]);
        $("#EndDateCustomer").attr("max", new Date().toISOString().split('T')[0]);
        
        function CustomerPagginationArrowControl(Page, PageSize, TotalRecord) {
            let searchData = $("#CustomerSearchBox").val().trim();
            let timeData = $("#CustomerTimeDropDown").val().trim();
            if (Page >= 1 && Page <= TotalRecord) {
                $.ajax({
                    type: "GET",
                    url: "/Customers/GetCustomerForPartialList",
                    data: { page: Page, pageSize: PageSize, SearchCriteria: searchData, timeDropDown: timeData },
                    success: function (data) {
                        $("#CustomerListTable").empty();
                        $("#CustomerListTable").html(data);
                    }
                });
            }
        }

        function OpenCustomDateModal() {
            // console.log("sfazfsasfaSf");
            $("#CustomerCustomDateRange").modal("show");
        }

        function OpenCustomerHistoryModal(elem) {
            let customerId = elem.id;
            $.ajax({
                type : "GET",
                url : "/Customers/CustomerHistory",
                data : {CustomerId : customerId},
                success : function(data){
                    if(data.status == false){
                        notyf.error("Customer not valid");
                        $("#CustomerHistoryModal").modal("hide");
                    }
                    else{
                        $("#CustomerHistoryModal .modal-content").empty();
                        $("#CustomerHistoryModal .modal-content").html(data);
                        $("#CustomerHistoryModal").modal("show");
                    }
                }
            });
        }

        $(document).off("change", "#CustomerListPageSizeDropDown").on("change", "#CustomerListPageSizeDropDown", function () {
            let PageSize = $("#CustomerListPageSizeDropDown").val().trim();
            let searchData = $("#CustomerSearchBox").val().trim();
            let timeData = $("#CustomerTimeDropDown").val().trim();
            $.ajax({
                type: "GET",
                url: "/Customers/GetCustomerForPartialList",
                data: { pageSize: PageSize, SearchCriteria: searchData, timeDropDown: timeData },
                success: function (data) {
                    $("#CustomerListTable").empty();
                    $("#CustomerListTable").html(data);
                }
            });
        })

        $(document).off("input", "#CustomerSearchBox").on("input", "#CustomerSearchBox", function () {
            let searchData = $("#CustomerSearchBox").val().trim();
            let PageSize = $("#CustomerListPageSizeDropDown").val().trim();
            let timeData = $("#CustomerTimeDropDown").val().trim();
            $.ajax({
                type: "GET",
                url: "/Customers/GetCustomerForPartialList",
                data: { SearchCriteria: searchData, pageSize: PageSize, timeDropDown: timeData, FromDate: fromDate, ToDate: toDate },
                success: function (data) {
                    $("#CustomerListTable").empty();
                    $("#CustomerListTable").html(data);
                }
            });
        })

        $(document).off("change", "#StartDateCustomer").on("change", "#StartDateCustomer", function () {
            $("#EndDateCustomer").prop("disabled", false);
            $('#EndDateCustomer').val(today);
            $("#EndDateCustomer").attr("min", $("#StartDateCustomer").val());
        })

        $(document).off("change", "#CustomerTimeDropDown").on("change", "#CustomerTimeDropDown", function (e) {
            let timeData = $("#CustomerTimeDropDown").val().trim();
            let PageSize = $("#CustomerListPageSizeDropDown").val().trim();
            let searchData = $("#CustomerSearchBox").val().trim();
            // console.log(e.currentTarget.value);
            if (timeData == "CustomDate") {
                $("#CustomerCustomDateRange").modal("show");
                return;
            }
            $.ajax({
                type: "GET",
                url: "/Customers/GetCustomerForPartialList",
                data: { timeDropDown: timeData, pageSize: PageSize, SearchCriteria: searchData },
                success: function (data) {
                    $("#CustomerListTable").empty();
                    $("#CustomerListTable").html(data);
                }
            });
        })

        $(document).off("click", "#CustomerDateRangeFormSubmit").on("click", "#CustomerDateRangeFormSubmit", function (e) {
            e.preventDefault();
            fromDate = $("#StartDateCustomer").val().trim();
            toDate = $("#EndDateCustomer").val().trim();
            let searchData = $("#CustomerSearchBox").val().trim();
            let PageSize = $("#CustomerListPageSizeDropDown").val().trim();
            $.ajax({
                type: "GET",
                url: "/Customers/GetCustomerForPartialList",
                data: { FromDate: fromDate, ToDate: toDate, pageSize: PageSize, SearchCriteria: searchData },
                success: function (data) {
                    $("#CustomerListTable").empty();
                    $("#CustomerListTable").html(data);
                    $("#CustomerCustomDateRange").modal("hide");
                    $("#CustomerDateRangeForm")[0].reset();
                }
            });
        })

        $(document).off("click", ".CustomerDateRangeFormCancle").on("click", ".CustomerDateRangeFormCancle", function (e) {
            event.preventDefault();
            $("#CustomerTimeDropDown").val("");
            $("#CustomerTimeDropDown").trigger("change");
            $("#CustomerDateRangeForm")[0].reset();
        })
    
        $(document).off("click" , ".customersortIcon").on("click" , ".customersortIcon" , function(){
            sortBy = $(this).siblings("span").text();
            desc = $(this).hasClass("desc");
            let searchData = $("#CustomerSearchBox").val().trim();
            let PageSize = $("#CustomerListPageSizeDropDown").val().trim();
            let timeData = $("#CustomerTimeDropDown").val().trim();
            $.ajax({
                type: "GET",
                url: "/Customers/GetCustomerForPartialList",
                data: { SortBy: sortBy, Desc: desc, pageSize: PageSize , SearchCriteria: searchData , timeDropDown: timeData},
                success: function (data) {
                    $("#CustomerListTable").empty();
                    $("#CustomerListTable").html(data);
                    $("#CustomerCustomDateRange").modal("hide");
                    $("#CustomerDateRangeForm")[0].reset();
                }
            });
        })
    
        $(document).off("click", "#ExportCustomer").on("click", "#ExportCustomer", function () {
            let timeData = $("#CustomerTimeDropDown").val().trim();
            let searchData = $("#CustomerSearchBox").val().trim();
            $.ajax({
                type: "GET",
                url: "/Customers/Exports",
                xhrFields: {
                    responseType: 'blob'
                },
                data: { SearchCriteria: searchData, timeDropDown: timeData, FromDate: fromDate, ToDate: toDate},
                success: function (data, status, xrh) {
                    // console.log(data);
                    let fileName = "CustomerData.xlsx";
                    let blob = new Blob([data], { type: "application/vnd.openxmlformats-officedocument.spreadsheetsml.sheet" });
                    let link = document.createElement('a');
                    link.href = window.URL.createObjectURL(blob);
                    link.download = fileName;
                    document.body.appendChild(link);
                    link.click();
                    document.body.removeChild(link);
                    notyf.success("File Downloded Succesfully");
                },
                error: function (data) {
                    console.log(data);
                }
            });
        })

