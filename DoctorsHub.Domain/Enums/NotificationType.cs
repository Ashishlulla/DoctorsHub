using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Domain.Enums
{
    public enum NotificationType
    {
        AppointmentConfirmed = 1,
        AppointmentDelayed = 2,
        AppointmentCancelled = 3,
        AppointmentCompleted = 4,
        BillPaid = 5
    }
}
