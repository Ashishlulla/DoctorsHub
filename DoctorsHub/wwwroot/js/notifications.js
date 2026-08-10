document.addEventListener("DOMContentLoaded", function () {

    const notificationBell = document.getElementById("notificationBell");
    const notificationList = document.getElementById("notificationList");
    const notificationBadge = document.getElementById("notificationBadge");

    if (!notificationBell)
        return;

    notificationBell.addEventListener("click", async function () {

        try {

            const response = await fetch("/Notification/Unread");

            if (!response.ok) {
                console.error("Failed to load notifications.");
                return;
            }

            const notifications = await response.json();

            

            console.log("Notifications received:", notifications);

            notificationList.innerHTML = "";

            notificationList.innerHTML = "";

            if (notifications.length === 0) {

                notificationList.innerHTML = `
                    <div class="text-center text-muted py-4">
                        No notifications
                    </div>
                `;

                notificationBadge.classList.add("d-none");

                return;
            }

            notificationBadge.textContent = notifications.length;
            notificationBadge.classList.remove("d-none");

            notifications.forEach(notification => {

                const notificationItem = document.createElement("div");

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
                        ${formatNotificationDate(notification.createdAt)}
                    </div>
                `;

                notificationList.appendChild(notificationItem);
            });

        }
        catch (error) {

            console.error("Error loading notifications:", error);

        }

    });


    function formatNotificationDate(date) {

        if (!date)
            return "";

        return new Date(date).toLocaleString();
    }

});



const markAllAsRead = document.getElementById("markAllAsRead");

if (markAllAsRead) {

    markAllAsRead.addEventListener("click", async function () {

        try {

            const response = await fetch("/Notification/MarkAllAsRead", {
                method: "POST"
            });

            if (!response.ok) {
                console.error("Failed to mark all notifications as read.");
                return;
            }

            // Hide unread badge
            notificationBadge.classList.add("d-none");
            notificationBadge.textContent = "0";

            // Update notification items
            const notificationItems =
                notificationList.querySelectorAll(".notification-item");

            notificationItems.forEach(item => {
                item.classList.remove("notification-unread");
            });

        }
        catch (error) {

            console.error(
                "Error marking all notifications as read:",
                error
            );

        }

    });
}