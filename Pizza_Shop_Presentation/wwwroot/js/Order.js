

        let sortBy;
        let desc;

        function sidebarColorChange() {
            let sidebarAnchor = document.getElementsByClassName("sidebar-anchor");
            for (let i = 0; i < sidebarAnchor.length; i++) {
                sidebarAnchor[i].classList.remove("active-sidebar");
                sidebarAnchor[i].querySelector(".sidebar-name").classList.add("text-dark");
                sidebarAnchor[i].querySelector(".sidebar-name").classList.remove("text-primary");    
            }
        }
        sidebarColorChange();
        document.getElementById("Orders-Sidebar").classList.add("active-sidebar");
        document.getElementById("Orders-Sidebar").querySelector(".sidebar-name").classList.remove("text-dark");
        document.getElementById("Orders-Sidebar").querySelector(".sidebar-name").classList.add("text-primary");

        function OrderPagginationArrowControl(Page , PageSize , TotalRecord){
            let searchData = $("#OrderSearchBox").val().trim();
            let status = $("#OrderStatusDropDown").val().trim(); 
            let timeData = $("#OrderTimeDropDown").val().trim();
            let fromDate = $("#FromDate").val().trim();
            let toDate = $("#ToDate").val().trim();
            if (Page >= 1 && Page <= TotalRecord) {
                $.ajax({
                    type : "GET",
                    url : "/Orders/GetOrderPartialView",
                    data : {page : Page , pageSize : PageSize , SortBy : sortBy , Desc : desc , SearchCriteria : searchData , Status : status , timeDropDown : timeData , FromDate : fromDate , ToDate : toDate},
                    success : function(data){
                        $("#OrderListTable").empty();
                        $("#OrderListTable").html(data);
                    }
                });
            }
        }

        function DownloadOrderDetails(elem) {
            let orderid = elem.id;
            $.ajax({
                type: "GET",
                url: "/Orders/ExportInvoiceToPDF",
                data: { OrderId: orderid },
                success: function (data) {
                    console.log(data);
                    console.log("fcafvavszdgv");
                    if(data.status == false){
                        notyf.error("Something went wrong");
                    }
                    else{
                        let hiddenDiv = document.createElement('div');
                        hiddenDiv.style.width = "1000px";
                        hiddenDiv.style.position = "absolute";
                        hiddenDiv.style.left = '-9999px';
                        hiddenDiv.innerHTML = data;
                        document.body.appendChild(hiddenDiv);
                        setTimeout(() => {
                            html2canvas(hiddenDiv, { scale: 2 }).then(canvas => {
                                const imgData = canvas.toDataURL('image/png');
                                const { jsPDF } = window.jspdf;
                                const doc = new jsPDF('p', 'mm', 'a4');
        
                                const pageWidth = 210;
                                const pageHeight = 297;
                                const imgWidth = pageWidth;
                                const imgHeight = (canvas.height * imgWidth) / canvas.width;
                                let heightLeft = imgHeight;
                                let position = 0;
        
                                doc.addImage(imgData, 'PNG', 0, position, imgWidth, imgHeight);
                                heightLeft -= pageHeight;
        
                                while (heightLeft > 0) {
                                    position -= pageHeight;
                                    doc.addPage();
                                    doc.addImage(imgData, 'PNG', 0, position, imgWidth, imgHeight);
                                    heightLeft -= pageHeight;
                                }
        
                                doc.save(`invoice_${orderid}.pdf`);
                                document.body.removeChild(hiddenDiv);
                            }).catch(error => {
                                console.log("Error - ", error);
                            })
                        }, 500);
                    }
    
                },
                error: function (data) {
                    setTimeout(() => {
                        console.log(data);
                    },10000);
                    notyf.error("Something went wrong");
                }
            });
        }

        $(document).off("click", "#ExportOrder").on("click", "#ExportOrder", function () {
            let status = $("#OrderStatusDropDown").val().trim();
            let timeData = $("#OrderTimeDropDown").val().trim();
            let searchData = $("#OrderSearchBox").val().trim();
    
            $.ajax({
                type: "GET",
                url: "/Orders/Exports",
                xhrFields: {
                    responseType: 'blob'
                },
                data: { SearchCriteria: searchData, timeDropDown: timeData, Status: status },
                success: function (data, status, xrh) {
                    let fileName = "OrderData.xlsx";
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

        $(document).off("input" , "#OrderSearchBox").on("input" , "#OrderSearchBox" , function(){
            let searchData = $("#OrderSearchBox").val().trim();
            let status = $("#OrderStatusDropDown").val().trim(); 
            let timeData = $("#OrderTimeDropDown").val().trim();
            let fromDate = $("#FromDate").val().trim();
            let toDate = $("#ToDate").val().trim();
            $.ajax({
                type : "GET",
                url : "/Orders/GetOrderPartialView",
                data : {SearchCriteria : searchData , SortBy : sortBy , Desc : desc , Status : status , timeDropDown : timeData , FromDate : fromDate , ToDate : toDate},
                success : function(data){
                    $("#OrderListTable").empty();
                    $("#OrderListTable").html(data);
                    $("#OrderSearchBox").val(searchData);
                    $("#OrderSearchBox").focus();
                }
            });
        })

        $(document).off("change" , "#OrderListPageSizeDropDown").on("change" , "#OrderListPageSizeDropDown" , function(){
            let PageSize = $("#OrderListPageSizeDropDown").val().trim(); 
            $.ajax({
                type : "GET",
                url : "/Orders/GetOrderPartialView",
                data : {pageSize : PageSize , SortBy : sortBy , Desc : desc},
                success : function(data){
                    $("#OrderListTable").empty();
                    $("#OrderListTable").html(data);
                }
            });
        })

        $(document).off("change" , "#OrderStatusDropDown").on("change" , "#OrderStatusDropDown" , function(){
            let status = $("#OrderStatusDropDown").val().trim(); 
            let timeData = $("#OrderTimeDropDown").val().trim();
            let PageSize = $("#OrderListPageSizeDropDown").val().trim(); 
            let searchData = $("#OrderSearchBox").val().trim();
            let fromDate = $("#FromDate").val().trim();
            let toDate = $("#ToDate").val().trim();
            $.ajax({
                type : "GET",
                url : "/Orders/GetOrderPartialView",
                data : {Status : status , SortBy : sortBy , Desc : desc , timeDropDown : timeData , pageSize : PageSize , SearchCriteria : searchData , FromDate : fromDate , ToDate : toDate},
                success : function(data){
                    $("#OrderListTable").empty();
                    $("#OrderListTable").html(data);
                }
            });
        })

        $(document).off("click" , ".ordersortIcon").on("click" , ".ordersortIcon" , function(){
            sortBy = $(this).siblings("span").text().replace('#' , '');
            desc = $(this).hasClass("desc");
            let PageSize = $("#OrderListPageSizeDropDown").val().trim(); 
            $.ajax({
                type : "GET",
                url : "/Orders/GetOrderPartialView",
                data : {SortBy : sortBy , Desc : desc , pageSize : PageSize},
                success : function(data){
                    $("#OrderListTable").empty();
                    $("#OrderListTable").html(data);
                }
            });
        })

        $(document).off("click" , ".OrderAccordsortIcon").on("click" , ".OrderAccordsortIcon" , function(){
            sortby = $("#OrderAccordSortDropDown").val();
            Desc = $(this).hasClass("desc");
            let PageSize = $("#OrderListPageSizeDropDown").val().trim(); 
            if(sortby != ""){
                $.ajax({
                    type : "GET",
                    url : "/Orders/GetOrderPartialView",
                    data : {SortBy : sortby , Desc : Desc , pageSize : PageSize},
                    success : function(data){
                        $("#OrderListTable").empty();
                        $("#OrderListTable").html(data);
                        $("#OrderAccordSortDropDown").val(sortby);
                    }
                });
            }
        })

        $(document).off("change" , "#OrderTimeDropDown").on("change" , "#OrderTimeDropDown" , function(){
            let status = $("#OrderStatusDropDown").val().trim(); 
            let timeData = $("#OrderTimeDropDown").val().trim();
            let PageSize = $("#OrderListPageSizeDropDown").val().trim(); 
            let searchData = $("#OrderSearchBox").val().trim();
            $("#FromDate").val("");
            $("#ToDate").val("");
            $.ajax({
                type : "GET",
                url : "/Orders/GetOrderPartialView",
                data : {timeDropDown : timeData , Status : status , SortBy : sortBy , Desc : desc , pageSize : PageSize , SearchCriteria : searchData},
                success : function(data){
                    $("#OrderListTable").empty();
                    $("#OrderListTable").html(data);
                }
            });
        })

        $(document).off("click" , "#IntervalSerachBtn").on("click" , "#IntervalSerachBtn" , function(){
            let fromDate = $("#FromDate").val().trim();
            let toDate = $("#ToDate").val().trim();
            let status = $("#OrderStatusDropDown").val().trim();
            let PageSize = $("#OrderListPageSizeDropDown").val().trim(); 
            let searchData = $("#OrderSearchBox").val().trim();
            $("#OrderTimeDropDown").val("");
            if(fromDate == ""){
                notyf.error("Select From Date");
                return;
            }
            if(toDate == ""){
                notyf.error("Select To Date");
                return;
            }
            if(Date.parse(fromDate) > Date.parse(toDate)){
                notyf.error("Select Correct Date Range");
                return;
            }
            $.ajax({
                type : "GET",
                url : "/Orders/GetOrderPartialView",
                data : {FromDate : fromDate , ToDate : toDate , SortBy : sortBy , Desc : desc , Status : status , pageSize : PageSize , SearchCriteria : searchData},
                success : function(data){
                    $("#OrderListTable").empty();
                    $("#OrderListTable").html(data);
                }
            });
        })

        $(document).off("click" , "#OrderFormBtn").on("click" , "#OrderFormBtn" , function(event){
            event.preventDefault();
            let status = false;
            $("#orderForm").find(':input').each(function(){
                if($(this).val()){
                    // console.log($(this).val());
                    status = true;
                    // console.log(status);
                    return false;
                }
            })
            if(status == false){
                return;
            }
            $("#OrderTimeDropDown").val("");
            $("#FromDate").val("");
            $("#ToDate").val("");
            $("#OrderStatusDropDown").val("");
            $("#OrderSearchBox").val("");
            $.ajax({
                type : "GET",
                url : "/Orders/GetOrderPartialView",
                success : function(data){
                    $("#OrderListTable").empty();
                    $("#OrderListTable").html(data);
                }
            });
        })
    