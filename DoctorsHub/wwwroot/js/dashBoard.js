

const chartData = window.appointmentStatusData;

console.log(chartData);

const labels = chartData.map(l => l.Status);
console.log(labels);
const values = chartData.map(l => l.Count);
console.log(values);


const ctx = document.getElementById("appointmentStatusChart");

new Chart(ctx, {
    type: "pie",
    data: {
        labels: labels,
        datasets: [{ data: values }]
    },
    options: {
        responsive: true,
        maintainAspectRatio: false
    }
});


//Line Chart
const lineChartData = window.monthlyappointments;

const monthNames = [
    "", "Jan", "Feb", "Mar", "Apr", "May", "Jun",
    "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
];

const lineLabels = lineChartData.map(x => monthNames[parseInt(x.Month)]);
const lineValues = lineChartData.map(x => x.Count);

const lineChart = document.getElementById("monthlyAppointmentsChart");

new Chart(lineChart, {
    type: "line",
    data: {
        labels: lineLabels,
        datasets: [{
            label: "Appointments",
            data: lineValues
        }]
    },
    options: {
        responsive: true,
        maintainAspectRatio: false
    }
});

//Bar Chart
const barChartData = window.appointmentsByDoctor;


const barLabels = barChartData.map(x => x.DoctorName);
const barValues = barChartData.map(x => x.Count);

const barChart = document.getElementById("appointmentsByDoctorChart");

new Chart(barChart, {
    type: "bar",
    data: {
        labels: barLabels,
        datasets: [{
            label: "Appointments By Doctors",
            data: barValues
        }]
    },
    options: {
        responsive: true,
        maintainAspectRatio: false
    }
});
