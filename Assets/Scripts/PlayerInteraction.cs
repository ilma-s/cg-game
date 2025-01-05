using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float raycastDistance = 100.0f; // Maximum distance for raycast
    private GameObject currentTarget;    // The object currently being targeted
    private int layerMask;               // Layer mask for ignoring specific layers

    void Start()
    {
        // Unlock and make the cursor visible for interactions
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("PlayerInteraction: Cursor unlocked and made visible.");

        // Create a layer mask to ignore Bowl, Hamper, and Table layers
        layerMask = ~LayerMask.GetMask("Bowl", "Hamper", "Table");
        Debug.Log($"PlayerInteraction: Ignoring layers: {layerMask}");
    }

    void Update()
    {
        HandleRaycast();

        if (Input.GetMouseButtonDown(0)) 
        {
            Debug.Log("PlayerInteraction: Left mouse button clicked.");
            HandleClick();

            // Ensure the cursor remains visible and unlocked after clicking
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("PlayerInteraction: Ensured cursor remains visible after click.");
        }
    }

    void HandleRaycast()
    {
        // Cast a ray from the center of the screen
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        Debug.Log($"Casting ray from center of screen with distance {raycastDistance}.");

        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, layerMask)) // Apply the layer mask
        {
            GameObject target = hit.collider.gameObject;

            // Log what the raycast is hitting
            Debug.Log($"Raycast hit: {target.name}");

            // Update the current target
            if (target != currentTarget)
            {
                currentTarget = target;
                Debug.Log($"Targeting {currentTarget.name}.");
            }
        }
        else
        {
            Debug.Log("Raycast did not hit anything.");
            currentTarget = null;
        }
    }

    void HandleClick()
    {
        if (currentTarget != null)
        {
            Debug.Log($"PlayerInteraction: Attempting to interact with {currentTarget.name}.");
            // Check if the target object has a FruitInteraction script
            FruitInteraction fruit = currentTarget.GetComponent<FruitInteraction>();
            if (fruit != null)
            {
                Debug.Log($"PlayerInteraction: {currentTarget.name} has a FruitInteraction component. Triggering interaction.");
                fruit.OnSelect(); // Trigger the interaction
            }
            else
            {
                Debug.LogWarning($"PlayerInteraction: Clicked on {currentTarget.name}, but it has no FruitInteraction component.");
            }
        }
        else
        {
            Debug.LogWarning("PlayerInteraction: No valid target to interact with.");
        }
    }
}
