using UnityEngine;
using TMPro;
using System.Collections;

public class BlackboardController : MonoBehaviour
{
    public TextMeshProUGUI blackboardText;

    public Color[] fruitColors = { Color.red, Color.green, Color.blue, Color.yellow };
    public string[] fruitNames = { "red", "green", "blue", "yellow" };

    void Start()
    {
        Debug.Log("BlackboardController: Start method called.");

        TableFruitSpawner spawner = FindObjectOfType<TableFruitSpawner>();

        if (spawner != null)
        {
            Debug.Log("BlackboardController: Found TableFruitSpawner.");
            
            // Subscribe to the OnFruitsSpawned event
            spawner.OnFruitsSpawned += InitializeBlackboard;
            Debug.Log("BlackboardController: Subscribed to OnFruitsSpawned event.");
        }
        else
        {
            Debug.LogError("BlackboardController: TableFruitSpawner not found in the scene!");
        }
    }

    // Initializes the blackboard with a random fruit and color
    void InitializeBlackboard()
    {
        Debug.Log("BlackboardController: InitializeBlackboard method called.");

        // Randomly select a color and fruit
        int randomIndex = Random.Range(0, fruitColors.Length);
        string selectedFruitName = fruitNames[randomIndex];
        Color selectedColor = fruitColors[randomIndex];

        Debug.Log($"BlackboardController: Selected fruit - {selectedFruitName}, Color - {selectedColor}");

        // Update the blackboard text
        UpdateBlackboardText($"Find all {selectedFruitName} fruits!", selectedColor);

        // Notify the GameManager
        Debug.Log("BlackboardController: Notifying GameManager about target fruit and color.");
        GameManager.Instance.InitializeGame(selectedColor, selectedFruitName, CountFruitsOfColor(selectedColor));

        Debug.Log("BlackboardController: Blackboard initialization complete.");
    }

    // Updates the blackboard text with a given message and color.
    public void UpdateBlackboardText(string message, Color color)
    {
        Debug.Log($"BlackboardController: UpdateBlackboardText called with message '{message}' and color '{color}'.");

        if (blackboardText != null)
        {
            Debug.Log("BlackboardController: blackboardText is valid. Updating text and color.");
            blackboardText.text = message;
            blackboardText.color = color;
        }
        else
        {
            Debug.LogError("BlackboardController: blackboardText is not assigned in the Inspector!");
        }
    }

    // Counts the number of fruits in the scene that match the target color
    private int CountFruitsOfColor(Color targetColor)
    {
        Debug.Log($"BlackboardController: Counting fruits of color {targetColor}.");

        FruitInteraction[] fruits = FindObjectsOfType<FruitInteraction>();
        Debug.Log($"BlackboardController: Found {fruits.Length} fruits in the scene.");

        int count = 0;
        foreach (FruitInteraction fruit in fruits)
        {
            Renderer renderer = fruit.GetComponent<Renderer>();
            if (renderer == null)
            {
                Debug.LogWarning($"BlackboardController: {fruit.name} is missing a Renderer component. Skipping.");
                continue; // Skip this fruit if the Renderer is missing
            }

            Color fruitColor = renderer.material.color;
            Debug.Log($"BlackboardController: Fruit '{fruit.name}' has color {fruitColor}.");

            if (fruitColor == targetColor)
            {
                count++;
            }
        }

        Debug.Log($"BlackboardController: Total fruits of color {targetColor} = {count}.");
        return count;
    }
}
