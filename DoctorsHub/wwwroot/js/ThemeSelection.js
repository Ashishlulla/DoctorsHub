const themeOptions =
    document.querySelectorAll('input[name="theme"]');


themeOptions.forEach(option => {

    option.addEventListener("change", function () {

        localStorage.setItem(
            "doctorshub-theme",
            this.value
        );

    });

});