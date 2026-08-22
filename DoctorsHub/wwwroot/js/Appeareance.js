const appearanceToggle =
    document.getElementById("appearanceToggle");

const appearanceOptions =
    document.getElementById("appearanceOptions");

const appearanceArrow =
    document.getElementById("appearanceArrow");


appearanceToggle.addEventListener("click", function () {

    appearanceOptions.classList.toggle("show");

    appearanceArrow.classList.toggle("rotate");

});