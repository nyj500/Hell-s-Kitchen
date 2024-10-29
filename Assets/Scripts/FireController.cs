//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class FireController : MonoBehaviour
//{
//    public playergrab playergrab;
//    public GameObject fireEffect; 
//    public GameObject fryingPan;  
//    public GameObject extinguisher;
//    public GameObject currentIngredient;
//    public float burnTime = 1f;
//    public bool hasIngredient = false;
//    public bool isOnFire = false;  
//    public float timeRange_1 = 60f;
//    public float timeRange_2 = 120f;
//    public float randomTime;

//    void Start()
//    {
//        float initialDelay = 20.0f;

//        StartCoroutine(DelayedStartFire(initialDelay));
//    }

//    IEnumerator DelayedStartFire(float delay)
//    {
//        // 초기 대기 시간만큼 대기
//        yield return new WaitForSeconds(delay);

//        // 대기 후 Fire 발생 루틴 시작
//        StartCoroutine(TriggerFire());
//    }

//    IEnumerator TriggerFire()
//    {
//        while (true)
//        {
//            randomTime = Random.Range(timeRange_1, timeRange_2);

//            // 설정된 시간만큼 대기
//            yield return new WaitForSeconds(randomTime);

//            // 대기 후 실행할 함수 호출
//            StartFire();
//        }
//    }

//    void StartFire()
//    {
//        isOnFire = true;
//        fireEffect.SetActive(true);
//        if (hasIngredient)
//        {
//            StartCoroutine(BurnIngredient());
//        }

//        Debug.Log("Fire");
//    }

//    public void ExtinguishFire()
//    {
//        isOnFire = false;
//        fireEffect.SetActive(false);

//        Debug.Log("Extinguish");
//    }

