//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class playergrab : MonoBehaviour
//{
//    private Animator animator;
//    private bool grabbed;
//    private int isgrab = 0;
//    GameObject spawnedObject = null;
//    public GameObject grabPoint;
//    public LightController lightController;
//    public int isFish = 0;
//    public int isCarrot = 0;
//    public int isPepper = 0;
//    public int isCucumber = 0;
//    public int isNori = 0;
//    public int isRice = 0;
//    public int isSalami = 0;
//    public int isExtinhuisher = 0;
//    public GameObject knifePrefab;
//    GameObject knife = null;

//    // Reference to the chopping board (for placing ingredients)
//    public Chopping choppingScript;

//    void Start()
//    {
//        lightController = GameObject.FindWithTag("Lever").GetComponent<LightController>();
//        animator = GetComponent<Animator>();
//        grabbed = false;
//    }

//    void Update()
//    {
//        animator.SetBool("holdingItem", grabbed);
//    }

//    void OnTriggerStay(Collider other)
//    {
//        if (isgrab == 0)
//        {
//            if (Input.GetKey(KeyCode.Space))
//            {
//                // Determine which prefab to instantiate based on the tag
//                string prefabName = "";
//                if (other.CompareTag("Fish")) { prefabName = "Fish"; isFish = 1; }
//                else if (other.CompareTag("Carrot")) { prefabName = "Carrot"; isCarrot = 1; }
//                else if (other.CompareTag("Pepper")) { prefabName = "Pepper"; isPepper = 1; }
//                else if (other.CompareTag("Cucumber")) { prefabName = "Cucumber"; isCucumber = 1; }
//                else if (other.CompareTag("Nori")) { prefabName = "Nori_001"; isNori = 1; }
//                else if (other.CompareTag("Rice")) { prefabName = "Rice_001"; isRice = 1; }
//                else if (other.CompareTag("Salami")) { prefabName = "Salami A"; isSalami = 1; }
//                else if (other.CompareTag("Extinguisher")) { prefabName = "Extinguisher"; isExtinhuisher = 1; }

//                if (!string.IsNullOrEmpty(prefabName))
//                {
//                    GameObject prefab = Resources.Load<GameObject>(prefabName);
//                    if (prefab != null)
//                    {
//                        spawnedObject = Instantiate(prefab, grabPoint.transform.position, grabPoint.transform.rotation);
//                        spawnedObject.transform.parent = grabPoint.transform; // Set the grabPoint as the parent
//                        if (prefabName == "Extinguisher")
//                        {
//                            spawnedObject.transform.localPosition = new Vector3(0.0f, 0.0f, -1.25f);
//                            spawnedObject.transform.Rotate(90, 0, 0);
//                        }
//                        isgrab = 1;
//                        grabbed = true;
//                    }
//                }

//                if (other.CompareTag("Lever"))
//                {
//                    lightController.SetSkyboxLighting();
//                }
//            }
//        }
//        else
//        {
//            // Handle placing the object on the cutting board or pan
//            if (Input.GetKey(KeyCode.Space))
//            {
//                if (other.CompareTag("ConveyorBelt"))
//                {
//                    // Place the object on the conveyor belt's placePoint
//                    Transform placePoint = other.transform.Find("placePoint");
//                    if (placePoint != null && spawnedObject != null)
//                    {
//                        // Move the object to the placePoint position and align rotation
//                        spawnedObject.transform.position = placePoint.position;
//                        spawnedObject.transform.rotation = placePoint.rotation;
//                        spawnedObject.transform.parent = other.transform; // Parent to conveyor if needed

//                        // Add object to conveyor's onBelt list and freeze rotation
//                        ConveyorBelt conveyorScript = other.GetComponent<ConveyorBelt>();
//                        if (conveyorScript != null)
//                        {
//                            conveyorScript.onBelt.Add(spawnedObject);
//                        }

//                        Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
//                        if (rb != null)
//                        {
//                            rb.constraints = RigidbodyConstraints.FreezeRotation;
//                        }

//                        // Reset player grab states
//                        isgrab = 0;
//                        grabbed = false;
//                        spawnedObject = null;

