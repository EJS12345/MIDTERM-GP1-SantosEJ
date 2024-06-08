using System.Collections;
using UnityEngine;
using TMPro;

public class LifecycleManager : MonoBehaviour
{
    public GameObject eggPrefab;
    public GameObject chickPrefab;
    public GameObject henPrefab;
    public GameObject roosterPrefab;

    public TMP_Text eggCountText;
    public TMP_Text chickCountText;
    public TMP_Text henCountText;
    public TMP_Text roosterCountText;

    private int eggCount = 0;
    private int chickCount = 0;
    private int henCount = 0;
    private int roosterCount = 0;

    public Vector2 spawnAreaMin = new Vector2(-50, -50);
    public Vector2 spawnAreaMax = new Vector2(50, 50);
    private Vector3 lastSpawnPosition;

    private float elapsedTime = 0f;
    private bool stopSpawning = false;
    private const float timeLimit = 4 * 60; // 4 minutes in seconds

    void Start()
    {
        Debug.Log("Starting Lifecycle Manager");
        UpdateCountText();
        SpawnEgg();  // Start with one egg
    }

    void Update()
    {
        if (!stopSpawning)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= timeLimit)
            {
                stopSpawning = true;
                Debug.Log("Time limit reached. Stopping spawning.");
            }
        }
    }

    IEnumerator EggLifecycle(GameObject egg)
    {
        yield return new WaitForSeconds(10);
        if (!stopSpawning)
        {
            SpawnChick(egg);
        }
    }

    void SpawnEgg()
    {
        if (stopSpawning) return;
        eggCount++;
        UpdateCountText();
        GameObject egg = Instantiate(eggPrefab, GetFarSpawnPosition(), Quaternion.identity);
        Debug.Log($"Egg Count: {eggCount}");
        StartCoroutine(EggLifecycle(egg));
    }

    void SpawnChick(GameObject egg)
    {
        if (stopSpawning) return;
        Destroy(egg);  // Destroy the egg object once it hatches
        eggCount--;
        chickCount++;
        UpdateCountText();
        GameObject chick = Instantiate(chickPrefab, GetFarSpawnPosition(), Quaternion.identity);
        Debug.Log($"Chick Count: {chickCount}");
        StartCoroutine(ChickLifecycle(chick));
    }

    IEnumerator ChickLifecycle(GameObject chick)
    {
        yield return new WaitForSeconds(10);
        if (!stopSpawning)
        {
            if (Random.Range(0, 2) == 0)
            {
                SpawnHen(chick);
            }
            else
            {
                SpawnRooster(chick);
            }
        }
    }

    void SpawnHen(GameObject chick)
    {
        if (stopSpawning) return;
        Destroy(chick);  // Destroy the chick object once it matures
        chickCount--;
        henCount++;
        UpdateCountText();
        GameObject hen = Instantiate(henPrefab, GetFarSpawnPosition(), Quaternion.identity);
        Debug.Log($"Hen Count: {henCount}");
        StartCoroutine(HenLifecycle(hen));
    }

    void SpawnRooster(GameObject chick)
    {
        if (stopSpawning) return;
        Destroy(chick);  // Destroy the chick object once it matures
        chickCount--;
        roosterCount++;
        UpdateCountText();
        GameObject rooster = Instantiate(roosterPrefab, GetFarSpawnPosition(), Quaternion.identity);
        Debug.Log($"Rooster Count: {roosterCount}");
        StartCoroutine(RoosterLifecycle(rooster));
    }

    IEnumerator HenLifecycle(GameObject hen)
    {
        yield return new WaitForSeconds(30);
        if (!stopSpawning)
        {
            int eggsLaid = Random.Range(2, 11);
            Debug.Log($"Hen Laying {eggsLaid} Eggs");
            for (int i = 0; i < eggsLaid; i++)
            {
                SpawnEgg();
            }
        }
        yield return new WaitForSeconds(40); // 30s + 40s = 70s total hen lifespan
        henCount--;
        UpdateCountText();
        Debug.Log($"Hen Count: {henCount}");
        Destroy(hen);
    }

    IEnumerator RoosterLifecycle(GameObject rooster)
    {
        yield return new WaitForSeconds(40);
        if (!stopSpawning)
        {
            roosterCount--;
            UpdateCountText();
            Debug.Log($"Rooster Count: {roosterCount}");
            Destroy(rooster);
        }
    }

    Vector3 GetFarSpawnPosition()
    {
        Vector3 spawnPosition;
        float y = 1f; // Adjust this value to set the height above ground level

        do
        {
            float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float z = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            spawnPosition = new Vector3(x, y, z);
        } while (Vector3.Distance(spawnPosition, lastSpawnPosition) < 10);

        lastSpawnPosition = spawnPosition;
        return spawnPosition;
    }

    void UpdateCountText()
    {
        if (eggCountText != null)
            eggCountText.text = $"Eggs: {eggCount}";
        if (chickCountText != null)
            chickCountText.text = $"Chicks: {chickCount}";
        if (henCountText != null)
            henCountText.text = $"Hens: {henCount}";
        if (roosterCountText != null)
            roosterCountText.text = $"Roosters: {roosterCount}";
    }
}

























