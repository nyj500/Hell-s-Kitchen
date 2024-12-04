//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI; // UI ����� ���� �ʿ�
//using UnityEngine.EventSystems; // ��ġ �̺�Ʈ�� ó���ϱ� ���� �ʿ�

//public class MobilePlayermovement : MonoBehaviour
//{
//    private SprintDustController dustController;

//    private Animator animator;
//    public float speed; // �̵� �ӵ�
//    private Rigidbody rb;
//    public bool canMove = true;
//    private bool alreadyup = false;
//    private AudioSource audioSource;
//    public AudioClip footstepClip;

//    private Vector3 movement = Vector3.zero; // �̵� ����
//    private Quaternion targetRotation; // ��ǥ ȸ�� ����
//    private bool isMoving = false;

//    public Button sprintButton; // �޸��� ��ư �߰�
//    private bool isSprinting = false; // �޸��� ���¸� ����

//    void Start()
//    {
//        dustController = GetComponentInChildren<SprintDustController>();
//        rb = GetComponent<Rigidbody>();
//        animator = GetComponent<Animator>();

//        GameObject audioObject = GameObject.Find("AudioSourceObject2");
//        if (audioObject != null)
//        {
//            audioSource = audioObject.GetComponent<AudioSource>();
//            audioSource.clip = footstepClip;
//        }
//        else
//        {
//            Debug.LogError("Object with the specified name not found.");
//        }

//        // �޸��� ��ư �̺�Ʈ ������ ����
//        if (sprintButton != null)
//        {
//            EventTrigger trigger = sprintButton.gameObject.AddComponent<EventTrigger>();

//            // PointerDown �̺�Ʈ �߰�
//            EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
//            pointerDownEntry.eventID = EventTriggerType.PointerDown;
//            pointerDownEntry.callback.AddListener((eventData) => StartSprint());
//            trigger.triggers.Add(pointerDownEntry);

//            // PointerUp �̺�Ʈ �߰�
//            EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
//            pointerUpEntry.eventID = EventTriggerType.PointerUp;
//            pointerUpEntry.callback.AddListener((eventData) => StopSprint());
//            trigger.triggers.Add(pointerUpEntry);
//        }
//    }

//    void Update()
//    {
//        if (!canMove)
//        {
//            return;
//        }

//        // �ӵ� ó��
//        if (speed < 3)
//        {
//            if (GameManager.instance.currentPlayer == GameManager.PlayerType.player1)
//                speed = 3f;
//            else if (GameManager.instance.currentPlayer == GameManager.PlayerType.player2)
//                speed = 3.5f;
//            else if (GameManager.instance.currentPlayer == GameManager.PlayerType.player3)
//                speed = 4f;
//        }
//        if (speed > 6)
//        {
//            speed = 6;
//        }

//        if (GameManager.instance.isspeedup && !alreadyup)
//        {
//            speed *= 1.5f;
//            alreadyup = true;
//        }
//        else if (!GameManager.instance.isspeedup && alreadyup)
//        {
//            speed /= 1.5f;
//            alreadyup = false;
//        }

//        // �̵� ó��
//        if (movement != Vector3.zero)
//        {
//            isMoving = true;
//            var velocity = movement * speed;
//            targetRotation = Quaternion.LookRotation(movement);

//            // �̵� �� ȸ��
//            Vector3 newPosition = rb.position + velocity * Time.deltaTime;
//            rb.MovePosition(newPosition);
//            transform.rotation = targetRotation;

//            animator.SetFloat("speed", velocity.magnitude);
//        }
//        else
//        {
//            isMoving = false;
//        }

//        // ���� ó��
//        if (isMoving)
//        {
//            if (!audioSource.isPlaying)
//            {
//                audioSource.Play();
//            }
//        }
//        else
//        {
//            if (audioSource.isPlaying)
//            {
//                audioSource.Stop();
//            }
//        }
//    }

//    // �޸��� ����
//    public void StartSprint()
//    {
//        if (!isSprinting && isMoving) // Only start sprint if the player is moving
//        {
//            isSprinting = true;
//            speed *= 2; // Double the speed for sprinting
//            audioSource.pitch = 1.8f; // Increase audio pitch for sprinting sound
//            animator.SetBool("shift", true); // Enable sprinting animation

//            // Play the dust effect only once
//            if (dustController != null)
//            {
//                dustController.PlayDustOnce();
//            }
//        }
//    }

//    public void StopSprint()
//    {
//        if (isSprinting)
//        {
//            isSprinting = false;
//            speed /= 2;
//            audioSource.pitch = 1.3f;
//            animator.SetBool("shift", false);
//            if (dustController != null)
//            {
//                dustController.StopDust();
//            }
//        }
//    }

