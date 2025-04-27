function sidebarColorChange() {
    let sidebarAnchor = document.getElementsByClassName("sidebar-anchor");
    for (let i = 0; i < sidebarAnchor.length; i++) {
        sidebarAnchor[i].classList.remove("active-sidebar");
        sidebarAnchor[i].querySelector(".sidebar-name").classList.add("text-dark");
        sidebarAnchor[i].querySelector(".sidebar-name").classList.remove("text-primary");
    }
}
sidebarColorChange();
document.getElementById("User-Sidebar").classList.add("active-sidebar");
document.getElementById("User-Sidebar").querySelector(".sidebar-name").classList.remove("text-dark");
document.getElementById("User-Sidebar").querySelector(".sidebar-name").classList.add("text-primary");

$(document).off("change", "#upload-file").on("change", "#upload-file", function (event) {

    let img_path = $("#upload-file").val().split("\\");
    let img = img_path[img_path.length - 1];

    // console.log("upload-file - ", img_path);
    // console.log("upload-file - ", img);

    document.getElementById("add-user-drag-drop").style.display = 'none';
    $("#Add-User-Img-Name").text(img);

    const input = event.target;
    const preview = document.getElementById("add-user-img-preview");

    if (input && input.files[0]) {
        const reader = new FileReader();
        reader.onload = function (e) {
            preview.src = e.target.result;
            preview.style.display = 'block';
        }
        reader.readAsDataURL(input.files[0]);
        document.getElementById("add-user-file-upload").style.display = 'none';
    }
    else {
        preview.src = "#";
        preview.style.display = 'none';
        document.getElementById("add-user-file-upload").style.display = 'block';
    }
})

$(document).ready(function () {
    $("#CountryDropDown").on("change", function () {
        let selectedCountryId = $("#CountryDropDown").val();
        // console.log("selectedCountryId - ", selectedCountryId);
        if (selectedCountryId != null) {
            $.ajax({
                url: "/User/GetStates",
                type: "json",
                data: { Countryid: selectedCountryId },
                success: function (data) {
                    let statedropdown = $("#StateDropDown");
                    $(statedropdown).empty().append('<option selected>select state</option>');
                    $("#CityDropDown").append('<option selected>select city</option>');
                    $.each(data, function (index, States) {
                        statedropdown.append('<option value="' + States.stateid + '">' + States.stateName + '</option>');
                    })
                },
                error: function () {
                    alert('Error!');
                }
            })
        }
        else {
            $("#StateDropDown").html('<option selected>select country</option>');
            $("#CityDropDown").html('<option selected>select state</option>');
        }
    });
    $("#StateDropDown").on("change", function () {
        let selectedStateId = $("#StateDropDown").val();
        // console.log("selectedStateId - ", selectedStateId);
        if (selectedStateId != null) {
            $.ajax({
                url: "/User/GetCities",
                type: "json",
                data: { Stateid: selectedStateId },
                success: function (data) {
                    let citydropdown = $("#CityDropDown");
                    $(citydropdown).empty().append('<option selected>select city</option>');
                    $.each(data, function (index, Cities) {
                        citydropdown.append('<option value="' + Cities.cityid + '">' + Cities.cityName + '</option>');
                    });
                },
                error: function () {
                    alert('Error!');
                }
            })
        }
        else {
            $("#StateDropDown").html('<option selected>select country</option>');
            $("#CityDropDown").html('<option selected>select state</option>');
        }
    });
})
