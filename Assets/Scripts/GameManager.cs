using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static GameManager instance;

    // �÷��̾� Ÿ��
    public enum PlayerType { player1, player2, player3 }
    public PlayerType currentPlayer;
    private int currentmoney;

    // ���� �ð�
    public float gameDuration = 60f; // 1�� (60��)
    private float remainingTime;

    // ���� ����
    public enum FoodType { Food1, Food2, Food3, Food4, Food5, Food6 }
    public FoodType currentOrder;

    private void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ���� �ٲ� ����
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

    // ���� ���� �Լ�
    public void StartGame()
    {
        remainingTime = gameDuration; // ���� �ð��� �ʱ�ȭ
        GenerateNewOrder(); // ù ��° �ֹ� ����
        StartCoroutine(GameTimer()); // Ÿ�̸� ����
        currentmoney = 0;
    }

    // ���� Ÿ�̸� ����
    private IEnumerator GameTimer()
    {
        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime; // ���� �ð� ����
            yield return null; // �����Ӹ��� ������Ʈ
        }

        EndGame(); // �ð��� �� ������ ���� ����
    }

    // ���� ���� �Լ�
    private void EndGame()
    {
        Debug.Log("Game Over");
        // ���� ���� ���� �߰�
    }

    // ���ο� �ֹ� ���� �Լ�
    public void GenerateNewOrder()
    {
        currentOrder = (FoodType)Random.Range(0, System.Enum.GetValues(typeof(FoodType)).Length);
        Debug.Log("New Order: " + currentOrder);
    }

    // �ֹ��� �Ϸ�Ǿ��� �� ȣ��
    public void CompleteOrder()
    {
        Debug.Log("Order Completed: " + currentOrder);
        currentmoney += 1000;
        GenerateNewOrder(); // ���ο� �ֹ� ����
    }

    // �ֹ��� �������� �� ȣ��
    public void MissOrder()
    {
        Debug.Log("Order Missed: " + currentOrder);
        currentmoney -= 500;
        GenerateNewOrder(); // ���ο� �ֹ� ����
    }

    // ���� ���� �ð��� ��ȯ�ϴ� �Լ�
    public float GetRemainingTime()
    {
        return remainingTime;
    }
}
