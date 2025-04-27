        let selectedTableList = [];
        let selectedSection;
        let maxCapacity = 0;
        
        $(".navbar-badges-container span").removeClass('selected_badge');
        $("#OrderAppTableBadge").find('span').addClass('selected_badge');

        function assigntimeTimmer(){
            $(".assigntimecounter").each(function (){
                let assigntimeDateStr = $(this).data("assigntime");
                let assigntimeDate = new Date(assigntimeDateStr).getTime();
                let current = new Date().getTime();
                let diff = current - assigntimeDate;

                let days = Math.floor(diff / (1000 * 60 * 60 * 24));
                let hours = Math.floor((diff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
                let minutes = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));
                let seconds = Math.floor((diff % (1000 * 60)) / 1000);

                $(this).find(".assigntimecountertext").text(`${days} d ${hours} h ${minutes} m ${seconds} s`);
            })
        }
        assigntimeTimmer();
        setInterval(assigntimeTimmer, 1000);

        $(document).off("click" , ".RunningTable").on("click" , ".RunningTable" , function(){
            let OrderId = $(this).data("orderid");
            let CustomerId = $(this).data("customerid");
            window.location.href = "/OrderAppMenu/OrderMenu?orderId=" + OrderId +"&customerId=" + CustomerId;
        })

        $(document).off("click" , ".AssignTable").on("click" , ".AssignTable" , function(){
            let CustomerId = $(this).data("customerid");
            let OrderId = $(this).data("orderid");
            window.location.href = "/OrderAppMenu/OrderMenu?orderId=" + OrderId +"&customerId=" + CustomerId;
        })

        $(document).off("click", ".Available").on("click", ".Available", function () {
            let section_id = $(this).data("sectionid");
            let assignbtn = `#Assign-btn-${section_id}`;
            let tableid = $(this).data("tableid");
            let capacity = $(this).data("tablecapacity");
            if (!selectedTableList.includes(tableid)) {
                $(assignbtn).prop("disabled", false);
                selectedTableList.push(tableid);
                maxCapacity += capacity;
                $(this).find(".avaliable_card").addClass("SelectedTable");
            }
            else {
                $(assignbtn).prop("disabled", true);
                selectedTableList.splice(selectedTableList.indexOf(tableid), 1);
                maxCapacity -= capacity;
                $(this).find(".avaliable_card").removeClass("SelectedTable");
            }
            // @* let assignbtn = `#Assign-btn-${section_id}`;
            // if (selectedTableList.length == 0) {
            //     $(assignbtn).prop("disabled", true);
            // }
            // else {
            //     $(assignbtn).prop("disabled", false);
            // } *@
        })

        $(document).off("click", ".assignbtn").on("click", ".assignbtn", function () {
            let section_id = $(this).data("sectionid");
            selectedSection = section_id;
            $.ajax({
                type: "GET",
                url: "/OrderAppTable/GetOffCanavasWaitingList",
                data: { SectionId: section_id },
                success: function (data) {
                    $("#offcanvaswaitinglist").html(data);
                    $.ajax({
                        type: "GET",
                        url: "/OrderAppTable/GetOffCanvasCustomerDetails",
                        data: { SectionId: section_id },
                        success: function (data) {
                            $("#offcanvascustomerdetail").html(data);
                            $("#AssignWaitigCustomerNoOfPerson").attr({"max" : maxCapacity , "min" : 1});
                        },
                        error : function(data){
                            console.log(data);
                        }
                    })
                }
            })
        })

        $(document).off("input","#AssignWaitigCustomerNoOfPerson").on("input","#AssignWaitigCustomerNoOfPerson" , function(event){
            event.preventDefault();
            let capacity = $(this).val();
            if(capacity > maxCapacity){
                notyf.error(`No of person should not be greates then ${maxCapacity}`);
                return;
            }
        })

        $(document).off("change", ".customerradiobtn").on("change", ".customerradiobtn", function () {
            let customer_id = $(this).data("customerid");
            $.ajax({
                type: "GET",
                url: "/OrderAppTable/GetOffCanvasCustomerDetails",
                data: { CustomerId : customer_id },
                success: function (data) {
                    $("#offcanvascustomerdetail").html(data);
                },
                error : function(data){
                    console.log(data);
                }
            })
        })

        $(document).off("change" , "#AssignWaitingCustomerEmail").on("change" , "#AssignWaitingCustomerEmail" , function(){
            let email = $("#AssignWaitingCustomerEmail").val().trim();
            $.ajax({
                type: "GET",
                url: "/OrderAppTable/GetCustomerDetailFromEmail",
                data: { SectionId: selectedSection , Email : email },
                success: function (data) {
                    $("#offcanvascustomerdetail").html(data);
                },
                error : function(data){
                    console.log(data);
                }
            })
        })

        $(document).off("click" , "#AddWaitingTokenBtn").on("click" , "#AddWaitingTokenBtn" , function(){
            let section_id = $(this).data("sectionid");
            selectedSection = section_id;
            $.ajax({
                type : "GET",
                url : "/OrderAppTable/GetDataForAddWaitingToken",
                data: { SectionId: section_id },
                success : function(data){
                    $("#AddWaitingTokenModal .modal-content").html(data);
                    $("#AddWaitingTokenModal").modal("show");
                }
            });
        })

        $(document).off("change" , "#WaitingTokenCustomerEmail").on("change" , "#WaitingTokenCustomerEmail" , function(){
            let email = $(this).val().trim();
            $.ajax({
                type : "GET",
                url : "/OrderAppTable/GetDataForAddWaitingToken",
                data: { SectionId: selectedSection , Email : email },
                success : function(data){
                    $("#AddWaitingTokenModal .modal-content").html(data);
                }
            });
        })

        $(document).off("click" , "#AddWaitingTokenFormSubmit").on("click" , "#AddWaitingTokenFormSubmit" , function(event){
            event.preventDefault();
            $("#AddWaitingTokenForm").removeAttr("novalidate");
            $.validator.unobtrusive.parse($("#AddWaitingTokenForm"));
            if (!$("#AddWaitingTokenForm").valid()) {
                return;
            }
            $.ajax({
                type : "GET",
                url : "/OrderAppTable/AddCustomerToWaitingList",
                data: $("#AddWaitingTokenForm").serialize(),
                success: function(data){
                    if(data.status == true){
                        notyf.success("Waiting Token Generated Successfully");
                        $("#AddWaitingTokenForm")[0].reset();
                        $("#AddWaitingTokenModal").modal("hide");
                    }
                    else{
                        notyf.error("Something Went Wrong");
                        $("#AddWaitingTokenForm")[0].reset();
                        $("#AddWaitingTokenModal").modal("hide");
                    }
                }
            });
        })

        $(document).off("submit","#CustomerDetailOffCanvasForm").on("submit","#CustomerDetailOffCanvasForm" , function(event){
            event.preventDefault();
            $("#AssignWaitigCustomerNoOfPerson").trigger("input");
            $("#CustomerDetailOffCanvasForm").removeAttr("novalidate");
            $.validator.unobtrusive.parse($("#CustomerDetailOffCanvasForm"));
            if (!$("#CustomerDetailOffCanvasForm").valid()) {
                return;
            }
            let formData = new FormData($("#CustomerDetailOffCanvasForm")[0]);
            formData.append("SelectedTable",JSON.stringify(selectedTableList));
            $.ajax({
                type : "POST",
                url : "/OrderAppTable/AssignTable",
                data : formData,
                contentType: false,
                processData: false,
                success : function(data){
                    if(data.status == true){
                        notyf.success("Table Assigned Successfully");
                        window.location.href = data.redirectUrl;
                    }
                    if(data.status == false){
                        notyf.error("Something Went Wrong");
                        $("#CustomerDetailOffCanvasForm")[0].reset();
                    }
                },
                error : function(data){
                    console.log(data);
                }
            });
        })
