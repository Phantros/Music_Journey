using System.Collections;
using UnityEngine;

public class LayerSwitch : MonoBehaviour
{
    public Collider flowerCollider;
    public Collider potCollider;
    public Transform flower;
    public Transform pot;
    public string InteractableLayer = "Interactable"; // Change this to the desired layer name
    public string HighlightLayer = "Highlight"; // Change this to the desired layer name

    private int originalLayer1;
    private int originalLayer2;
    private Collider lastHitCollider;

    private void Start()
    {
        // Store the original layers of the child objects
        originalLayer1 = flower.gameObject.layer;
        originalLayer2 = pot.gameObject.layer;
    }

    private void Update()
    {
        // Create a ray from the camera
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider == flowerCollider)
            {
                Debug.Log("hit flower");
                // Change the layer of the first child object
                SetLayerRecursively(hit.collider.gameObject, "Highlight");
                SetLayerRecursively(pot.gameObject, "Interactable");
                lastHitCollider = flowerCollider;
            }
            else if (hit.collider == potCollider)
            {
                Debug.Log("hit pot");
                // Change the layer of the second child object
                SetLayerRecursively(hit.collider.gameObject, "Highlight");
                SetLayerRecursively(flower.gameObject, "Interactable");
                lastHitCollider = potCollider;
            }
            else
            {
                // If neither collider1 nor collider2 is hit, revert both child objects' layers
                SetLayerRecursively(pot.gameObject, "Interactable");
                SetLayerRecursively(flower.gameObject, "Interactable");
                lastHitCollider = null;
            }
        }
        else
        {
            // If the raycast no longer hits anything, revert both child objects' layers
            SetLayerRecursively(pot.gameObject, "Interactable");
            SetLayerRecursively(flower.gameObject, "Interactable");
            lastHitCollider = null;
        }

        // Handle the case when the ray stops hitting
        if (lastHitCollider != null && !Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            SetLayerRecursively(pot.gameObject, "Interactable");
            SetLayerRecursively(flower.gameObject, "Interactable");
            lastHitCollider = null;
        }
    }

    private void SetLayerRecursively(GameObject obj, string layerName)
    {
        obj.layer = LayerMask.NameToLayer(layerName);
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layerName);
        }
    }
}
