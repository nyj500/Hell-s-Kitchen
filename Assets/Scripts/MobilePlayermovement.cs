using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 사용을 위해 필요
using UnityEngine.EventSystems; // 터치 이벤트를 처리하기 위해 필요

public class MobilePlayermovement : MonoBehaviour
{
    private Animator animator;
    public float speed; // 이동 속도
    private Rigidbody rb;
    public bool canMove = true;
    private bool alreadyup = false;
    private AudioSource audioSource;
    public AudioClip footstepClip;

    private Vector3 movement = Vector3.zero; // 이동 방향
    private Quaternion targetRotation; // 목표 회전 방향
    private bool isMoving = false;

    public Button sprintButton; // 달리기 버튼 추가
    private bool isSprinting = false; // 달리기 상태를 추적

    void Start()
    {
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

        // 달리기 버튼 이벤트 리스너 설정
        if (sprintButton != null)
        {
            EventTrigger trigger = sprintButton.gameObject.AddComponent<EventTrigger>();

            // PointerDown 이벤트 추가
            EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
            pointerDownEntry.eventID = EventTriggerType.PointerDown;
            pointerDownEntry.callback.AddListener((eventData) => StartSprint());
            trigger.triggers.Add(pointerDownEntry);

            // PointerUp 이벤트 추가
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

        // 속도 처리
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

        // 이동 처리
        if (movement != Vector3.zero)
        {
            isMoving = true;
            var velocity = movement * speed;
            targetRotation = Quaternion.LookRotation(movement);

            // 이동 및 회전
            Vector3 newPosition = rb.position + velocity * Time.deltaTime;
            rb.MovePosition(newPosition);
            transform.rotation = targetRotation;

            animator.SetFloat("speed", velocity.magnitude);
        }
        else
        {
            isMoving = false;
        }

        // 사운드 처리
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

    // 달리기 시작
    public void StartSprint()
    {
        if (!isSprinting)
        {
            isSprinting = true;
            speed *= 2; // 속도 증가
            audioSource.pitch = 1.8f; // 발소리 속도 증가
            animator.SetBool("shift", true); // 달리기 애니메이션 활성화
        }
    }

    // 달리기 중지
    public void StopSprint()
    {
        if (isSprinting)
        {
            isSprinting = false;
            speed /= 2; // 속도 복원
            audioSource.pitch = 1.3f; // 발소리 속도 복원
            animator.SetBool("shift", false); // 달리기 애니메이션 비활성화
        }
    }

    // 이동 방향 설정
    public void SetMovement(Vector3 direction)
    {
        movement = direction;
    }

    // 버튼에서 손을 뗐을 때 정지
    public void StopMovement()
    {
        movement = Vector3.zero;
    }

    public void MoveUp()
    {
        SetMovement(Vector3.back); // 위쪽 방향으로 이동
    }

    public void MoveDown()
    {
        SetMovement(Vector3.forward); // 아래쪽 방향으로 이동
    }

    public void MoveLeft()
    {
        SetMovement(Vector3.right); // 왼쪽 방향으로 이동
    }

    public void MoveRight()
    {
        SetMovement(Vector3.left); // 오른쪽 방향으로 이동
    }
}





