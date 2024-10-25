using System.Collections;
using UnityEngine;
using TMPro;

public class Chopping : MonoBehaviour
{
    public GameObject player; // Reference to the player
    public GameObject knifeUnderTable; // Reference to the knife under "Sushi Wood Table"
    public GameObject knifePrefab; // The knife prefab to spawn in the player's hand
    public Transform playerGrabPoint; // The grab point on the player
    public Animator playerAnimator; // The player's animator
    public TextMeshProUGUI timerText; // The UI Text to display the timer
    public float choppingTime = 7f; // The time for chopping in seconds

    // Prefab references for particles
    public GameObject fineDustParticlesPrefab; // Prefab for fine dust particles
    public GameObject choppingFragmentsParticlesPrefab; // Prefab for chopping fragments particles

    // Prefabs for chopped versions of ingredients
    public GameObject choppedPepperPrefab;
    public GameObject choppedCucumberPrefab;
    public GameObject choppedCarrotPrefab;
    public GameObject choppedFishPrefab; // Prefab for chopped fish

    // Emission point for particles
    public Transform particleEmissionPoint; // Point where particles should be emitted

    // The cookpoint to parent ingredients
    public Transform cookpoint; // Reference to the cookpoint on the chopping board

    private GameObject spawnedKnife; // Reference to the spawned knife in the player's hand
    private GameObject fineDustParticlesInstance; // Instance of the fine dust particle prefab
    private GameObject choppingFragmentsParticlesInstance; // Instance of the chopping fragments particle prefab

    private ParticleSystem fineDustParticleSystem; // Particle system for fine dust
    private ParticleSystem choppingFragmentsParticleSystem; // Particle system for chopping fragments

    private bool isPlayerNear = false;
    private bool isChopping = false;
    private Coroutine choppingCoroutine;
    private float currentTimer;

    private GameObject currentIngredient; // Current ingredient being chopped
    private GameObject currentChoppedPrefab; // Chopped version of the current ingredient

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerAnimator = player.GetComponent<Animator>();
        Transform[] allChildren = player.GetComponentsInChildren<Transform>();

        // 자식 오브젝트들 중 이름이 grabpoint인 오브젝트를 찾음
        foreach (Transform child in allChildren)
        {
            if (child.name == "grabpoint")
            {
                playerGrabPoint = child;
                break;
            }
        }

        switch (GameManager.instance.currentPlayer)
        {
            case GameManager.PlayerType.player1:
                choppingTime = 7f; // player1의 기본 시간
                break;
            case GameManager.PlayerType.player2:
                choppingTime = 8f; // player2의 기본 시간
                break;
            case GameManager.PlayerType.player3:
                choppingTime = 9f; // player3의 기본 시간
                break;
            default:
                choppingTime = 7f; // 기본값
                break;
        }

        // Ensure the knife under the table is visible initially
        if (knifeUnderTable != null)
        {
            knifeUnderTable.SetActive(true);
        }

