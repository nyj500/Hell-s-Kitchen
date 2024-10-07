using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermovement : MonoBehaviour
{

    public float speed = 5f;            // �̵� �ӵ�
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // �̵� ���� �ʱ�ȭ
        Vector3 movement = Vector3.zero;
        Quaternion targetRotation = transform.rotation; // �⺻������ ���� ȸ�� ����

        // W �Ǵ� UpArrow Ű �Է� �� ����(+Z)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            movement = Vector3.back;
            targetRotation = Quaternion.LookRotation(Vector3.back); // ����(+Z) �ٶ�
        }
        // S �Ǵ� DownArrow Ű �Է� �� ����(-Z)
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            movement = Vector3.forward;
            targetRotation = Quaternion.LookRotation(Vector3.forward); // ����(-Z) �ٶ�
        }
        // A �Ǵ� LeftArrow Ű �Է� �� ����(-X)
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            movement = Vector3.right;
            targetRotation = Quaternion.LookRotation(Vector3.right); // ����(-X) �ٶ�
        }
        // D �Ǵ� RightArrow Ű �Է� �� ����(+X)
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            movement = Vector3.left;
            targetRotation = Quaternion.LookRotation(Vector3.left); // ����(+X) �ٶ�
        }

        // ȸ�� ����
        if (movement != Vector3.zero)
        {
            transform.rotation = targetRotation;

            // �̵�
            Vector3 newPosition = rb.position + movement * speed * Time.deltaTime;
            rb.MovePosition(newPosition);
        }
    }
}
