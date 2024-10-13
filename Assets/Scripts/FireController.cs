using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    public GameObject fireEffect; 
    public GameObject fryingPan;  
    public GameObject extinguisher;
    public GameObject currentIngredient;
    public float burnTime = 1f;
    public bool hasIngredient = false;
    private bool isOnFire = false;  
    private bool isExtinguisherHeld = false;

    void Update()
    {
        // 화재 조건
        if (/* 임의의 조건 */ !isOnFire && Input.GetKeyDown(KeyCode.B))
        {
            StartFire(); 
        }
        // 소화기를 들고 상호작용
        if (/* 임의의 조건 */ isOnFire && Input.GetKeyDown(KeyCode.N))
        {
            ExtinguishFire();
        }
    }

    void StartFire()
    {
        isOnFire = true;
        fireEffect.SetActive(true);
        if (hasIngredient)
        {
            StartCoroutine(BurnIngredient());
        }
            // 프라이팬 사용 불가 
        Debug.Log("Fire");
    }

    void ExtinguishFire()
    {
        isOnFire = false;
        fireEffect.SetActive(false);
            // 프라이팬 사용 가능 
        Debug.Log("Extinguish");
    }

    IEnumerator BurnIngredient()
    {
        yield return new WaitForSeconds(burnTime);
        if (isOnFire && currentIngredient != null)  
        {
            Destroy(currentIngredient);
            hasIngredient = false;
        }
    }
}
