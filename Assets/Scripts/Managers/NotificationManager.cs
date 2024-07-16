using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : MonoBehaviour {
    public GameObject holder;
    public Transform notificationsHolder;
    public RectTransform notificationsHolderRT;
    public GameObject notificationPrefab;
    [HideInInspector] public List<NotificationScript> notifications = new List<NotificationScript>();
    public float notificationTimeout = 5f;

    private void Start() {
        CreateNotification("Welcome to the game!");
    }

    private void Update() {
        notificationsHolderRT.sizeDelta = new Vector2(288, 6 + 38 * notifications.Count);
    }

    public void CreateNotification(string text) {
        GameObject notification = Instantiate(notificationPrefab, notificationsHolder);
        NotificationScript notificationScript = notification.GetComponent<NotificationScript>();
        notificationScript.SetText(this, text);
        notifications.Add(notificationScript);
    }
}

public static class Notifier {
    public static void Notify(string text) {
        GameObject.Find("MANAGER").GetComponent<NotificationManager>().CreateNotification(text);
    }
}