using UnityEngine;

public class NotificationScript : MonoBehaviour {
    private NotificationManager notificationManager;
    public TMPro.TMP_Text notificationText;

    public void SetText(NotificationManager notificationManager, string text) {
        this.notificationManager = notificationManager;
        notificationText.text = text;
        Invoke("DestroyNotification", notificationManager.notificationTimeout);
    }

    void DestroyNotification() {
        notificationManager.notifications.Remove(this);
        Destroy(gameObject);
    }
}