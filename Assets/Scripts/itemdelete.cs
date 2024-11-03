using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemdelete : MonoBehaviour
{
    public float lifetime = 5f; // 오브젝트가 사라지기까지의 시간 (기본값 5초)

    void Start()
    {
        // 지정된 시간 후 오브젝트를 파괴
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("speedup"))
            {
                GameManager.instance.ActivateSpeedUp();
            }
            else if (gameObject.CompareTag("cookup"))
            {
                GameManager.instance.ActivateCookUp();
            }
            else if (gameObject.CompareTag("addmoney"))
            {
                GameManager.instance.ActivateAddMoney();
            }
            Destroy(gameObject);
        }
    }
}