//                        Debug.Log("Object placed on conveyor belt at placePoint.");
//                    }
//                    else
//                    {
//                        Debug.LogWarning("placePoint not found on the conveyor belt.");
//                    }
//                }
//                if (other.CompareTag("cutting"))
//                {
//                    // Dynamically get the `Chopping` script from the specific cutting board you're interacting with
//                    Chopping choppingScript = other.GetComponent<Chopping>();
//                    isfoodinhere cuttingScript = other.GetComponent<isfoodinhere>();

//                    if (cuttingScript != null && choppingScript != null)
//                    {
//                        // Proceed only if there is no item already on the chopping board
//                        if (cuttingScript.ishere == false && isFish == 1)
//                        {
//                            choppingScript.PlaceIngredientOnCookpoint(spawnedObject);
//                            // Optionally rotate or modify the fish placement if needed
//                            isFish = 0;
//                            isgrab = 0;
//                            grabbed = false;
//                            spawnedObject = null;
//                            cuttingScript.ishere = true; // Mark the board as occupied
//                            Debug.Log("Fish placed on the chopping board.");
//                        }
//                        else if (cuttingScript.ishere == false && isCarrot == 1)
//                        {
//                            choppingScript.PlaceIngredientOnCookpoint(spawnedObject);
//                            spawnedObject.transform.Rotate(90, 90, 0);
//                            isCarrot = 0;
//                            isgrab = 0;
//                            grabbed = false;
//                            spawnedObject = null;
//                            cuttingScript.ishere = true; // Mark the board as occupied
//                        }
//                        else if (cuttingScript.ishere == false && isPepper == 1)
//                        {
//                            choppingScript.PlaceIngredientOnCookpoint(spawnedObject);
//                            spawnedObject.transform.Rotate(90, 180, 0);
//                            isPepper = 0;
//                            isgrab = 0;
//                            grabbed = false;
//                            spawnedObject = null;
//                            cuttingScript.ishere = true;
//                        }
//                        else if (cuttingScript.ishere == false && isCucumber == 1)
//                        {
//                            choppingScript.PlaceIngredientOnCookpoint(spawnedObject);
//                            isCucumber = 0;
//                            isgrab = 0;
//                            grabbed = false;
//                            spawnedObject = null;
//                            cuttingScript.ishere = true;
//                        }
//                    }
//                    else
//                    {
//                        if (cuttingScript == null)
//                        {
//                            Debug.LogWarning("The object you are interacting with does not have the 'isfoodinhere' component.");
//                        }
//                        if (choppingScript == null)
//                        {
//                            Debug.LogWarning("The object you are interacting with does not have the 'Chopping' component.");
//                        }
//                    }
//                }
//                else if (other.CompareTag("Pan"))
//                {
//                    // Handle placing the Salami (Sausage) on the pan
//                    isfoodinhere panScript = other.GetComponent<isfoodinhere>();

//                    if (panScript != null && panScript.ishere == false && isSalami == 1)
//                    {
//                        // Load the salami prefab
//                        GameObject prefab = Resources.Load<GameObject>("Salami A");

//                        // Find the cookpoint transform on the pan
//                        Transform cookpointTransform = other.transform.Find("cookpoint");

//                        if (cookpointTransform != null)
//                        {
//                            // Instantiate the salami at the cookpoint position and set it as a child of the cookpoint
//                            GameObject placedSalami = Instantiate(prefab, cookpointTransform.position, cookpointTransform.rotation);
//                            placedSalami.transform.SetParent(cookpointTransform);  // Make the salami a child of the cookpoint

//                            // Reset the grab states
//                            isSalami = 0;
//                            isgrab = 0;
//                            grabbed = false;
//                            panScript.ishere = true; // Mark the pan as occupied
//                            Debug.Log("Salami placed on the pan at cookpoint.");

//                            // Start cooking when salami is placed on the pan
//                            FireController fireController = other.GetComponent<FireController>();
//                            if (fireController != null)
//                            {
//                                fireController.currentIngredient = placedSalami;  // Set the salami as the current ingredient in FireController
//                                fireController.hasIngredient = true;  // Set the flag that there's an ingredient
//                                fireController.StartCooking();  // Start the cooking process
//                                Debug.Log("Cooking started for the salami.");
//                            }

//                            // Destroy the object held by the player AFTER the salami has been placed on the pan
//                            if (spawnedObject != null)
//                            {
//                                Destroy(spawnedObject);
//                                spawnedObject = null;  // Now set spawnedObject to null
//                            }
//                        }
//                        else
//                        {
//                            Debug.LogWarning("Cookpoint transform not found on the pan!");
//                        }
//                    }

