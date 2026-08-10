document.addEventListener("DOMContentLoaded", function () {

    const notificationBell =
        document.getElementById("notificationBell");

    const notificationList =
        document.getElementById("notificationList");

    const notificationBadge =
        document.getElementById("notificationBadge");

    const markAllAsRead =
        document.getElementById("markAllAsRead");


    // ==========================================
    // Validate notification elements
    // ==========================================

    if (!notificationBell || !notificationBadge)
        return;


    // ==========================================
    // Load unread count when application loads
    // ==========================================

    loadUnreadNotificationCount();


    async function loadUnreadNotificationCount() {

        try {

            const response =
                await fetch("/Notification/Unread");

            if (!response.ok) {

                console.error(
                    "Failed to load unread notifications."
                );

                return;
            }

            const notifications =
                await response.json();

            updateNotificationBadge(notifications.length);

        }
        catch (error) {

            console.error(
                "Error loading unread notifications:",
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
    // Bell click
    // Load notifications into popup
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


                // No unread notifications
                if (notifications.length === 0) {

                    notificationList.innerHTML = `
                        <div class="text-center text-muted py-4">
                            No notifications
                        </div>
                    `;

                    updateNotificationBadge(0);

                    return;
                }


                // Update badge
                updateNotificationBadge(notifications.length);


                // Display notifications
                notifications.forEach(notification => {

                    const notificationItem =
                        document.createElement("div");

                    notificationItem.className =
                        "px-3 py-3 border-bottom notification-item";


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
    // Mark All As Read
    // ==========================================

    if (markAllAsRead) {

        markAllAsRead.addEventListener(
            "click",
            async function () {

                try {

                    const response =
                        await fetch(
                            "~/Notification/MarkAllAsRead",
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


                    // Hide badge
                    updateNotificationBadge(0);


                    // Clear notification popup
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