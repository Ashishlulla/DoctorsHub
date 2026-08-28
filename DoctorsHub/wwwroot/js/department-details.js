document.addEventListener("DOMContentLoaded", function () {

    const searchInput =
        document.getElementById("doctorSearch");

    const searchButton =
        document.getElementById("searchDoctorButton");

    const tableBody =
        document.getElementById("doctorTableBody");


    if (!searchInput || !searchButton || !tableBody)
        return;


    function searchDoctors() {

        const searchText =
            searchInput.value.toLowerCase().trim();

        const rows =
            tableBody.querySelectorAll("tr");


        rows.forEach(function (row) {

            const doctorName =
                row.cells[1]?.textContent
                    .toLowerCase()
                    .trim();

            const specialization =
                row.cells[2]?.textContent
                    .toLowerCase()
                    .trim();

            const qualification =
                row.cells[3]?.textContent
                    .toLowerCase()
                    .trim();


            const matches =
                doctorName.includes(searchText) ||
                specialization.includes(searchText) ||
                qualification.includes(searchText);


            row.style.display =
                matches ? "" : "none";

        });

    }


    // Search button
    searchButton.addEventListener(
        "click",
        searchDoctors
    );


    // Search on Enter
    searchInput.addEventListener(
        "keydown",
        function (event) {

            if (event.key === "Enter") {

                event.preventDefault();

                searchDoctors();

            }

        }
    );

});