        // Initialize the timer as hidden
        HideTimer();
    }

    void Update()
    {
        // Check if the player is near and holding E
        if (isPlayerNear)
        {
            if (Input.GetKeyDown(KeyCode.E) && !isChopping) // Start chopping when E is pressed
            {
                StartChopping();
            }
            else if (Input.GetKeyUp(KeyCode.E) && isChopping) // Stop chopping when E is released
            {
                StopChopping();
            }
        }
    }

    // Trigger when player enters the chopping zone
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerNear = true;
        }

        // Detect ingredient on the board and parent it to the cookpoint
        if (other.CompareTag("Pepper"))
        {
            Debug.Log("Pepper");
            currentIngredient = other.gameObject;
            currentChoppedPrefab = choppedPepperPrefab;
        }
        else if (other.CompareTag("Cucumber"))
        {
            Debug.Log("Cucumber");
            currentIngredient = other.gameObject;
            currentChoppedPrefab = choppedCucumberPrefab;
        }
        else if (other.CompareTag("Carrot"))
        {
            Debug.Log("Carrot");
            currentIngredient = other.gameObject;
            currentChoppedPrefab = choppedCarrotPrefab;
        }
        else if (other.CompareTag("Fish"))
        {
            Debug.Log("Fish placed on the chopping board.");
            currentIngredient = other.gameObject;
            currentChoppedPrefab = choppedFishPrefab;
            Debug.Log("Chopped fish prefab assigned: " + (choppedFishPrefab != null ? choppedFishPrefab.name : "NULL"));
        }
    }

    // Trigger when player exits the chopping zone
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerNear = false;
            if (isChopping)
            {
                StopChopping(); // Stop chopping if the player moves away
            }
        }
    }

    void StartChopping()
    {
        isChopping = true;

        // Hide the knife under the table
        if (knifeUnderTable != null)
        {
            knifeUnderTable.SetActive(false);
        }

        // Spawn the knife in the player's hand (grab point) if it's not already spawned
        if (knifePrefab != null && spawnedKnife == null)
        {
            spawnedKnife = Instantiate(knifePrefab, playerGrabPoint.position, playerGrabPoint.rotation);
            spawnedKnife.transform.parent = playerGrabPoint; // Attach the knife to the grab point

            spawnedKnife.transform.localPosition = new Vector3(0.138378367f, 0.0690211654f, -0.0262892991f);
            spawnedKnife.transform.localRotation = new Quaternion(-0.772405982f, -0.383837044f, -0.341076404f, 0.373797953f);
        }

        // Start chopping animation
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("chopping", true);
        }

        // Instantiate particle systems at the emission point (if they haven't been already)
        if (fineDustParticlesInstance == null && fineDustParticlesPrefab != null)
        {
            fineDustParticlesInstance = Instantiate(fineDustParticlesPrefab, particleEmissionPoint.position, particleEmissionPoint.rotation);
            fineDustParticleSystem = fineDustParticlesInstance.GetComponent<ParticleSystem>();
        }

        if (choppingFragmentsParticlesInstance == null && choppingFragmentsParticlesPrefab != null)
        {
            choppingFragmentsParticlesInstance = Instantiate(choppingFragmentsParticlesPrefab, particleEmissionPoint.position, particleEmissionPoint.rotation);
            choppingFragmentsParticleSystem = choppingFragmentsParticlesInstance.GetComponent<ParticleSystem>();
        }

        // Play particle systems
        if (fineDustParticleSystem != null && !fineDustParticleSystem.isPlaying)
        {
            fineDustParticleSystem.Play();
        }

        if (choppingFragmentsParticleSystem != null && !choppingFragmentsParticleSystem.isPlaying)
        {
            choppingFragmentsParticleSystem.Play();
        }

        // Start the countdown timer
        if (choppingCoroutine == null)
        {
            choppingCoroutine = StartCoroutine(ChoppingCoroutine());
        }

        Debug.Log("Chopping started.");
    }

    void StopChopping()
    {
        isChopping = false;

        // Show the knife under the table again
        if (knifeUnderTable != null)
        {
            knifeUnderTable.SetActive(true);
        }

        // Remove the knife from the player's hand
        if (spawnedKnife != null)
        {
            Destroy(spawnedKnife);
            spawnedKnife = null; // Reset the reference to the knife
        }

        // Stop chopping animation
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("chopping", false);
        }

        // Stop particle systems
        if (fineDustParticleSystem != null && fineDustParticleSystem.isPlaying)
        {
            fineDustParticleSystem.Stop();
        }

        if (choppingFragmentsParticleSystem != null && choppingFragmentsParticleSystem.isPlaying)
        {
            choppingFragmentsParticleSystem.Stop();
        }

        // Stop and reset the timer
        if (choppingCoroutine != null)
        {
            StopCoroutine(choppingCoroutine);
            choppingCoroutine = null;
        }

        HideTimer(); // Hide the timer when chopping stops
        Debug.Log("Chopping stopped.");
    }

    IEnumerator ChoppingCoroutine()
    {
        currentTimer = choppingTime; // Initialize timer
        ShowTimer(); // Show the timer UI

        while (currentTimer > 0f)
        {
            currentTimer -= Time.deltaTime; // Decrease the timer
            UpdateTimerText(currentTimer);

            // Stop if the player releases the "E" key
            if (!Input.GetKey(KeyCode.E))
            {
                StopChopping();
                yield break;
            }

            yield return null; // Wait for the next frame
        }

        // Timer reached 0, chopping completed
        Debug.Log("Chopping completed!");

        // Replace the original ingredient with the chopped version
        ReplaceIngredientWithChoppedVersion();

        StopChopping(); // Stop everything when timer reaches 0
    }

    void ReplaceIngredientWithChoppedVersion()
    {
        // If there is a current ingredient and a corresponding chopped prefab
        if (currentIngredient != null && currentChoppedPrefab != null)
        {
            // Get the position and rotation of the current ingredient
            Vector3 ingredientPosition = currentIngredient.transform.position;
            Quaternion ingredientRotation = currentIngredient.transform.rotation;

            // Destroy the original ingredient
            Debug.Log("Destroying original ingredient: " + currentIngredient.name);
            Destroy(currentIngredient);

            // Instantiate the chopped version at the same position and parent it to the cookpoint
            GameObject choppedObject = Instantiate(currentChoppedPrefab, ingredientPosition, ingredientRotation);
            choppedObject.transform.SetParent(cookpoint); // Make the chopped object a child of the cookpoint

            Debug.Log("Chopped object instantiated: " + choppedObject.name);
        }
        else if(currentChoppedPrefab == null)
        {
            Debug.LogError("chopped prefab is null.");
        }
        else if (currentIngredient == null)
        {
            Debug.LogError("Current ingredient  is null.");
        }
    }

    public void PlaceIngredientOnCookpoint(GameObject ingredient)
    {
        // Set the ingredient's position to match the cookpoint and parent it to the cookpoint
        ingredient.transform.position = cookpoint.position;
        ingredient.transform.rotation = cookpoint.rotation;
        ingredient.transform.SetParent(cookpoint); // Make it a child of the cookpoint
    }

    void UpdateTimerText(float time)
    {
        // Show the remaining time with 2 decimal places, ensure it doesn't go below 0
        timerText.text = Mathf.Max(time, 0f).ToString("F2") + "s";
    }

    void HideTimer()
    {
        // Hide the timer text by setting it to empty
        timerText.text = "";
    }

    void ShowTimer()
    {
        // Show the timer by initializing it with the chopping time
        timerText.text = choppingTime.ToString("F2") + "s";
    }
}
