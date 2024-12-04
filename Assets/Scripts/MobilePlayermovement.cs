using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI ����� ���� �ʿ�
using UnityEngine.EventSystems; // ��ġ �̺�Ʈ�� ó���ϱ� ���� �ʿ�

public class MobilePlayermovement : MonoBehaviour
{
    private SprintDustController dustController;

    private Animator animator;
    public float speed; // �̵� �ӵ�
    private Rigidbody rb;
    public bool canMove = true;
    private bool alreadyup = false;
    private AudioSource audioSource;
    public AudioClip footstepClip;

    private Vector3 movement = Vector3.zero; // �̵� ����
    private Quaternion targetRotation; // ��ǥ ȸ�� ����
    private bool isMoving = false;

    public Button sprintButton; // �޸��� ��ư �߰�
    private bool isSprinting = false; // �޸��� ���¸� ����

    void Start()
    {
        dustController = GetComponentInChildren<SprintDustController>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        GameObject audioObject = GameObject.Find("AudioSourceObject2");
        if (audioObject != null)
        {
            audioSource = audioObject.GetComponent<AudioSource>();
            audioSource.clip = footstepClip;
        }
        else
        {
            Debug.LogError("Object with the specified name not found.");
        }

        // �޸��� ��ư �̺�Ʈ ������ ����
        if (sprintButton != null)
        {
            EventTrigger trigger = sprintButton.gameObject.AddComponent<EventTrigger>();

            // PointerDown �̺�Ʈ �߰�
            EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
            pointerDownEntry.eventID = EventTriggerType.PointerDown;
            pointerDownEntry.callback.AddListener((eventData) => StartSprint());
            trigger.triggers.Add(pointerDownEntry);

            // PointerUp �̺�Ʈ �߰�
            EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
            pointerUpEntry.eventID = EventTriggerType.PointerUp;
            pointerUpEntry.callback.AddListener((eventData) => StopSprint());
            trigger.triggers.Add(pointerUpEntry);
        }
    }

    void Update()
    {
        if (!canMove)
        {
            return;
        }

        // �ӵ� ó��
        if (speed < 3)
        {
            if (GameManager.instance.currentPlayer == GameManager.PlayerType.player1)
                speed = 3f;
            else if (GameManager.instance.currentPlayer == GameManager.PlayerType.player2)
                speed = 3.5f;
            else if (GameManager.instance.currentPlayer == GameManager.PlayerType.player3)
                speed = 4f;
        }
        if (speed > 6)
        {
            speed = 6;
        }

        if (GameManager.instance.isspeedup && !alreadyup)
        {
            speed *= 1.5f;
            alreadyup = true;
        }
        else if (!GameManager.instance.isspeedup && alreadyup)
        {
            speed /= 1.5f;
            alreadyup = false;
        }

        // �̵� ó��
        if (movement != Vector3.zero)
        {
            isMoving = true;
            var velocity = movement * speed;
            targetRotation = Quaternion.LookRotation(movement);

            // �̵� �� ȸ��
            Vector3 newPosition = rb.position + velocity * Time.deltaTime;
            rb.MovePosition(newPosition);
            transform.rotation = targetRotation;

            animator.SetFloat("speed", velocity.magnitude);
        }
        else
        {
            isMoving = false;
        }

        // ���� ó��
        if (isMoving)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    // �޸��� ����
    public void StartSprint()
    {
        if (!isSprinting && isMoving) // Only start sprint if the player is moving
        {
            isSprinting = true;
            speed *= 2; // Double the speed for sprinting
            audioSource.pitch = 1.8f; // Increase audio pitch for sprinting sound
            animator.SetBool("shift", true); // Enable sprinting animation

            // Play the dust effect only once
            if (dustController != null)
            {
                dustController.PlayDustOnce();
            }
        }
    }

    public void StopSprint()
    {
        if (isSprinting)
        {
            isSprinting = false;
            speed /= 2;
            audioSource.pitch = 1.3f;
            animator.SetBool("shift", false);
            if (dustController != null)
            {
                dustController.StopDust();
            }
        }
    }

    // �̵� ���� ����
    public void SetMovement(Vector3 direction)
    {
        movement = direction;
    }

    // ��ư���� ���� ���� �� ����
    public void StopMovement()
    {
        movement = Vector3.zero;
    }

    public void MoveUp()
    {
        SetMovement(Vector3.back); // ���� �������� �̵�
    }

    public void MoveDown()
    {
        SetMovement(Vector3.forward); // �Ʒ��� �������� �̵�
    }

    public void MoveLeft()
    {
        SetMovement(Vector3.right); // ���� �������� �̵�
    }

    public void MoveRight()
    {
        SetMovement(Vector3.left); // ������ �������� �̵�
    }
}





