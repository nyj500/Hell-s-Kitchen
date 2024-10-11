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

    public int isFish = 0;
    public int isCarrot = 0;
    public int isPepper = 0;
    public int isCucumber = 0;
    public int isNori = 0;
    public int isRice = 0;
    public int isSalami = 0;
    public int isFire = 0;

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

                if (!string.IsNullOrEmpty(prefabName))
                {
                    GameObject prefab = Resources.Load<GameObject>(prefabName);
                    if (prefab != null)
                    {
                        spawnedObject = Instantiate(prefab, grabPoint.transform.position, grabPoint.transform.rotation);
                        spawnedObject.transform.parent = grabPoint.transform; // Set the grabPoint as the parent
                        isgrab = 1;
                        grabbed = true;
                    }
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
            }
        }
    }
}
