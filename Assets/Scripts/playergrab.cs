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
    public int isExtinhuisher = 0;
    public GameObject knifePrefab;
    GameObject knife = null;

    // Reference to the chopping board (for placing ingredients)
    public Chopping choppingScript;

    void Start()
    {
        animator = GetComponent<Animator>();
        grabbed = false;
    }

    void Update()
    {
        animator.SetBool("holdingItem", grabbed);
    }

    void OnTriggerStay(Collider other)
    {
        if (isgrab == 0)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                // Determine which prefab to instantiate based on the tag
                string prefabName = "";
                if (other.CompareTag("Fish")) { prefabName = "Fish"; isFish = 1; }
                else if (other.CompareTag("Carrot")) { prefabName = "Carrot"; isCarrot = 1; }
                else if (other.CompareTag("Pepper")) { prefabName = "Pepper"; isPepper = 1; }
                else if (other.CompareTag("Cucumber")) { prefabName = "Cucumber"; isCucumber = 1; }
                else if (other.CompareTag("Nori")) { prefabName = "Nori_001"; isNori = 1; }
                else if (other.CompareTag("Rice")) { prefabName = "Rice_001"; isRice = 1; }
                else if (other.CompareTag("Salami")) { prefabName = "Salami A"; isSalami = 1; }
                else if (other.CompareTag("Extinguisher")) { prefabName = "Extinguisher"; isExtinhuisher = 1; }

                if (!string.IsNullOrEmpty(prefabName))
                {
                    GameObject prefab = Resources.Load<GameObject>(prefabName);
                    if (prefab != null)
                    {
                        spawnedObject = Instantiate(prefab, grabPoint.transform.position, grabPoint.transform.rotation);
                        spawnedObject.transform.parent = grabPoint.transform; // Set the grabPoint as the parent
                        if (prefabName == "Extinguisher")
                        {
                            spawnedObject.transform.localPosition = new Vector3(0.0f, 0.0f, -1.25f);
                            spawnedObject.transform.Rotate(90, 0, 0);
                        }
                        isgrab = 1;
                        grabbed = true;
                    }
                }

                if (other.CompareTag("Lever"))
                {
                    lightController.SetSkyboxLighting();
                }
            }
        }
        else
        {
            // Handle placing the object on the cutting board or pan
            if (Input.GetKey(KeyCode.Space))
            {
                if (other.CompareTag("cutting"))
                {
                    // Dynamically get the `Chopping` script from the specific cutting board you're interacting with
                    Chopping choppingScript = other.GetComponent<Chopping>();
                    isfoodinhere cuttingScript = other.GetComponent<isfoodinhere>();

                    if (cuttingScript != null && choppingScript != null)
                    {
                        // Proceed only if there is no item already on the chopping board
                        if (cuttingScript.ishere == false && isFish == 1)
                        {
                            choppingScript.PlaceIngredientOnCookpoint(spawnedObject);
                            // Optionally rotate or modify the fish placement if needed
                            isFish = 0;
                            isgrab = 0;
                            grabbed = false;
                            spawnedObject = null;
                            cuttingScript.ishere = true; // Mark the board as occupied
                            Debug.Log("Fish placed on the chopping board.");
                        }
                        else if (cuttingScript.ishere == false && isCarrot == 1)
                        {
                            choppingScript.PlaceIngredientOnCookpoint(spawnedObject);
                            spawnedObject.transform.Rotate(90, 90, 0);
                            isCarrot = 0;
                            isgrab = 0;
                            grabbed = false;
                            spawnedObject = null;
                            cuttingScript.ishere = true; // Mark the board as occupied
                        }
                        else if (cuttingScript.ishere == false && isPepper == 1)
                        {
                            choppingScript.PlaceIngredientOnCookpoint(spawnedObject);
                            spawnedObject.transform.Rotate(90, 180, 0);
                            isPepper = 0;
                            isgrab = 0;
                            grabbed = false;
                            spawnedObject = null;
                            cuttingScript.ishere = true;
                        }
                        else if (cuttingScript.ishere == false && isCucumber == 1)
                        {
                            choppingScript.PlaceIngredientOnCookpoint(spawnedObject);
                            isCucumber = 0;
                            isgrab = 0;
                            grabbed = false;
                            spawnedObject = null;
                            cuttingScript.ishere = true;
                        }
                    }
                    else
                    {
                        if (cuttingScript == null)
                        {
                            Debug.LogWarning("The object you are interacting with does not have the 'isfoodinhere' component.");
                        }
                        if (choppingScript == null)
                        {
                            Debug.LogWarning("The object you are interacting with does not have the 'Chopping' component.");
                        }
                    }
                }
                else if (other.CompareTag("Pan"))
                {
                    // Handle placing the Salami (Sausage) on the pan
                    isfoodinhere panScript = other.GetComponent<isfoodinhere>();

                    if (panScript != null && panScript.ishere == false && isSalami == 1)
                    {
                        // Spawn the salami prefab on the pan
                        GameObject prefab = Resources.Load<GameObject>("Salami A");
                        Destroy(spawnedObject);
                        Transform cookpointTransform = other.transform.Find("cookpoint");
                        spawnedObject = Instantiate(prefab, cookpointTransform.position, cookpointTransform.rotation);
                        isSalami = 0;
                        isgrab = 0;
                        grabbed = false;
                        spawnedObject = null;
                        panScript.ishere = true; // Mark the pan as occupied
                        Debug.Log("Salami placed on the pan.");
                    }

                    FireController fireController = other.GetComponent<FireController>();
                    if (fireController != null && isExtinhuisher == 1 && fireController.isOnFire)
                    {
                        // Extinguish the fire if the player is holding an extinguisher
                        fireController.ExtinguishFire();
                        Destroy(spawnedObject); // Remove the extinguisher from hand
                        isExtinhuisher = 0; // Reset extinguisher state
                        isgrab = 0;
                        grabbed = false;
                        spawnedObject = null;
                        Debug.Log("Fire extinguished!");
                    }
                }
                else if (other.CompareTag("Trashcan"))
                {
                    // If the player throws the object into the trash can
                    if (spawnedObject != null)
                    {
                        Destroy(spawnedObject);
                    }

                    // Reset states
                    grabbed = false;
                    isgrab = 0;
                    isFish = 0;
                    isCarrot = 0;
                    isPepper = 0;
                    isCucumber = 0;
                    isNori = 0;
                    isRice = 0;
                    isSalami = 0;
                    isExtinhuisher = 0; // Reset extinguisher state
                    spawnedObject = null;
                }
            }
        }
    }
}
