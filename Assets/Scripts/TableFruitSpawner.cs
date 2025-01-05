using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TableFruitSpawner : MonoBehaviour
{
    public GameObject bananaPrefab;
    public GameObject applePrefab;
    public GameObject blackberryPrefab;
    public GameObject grapesPrefab;
    public GameObject limePrefab;
    public GameObject lemonPrefab;
    public GameObject orangePrefab;
    public GameObject pearPrefab;
    public GameObject raspberryPrefab;
    public GameObject strawberryPrefab;

    public Transform[] spawnPoints;  // Array of spawn points
    public LayerMask tableLayer;     // Layer for the table surface

    public int targetFruitsToSpawn = 4;     
    public int distractorFruitsToSpawn = 4;  
    public Action OnFruitsSpawned;           
    private Color targetColor;             

    private Dictionary<Color, List<GameObject>> fruitColorMap; 

    void Start()
    {
        InitializeFruitColorMap();
        SpawnFruitsWithTargetColor();
        StartCoroutine(InvokeOnFruitsSpawnedAfterDelay());
    }

    void InitializeFruitColorMap()
    {
        // Initialize the dictionary mapping colors to fruit prefabs
        fruitColorMap = new Dictionary<Color, List<GameObject>>
        {
            { Color.yellow, new List<GameObject> { bananaPrefab, lemonPrefab } },
            { Color.red, new List<GameObject> { applePrefab, strawberryPrefab, raspberryPrefab } },
            { new Color(0.0f, 0.0f, 1.0f), new List<GameObject> { blackberryPrefab } }, // Blue
            { new Color(0.5f, 0.0f, 0.5f), new List<GameObject> { grapesPrefab } }, // Purple
            { Color.green, new List<GameObject> { limePrefab, pearPrefab } },
            { new Color(1.0f, 0.5f, 0.0f), new List<GameObject> { orangePrefab } }, // Orange
            { new Color(1.0f, 0.2f, 0.3f), new List<GameObject> { raspberryPrefab } } // Pinkish-red
        };
    }

    IEnumerator InvokeOnFruitsSpawnedAfterDelay()
    {
        yield return null; // Wait until the end of the current frame
        OnFruitsSpawned?.Invoke();
        Debug.Log("All fruits have been spawned!");
    }
void SpawnFruitsWithTargetColor()
{
    Debug.Log("TableFruitSpawner: Starting to spawn fruits...");

    // Shuffle spawn points for randomness
    Transform[] shuffledSpawnPoints = ShuffleSpawnPoints(spawnPoints);

    // Select a target color that exists in the map
    targetColor = EnsureTargetColorHasFruit();

    // Get all fruits for the target color
    List<GameObject> targetFruits = fruitColorMap[targetColor];

    // Spawn target fruits (ensure exactly 4 fruits)
    int targetFruitsSpawned = 0;
    for (int i = 0; i < targetFruitsToSpawn && i < shuffledSpawnPoints.Length; i++)
    {
        Transform spawnPoint = shuffledSpawnPoints[i];
        GameObject targetFruit = targetFruits[targetFruitsSpawned % targetFruits.Count];
        SpawnFruit(targetFruit, spawnPoint);
        targetFruitsSpawned++;
    }

    // Notify GameManager with the number of target fruits spawned
    GameManager.Instance.InitializeGame(targetColor, "Target Fruit", targetFruitsSpawned);

    // Spawn distractor fruits
    int distractorsSpawned = 0;
    for (int i = targetFruitsSpawned; i < Mathf.Min(targetFruitsToSpawn + distractorFruitsToSpawn, shuffledSpawnPoints.Length); i++)
    {
        Transform spawnPoint = shuffledSpawnPoints[i];
        Color distractorColor;
        do
        {
            distractorColor = GetRandomColor();
        } while (distractorColor == targetColor); // Ensure distractor is not the target color

        List<GameObject> distractorFruits = fruitColorMap[distractorColor];
        GameObject distractorFruit = distractorFruits[UnityEngine.Random.Range(0, distractorFruits.Count)];
        SpawnFruit(distractorFruit, spawnPoint);
        distractorsSpawned++;
    }

    Debug.Log($"Spawned {targetFruitsSpawned} target fruits and {distractorsSpawned} distractor fruits.");
}




    Color EnsureTargetColorHasFruit()
    {
        Color selectedColor;
        do
        {
            selectedColor = GetRandomColor();
        } while (!fruitColorMap.ContainsKey(selectedColor));

        Debug.Log($"Selected target color: {selectedColor}");
        return selectedColor;
    }

    Color GetRandomColor()
    {
        List<Color> colors = new List<Color>(fruitColorMap.Keys);
        return colors[UnityEngine.Random.Range(0, colors.Count)];
    }

    GameObject GetRandomFruitForColor(Color color)
    {
        if (fruitColorMap.ContainsKey(color))
        {
            List<GameObject> fruits = fruitColorMap[color];
            return fruits[UnityEngine.Random.Range(0, fruits.Count)];
        }

        Debug.LogWarning($"No fruits found for color {color}. Using fallback fruit.");
        return bananaPrefab; // Fallback fruit
    }

    void SpawnFruit(GameObject fruitPrefab, Transform spawnPoint)
    {
        GameObject fruitInstance = Instantiate(fruitPrefab);
        fruitInstance.transform.position = spawnPoint.position;
        fruitInstance.transform.rotation = spawnPoint.rotation;

        Debug.Log($"Spawned fruit: {fruitInstance.name} at {spawnPoint.position}");

        // Align the fruit with the table surface if needed
        RaycastHit hit;
        if (Physics.Raycast(spawnPoint.position, Vector3.down, out hit, Mathf.Infinity, tableLayer))
        {
            float surfaceY = hit.point.y;
            Collider fruitCollider = fruitInstance.GetComponent<Collider>();

            if (fruitCollider != null)
            {
                float colliderHeight = fruitCollider.bounds.extents.y;
                fruitInstance.transform.position = new Vector3(
                    spawnPoint.position.x,
                    surfaceY + colliderHeight,
                    spawnPoint.position.z
                );
            }
            else
            {
                fruitInstance.transform.position = new Vector3(
                    spawnPoint.position.x,
                    surfaceY,
                    spawnPoint.position.z
                );
            }
        }
    }

    Transform[] ShuffleSpawnPoints(Transform[] points)
    {
        Transform[] shuffled = (Transform[])points.Clone();
        for (int i = 0; i < shuffled.Length; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, shuffled.Length);
            Transform temp = shuffled[i];
            shuffled[i] = shuffled[randomIndex];
            shuffled[randomIndex] = temp;
        }
        return shuffled;
    }
}
