using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playergrab : MonoBehaviour
{
    private int isgrab =0;
    GameObject spawnedObject = null;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if(isgrab == 0)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (other.CompareTag("Fish"))
                {
                    GameObject prefab = Resources.Load<GameObject>("Fish");
                    spawnedObject = Instantiate(prefab);
                    foodmove script = spawnedObject.GetComponent<foodmove>();
                    if (script != null)
                    {
                        Transform childTransform = this.transform.Find("grabpoint");
                        script.target = childTransform.gameObject;

                    }
                    isgrab = 1;
                    isFish = 1;
                }

                if (other.CompareTag("Carrot"))
                {
                    GameObject prefab = Resources.Load<GameObject>("Carrot");
                    spawnedObject = Instantiate(prefab);
                    foodmove script = spawnedObject.GetComponent<foodmove>();
                    if (script != null)
                    {
                        Transform childTransform = this.transform.Find("grabpoint");
                        script.target = childTransform.gameObject;

                    }
                    isgrab = 1;
                    isCarrot = 1;
                }

                if (other.CompareTag("Pepper"))
                {
                    GameObject prefab = Resources.Load<GameObject>("Pepper");
                    spawnedObject = Instantiate(prefab);
                    foodmove script = spawnedObject.GetComponent<foodmove>();
                    if (script != null)
                    {
                        Transform childTransform = this.transform.Find("grabpoint");
                        script.target = childTransform.gameObject;

                    }
                    isgrab = 1;
                    isPepper = 1;
                }

                if (other.CompareTag("Cucumber"))
                {
                    GameObject prefab = Resources.Load<GameObject>("Cucumber");
                    spawnedObject = Instantiate(prefab);
                    foodmove script = spawnedObject.GetComponent<foodmove>();
                    if (script != null)
                    {
                        Transform childTransform = this.transform.Find("grabpoint");
                        script.target = childTransform.gameObject;

                    }
                    isgrab = 1;
                    isCucumber = 1;
                }

                if (other.CompareTag("Nori"))
                {
                    GameObject prefab = Resources.Load<GameObject>("Nori_001");
                    spawnedObject = Instantiate(prefab);
                    foodmove script = spawnedObject.GetComponent<foodmove>();
                    if (script != null)
                    {
                        Transform childTransform = this.transform.Find("grabpoint");
                        script.target = childTransform.gameObject;

                    }
                    isgrab = 1;
                    isNori = 1;
                }

                if (other.CompareTag("Rice"))
                {
                    GameObject prefab = Resources.Load<GameObject>("Rice_001");
                    spawnedObject = Instantiate(prefab);
                    foodmove script = spawnedObject.GetComponent<foodmove>();
                    if (script != null)
                    {
                        Transform childTransform = this.transform.Find("grabpoint");
                        script.target = childTransform.gameObject;

                    }
                    isgrab = 1;
                    isRice = 1;
                }

                if (other.CompareTag("Salami"))
                {
                    GameObject prefab = Resources.Load<GameObject>("Salami A");
                    spawnedObject = Instantiate(prefab);
                    foodmove script = spawnedObject.GetComponent<foodmove>();
                    if (script != null)
                    {
                        Transform childTransform = this.transform.Find("grabpoint");
                        script.target = childTransform.gameObject;

                    }
                    isgrab = 1;
                    isSalami = 1;
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
