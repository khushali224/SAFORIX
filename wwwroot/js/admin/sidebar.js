document.addEventListener("DOMContentLoaded", () => {

    const sidebar = document.getElementById("sidebar");
    const main = document.getElementById("mainContent");
    const toggle = document.getElementById("toggleSidebar");

    toggle.addEventListener("click", () => {
        sidebar.classList.toggle("collapsed");
        main.classList.toggle("collapsed");
    });

    document.querySelectorAll(".has-submenu > a").forEach(link => {
        link.addEventListener("click", () => {
            link.parentElement.classList.toggle("open");
        });
    });

});
