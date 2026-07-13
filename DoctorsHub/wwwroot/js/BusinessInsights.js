//Appointment Status Chart





const appointmentStatusData = window.appointmentStatus;

const statusLabels = appointmentStatusData.map(x => x.Status);
const statusValues = appointmentStatusData.map(x => x.Count);

const ctx = document.getElementById("appointmentStatusChart");

new Chart(ctx,
    {
        type: "doughnut",
        data: {
            labels: statusLabels,
            datasets: [{
                labels: "Appointments",
                data: statusValues
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,

            cutout:"65%"
        }
        
    }
);

//Appointment Trend Chart (Line Chart)


const appointmentTrendData = window.appointmentTrend
console.log(appointmentTrendData);

const trendsLabels = appointmentTrendData.map(l => l.label);
const trendValues = appointmentTrendData.map(v => v.Count);
console.log(trendsLabels);
console.log(trendValues);


const trendChart = document.getElementById("appointmentTrendChart");

new Chart(trendChart, {
    type: "line",
    labels: trendsLabels,
    data: {
        labels: trendsLabels,
        datasets: [{
            label: "Appointments Trend",
            data: trendValues
        }]
    },
    options: {
        responsive: true,
        maintainAspectRatio: false
    }
});


//Appointment By Doctor (Bar Chart)

const appointmentByDoctorData = window.appointmentByDoctors
console.log(appointmentByDoctorData);




const doctorsLabels = appointmentByDoctorData.map(l => l.DoctorName);
const doctorsValues = appointmentByDoctorData.map(v => v.Count);
console.log(doctorsLabels);
console.log(doctorsValues);


const doctorChart = document.getElementById("appointmentsByDoctorChart");

new Chart(doctorChart, {
    type: "bar",
    labels: doctorsLabels,
    data: {
        labels: doctorsLabels,
        datasets: [{
            label: "Appointments Trend",
            data: doctorsValues
        }]
    },
    options: {
        responsive: true,
        maintainAspectRatio: false
    }
});


//Peak Apointment Houe Chart (column chart)

const peakHourData = window.peakAppointmentHours;

console.log(peakHourData);

const peakLabels = peakHourData.map(l => {
    if (l.Hour === 0) return "12 AM";
    if (l.Hour === 12) return "12 PM";
    if (l.Hour > 12) return `${l.Hour - 12} PM`;
    return `${l.Hour} AM`;
});

const peakValues = peakHourData.map(v => v.Count);

console.log(peakLabels);
console.log(peakValues);

const peakHourChart = document.getElementById("peakHoursChart");

new Chart(peakHourChart, {
    type: "bar",

    data: {
        labels: peakLabels,
        datasets: [{
            label: "Peak Appointment Hours",
            data: peakValues
        }]
    },

    options: {
        indexAxis: "y",
        responsive: true,
        maintainAspectRatio: false
    }
});


//Revenue Charts

//Revenue Trend Chart ( Line Chart)
const revenueTrendData = window.revenueTrend;
console.log("Rwevenue Trend", revenueTrendData)

const revenueLabels = revenueTrendData.map(l => l.MonthYear);
const revenueValue = revenueTrendData.map(v => v.Revenue)

console.log("Labels", revenueLabels);
console.log("Values", revenueValue)


const revenueTrendChart = document.getElementById("revenueTrendChart");

new Chart(revenueTrendChart, {
    type: "line",

    labels: revenueLabels,
    data: {
        labels: revenueLabels,
        datasets: [{
            labels: "Revenue trends",
            data: revenueValue
        }]
    },
    options: {
        responsive: true,
        maintainAspectRatio: false
    }
});