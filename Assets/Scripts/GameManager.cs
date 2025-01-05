using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance for easy access across scripts

    public Color targetColor; 
    public string targetFruitName; 
    public int totalFruitsToFind; 
    public int fruitsFound; 

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

  void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("GameManager: Initialized");
    }
    else
    {
        Destroy(gameObject);
    }
}


    public void InitializeGame(Color color, string fruitName, int totalFruits)
    {
        targetColor = color;
        targetFruitName = fruitName;
        totalFruitsToFind = totalFruits;
        fruitsFound = 0;

        Debug.Log($"Game initialized! Target: {fruitName} fruits. Total to find: {totalFruits}");

        // Notify the ScoreManager to update the total fruits
        ScoreManager uiManager = FindObjectOfType<ScoreManager>();
        if (uiManager != null)
        {
            uiManager.InitializeScore(totalFruits); // Trigger UI update
        }
        else
        {
            Debug.LogWarning("ScoreManager not found!");
        }
    }



    public void FruitFound()
    {
        fruitsFound++;
        Debug.Log($"Progress: {fruitsFound}/{totalFruitsToFind} fruits found.");

        if (fruitsFound >= totalFruitsToFind)
        {
            Debug.Log("Congratulations! You found all the fruits!");
            
        }
    }


}
