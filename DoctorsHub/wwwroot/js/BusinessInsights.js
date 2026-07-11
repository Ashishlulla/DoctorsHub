//Appointment Status Chart

const appointmentStatusData = window.appointmentStatus;

const labels = appointmentStatusData.map(x => x.Status);
const values = appointmentStatusData.map(x => x.Count);

const ctx = document.getElementById("appointmentStatusChart");

new Chart(ctx,
    {
        type: "doughnut",
        data: {
            labels: labels,
            datasets: [{
                labels: "Appointments",
                data: values
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,

            cutout:"65%"
        }
        
    }
);