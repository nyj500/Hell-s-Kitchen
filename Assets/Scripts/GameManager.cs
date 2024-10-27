using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static GameManager instance;

    // 플레이어 타입
    public enum PlayerType { player1, player2, player3 }
    public PlayerType currentPlayer;
    private int currentmoney;

    // 게임 시간
    public float gameDuration = 60f; // 1분 (60초)
    private float remainingTime;

    // 음식 종류
    public enum FoodType { Food1, Food2, Food3, Food4, Food5, Food6 }
    public FoodType currentOrder;

    private void Awake()
    {
        // 싱글톤 인스턴스 생성
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartGame();
    }

    // 게임 시작 함수
    public void StartGame()
    {
        remainingTime = gameDuration; // 남은 시간을 초기화
        GenerateNewOrder(); // 첫 번째 주문 생성
        StartCoroutine(GameTimer()); // 타이머 시작
        currentmoney = 0;
    }

    // 게임 타이머 관리
    private IEnumerator GameTimer()
    {
        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime; // 남은 시간 감소
            yield return null; // 프레임마다 업데이트
        }

        EndGame(); // 시간이 다 지나면 게임 종료
    }

    // 게임 종료 함수
    private void EndGame()
    {
        Debug.Log("Game Over");
        // 게임 종료 로직 추가
    }

    // 새로운 주문 생성 함수
    public void GenerateNewOrder()
    {
        currentOrder = (FoodType)Random.Range(0, System.Enum.GetValues(typeof(FoodType)).Length);
        Debug.Log("New Order: " + currentOrder);
    }

    // 주문이 완료되었을 때 호출
    public void CompleteOrder()
    {
        Debug.Log("Order Completed: " + currentOrder);
        currentmoney += 1000;
        GenerateNewOrder(); // 새로운 주문 생성
    }

    // 주문이 실패했을 때 호출
    public void MissOrder()
    {
        Debug.Log("Order Missed: " + currentOrder);
        currentmoney -= 500;
        GenerateNewOrder(); // 새로운 주문 생성
    }

    // 현재 남은 시간을 반환하는 함수
    public float GetRemainingTime()
    {
        return remainingTime;
    }
}