//                    FireController fireControllerExtinguish = other.GetComponent<FireController>();
//                    if (fireControllerExtinguish != null && isExtinhuisher == 1 && fireControllerExtinguish.isOnFire)
//                    {
//                        fireControllerExtinguish.ExtinguishFire();
//                        Destroy(spawnedObject); // Remove the extinguisher from hand
//                        isExtinhuisher = 0; // Reset extinguisher state
//                        isgrab = 0;
//                        grabbed = false;
//                        spawnedObject = null;
//                        Debug.Log("Fire extinguished!");
//                    }
//                }

//                else if (other.CompareTag("Trashcan"))
//                {
//                    // If the player throws the object into the trash can
//                    if (spawnedObject != null)
//                    {
//                        Destroy(spawnedObject);
//                    }

//                    // Reset states
//                    grabbed = false;
//                    isgrab = 0;
//                    isFish = 0;
//                    isCarrot = 0;
//                    isPepper = 0;
//                    isCucumber = 0;
//                    isNori = 0;
//                    isRice = 0;
//                    isSalami = 0;
//                    isExtinhuisher = 0; // Reset extinguisher state
//                    spawnedObject = null;
//                }
//            }
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playergrab : MonoBehaviour
{
    private Animator animator;
    private bool grabbed;
    private int isgrab = 0;
    GameObject spawnedObject = null;
    public GameObject grabPoint;
    public LightController lightController;
    public int isFish = 0;
    public int isCarrot = 0;
    public int isPepper = 0;
    public int isCucumber = 0;
    public int isNori = 0;
    public int isRice = 0;
    public int isSalami = 0;
    public int isCookedSalami = 0;
    public int isExtinguisher = 0;
    public int isChoppedPepper = 0;
    public int isChoppedCucumber = 0;
    public int isChoppedFish = 0;
    public int isChoppedCarrot = 0;
    public GameObject knifePrefab;
    GameObject knife = null;
    private bool isChopped = false;

    // Reference to the chopping board (for placing ingredients)
    public Chopping choppingScript;
    private Chopping choppingInstance;

    void Start()
    {
        lightController = GameObject.FindWithTag("Lever").GetComponent<LightController>();
        animator = GetComponent<Animator>();
        grabbed = false;
        choppingInstance = GameObject.FindObjectOfType<Chopping>();
    }

    void Update()
    {
        animator.SetBool("holdingItem", grabbed);
    }

    void OnTriggerStay(Collider other)
    {
        if (isgrab == 0 && Input.GetKey(KeyCode.Space) && (choppingInstance == null || !choppingInstance.isChopping))
        {
            TryGrabItem(other);
        }
        else if (isgrab == 1 && Input.GetKey(KeyCode.Space))
        {
            // Handle placing the object on various stations
            if (other.CompareTag("ConveyorBelt"))
            {
                PlaceOnConveyorBelt(other);
            }
            else if (other.CompareTag("cutting"))
            {
                PlaceOnCuttingBoard(other);
            }
            else if (other.CompareTag("Pan"))
            {
                PlaceOnPan(other);
                ExtinguishFire(other);
            }
            else if (other.CompareTag("Trashcan"))
            {
                DiscardItem();
            }
            else if (other.CompareTag("Pan")) // Check for fire to extinguish
            {
                //ExtinguishFire(other);
            }
            else if (other.CompareTag("Plate"))
            {
                PlaceOnPlate(other);
            }
        }
    }


    private void PlaceOnPlate(Collider plate)
    {
        Plate plateScript = plate.GetComponent<Plate>();

        if (plateScript == null || plateScript.hasKimbap)
        {
            Debug.LogWarning("Kimbap already on plate. Cannot place more items.");
            return; // 김밥이 이미 있으면 추가 재료를 놓지 않음
        }

        Transform placePoint = null;

        if (spawnedObject.CompareTag("Nori") && !plateScript.hasNori)
        {
            placePoint = plate.transform.Find("PlatePlacePoint");
            if (placePoint != null && spawnedObject != null)
            {
                // Place the object at the placePoint position
                spawnedObject.transform.position = placePoint.position;
                spawnedObject.transform.rotation = placePoint.rotation;
                spawnedObject.transform.parent = plate.transform;

                // Mark the presence of Nori on the plate
                plateScript.hasNori = true;

                // Reset player grab states since the object is now on the plate
                ResetGrabState();

                Debug.Log("Nori placed on the plate.");
            }
            else
            {
                Debug.LogWarning("placePoint not found on the plate or no item to place.");
            }
        }
        else if (spawnedObject.CompareTag("Rice") && !plateScript.hasRice)
        {
            placePoint = plate.transform.Find("PlatePlacePointRice");
            if (placePoint != null && spawnedObject != null)
            {
                spawnedObject.transform.position = placePoint.position;
                spawnedObject.transform.rotation = placePoint.rotation;
                spawnedObject.transform.parent = plate.transform;

                plateScript.hasRice = true;

                ResetGrabState();

                Debug.Log("Rice placed on the plate.");
            }
            else
            {
                Debug.LogWarning("placePoint not found on the plate or no item to place.");
            }
        }
        else if ((spawnedObject.CompareTag("ChoppedFish") || spawnedObject.CompareTag("CookedSalami")) && !plateScript.hasChoppedFish && !plateScript.hasCookedSalami)
        {
            placePoint = plate.transform.Find("PlatePlacePointMain");
            if (placePoint != null && spawnedObject != null)
            {

                if (spawnedObject.CompareTag("ChoppedFish"))
                {
                    spawnedObject.transform.position = placePoint.position;
                    spawnedObject.transform.rotation = placePoint.rotation;
                    spawnedObject.transform.parent = plate.transform;
                    plateScript.hasChoppedFish = true;
                }
                else
                {
                    spawnedObject.transform.position = placePoint.position;
                    spawnedObject.transform.rotation = placePoint.rotation;
                    spawnedObject.transform.parent = plate.transform;
                    plateScript.hasCookedSalami = true;
                }

                ResetGrabState();

                Debug.Log("CookedMainFood placed on the plate.");
            }
            else
            {
                Debug.LogWarning("placePoint not found on the plate or no item to place.");
            }
        }
        else if ((spawnedObject.CompareTag("ChoppedPepper") || spawnedObject.CompareTag("ChoppedCucumber") || spawnedObject.CompareTag("ChoppedCarrot"))
                 && !plateScript.hasChoppedPepper && !plateScript.hasChoppedCucumber && !plateScript.hasChoppedCarrot)
        {
            placePoint = plate.transform.Find("PlatePlacePointVeg");
            if (placePoint != null && spawnedObject != null)
            {
                spawnedObject.transform.position = placePoint.position;
                spawnedObject.transform.rotation = placePoint.rotation;
                spawnedObject.transform.parent = plate.transform;

                // �� ��ä ������ ���� ���� ����
                if (spawnedObject.CompareTag("ChoppedPepper")) plateScript.hasChoppedPepper = true;
                else if (spawnedObject.CompareTag("ChoppedCucumber")) plateScript.hasChoppedCucumber = true;
                else if (spawnedObject.CompareTag("ChoppedCarrot")) plateScript.hasChoppedCarrot = true;

                ResetGrabState();

                Debug.Log(spawnedObject.tag + " placed on the plate.");
            }
            else
            {
                Debug.LogWarning("placePoint not found on the plate or no item to place.");
            }
        }
        else
        {
            Debug.LogWarning("Ingredient is already on the plate, cannot place again.");
        }

    }
    private void TryGrabItem(Collider other)
    {
        string prefabName = "";

        bool isChoppedFromBoard = false; // Flag to check if item is a chopped ingredient from chopping board
        bool isFromPan = false;
        if (other.CompareTag("Fish")) { prefabName = "Fish"; isFish = 1; }
        else if (other.CompareTag("Carrot")) { prefabName = "Carrot"; isCarrot = 1; }
        else if (other.CompareTag("Pepper")) { prefabName = "Pepper"; isPepper = 1; }
        else if (other.CompareTag("Cucumber")) { prefabName = "Cucumber"; isCucumber = 1; }
        else if (other.CompareTag("Nori")) { prefabName = "Nori_001"; isNori = 1; }
        else if (other.CompareTag("Rice")) { prefabName = "Rice_001"; isRice = 1; }
        else if (other.CompareTag("Salami")) { prefabName = "Salami"; isSalami = 1; }
        else if (other.CompareTag("Extinguisher")) { prefabName = "Extinguisher"; isExtinguisher = 1; }
        else if (other.CompareTag("CookedSalami")) { prefabName = "CookedSalami"; isCookedSalami = 1; isFromPan = true; }


        if (other.CompareTag("ChoppedPepper")) { prefabName = "ChoppedPepper"; isChoppedPepper = 1; isChoppedFromBoard = true; isChopped = true; }
        else if (other.CompareTag("ChoppedCucumber")) { prefabName = "ChoppedCucumber"; isChoppedCucumber = 1; isChoppedFromBoard = true; isChopped = true; }
        else if (other.CompareTag("ChoppedFish")) { prefabName = "ChoppedFish"; isChoppedFish = 1; isChoppedFromBoard = true; isChopped = true; }
        else if (other.CompareTag("ChoppedCarrot")) { prefabName = "ChoppedCarrot"; isChoppedCarrot = 1; isChoppedFromBoard = true; isChopped = true; }

        if (!string.IsNullOrEmpty(prefabName))
        {
           

            GameObject prefab = Resources.Load<GameObject>(prefabName);
            if (prefab != null)
            {
                // Destroy the original object on the chopping board
                if (isChoppedFromBoard)
                {
                    isfoodinhere cuttingScript = other.GetComponentInParent<isfoodinhere>();
                    if (cuttingScript != null)
                    {
                        cuttingScript.ishere = false; // ���� ���¸� ����������� ǥ��
                    }
                    Destroy(other.gameObject);
                    Debug.Log("Destroyed chopped ingredient on chopping board: " + prefabName);
                }
                if (isFromPan)
                {
                    FireController fireController = other.GetComponentInParent<FireController>();
                    if (fireController != null)
                    {
                        fireController.GrabbedIngredient();
                    }

                    isfoodinhere panScript = other.GetComponentInParent<isfoodinhere>();
                    if (panScript != null)
                    {
                        panScript.ishere = false;
                    }
                    Destroy(other.gameObject);
                    Debug.Log("Destroyed cooked ingredient on pan: " + prefabName);
                }

                // Instantiate a new object in the player's hand
                spawnedObject = Instantiate(prefab, grabPoint.transform.position, grabPoint.transform.rotation);
                spawnedObject.transform.parent = grabPoint.transform;
                if (prefabName == "Extinguisher")
                {
                    spawnedObject.transform.localPosition = new Vector3(0.0f, 0.0f, -1.25f);
                    spawnedObject.transform.Rotate(90, 0, 0);
                }
                isgrab = 1;
                grabbed = true;
                Debug.Log("Grabbed chopped item: " + prefabName);
            }
        }

        if (other.CompareTag("Lever"))
        {
            lightController.SetSkyboxLighting();
        }

        
    }

    private void ExtinguishFire(Collider other)
    {
        FireController fireControllerExtinguish = other.GetComponent<FireController>();

        if (fireControllerExtinguish != null && isExtinguisher == 1 && fireControllerExtinguish.isOnFire)
        {
            fireControllerExtinguish.ExtinguishFire();
            Destroy(spawnedObject); // Remove the extinguisher from hand
            isExtinguisher = 0; // Reset extinguisher state
            isgrab = 0;
            grabbed = false;
            spawnedObject = null;
            Debug.Log("Fire extinguished!");
        }
    }

    // Methods for handling placements on various stations
    private void PlaceOnConveyorBelt(Collider other)
    {
        Transform placePoint = other.transform.Find("placePoint");
        if (placePoint != null && spawnedObject != null)
        {
            spawnedObject.transform.position = placePoint.position;
            spawnedObject.transform.rotation = placePoint.rotation;
            spawnedObject.transform.parent = other.transform;

            ConveyorBelt conveyorScript = other.GetComponent<ConveyorBelt>();
            if (conveyorScript != null)
            {
                conveyorScript.onBelt.Add(spawnedObject);
            }

            Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // Enable physics for conveyor movement
                rb.constraints = RigidbodyConstraints.FreezeRotation;
            }

            ResetGrabState();
            Debug.Log("Object placed on conveyor belt at placePoint.");
        }
    }


    private void PlaceOnCuttingBoard(Collider other)
    {
        // Check for valid components on the cutting board
        Chopping choppingScript = other.GetComponent<Chopping>();
        isfoodinhere cuttingScript = other.GetComponent<isfoodinhere>();

        if (cuttingScript != null && choppingScript != null)
        {

            if (isChopped || isSalami == 1)
            {
                Debug.Log("Processed ingredients cannot be placed on the cutting board.");
                return;
            }
            // Only place if the cutting board is available and the player is holding an ingredient
            if (!cuttingScript.ishere && spawnedObject != null)
            {
                // Detach the object from the player's hand
                spawnedObject.transform.SetParent(choppingScript.cookpoint);

                // Place the object on the board at the cookpoint
                spawnedObject.transform.position = choppingScript.cookpoint.position;
                spawnedObject.transform.rotation = choppingScript.cookpoint.rotation;

                // Mark the board as occupied and reset player grab state
                cuttingScript.ishere = true;

                // Release the reference to the held object and reset states
                spawnedObject = null;
                ResetGrabState();

                Debug.Log("Ingredient placed on cutting board and detached from player.");
            }
        }
    }

    private void PlaceOnPan(Collider other)
    {
        isfoodinhere panScript = other.GetComponent<isfoodinhere>();

        if (panScript != null && !panScript.ishere && isSalami == 1)
        {
            Transform cookpointTransform = other.transform.Find("cookpoint");
            if (cookpointTransform != null)
            {
                spawnedObject.transform.position = cookpointTransform.position;
                spawnedObject.transform.rotation = cookpointTransform.rotation;
                spawnedObject.transform.SetParent(cookpointTransform);

                FireController fireController = other.GetComponent<FireController>();
                if (fireController != null)
                {
                    fireController.currentIngredient = spawnedObject;
                    fireController.hasIngredient = true;
                    fireController.StartCooking();
                    Debug.Log("Cooking started for the salami.");
                }

                panScript.ishere = true;
                ResetGrabState();
                Debug.Log("Salami placed on the pan at cookpoint.");
            }
        }
    }

    private void DiscardItem()
    {
        if (spawnedObject != null)
        {
            Destroy(spawnedObject);
            ResetGrabState();
            Debug.Log("Item discarded.");
        }
    }

    private void ResetGrabState()
    {
        isgrab = 0;
        grabbed = false;
        isFish = 0;
        isCarrot = 0;
        isPepper = 0;
        isCucumber = 0;
        isNori = 0;
        isRice = 0;
        isSalami = 0;
        isExtinguisher = 0;
        isChoppedPepper = 0;
        isChoppedCucumber = 0;
        isChoppedFish = 0;
        isChoppedCarrot = 0;
        isCookedSalami = 0;
        isChopped = false;
        spawnedObject = null; // Clear the reference to the held object
    }

}
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class playergrab : MonoBehaviour
//{
//    private Animator animator;
//    private bool grabbed;
//    private int isgrab = 0;
//    GameObject spawnedObject = null;
//    public GameObject grabPoint;
//    public LightController lightController;
//    public int isFish = 0;
//    public int isCarrot = 0;
//    public int isPepper = 0;
//    public int isCucumber = 0;
//    public int isNori = 0;
//    public int isRice = 0;
//    public int isSalami = 0;
//    public int isExtinhuisher = 0;
//    public int isChoppedFish = 0;
//    public int isChoppedCarrot = 0;
//    public int isChoppedPepper = 0;
//    public int isChoppedCucumber = 0;
//    public GameObject knifePrefab;
//    GameObject knife = null;

