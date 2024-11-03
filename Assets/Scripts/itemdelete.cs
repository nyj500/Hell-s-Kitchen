using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemdelete : MonoBehaviour
{
    public float lifetime = 5f; // ������Ʈ�� ������������ �ð� (�⺻�� 5��)

    void Start()
    {
        // ������ �ð� �� ������Ʈ�� �ı�
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
