using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : MonoBehaviour {
    public GameObject holder;
    public Transform notificationsHolder;
    public GameObject notificationPrefab;
    [HideInInspector] public List<NotificationScript> notifications = new List<NotificationScript>();
    public float notificationTimeout = 5f;
    
    private void Update() {
        if (notifications.Count > 0) {
            holder.SetActive(true);
        } else {
            holder.SetActive(false);
        }
    }

    public void CreateNotification(string text) {
        GameObject notification = Instantiate(notificationPrefab, notificationsHolder);
        NotificationScript notificationScript = notification.GetComponent<NotificationScript>();
        notificationScript.SetText(this, text);
        notifications.Add(notificationScript);
    }
}