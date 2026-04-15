document.addEventListener("DOMContentLoaded", function () {

    const authWrapper = document.querySelector('.auth-wrapper');
    const loginTrigger = document.querySelector('.login-trigger');
    const registerTrigger = document.querySelector('.register-trigger');

    if (!authWrapper) return;

    registerTrigger?.addEventListener('click', e => {
        e.preventDefault();
        authWrapper.classList.add('toggled');
    });

    loginTrigger?.addEventListener('click', e => {
        e.preventDefault();
        authWrapper.classList.remove('toggled');
    });

    if (authWrapper.dataset.showLogin === "True") {
        authWrapper.classList.remove('toggled');
    }
});
//
