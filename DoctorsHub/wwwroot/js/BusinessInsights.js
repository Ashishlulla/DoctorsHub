//Appointment Status Chart


const theme = {
    primary: "#0F766E",
    secondary: "#14B8A6",
    light: "#5EEAD4",
    pale: "#CCFBF1",
    dark: "#115E59",
    grid: "#D1FAE5"
};


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
                label: "Appointments",
                data: statusValues,
                backgroundColor: [
                    theme.primary,
                    theme.secondary,
                    theme.light,
                    "#99F6E4",
                    "#2DD4BF"
                ],
                borderWidth: 2,
                borderColor: "#fff"
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
            label: "Appointment Trend",
            data: trendValues,
            borderColor: theme.primary,
            backgroundColor: "rgba(20,184,166,0.20)",
            fill: true,
            tension: 0.4,
            pointBackgroundColor: theme.primary,
            pointRadius: 4
        }]
    },
    options: {
        responsive: true,
        maintainAspectRatio: false,
        scales: {
            x: {
                grid: { color: theme.grid },
                ticks: { color: theme.dark }
            },
            y: {
                grid: { color: theme.grid },
                ticks: { color: theme.dark }
            }
        }
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
            label: "Appointments",
            data: doctorsValues,
            backgroundColor: theme.secondary,
            borderColor: theme.primary,
            borderWidth: 1,
            borderRadius: 8
        }]
    },
    options: {
        responsive: true,
        maintainAspectRatio: false
    }
});


//Peak Apointment Hour Chart (column chart)

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
            data: peakValues,
            backgroundColor: theme.secondary,
            borderColor: theme.primary,
            borderWidth: 1,
            borderRadius: 8
        }]
    },

    options: {
        indexAxis: "y",
        responsive: true,
        maintainAspectRatio: false,
        scales: {
            x: {
                grid: { color: theme.grid },
                ticks: { color: theme.dark }
            },
            y: {
                grid: { display: false },
                ticks: { color: theme.dark }
            }
        }
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
            label: "Revenue",
            data: revenueValue,
            borderColor: theme.primary,
            backgroundColor: "rgba(20,184,166,0.20)",
            fill: true,
            tension: 0.4,
            pointRadius: 4,
            pointBackgroundColor: theme.primary
        }]
    },
    options: {
        responsive: true,
        maintainAspectRatio: false
    }
});


//Revenue By Doctors (Vertical bar chart)

const revenueByDoctorData = window.revenueByDoctor;

console.log(revenueByDoctorData);

const revenueByLabels = revenueByDoctorData.map(l => l.DoctorName);

const revenueByValues = revenueByDoctorData.map(v => v.Revenue);

console.log("Revenue By Doctor Label", revenueByLabels);
console.log("Revenue By Doctor values", revenueByValues);

const revenueByDoctorChart = document.getElementById("revenueByDoctorChart");

new Chart(revenueByDoctorChart, {
    type: "bar",

    data: {
        labels: revenueByLabels,
        datasets: [{
            label: "Revenue By Doctors",
            data: revenueByValues,
            backgroundColor: theme.secondary,
            borderColor: theme.primary,
            borderWidth: 1,
            borderRadius: 8
        }]
    },

    options: {
        indexAxis: "y",
        responsive: true,
        maintainAspectRatio: false
    }
});

//Average Revenue By Doctor (Vertical Bar Chart)

const averageRevenueByDoctorData = window.averageRevenueByDoctor;

console.log(averageRevenueByDoctorData);

const averageRevenueByLabels = averageRevenueByDoctorData.map(l => l.DoctorName);

const averageRevenueByValues = averageRevenueByDoctorData.map(v => v.AverageRevenue);

console.log("Revenue By Doctor Label", averageRevenueByLabels);
console.log("Revenue By Doctor values", averageRevenueByValues);

const averageRevenueByDoctorChart = document.getElementById("averageRevenueChart");

new Chart(averageRevenueByDoctorChart, {
    type: "bar",

    data: {
        labels: averageRevenueByLabels,
        datasets: [{
            label: "Average Revenue",
            data: averageRevenueByValues,
            backgroundColor: theme.secondary,
            borderColor: theme.primary,
            borderWidth: 1,
            borderRadius: 8
        }]
    },

    options: {
        indexAxis: "y",
        responsive: true,
        maintainAspectRatio: false
    }
});

// Top Revenue Generating Doctors

const topRevenueByDoctorData = window.topRevenueGeneratedByDoctor;

console.log(topRevenueByDoctorData);

const topRevenueByLabels = topRevenueByDoctorData.map(l => l.DoctorName);

const topRevenueByValues = topRevenueByDoctorData.map(v => v.RevenueGenerated);

console.log("Revenue By Doctor Label", topRevenueByLabels);
console.log("Revenue By Doctor values", topRevenueByValues);

const toRevenueByDoctorChart = document.getElementById("topRevenueDoctorsChart");

new Chart(toRevenueByDoctorChart, {
    type: "bar",

    data: {
        labels: topRevenueByLabels,
        datasets: [{
            label: "Top Revenue Generating Doctors",
            data: topRevenueByValues,
            backgroundColor: theme.primary,
            borderColor: theme.dark,
            borderWidth: 1,
            borderRadius: 8
        }]
    },

    options: {
        
        responsive: true,
        maintainAspectRatio: false
    }
});