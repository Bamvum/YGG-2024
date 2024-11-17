using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EarnMoney : MonoBehaviour
{
    //public GameObject npcPrefab;
    public GameObject[] npcPrefabs; // Array of NPC prefabs
    public Transform shopTransform;
    public float spawnInterval = 5f; // Time interval for spawning NPCs
    public Vector3[] spawnPoints; // Array of random spawn points
    public float npcSpeed = 2f; // Speed of NPC movement

    public Button EarnMoneyBTN;
    public Button StartEarnBTN;
    public Image[] energyIcons; // Array of energy images to represent player energy
    public TMP_Text timerText;
    public TMP_Text refillTimerText;

    private int playerEnergy = 5;
    public float energyEarnTimeLimit = 10f; // 5 minutes (in seconds) 300
    public float cooldownTime = 10f; // 2 minutes (in seconds) 120
    private bool isCooldown = false;

    public GameObject npcSpawnsContainer; // Parent GameObject for NPCs
    void Start()
    {
        EarnMoneyBTN.onClick.AddListener(SpawnNPC);
        StartEarnBTN.onClick.AddListener(StartEarning);
        UpdateEnergyUI();
        EarnMoneyBTN.gameObject.SetActive(false);
        refillTimerText.gameObject.SetActive(false);
    }

    void StartEarning()
    {
        if (playerEnergy > 0)
        {
            playerEnergy--;
            UpdateEnergyUI();
            EarnMoneyBTN.gameObject.SetActive(true);
            StartEarnBTN.gameObject.SetActive(false);
            StartCoroutine(EarningCountdown());
        }
    }

    IEnumerator EarningCountdown()
    {
        float timer = energyEarnTimeLimit;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            UpdateTimerText(timer);
            yield return null;
        }
        EndEarning();
    }

    void EndEarning()
    {
        EarnMoneyBTN.gameObject.SetActive(false);
        DestroyAllNPCs(); // Destroy remaining NPCs after timer ends
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        StartEarnBTN.gameObject.SetActive(true);
        isCooldown = true;
        StartEarnBTN.interactable = false;
        float timer = cooldownTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            UpdateCooldownText(timer);
            yield return null;
        }
        isCooldown = false;
        if (playerEnergy > 0)
        {
            StartEarnBTN.interactable = true;
        }
        refillTimerText.gameObject.SetActive(false);
        UpdateTimerText(0);
        StartEarnBTN.gameObject.SetActive(true);
    }

    void UpdateEnergyUI()
    {
        for (int i = 0; i < energyIcons.Length; i++)
        {
            energyIcons[i].enabled = i < playerEnergy;
        }
        if (playerEnergy <= 0)
        {
            StartEarnBTN.interactable = false;
            StartCoroutine(DailyEnergyRefill());
        }
    }

    void UpdateTimerText(float timeLeft)
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void UpdateCooldownText(float timeLeft)
    {
        refillTimerText.gameObject.SetActive(true);

        if (timeLeft >= 86000) // Check if timeLeft is more than or equal to a day
        {
            int days = Mathf.FloorToInt(timeLeft / 86400);
            int hours = Mathf.FloorToInt((timeLeft % 86400) / 3600);
            int minutes = Mathf.FloorToInt((timeLeft % 3600) / 60);
            int seconds = Mathf.FloorToInt(timeLeft % 60);

            refillTimerText.text = "Refill In: " + string.Format("{0:00}d:{1:00}h:{2:00}m:{3:00}s", days, hours, minutes, seconds);
        }
        else // Display the regular format if less than a day
        {
            int hours = Mathf.FloorToInt(timeLeft / 3600);
            int minutes = Mathf.FloorToInt((timeLeft % 3600) / 60);
            int seconds = Mathf.FloorToInt(timeLeft % 60);

            refillTimerText.text = "Cooldown: " + string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    IEnumerator DailyEnergyRefill()
    {
        // Assuming daily refill happens after 24 hours (86400 seconds)
        float refillTimer = 86400f;
        while (refillTimer > 0)
        {
            refillTimer -= Time.deltaTime;
            UpdateCooldownText(refillTimer);
            yield return null;
        }
        playerEnergy = 5;
        UpdateEnergyUI();
        StartEarnBTN.interactable = true;
        refillTimerText.gameObject.SetActive(false);
    }

    //void SpawnNPC()
    //{
    //    // Select a random spawn point
    //    Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)];
    //    GameObject npc = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);

    //    // Initialize the NPC movement with references to the shop and spawner for interactions
    //    NPCMovement npcMovement = npc.GetComponent<NPCMovement>();
    //    if (npcMovement != null)
    //    {
    //        npcMovement.Initialize(shopTransform, npcSpeed);
    //    }
    //}

    void SpawnNPC()
    {
        Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Select a random NPC prefab from the list
        GameObject randomNpcPrefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];

        // Instantiate the selected NPC prefab with the npcSpawnsContainer as parent
        GameObject npc = Instantiate(randomNpcPrefab, npcSpawnsContainer.transform);

        // Set the local position of the NPC to the spawn position
        npc.transform.localPosition = spawnPosition;

        // Initialize the NPC movement with references to the shop and spawner for interactions
        NPCMovement npcMovement = npc.GetComponent<NPCMovement>();
        if (npcMovement != null)
        {
            npcMovement.Initialize(shopTransform, npcSpeed);
        }
    }

    void DestroyAllNPCs()
    {
        // Destroy all child objects under npcSpawnsContainer
        foreach (Transform child in npcSpawnsContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

}
