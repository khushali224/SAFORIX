document.querySelectorAll('.stat-card').forEach(card => {
    card.addEventListener('mousemove', e => {
        card.style.boxShadow =
            `0 0 40px rgba(0,229,255,0.6)`;
    });

    card.addEventListener('mouseleave', e => {
        card.style.boxShadow = '';
    });
});