//    // Reference to the chopping board (for placing ingredients)
//    public Chopping choppingScript;

//    void Start()
//    {
//        lightController = GameObject.FindWithTag("Lever").GetComponent<LightController>();
//        animator = GetComponent<Animator>();
//        grabbed = false;
//    }

//    void Update()
//    {
//        animator.SetBool("holdingItem", grabbed);
//    }

//    void OnTriggerStay(Collider other)
//    {
//        if (isgrab == 0)
//        {
//            if (Input.GetKey(KeyCode.Space))
//            {
//                // Determine which prefab to instantiate based on the tag
//                string prefabName = "";
//                if (other.CompareTag("Fish")) { prefabName = "Fish"; isFish = 1; }
//                else if (other.CompareTag("Carrot")) { prefabName = "Carrot"; isCarrot = 1; }
//                else if (other.CompareTag("Pepper")) { prefabName = "Pepper"; isPepper = 1; }
//                else if (other.CompareTag("Cucumber")) { prefabName = "Cucumber"; isCucumber = 1; }
//                else if (other.CompareTag("Nori")) { prefabName = "Nori_001"; isNori = 1; }
//                else if (other.CompareTag("Rice")) { prefabName = "Rice_001"; isRice = 1; }
//                else if (other.CompareTag("Salami")) { prefabName = "Salami A"; isSalami = 1; }
//                else if (other.CompareTag("Extinguisher")) { prefabName = "Extinguisher"; isExtinhuisher = 1; }
//                else if (other.CompareTag("ChoppedFish")) { prefabName = "choppedFish"; isChoppedFish = 1; }
//                else if (other.CompareTag("ChoppedCarrot")) { prefabName = "choppedCarrot"; isChoppedCarrot = 1; }
//                else if (other.CompareTag("ChoppedPepper")) { prefabName = "choppedPepper"; isChoppedPepper = 1; }
//                else if (other.CompareTag("ChoppedCucumber")) { prefabName = "choppedCucumber"; isChoppedCucumber = 1; }

