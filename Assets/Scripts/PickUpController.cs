using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public Transform holdPoint;      // Where the cup will be held
    public float interactRange = 2f; // How far player can "reach"
    private GameObject heldObject;

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
                DropCup();
            }
        }
    }

    void TryPickupCup()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactRange))
        {
            if (hit.collider.CompareTag("Tankard"))
            {
                heldObject = hit.collider.gameObject;
                heldObject.transform.SetParent(holdPoint);
                heldObject.transform.localPosition = Vector3.zero;
                heldObject.transform.localRotation = Quaternion.identity;

                // Disable physics so it doesn’t fall or spin
                Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }
            }
        }
    }

    void DropCup()
    {
        heldObject.transform.SetParent(null);

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        heldObject = null;
    }
}