//    IEnumerator BurnIngredient()
//    {
//        yield return new WaitForSeconds(burnTime);
//        if (isOnFire && currentIngredient != null)  
//        {
//            Destroy(currentIngredient);
//            hasIngredient = false;
//        }
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FireController : MonoBehaviour
{
    public playergrab playergrab;
    public GameObject fireEffect;
    public GameObject fryingPan;
    public GameObject extinguisher;
    public GameObject currentIngredient;
    public GameObject burntIngredientPrefab; // Prefab for the burnt version
    public TextMeshProUGUI panTimerText; // UI text to display the timer above the pan
    public float burnTime = 1f;
    public float cookingTime = 10f; // Time it takes to cook before burning
    public float fireStartTime = 5f; // Time after which fire starts if the ingredient is burnt
    public bool hasIngredient = false;
    public bool isOnFire = false;
    public float timeRange_1 = 60f;
    public float timeRange_2 = 120f;
    public float randomTime;

    private bool isCooking = false; // Flag to track if cooking has started
    private Coroutine cookingCoroutine; // To handle cooking time
    private Coroutine burnCoroutine; // To handle fire after burnt
    private float currentTimer;

    void Start()
    {
        float initialDelay = 20.0f;
        switch (GameManager.instance.currentPlayer)
        {
            case GameManager.PlayerType.player1:
                cookingTime = 8f; // player1의 기본 시간
                break;
            case GameManager.PlayerType.player2:
                cookingTime = 9f; // player2의 기본 시간
                break;
            case GameManager.PlayerType.player3:
                cookingTime = 10f; // player3의 기본 시간
                break;
            default:
                cookingTime = 8f; // 기본값
                break;
        }
        HideTimer(); // Hide the timer UI initially
        StartCoroutine(DelayedStartFire(initialDelay));
    }

    IEnumerator DelayedStartFire(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(TriggerFire());
    }

    IEnumerator TriggerFire()
    {
        while (true)
        {
            randomTime = Random.Range(timeRange_1, timeRange_2);
            yield return new WaitForSeconds(randomTime);
            StartFire();
        }
    }

    void StartFire()
    {
        isOnFire = true;
        fireEffect.SetActive(true);
        if (hasIngredient && isCooking) // Check if there is an ingredient and it's cooking
        {
            StartCoroutine(BurnIngredient());
        }

        Debug.Log("Fire started!");
    }

    public void ExtinguishFire()
    {
        isOnFire = false;
        fireEffect.SetActive(false);
        HideTimer(); // Hide the timer once fire is extinguished
        Debug.Log("Fire extinguished!");
    }

    public void StartCooking()
    {
        if (!isCooking && hasIngredient)
        {
            isCooking = true;

            if (cookingCoroutine != null)
                StopCoroutine(cookingCoroutine);

            cookingCoroutine = StartCoroutine(CookingCoroutine());
            ShowTimer(); // Show the timer when cooking starts
        }
    }


    IEnumerator CookingCoroutine()
    {
        currentTimer = cookingTime;
        while (currentTimer > 0f)
        {
            currentTimer -= Time.deltaTime; // Decrease the timer
            UpdateTimerText(currentTimer);  // Update the timer UI above the pan
            yield return null; // Wait for the next frame
        }

        // Cooking completed, now the ingredient burns
        Debug.Log("Cooking completed! Ingredient is now burning.");
        ReplaceIngredientWithBurntVersion();

        // Start fire if the burnt ingredient stays too long on the pan
        burnCoroutine = StartCoroutine(BurnAfterTime());
    }

    void ReplaceIngredientWithBurntVersion()
    {
        // Ensure both the current ingredient and burnt prefab are set
        if (currentIngredient != null && burntIngredientPrefab != null)
        {
            // Get the position and rotation of the current ingredient
            Vector3 ingredientPosition = currentIngredient.transform.position;
            Quaternion ingredientRotation = currentIngredient.transform.rotation;

            // Destroy the original (cooked or burning) ingredient
            Destroy(currentIngredient);

            // Instantiate the burnt version at the same position
            GameObject burntVersion = Instantiate(burntIngredientPrefab, ingredientPosition, ingredientRotation);
            burntVersion.transform.SetParent(this.transform); // Parent to the frying pan or appropriate object

            currentIngredient = burntVersion; // Update the currentIngredient reference to the burnt version

            Debug.Log("Ingredient replaced with burnt version.");
        }
        else
        {
            Debug.LogError("Current ingredient or burnt prefab is null.");
        }
    }


    IEnumerator BurnAfterTime()
    {
        currentTimer = fireStartTime;
        while (currentTimer > 0f)
        {
            currentTimer -= Time.deltaTime;
            UpdateTimerText(currentTimer);  // Update the timer during the burning period
            yield return null;
        }

        if (currentIngredient != null && !isOnFire) // If still no fire and ingredient is present
        {
            StartFire();
        }
    }

    IEnumerator BurnIngredient()
    {
        yield return new WaitForSeconds(burnTime);
        if (isOnFire && currentIngredient != null)
        {
            Destroy(currentIngredient);
            hasIngredient = false;
            HideTimer(); // Hide the timer when the ingredient is destroyed
        }
    }
    //private void OnCollisionEnter(Collision other)
    //{
        
    //}

    void OnTriggerEnter(Collider other)
    {

        // If an ingredient like salami is placed on the frying pan
        if (other.CompareTag("Salami"))
        {
            Debug.Log("AAAAA");
            currentIngredient = other.gameObject;
            Debug.Log(currentIngredient);
            Debug.Log("BBBB");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Stop cooking if the ingredient is removed from the pan
        if (other.CompareTag("Salami") && currentIngredient == other.gameObject)
        {
            StopCooking();
        }
    }

    void StopCooking()
    {
        if (isCooking)
        {
            isCooking = false;
            if (cookingCoroutine != null)
            {
                StopCoroutine(cookingCoroutine);
                cookingCoroutine = null;
            }

            if (burnCoroutine != null)
            {
                StopCoroutine(burnCoroutine);
                burnCoroutine = null;
            }

            HideTimer(); // Hide the timer when cooking stops
            Debug.Log("Cooking stopped.");
        }
    }

    // UI Timer Functions
    void UpdateTimerText(float time)
    {
        // Update the timer text with two decimal places
        panTimerText.text = Mathf.Max(time, 0f).ToString("F2") + "s";
    }

    void HideTimer()
    {
        // Hide the timer text by clearing the text content
        panTimerText.text = "";
    }

    void ShowTimer()
    {
        // Show the timer with the initial cooking time
        panTimerText.text = cookingTime.ToString("F2") + "s";
    }
}
