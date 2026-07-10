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