//                if (!string.IsNullOrEmpty(prefabName))
//                {
//                    GameObject prefab = Resources.Load<GameObject>(prefabName);
//                    if (prefab != null)
//                    {
//                        spawnedObject = Instantiate(prefab, grabPoint.transform.position, grabPoint.transform.rotation);
//                        spawnedObject.transform.parent = grabPoint.transform; // Set the grabPoint as the parent
//                        if (prefabName == "Extinguisher")
//                        {
//                            spawnedObject.transform.localPosition = new Vector3(0.0f, 0.0f, -1.25f);
//                            spawnedObject.transform.Rotate(90, 0, 0);
//                        }
//                        isgrab = 1;
//                        grabbed = true;

//                        // Destroy the object on the board after picking up a chopped ingredient and reset the board state
//                        if (prefabName.StartsWith("chopped"))
//                        {
//                            Destroy(other.gameObject);

//                            // Reset the chopping board's "ishere" status to allow new items to be placed
//                            isfoodinhere cuttingScript = other.GetComponentInParent<isfoodinhere>();
//                            if (cuttingScript != null)
//                            {
//                                cuttingScript.ishere = false;
//                                Debug.Log("Chopping board is now available for new items.");
//                            }

//                            Debug.Log($"{prefabName} picked up from chopping board.");
//                        }
//                    }
//                }

