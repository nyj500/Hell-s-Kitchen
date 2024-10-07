using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermovement : MonoBehaviour
{

    public float speed = 5f;            // 이동 속도
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // 이동 벡터 초기화
        Vector3 movement = Vector3.zero;
        Quaternion targetRotation = transform.rotation; // 기본적으로 현재 회전 유지

        // W 또는 UpArrow 키 입력 시 북쪽(+Z)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            movement = Vector3.back;
            targetRotation = Quaternion.LookRotation(Vector3.back); // 북쪽(+Z) 바라봄
        }
        // S 또는 DownArrow 키 입력 시 남쪽(-Z)
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            movement = Vector3.forward;
            targetRotation = Quaternion.LookRotation(Vector3.forward); // 남쪽(-Z) 바라봄
        }
        // A 또는 LeftArrow 키 입력 시 서쪽(-X)
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            movement = Vector3.right;
            targetRotation = Quaternion.LookRotation(Vector3.right); // 서쪽(-X) 바라봄
        }
        // D 또는 RightArrow 키 입력 시 동쪽(+X)
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            movement = Vector3.left;
            targetRotation = Quaternion.LookRotation(Vector3.left); // 동쪽(+X) 바라봄
        }

        // 회전 적용
        if (movement != Vector3.zero)
        {
            transform.rotation = targetRotation;

            // 이동
            Vector3 newPosition = rb.position + movement * speed * Time.deltaTime;
            rb.MovePosition(newPosition);
        }
    }
}
