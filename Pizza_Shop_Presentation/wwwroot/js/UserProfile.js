$(document).ready(function(){

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

    function loadStates(CountryId){
        if(CountryId != null){
            $.ajax({
                 url : "/User/GetStates",
                 type : "GET",
                 data : {Countryid : CountryId},
                 success : function(data){
                    let StateDropDown = $("#stateDropDown");
                    $(StateDropDown).empty().append('<option selected>select state</option>');
                    $("#cityDropDown").append('<option selected>select city</option>');
                    $.each(data , function(index , States){
                        StateDropDown.append('<option value="'+States.stateid + '">' + States.stateName + '</option>');
                    })
                 },
                 error : function(){
                    alert("Error");
                 }
            })
        }
    }

    function loadCities(StateId){
        if(StateId != null){
            $.ajax({
                 url : "/User/GetCities",
                 type : "GET",
                 data : {Stateid : StateId},
                 success : function(data){
                    let CityDropDown = $("#cityDropDown");
                    $(CityDropDown).empty().append('<option selected>select city</option>');
                    $.each(data , function(index , Cities){
                        CityDropDown.append('<option value="'+Cities.cityid + '">' + Cities.cityName + '</option>');
                    })
                 },
                 error : function(){
                    alert("Error");
                 }
            })
        }
    }

    // @* loadStates(predefinedCountryId); *@

    // @* loadCities(predefinedStateId); *@

    $("#countryDropDown").on("change",function(){
        let selectedCountryId = $("#countryDropDown").val();
        $("#stateDropDown").empty();
        $("#cityDropDown").empty();
        loadStates(selectedCountryId);
    }),

    $("#stateDropDown").on("change",function(){
        let selectedStateId = $("#stateDropDown").val();
        $("#cityDropDown").empty();
        loadCities(selectedStateId);
    })

})