//                if (other.CompareTag("Lever"))
//                {
//                    lightController.SetSkyboxLighting();
//                }
//            }
//        }
//        else
//        {
//            // Handle placing the object on the cutting board or pan
//            if (Input.GetKey(KeyCode.Space))
//            {
//                if (other.CompareTag("ConveyorBelt"))
//                {
//                    // Place the object on the conveyor belt's placePoint
//                    Transform placePoint = other.transform.Find("placePoint");
//                    if (placePoint != null && spawnedObject != null)
//                    {
//                        // Move the object to the placePoint position and align rotation
//                        spawnedObject.transform.position = placePoint.position;
//                        spawnedObject.transform.rotation = placePoint.rotation;
//                        spawnedObject.transform.parent = other.transform; // Parent to conveyor if needed

//                        // Add object to conveyor's onBelt list and freeze rotation
//                        ConveyorBelt conveyorScript = other.GetComponent<ConveyorBelt>();
//                        if (conveyorScript != null)
//                        {
//                            conveyorScript.onBelt.Add(spawnedObject);
//                        }

//                        Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
//                        if (rb != null)
//                        {
//                            rb.constraints = RigidbodyConstraints.FreezeRotation;
//                        }

//                        // Reset player grab states
//                        isgrab = 0;
//                        grabbed = false;
//                        spawnedObject = null;

