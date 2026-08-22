function updateDateTime() {

    const now = new Date();

    const options = {
        weekday: 'long',
        day: '2-digit',
        month: 'long',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit',
        hour12: true
    };

    const formatted = now.toLocaleString('en-IN', options);

    const parts = formatted.split(' at ');

    document.getElementById("currentDateTime").textContent =
        `🗓️ ${parts[0]} | 🕐 ${parts[1]}`;
}

updateDateTime();

// Update every minute — no seconds displayed
setInterval(updateDateTime, 60000);