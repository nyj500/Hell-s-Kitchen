using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI timeText; // TextMeshPro 텍스트 참조
    public Image orderDisplay; // 주문을 표시할 Image 참조
    public Sprite[] orderSprites; // 6개의 주문 이미지 배열

    public static UIManager instance;

    private void Awake()
    {
        // 싱글톤 인스턴스 생성
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재한다면 현재 오브젝트를 파괴
        }
    }

    private void Start()
    {
        UpdateOrderDisplay(); // 초기 주문 표시 업데이트
    }

    private void Update()
    {
        if (GameManager.instance != null)
        {
            // GameManager의 남은 시간을 가져와 텍스트에 표시
            float remainingTime = GameManager.instance.GetRemainingTime();
            timeText.text = timeText.text = $"{remainingTime:00.00}";
        }
    }

    public void UpdateOrderDisplay()
    {
        if (GameManager.instance != null)
        {
            // GameManager에서 현재 주문을 가져옵니다.
            int orderIndex = (int)GameManager.instance.currentOrder;

            // 주문에 맞는 이미지를 orderDisplay에 설정
            if (orderIndex >= 0 && orderIndex < orderSprites.Length)
            {
                orderDisplay.sprite = orderSprites[orderIndex];
            }
        }
    }
}