//                        Debug.Log("Object placed on conveyor belt at placePoint.");
//                    }
//                    else
//                    {
//                        Debug.LogWarning("placePoint not found on the conveyor belt.");
//                    }
//                }
//                if (other.CompareTag("cutting"))
//                {
//                    Chopping choppingScript = other.GetComponent<Chopping>();
//                    isfoodinhere cuttingScript = other.GetComponent<isfoodinhere>();

//                    if (cuttingScript != null && choppingScript != null)
//                    {
//                        // Proceed only if there is no item already on the chopping board
//                        if (cuttingScript.ishere == false && isFish == 1)
//                        {
//                            choppingScript.PlaceIngredientOnCookpoint(spawnedObject);
//                            isFish = 0;
//                            isgrab = 0;
//                            grabbed = false;
//                            spawnedObject = null;
//                            cuttingScript.ishere = true; // Mark the board as occupied
//                            Debug.Log("Fish placed on the chopping board.");
//                        }
//                        else if (cuttingScript.ishere == false && isCarrot == 1)
//                        {
//                            choppingScript.PlaceIngredientOnCookpoint(spawnedObject);
//                            spawnedObject.transform.Rotate(90, 90, 0);
//                            isCarrot = 0;
//                            isgrab = 0;
//                            grabbed = false;
//                            spawnedObject = null;
//                            cuttingScript.ishere = true; // Mark the board as occupied
//                        }
//                        else if (cuttingScript.ishere == false && isPepper == 1)
//                        {
//                            choppingScript.PlaceIngredientOnCookpoint(spawnedObject);
//                            spawnedObject.transform.Rotate(90, 180, 0);
//                            isPepper = 0;
//                            isgrab = 0;
//                            grabbed = false;
//                            spawnedObject = null;
//                            cuttingScript.ishere = true;
//                        }
//                        else if (cuttingScript.ishere == false && isCucumber == 1)
//                        {
//                            choppingScript.PlaceIngredientOnCookpoint(spawnedObject);
//                            isCucumber = 0;
//                            isgrab = 0;
//                            grabbed = false;
//                            spawnedObject = null;
//                            cuttingScript.ishere = true;
//                        }
//                    }
//                }
//                else if (other.CompareTag("Pan"))
//                {
//                    // Handle placing the Salami (Sausage) on the pan
//                    isfoodinhere panScript = other.GetComponent<isfoodinhere>();

