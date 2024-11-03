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
    public bool isChopping = false;
    private Coroutine choppingCoroutine;
    private float currentTimer;

    private GameObject currentIngredient; // Current ingredient being chopped
    private GameObject currentChoppedPrefab; // Chopped version of the current ingredient
    public bool alreadyup = false;
    private float originalChoppingTime;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerAnimator = player.GetComponent<Animator>();
        Transform[] allChildren = player.GetComponentsInChildren<Transform>();

        // �ڽ� ������Ʈ�� �� �̸��� grabpoint�� ������Ʈ�� ã��
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
                choppingTime = 5f; // player1�� �⺻ �ð�
                originalChoppingTime = choppingTime;
                break;
            case GameManager.PlayerType.player2:
                choppingTime = 6f; // player2�� �⺻ �ð�
                originalChoppingTime = choppingTime;
                break;
            case GameManager.PlayerType.player3:
                choppingTime = 7f; // player3�� �⺻ �ð�
                originalChoppingTime = choppingTime;
                break;
            default:
                choppingTime = 5f; // �⺻��
                originalChoppingTime = choppingTime;
                break;
        }

        if (knifeUnderTable != null)
        {
            knifeUnderTable.SetActive(true);
        }

        HideTimer();
    }

    void Update()
    {
        if (isPlayerNear)
        {
            if (Input.GetKeyDown(KeyCode.E) && !isChopping)
            {
                StartChopping();
            }
            else if (Input.GetKeyUp(KeyCode.E) && isChopping)
            {
                StopChopping();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerNear = true;
        }

        if (other.CompareTag("Pepper"))
        {
            currentIngredient = other.gameObject;
            currentChoppedPrefab = choppedPepperPrefab;
        }
        else if (other.CompareTag("Cucumber"))
        {
            currentIngredient = other.gameObject;
            currentChoppedPrefab = choppedCucumberPrefab;
        }
        else if (other.CompareTag("Carrot"))
        {
            currentIngredient = other.gameObject;
            currentChoppedPrefab = choppedCarrotPrefab;
        }
        else if (other.CompareTag("Fish"))
        {
            currentIngredient = other.gameObject;
            currentChoppedPrefab = choppedFishPrefab;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerNear = false;
            if (isChopping)
            {
                StopChopping();
            }
        }
    }

    void StartChopping()
    {
        isChopping = true;

        if (GameManager.instance.iscookup)
        {
            choppingTime *= 0.7f; // Reduce chopping time by 30% if iscookup is true
        }

        if (knifeUnderTable != null)
        {
            knifeUnderTable.SetActive(false);
        }

        if (knifePrefab != null && spawnedKnife == null)
        {
            spawnedKnife = Instantiate(knifePrefab, playerGrabPoint.position, playerGrabPoint.rotation);
            spawnedKnife.transform.parent = playerGrabPoint; 

            spawnedKnife.transform.localPosition = new Vector3(0.138378367f, 0.0690211654f, -0.0262892991f);
            spawnedKnife.transform.localRotation = new Quaternion(-0.772405982f, -0.383837044f, -0.341076404f, 0.373797953f);
        }

        if (playerAnimator != null)
        {
            playerAnimator.SetBool("chopping", true);
        }

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

        if (fineDustParticleSystem != null && !fineDustParticleSystem.isPlaying)
        {
            fineDustParticleSystem.Play();
        }

        if (choppingFragmentsParticleSystem != null && !choppingFragmentsParticleSystem.isPlaying)
        {
            choppingFragmentsParticleSystem.Play();
        }

        if (choppingCoroutine == null)
        {
            choppingCoroutine = StartCoroutine(ChoppingCoroutine());
        }

        Debug.Log("Chopping started.");
    }

    void StopChopping()
    {
        isChopping = false;

        if (knifeUnderTable != null)
        {
            knifeUnderTable.SetActive(true);
        }

        if (spawnedKnife != null)
        {
            Destroy(spawnedKnife);
            spawnedKnife = null;
        }

        if (playerAnimator != null)
        {
            playerAnimator.SetBool("chopping", false);
        }

        if (fineDustParticleSystem != null && fineDustParticleSystem.isPlaying)
        {
            fineDustParticleSystem.Stop();
        }

        if (choppingFragmentsParticleSystem != null && choppingFragmentsParticleSystem.isPlaying)
        {
            choppingFragmentsParticleSystem.Stop();
        }

        if (choppingCoroutine != null)
        {
            StopCoroutine(choppingCoroutine);
            choppingCoroutine = null;
        }

        if (GameManager.instance.iscookup)
        {
            choppingTime = originalChoppingTime;
        }

        HideTimer();
        Debug.Log("Chopping stopped.");
    }

    IEnumerator ChoppingCoroutine()
    {
        currentTimer = choppingTime;
        ShowTimer();

        while (currentTimer > 0f)
        {
            currentTimer -= Time.deltaTime;
            UpdateTimerText(currentTimer);

            if (!Input.GetKey(KeyCode.E))
            {
                StopChopping();
                yield break;
            }

            yield return null; 
        }

        Debug.Log("Chopping completed!");

        ReplaceIngredientWithChoppedVersion();

        StopChopping(); 
    }

    void ReplaceIngredientWithChoppedVersion()
    {
        if (currentIngredient != null && currentChoppedPrefab != null)
        {
            Vector3 ingredientPosition = currentIngredient.transform.position;
            Quaternion ingredientRotation = currentIngredient.transform.rotation;

            Debug.Log("Destroying original ingredient: " + currentIngredient.name);
            Destroy(currentIngredient);

            GameObject choppedObject = Instantiate(currentChoppedPrefab, ingredientPosition, ingredientRotation);

            isfoodinhere cuttingScript = GetComponent<isfoodinhere>();
            if (cuttingScript != null)
            {
                cuttingScript.ishere = false;
            }

            Debug.Log("Chopped object instantiated: " + choppedObject.name);
        }
        else if (currentChoppedPrefab == null)
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
        ingredient.transform.position = cookpoint.position;
        ingredient.transform.rotation = cookpoint.rotation;
        ingredient.transform.SetParent(cookpoint);
    }

    void UpdateTimerText(float time)
    {
        timerText.text = Mathf.Max(time, 0f).ToString("F2") + "s";
    }

    void HideTimer()
    {
        timerText.text = "";
    }

    void ShowTimer()
    {
        timerText.text = choppingTime.ToString("F2") + "s";
    }
}