//    // �̵� ���� ����
//    public void SetMovement(Vector3 direction)
//    {
//        movement = direction;
//    }

//    // ��ư���� ���� ���� �� ����
//    public void StopMovement()
//    {
//        movement = Vector3.zero;
//    }

//    public void MoveUp()
//    {
//        SetMovement(Vector3.back); // ���� �������� �̵�
//    }

//    public void MoveDown()
//    {
//        SetMovement(Vector3.forward); // �Ʒ��� �������� �̵�
//    }

//    public void MoveLeft()
//    {
//        SetMovement(Vector3.right); // ���� �������� �̵�
//    }

//    public void MoveRight()
//    {
//        SetMovement(Vector3.left); // ������ �������� �̵�
//    }
//}





using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobilePlayermovement : MonoBehaviour
{
    private Animator animator; // Animator for controlling animations
    public float speed = 3f; // Movement speed
    private Rigidbody rb; // Rigidbody for physics-based movement
    public bool canMove = true; // Flag to enable/disable movement

    private Vector3 movement = Vector3.zero; // Current movement direction
    private Quaternion targetRotation; // Target rotation direction
    private bool isMoving = false; // Tracks if the player is moving
    private bool isSprinting = false; // Tracks if the player is sprinting

    private AudioSource audioSource; // Audio source for footstep sounds
    public AudioClip footstepClip; // Footstep audio clip

    public Button sprintButton; // Sprint button for mobile
    private SprintDustController dustController; // Reference to sprint dust particle controller

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        dustController = GetComponentInChildren<SprintDustController>();

        GameObject audioObject = GameObject.Find("AudioSourceObject2");
        if (audioObject != null)
        {
            audioSource = audioObject.GetComponent<AudioSource>();
            audioSource.clip = footstepClip;
        }
        else
        {
            Debug.LogError("AudioSourceObject2 not found in the scene!");
        }

        // Add sprint button events
        if (sprintButton != null)
        {
            EventTrigger trigger = sprintButton.gameObject.AddComponent<EventTrigger>();

            // Pointer Down (Start Sprint)
            EventTrigger.Entry pointerDown = new EventTrigger.Entry();
            pointerDown.eventID = EventTriggerType.PointerDown;
            pointerDown.callback.AddListener((eventData) => StartSprint());
            trigger.triggers.Add(pointerDown);

            // Pointer Up (Stop Sprint)
            EventTrigger.Entry pointerUp = new EventTrigger.Entry();
            pointerUp.eventID = EventTriggerType.PointerUp;
            pointerUp.callback.AddListener((eventData) => StopSprint());
            trigger.triggers.Add(pointerUp);
        }
    }

    void Update()
    {
        if (!canMove) return;

        if (movement != Vector3.zero)
        {
            isMoving = true;
            targetRotation = Quaternion.LookRotation(movement);

            // Apply movement
            Vector3 velocity = movement * speed;
            Vector3 newPosition = rb.position + velocity * Time.deltaTime;
            rb.MovePosition(newPosition);
            transform.rotation = targetRotation;

            // Update animation
            animator.SetFloat("speed", velocity.magnitude);
        }
        else
        {
            isMoving = false;
            animator.SetFloat("speed", 0f); // Stop movement animation
        }

        // Handle footstep sounds
        HandleFootstepSound();
    }

    public void MoveUp()
    {
        SetMovement(Vector3.back); // Move upward (negative Z-axis)
    }

    public void MoveDown()
    {
        SetMovement(Vector3.forward); // Move downward (positive Z-axis)
    }

    public void MoveLeft()
    {
        SetMovement(Vector3.right); // Move left (negative X-axis)
    }

    public void MoveRight()
    {
        SetMovement(Vector3.left); // Move right (positive X-axis)
    }

    public void StopMovement()
    {
        SetMovement(Vector3.zero); // Stop movement
    }

    private void SetMovement(Vector3 direction)
    {
        movement = direction;

        if (movement != Vector3.zero)
        {
            isMoving = true;
            animator.SetFloat("speed", speed); // Play movement animation
        }
        else
        {
            isMoving = false;
            animator.SetFloat("speed", 0f); // Stop movement animation
        }
    }

    public void StartSprint()
    {
        if (!isSprinting && isMoving) // Only sprint if moving
        {
            isSprinting = true;
            speed *= 2;
            audioSource.pitch = 1.8f;
            animator.SetBool("shift", true);

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

    private void HandleFootstepSound()
    {
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
}
