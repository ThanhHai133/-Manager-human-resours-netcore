const hamBurger = document.querySelector(".toggle-btn");

hamBurger.addEventListener("click", function () {
    document.querySelector("#sidebar").classList.toggle("expand");
});
document.querySelector('.toggle-btn').addEventListener('click', function () {
    const icon = document.getElementById('toggle-icon');
    if (icon.textContent === 'toggle_off') {
        icon.textContent = 'toggle_on'; // Thay đổi thành toggle_off
    } else {
        icon.textContent = 'toggle_off'; // Thay đổi lại thành toggle_on
    }
});
