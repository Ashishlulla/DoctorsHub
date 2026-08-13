document.addEventListener("DOMContentLoaded", function () {

    const notificationBell =
        document.getElementById("notificationBell");

    const notificationList =
        document.getElementById("notificationList");

    const notificationBadge =
        document.getElementById("notificationBadge");

    const markAllAsRead =
        document.getElementById("markAllAsRead");


    if (!notificationBell || !notificationBadge)
        return;


    // ==========================================
    // Load unread notification count
    // ==========================================

    loadUnreadNotificationCount();


    async function loadUnreadNotificationCount() {

        try {

            const response =
                await fetch("/Notification/Unread");

            if (!response.ok)
                return;

            const notifications =
                await response.json();

            updateNotificationBadge(notifications.length);

        }
        catch (error) {

            console.error(
                "Error loading notification count:",
                error
            );

        }
    }


    // ==========================================
    // Update notification badge
    // ==========================================

    function updateNotificationBadge(count) {

        if (count === 0) {

            notificationBadge.textContent = "0";
            notificationBadge.classList.add("d-none");

        }
        else {

            notificationBadge.textContent = count;
            notificationBadge.classList.remove("d-none");

        }
    }


    // ==========================================
    // Load notifications when bell is clicked
    // ==========================================

    notificationBell.addEventListener(
        "click",
        async function () {

            try {

                const response =
                    await fetch("/Notification/Unread");

                if (!response.ok) {

                    console.error(
                        "Failed to load notifications."
                    );

                    return;
                }

                const notifications =
                    await response.json();


                console.log(
                    "Notifications received:",
                    notifications
                );


                notificationList.innerHTML = "";


                if (notifications.length === 0) {

                    notificationList.innerHTML = `
                        <div class="text-center text-muted py-4">
                            No notifications
                        </div>
                    `;

                    updateNotificationBadge(0);

                    return;
                }


                updateNotificationBadge(
                    notifications.length
                );


                // ==========================================
                // Create notification items
                // ==========================================

                notifications.forEach(function (notification) {

                    const notificationItem =
                        document.createElement("div");


                    notificationItem.className =
                        "px-3 py-3 border-bottom notification-item";


                    notificationItem.style.cursor =
                        "pointer";


                    // Store IDs directly on the element
                    notificationItem.setAttribute(
                        "data-appointment-id",
                        notification.appointmentId || ""
                    );

                    notificationItem.setAttribute(
                        "data-bill-id",
                        notification.billId || ""
                    );


                    notificationItem.innerHTML = `
                        <div class="fw-semibold">
                            ${notification.title}
                        </div>

                        <div class="text-muted small mt-1">
                            ${notification.message}
                        </div>

                        <div class="text-muted small mt-1">
                            ${formatNotificationDate(
                        notification.createdAt
                    )}
                        </div>
                    `;


                    notificationList.appendChild(
                        notificationItem
                    );

                });

            }
            catch (error) {

                console.error(
                    "Error loading notifications:",
                    error
                );

            }

        }
    );


    // ==========================================
    // Notification Click
    // ==========================================

    notificationList.addEventListener(
        "click",
        function (event) {

            const notificationItem =
                event.target.closest(
                    ".notification-item"
                );


            if (!notificationItem) {

                return;
            }


            const appointmentId =
                notificationItem.getAttribute(
                    "data-appointment-id"
                );


            const billId =
                notificationItem.getAttribute(
                    "data-bill-id"
                );


            console.log(
                "Notification clicked",
                appointmentId,
                billId
            );


            // ==========================================
            // Appointment Notification
            // ==========================================

            if (appointmentId) {

                window.location.href =
                    "/Appointments/Details?id=" +
                    appointmentId;

                return;
            }


            // ==========================================
            // Bill Notification
            // ==========================================

            if (billId) {

                window.location.href =
                    "/Billing/Details?id=" +
                    billId;

                return;
            }

        }
    );


    // ==========================================
    // Mark All As Read
    // ==========================================

    if (markAllAsRead) {

        markAllAsRead.addEventListener(
            "click",
            async function () {

                try {

                    const response =
                        await fetch(
                            "/Notification/MarkAllAsRead",
                            {
                                method: "POST"
                            }
                        );


                    if (!response.ok) {

                        console.error(
                            "Failed to mark all notifications as read."
                        );

                        return;
                    }


                    updateNotificationBadge(0);


                    notificationList.innerHTML = `
                        <div class="text-center text-muted py-4">
                            No notifications
                        </div>
                    `;

                }
                catch (error) {

                    console.error(
                        "Error marking notifications as read:",
                        error
                    );

                }

            }
        );

    }


    // ==========================================
    // Format notification date
    // ==========================================

    function formatNotificationDate(date) {

        if (!date)
            return "";

        return new Date(date).toLocaleString();

    }

});