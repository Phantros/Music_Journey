using UnityEngine;

public class MaterialController : MonoBehaviour
{
    // Reference to the object's renderer
    private Renderer objectRenderer;

    // References to the materials you want to control
    public Material material1;
    public Material material2;

    private bool isAimedAt = false; // Flag to track whether the object is aimed at
    
    private void Start()
    {
        // Get the object's renderer
        objectRenderer = GetComponent<Renderer>();
    }

    private void Update()
    { // Create a ray from the center of the screen
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        // Check if the ray hits an object
        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
        {
            // The object is aimed at
            if (!isAimedAt)
            {
                // If it wasn't aimed at previously, switch to material2
                SetMaterial(material2);
                isAimedAt = true;
            }
        }
        else
        {
            // The object is not aimed at
            if (isAimedAt)
            {
                // If it was aimed at previously, switch back to material1
                SetMaterial(material1);
                isAimedAt = false;
            }
        }
    }

    private void SetMaterial(Material newMaterial)
    {
        // Assign the new material to the object's renderer
        objectRenderer.materials = new Material[] { newMaterial };
        Debug.Log("Material" + newMaterial.name);
    }
}

