using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    public playergrab playergrab;
    public GameObject fireEffect; 
    public GameObject fryingPan;  
    public GameObject extinguisher;
    public GameObject currentIngredient;
    public float burnTime = 1f;
    public bool hasIngredient = false;
    public bool isOnFire = false;  
    public float timeRange_1 = 60f;
    public float timeRange_2 = 120f;
    public float randomTime;

    void Start()
    {
        float initialDelay = 20.0f;

        StartCoroutine(DelayedStartFire(initialDelay));
    }

    IEnumerator DelayedStartFire(float delay)
    {
        // 초기 대기 시간만큼 대기
        yield return new WaitForSeconds(delay);

        // 대기 후 Fire 발생 루틴 시작
        StartCoroutine(TriggerFire());
    }

    IEnumerator TriggerFire()
    {
        while (true)
        {
            randomTime = Random.Range(timeRange_1, timeRange_2);
        
            // 설정된 시간만큼 대기
            yield return new WaitForSeconds(randomTime);

            // 대기 후 실행할 함수 호출
            StartFire();
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
        
        Debug.Log("Fire");
    }

    public void ExtinguishFire()
    {
        isOnFire = false;
        fireEffect.SetActive(false);
        
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
