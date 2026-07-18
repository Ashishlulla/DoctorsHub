document.addEventListener("DOMContentLoaded", () => {

    const profileBtn = document.getElementById("userProfileBtn");
    const dropdown = document.getElementById("userDropdownMenu");

    if (!profileBtn || !dropdown)
        return;

    profileBtn.addEventListener("click", function (e) {

        e.stopPropagation();

        dropdown.classList.toggle("show");

    });

    document.addEventListener("click", function () {

        dropdown.classList.remove("show");

    });

    dropdown.addEventListener("click", function (e) {

        e.stopPropagation();

    });

});


//Sidebar


const sidebar = document.getElementById("sidebar");
const btn = document.getElementById("sidebarToggle");

btn.addEventListener("click", () => {

    sidebar.classList.toggle("collapsed");

    localStorage.setItem(
        "sidebar",
        sidebar.classList.contains("collapsed")
    );

});

if (localStorage.getItem("sidebar") === "true") {

    sidebar.classList.add("collapsed");

}