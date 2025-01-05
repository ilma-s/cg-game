using UnityEngine;
using System.Collections.Generic;

public class FruitInteraction : MonoBehaviour
{
    private Color fruitColor; 
    private static Dictionary<Color, List<string>> fruitColorMap; // Static to avoid redefining

    void Start()
    {
        // Initialize the fruit color map (if not already initialized)
        InitializeFruitColorMap();

        // Determine the fruit's color based on the map
        fruitColor = DetermineFruitColor();
        Debug.Log($"{gameObject.name} assigned color: {fruitColor}");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PerformRaycast();
        }
    }

    private void PerformRaycast()
    {
        // Cast a ray from the center of the screen
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Debug.Log("FruitInteraction: Performing raycast...");

        if (Physics.Raycast(ray, out hit, 100f))
        {
            Debug.Log($"Raycast hit: {hit.collider.gameObject.name}");

            // Check if the clicked object is this fruit
            if (hit.collider.gameObject == gameObject)
            {
                Debug.Log("FruitInteraction: This fruit was clicked.");
                OnSelect();
            }
            else
            {
                Debug.Log($"FruitInteraction: Raycast hit {hit.collider.gameObject.name}, not this fruit.");
            }
        }
        else
        {
            Debug.Log("FruitInteraction: Raycast did not hit anything.");
        }
    }

   public void OnSelect()
{
    Debug.Log($"{gameObject.name} has been selected!");

    MessageController messageController = FindObjectOfType<MessageController>();
    ScoreManager scoreManager = FindObjectOfType<ScoreManager>();

    // Check if this fruit matches the target color
    if (fruitColor == GameManager.Instance.targetColor)
    {
        Debug.Log($"Correct fruit! Color: {fruitColor}");
        GameManager.Instance.FruitFound();

        if (scoreManager != null)
        {
            scoreManager.IncrementFruitCounter(); // Update the counter on the UI
        }

        if (messageController != null) 
        {
            messageController.ShowSuccessMessage(); // Show success
        }

        Destroy(gameObject);
    }
    else
    {
        Debug.Log($"Wrong fruit! Color: {fruitColor}");
        if (messageController != null) 
        {
            messageController.ShowFailureMessage(); // Show failure
        }
    }
}

    private void InitializeFruitColorMap()
    {
        if (fruitColorMap != null) return; 

        fruitColorMap = new Dictionary<Color, List<string>>
        {
            { Color.yellow, new List<string> { "Banana", "Lemon" } },
            { Color.red, new List<string> { "Apple", "Strawberry", "Raspberry" } },
            { new Color(0.0f, 0.0f, 1.0f), new List<string> { "Blackberry" } }, // Blue
            { new Color(0.5f, 0.0f, 0.5f), new List<string> { "Grapes" } }, // Purple
            { Color.green, new List<string> { "Lime", "Pear" } },
            { new Color(1.0f, 0.5f, 0.0f), new List<string> { "Orange" } }, // Orange
            { new Color(1.0f, 0.2f, 0.3f), new List<string> { "Raspberry" } } // Pinkish-red
        };
    }

    private Color DetermineFruitColor()
    {
        foreach (KeyValuePair<Color, List<string>> pair in fruitColorMap)
        {
            foreach (string fruitName in pair.Value)
            {
                if (gameObject.name.Contains(fruitName)) 
                {
                    return pair.Key;
                }
            }
        }

        Debug.LogWarning($"FruitInteraction: Color for {gameObject.name} not found in the FruitColorMap.");
        return Color.clear;
    }
}
