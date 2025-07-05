using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PickUpController : MonoBehaviour
{
    public Transform holdPoint;
    public float interactRange = 3f;
    public LayerMask interactMask;
    private GameObject heldObject;

    public TextMeshProUGUI serveNotificationText;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
            {
                TryPickupCup();
            }
            else
            {
                TryServeToTableOrDrop();
            }
        }
    }

    void TryPickupCup()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactMask))
        {
            if (hit.collider.CompareTag("Tankard"))
            {
                heldObject = hit.collider.gameObject;
                heldObject.transform.SetParent(holdPoint);
                heldObject.transform.localPosition = Vector3.zero;
                heldObject.transform.localRotation = Quaternion.identity;

                var rb = heldObject.GetComponent<Rigidbody>();
                if (rb) rb.isKinematic = true;
            }
        }
    }

    void TryServeToTableOrDrop()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            if (hit.collider.CompareTag("Table"))
            {
                // Serve to table
                heldObject.transform.SetParent(null);
                heldObject.transform.position = hit.point + Vector3.up * 0.1f;
                heldObject.transform.rotation = Quaternion.identity;

                Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                if (rb) rb.isKinematic = false;

                heldObject = null;

                ShowNotification("Beer served!");
                return;
            }
        }

        // Not a table — just drop the cup
        heldObject.transform.SetParent(null);
        Rigidbody dropRb = heldObject.GetComponent<Rigidbody>();
        if (dropRb) dropRb.isKinematic = false;
        heldObject = null;
    }

    void ShowNotification(string message)
    {
        if (serveNotificationText == null) return;

        StopAllCoroutines(); // In case previous text is still fading
        serveNotificationText.text = message;
        serveNotificationText.color = Color.green;
        serveNotificationText.enabled = true;
        StartCoroutine(HideNotificationAfterDelay(2f));
    }

    System.Collections.IEnumerator HideNotificationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        serveNotificationText.enabled = false;
    }
}