//                    if (panScript != null && panScript.ishere == false && isSalami == 1)
//                    {
//                        GameObject prefab = Resources.Load<GameObject>("Salami A");
//                        Transform cookpointTransform = other.transform.Find("cookpoint");

//                        if (cookpointTransform != null)
//                        {
//                            GameObject placedSalami = Instantiate(prefab, cookpointTransform.position, cookpointTransform.rotation);
//                            placedSalami.transform.SetParent(cookpointTransform);

//                            isSalami = 0;
//                            isgrab = 0;
//                            grabbed = false;
//                            panScript.ishere = true;
//                            Debug.Log("Salami placed on the pan at cookpoint.");

//                            FireController fireController = other.GetComponent<FireController>();
//                            if (fireController != null)
//                            {
//                                fireController.currentIngredient = placedSalami;
//                                fireController.hasIngredient = true;
//                                fireController.StartCooking();
//                                Debug.Log("Cooking started for the salami.");
//                            }

//                            if (spawnedObject != null)
//                            {
//                                Destroy(spawnedObject);
//                                spawnedObject = null;
//                            }
//                        }
//                        else
//                        {
//                            Debug.LogWarning("Cookpoint transform not found on the pan!");
//                        }
//                    }

//                    FireController fireControllerExtinguish = other.GetComponent<FireController>();
//                    if (fireControllerExtinguish != null && isExtinhuisher == 1 && fireControllerExtinguish.isOnFire)
//                    {
//                        fireControllerExtinguish.ExtinguishFire();
//                        Destroy(spawnedObject);
//                        isExtinhuisher = 0;
//                        isgrab = 0;
//                        grabbed = false;
//                        spawnedObject = null;
//                        Debug.Log("Fire extinguished!");
//                    }
//                }
//                else if (other.CompareTag("Trashcan"))
//                {
//                    if (spawnedObject != null)
//                    {
//                        Destroy(spawnedObject);
//                    }

//                    grabbed = false;
//                    isgrab = 0;
//                    isFish = 0;
//                    isCarrot = 0;
//                    isPepper = 0;
//                    isCucumber = 0;
//                    isNori = 0;
//                    isRice = 0;
//                    isSalami = 0;
//                    isExtinhuisher = 0;
//                    isChoppedFish = 0;
//                    isChoppedCarrot = 0;
//                    isChoppedPepper = 0;
//                    isChoppedCucumber = 0;
//                    spawnedObject = null;
//                }
//            }
//        }
//    }
//}
