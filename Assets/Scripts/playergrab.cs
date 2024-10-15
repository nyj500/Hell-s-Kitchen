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
    public FireController fireController;
    public int isFish = 0;
    public int isCarrot = 0;
    public int isPepper = 0;
    public int isCucumber = 0;
    public int isNori = 0;
    public int isRice = 0;
    public int isSalami = 0;
    public int isExtinhuisher = 0;
    public bool isOnFire = false;
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
            if (Input.GetKey(KeyCode.Space))
            {
                if (other.CompareTag("Trashcan"))
                {
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
                    spawnedObject = null;
                }
                else if (other.CompareTag("Pan") && !isOnFire)
                {
                    isfoodinhere panScript = other.GetComponent<isfoodinhere>();

                    if (panScript.ishere == false && isSalami == 1)
                    {
                        GameObject prefab = Resources.Load<GameObject>("Salami A");
                        Destroy(spawnedObject);
                        Transform cookpointTransform = other.transform.Find("cookpoint");
                        spawnedObject = Instantiate(prefab, cookpointTransform.position, cookpointTransform.rotation);
                        isSalami = 0;
                        isgrab = 0;
                        grabbed = false;
                        spawnedObject = null;
                        panScript.ishere = true;
                    }                 
                }
                else if (other.CompareTag("Pan") && isOnFire)
                {
                    if (isExtinhuisher == 1)
                    {
                        fireController.ExtinguishFire();
                        other.enabled = true;
                        Destroy(spawnedObject);
                        isExtinhuisher = 0;
                        isgrab = 0;
                        grabbed = false;
                        spawnedObject = null;
                        isOnFire = false;
                    }
                }
                else if (other.CompareTag("cutting"))
                {
                    isfoodinhere cuttingScript = other.GetComponent<isfoodinhere>();

                    if (cuttingScript.ishere == false && isFish == 1)
                    {
                        GameObject prefab = Resources.Load<GameObject>("Fish");
                        Destroy(spawnedObject);
                        Transform cookpointTransform = other.transform.Find("cookpoint");
                        spawnedObject = Instantiate(prefab, cookpointTransform.position, cookpointTransform.rotation);
                        isFish = 0;
                        isgrab = 0;
                        grabbed = false;
                        spawnedObject = null;
                        cuttingScript.ishere = true;
                    }
                    else if (cuttingScript.ishere == false && isCarrot == 1)
                    {
                        GameObject prefab = Resources.Load<GameObject>("Carrot");
                        Destroy(spawnedObject);
                        Transform cookpointTransform = other.transform.Find("cookpoint");
                        spawnedObject = Instantiate(prefab, cookpointTransform.position, cookpointTransform.rotation);
                        spawnedObject.transform.Rotate(90, 90, 0);
                        isCarrot = 0;
                        isgrab = 0;
                        grabbed = false;
                        spawnedObject = null;
                        cuttingScript.ishere = true;
                    }
                    else if (cuttingScript.ishere == false && isPepper == 1)
                    {
                        GameObject prefab = Resources.Load<GameObject>("Pepper");
                        Destroy(spawnedObject);
                        Transform cookpointTransform = other.transform.Find("cookpoint");
                        spawnedObject = Instantiate(prefab, cookpointTransform.position, cookpointTransform.rotation);
                        spawnedObject.transform.Rotate(90, 180, 0);
                        isPepper = 0;
                        isgrab = 0;
                        grabbed = false;
                        spawnedObject = null;
                        cuttingScript.ishere = true;
                    }
                    else if (cuttingScript.ishere == false && isCucumber == 1)
                    {
                        GameObject prefab = Resources.Load<GameObject>("Cucumber");
                        Destroy(spawnedObject);
                        Transform cookpointTransform = other.transform.Find("cookpoint");
                        spawnedObject = Instantiate(prefab, cookpointTransform.position, cookpointTransform.rotation);
                        isCucumber = 0;
                        isgrab = 0;
                        grabbed = false;
                        spawnedObject = null;
                        cuttingScript.ishere = true;
                    }
                }
            }
        }
    }
}
