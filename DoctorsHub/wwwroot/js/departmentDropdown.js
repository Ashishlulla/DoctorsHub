document.addEventListener("DOMContentLoaded", function () {

    const dropdown = document.querySelector(".department-dropdown");
    const toggle = document.getElementById("departmentDropdownToggle");
    const menu = document.getElementById("departmentDropdownMenu");
    const selectedText = document.getElementById("departmentSelectedText");

    if (!dropdown || !toggle || !menu || !selectedText) {
        return;
    }

    const checkboxes = menu.querySelectorAll(".department-checkbox");


    // ==========================================
    // OPEN / CLOSE
    // ==========================================

    toggle.addEventListener("click", function (event) {

        event.preventDefault();
        event.stopPropagation();

        const isOpen = menu.style.display === "block";

        if (isOpen) {
            closeDropdown();
        }
        else {
            openDropdown();
        }
    });


    function openDropdown() {

        menu.style.display = "block";
        dropdown.classList.add("open");

    }


    function closeDropdown() {

        menu.style.display = "none";
        dropdown.classList.remove("open");

    }


    // ==========================================
    // CHECKBOXES
    // ==========================================

    checkboxes.forEach(function (checkbox) {

        checkbox.addEventListener("change", function () {

            updateSelectedText();

        });

    });


    // ==========================================
    // SELECTED TEXT
    // ==========================================

    function updateSelectedText() {

        const selected = [];

        checkboxes.forEach(function (checkbox) {

            if (checkbox.checked) {

                const name = checkbox.getAttribute("data-name");

                if (name) {
                    selected.push(name);
                }
            }

        });


        if (selected.length === 0) {

            selectedText.textContent = "Select Departments";

        }
        else if (selected.length <= 2) {

            selectedText.textContent = selected.join(", ");

        }
        else {

            selectedText.textContent =
                selected.length + " Departments Selected";
        }
    }


    // ==========================================
    // CLICK OUTSIDE
    // ==========================================

    document.addEventListener("click", function (event) {

        if (!dropdown.contains(event.target)) {

            closeDropdown();

        }

    });


    // ==========================================
    // INITIAL STATE
    // ==========================================

    closeDropdown();
    updateSelectedText();

});