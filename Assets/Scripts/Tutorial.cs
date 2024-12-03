using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorialChat; 
    public TMP_Text contents; 
    public Image image; 

    public float typingSpeed = 0.02f;

    private bool isInteractable = false;
    private Coroutine hideCoroutine;
    private Coroutine typingCoroutine;
    private bool isTyping;

    private Queue<string> messageQueue = new Queue<string>(); 
    private bool isMessageQueueRunning = false;

    void Start()
    {
        if (GameManager.instance.currentStage == 0)
        {
            EnqueueMessage("Check the order in the upper left corner.", false, 5f);
            EnqueueMessage("Place the vegetables and fish on the cutting board and press the \"E\" key to chop..", false, 10f);
            EnqueueMessage("Or, place the sausages in a frying pan and take them out when the time is up.", false, 8f);
            EnqueueMessage("Plate the seaweed and rice. The order of the plates of the ingredients does not matter.", false, 6f);
            EnqueueMessage("Kimbap is made when 4 ingredients, including seaweed and rice, are placed together.", false, 6f);
            EnqueueMessage("Submit your kimbap and get paid.", false, 4f);
        }
        else
        {
            tutorialChat.SetActive(false);
        }
    }

    public void EnqueueMessage(string message, bool requireInteraction, float time)
    {
        messageQueue.Enqueue(message); // 메시지 추가
        if (!isMessageQueueRunning)
        {
            StartCoroutine(ProcessMessageQueue(requireInteraction, time));
        }
    }

    private IEnumerator ProcessMessageQueue(bool requireInteraction, float time)
    {
        isMessageQueueRunning = true;

        while (messageQueue.Count > 0)
        {
            string currentMessage = messageQueue.Dequeue();
            ShowChatBubble(currentMessage, requireInteraction);
            yield return new WaitUntil(() => !isTyping);

            // 상호작용이 필요한 경우 사용자가 처리할 때까지 대기
            if (requireInteraction)
            {
                yield return new WaitUntil(() => !isInteractable);
            }
            else
            {
                // 상호작용 필요 없을 경우 자동 숨김 시간 대기
                yield return new WaitForSeconds(time);
                // HideChatBubble();
            }
        }

        isMessageQueueRunning = false;

        tutorialChat.SetActive(false);
    }

    public void ShowChatBubble(string message, bool requireInteraction)
    {
        tutorialChat.SetActive(true);

        // 상호작용 여부 설정
        isInteractable = requireInteraction;

        // 이전에 실행 중이던 코루틴 중지
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        // 상호작용이 필요 없다면 일정 시간이 지난 후 자동으로 숨김
        // if (!requireInteraction)
        // {
        //     hideCoroutine = StartCoroutine(HideAfterTime(displayTime));
        // }

        // 타이핑 시작
        typingCoroutine = StartCoroutine(TypeText(message));
    }

    private IEnumerator HideAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        tutorialChat.SetActive(false);
    }

    private IEnumerator TypeText(string message)
    {
        isTyping = true;
        contents.text = ""; // 기존 텍스트 초기화

        foreach (char letter in message)
        {
            contents.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false; // 타이핑 완료
    }
}
