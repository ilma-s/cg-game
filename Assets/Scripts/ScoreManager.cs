using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI fruitCounterText;
    private int fruitsFound = 0;
    private int totalFruits;

    public void InitializeScore(int totalFruits)
    {
        this.totalFruits = totalFruits;
        fruitsFound = 0; // Reset fruits found
        UpdateFruitCounter();
    }

    public void IncrementFruitCounter()
    {
        fruitsFound++;
        UpdateFruitCounter();
    }

    private void UpdateFruitCounter()
    {
        if (fruitCounterText != null)
        {
            fruitCounterText.text = $"Fruits Found: {fruitsFound}/{totalFruits}";
        }
        else
        {
            Debug.LogError("FruitCounter Text is not assigned!");
        }
    }
}
