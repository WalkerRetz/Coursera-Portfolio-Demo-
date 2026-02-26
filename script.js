// Basic JavaScript for portfolio

// Example: simple greeting in console
console.log('Welcome to my portfolio!');

// You can add interactivity here

document.addEventListener('DOMContentLoaded', () => {
    const toggle = document.getElementById('theme-toggle');
    toggle.addEventListener('click', () => {
        document.body.classList.toggle('dark');
    });
});
