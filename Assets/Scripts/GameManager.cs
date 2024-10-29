//using System.Collections;
//using UnityEngine;

//public class GameManager : MonoBehaviour
//{
//    // �̱��� �ν��Ͻ�
//    public static GameManager instance;

//    // �÷��̾� Ÿ��
//    public enum PlayerType { player1, player2, player3 }
//    public PlayerType currentPlayer;
//    private int currentmoney;

//    // ���� �ð�
//    public float gameDuration = 60f; // 1�� (60��)
//    private float remainingTime;

//    // ���� ����
//    public enum FoodType { Food1, Food2, Food3, Food4, Food5, Food6 }
//    public FoodType currentOrder;

//    private void Awake()
//    {
//        // �̱��� �ν��Ͻ� ����
//        if (instance == null)
//        {
//            instance = this;
//            DontDestroyOnLoad(gameObject); // ���� �ٲ� ����
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    private void Start()
//    {
//        StartGame();
//    }

//    // ���� ���� �Լ�
//    public void StartGame()
//    {
//        remainingTime = gameDuration; // ���� �ð��� �ʱ�ȭ
//        GenerateNewOrder(); // ù ��° �ֹ� ����
//        StartCoroutine(GameTimer()); // Ÿ�̸� ����
//        currentmoney = 0;
//    }

//    // ���� Ÿ�̸� ����
//    private IEnumerator GameTimer()
//    {
//        while (remainingTime > 0)
//        {
//            remainingTime -= Time.deltaTime; // ���� �ð� ����
//            yield return null; // �����Ӹ��� ������Ʈ
//        }

//        EndGame(); // �ð��� �� ������ ���� ����
//    }

//    // ���� ���� �Լ�
//    private void EndGame()
//    {
//        Debug.Log("Game Over");
//        // ���� ���� ���� �߰�
//    }

//    // ���ο� �ֹ� ���� �Լ�
//    public void GenerateNewOrder()
//    {
//        currentOrder = (FoodType)Random.Range(0, System.Enum.GetValues(typeof(FoodType)).Length);
//        Debug.Log("New Order: " + currentOrder);
//        UIManager.instance.UpdateOrderDisplay();
//    }

//    // �ֹ��� �Ϸ�Ǿ��� �� ȣ��
//    public void CompleteOrder()
//    {
//        Debug.Log("Order Completed: " + currentOrder);
//        currentmoney += 1000;
//        GenerateNewOrder(); // ���ο� �ֹ� ����
//    }

//    // �ֹ��� �������� �� ȣ��
//    public void MissOrder()
//    {
//        Debug.Log("Order Missed: " + currentOrder);
//        currentmoney -= 500;
//        GenerateNewOrder(); // ���ο� �ֹ� ����
//    }

//    // ���� ���� �ð��� ��ȯ�ϴ� �Լ�
//    public float GetRemainingTime()
//    {
//        return remainingTime;
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager instance;

    // Player Type Enum
    public enum PlayerType { player1, player2, player3 }
    public PlayerType currentPlayer;
    private int currentmoney;

    // Game Timer
    public float gameDuration = 60f; // 1 minute (60 seconds)
    private float remainingTime;

    // Order Tracking
    public enum FoodType { Food1, Food2, Food3, Food4, Food5, Food6 }
    public FoodType currentOrder;

    private void Awake()
    {
        // Singleton instance setup
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep GameManager across scenes
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

    // Recipe Class
    [System.Serializable]
    public class Recipe
    {
        public string recipeName;
        public List<string> requiredIngredients;

        public Recipe(string name, List<string> ingredients)
        {
            recipeName = name;
            requiredIngredients = ingredients;
        }

        // Checks if a dish matches this recipe
        public bool IsMatch(Dish dish)
        {
            if (dish.ingredients.Count != requiredIngredients.Count) return false;
            for (int i = 0; i < requiredIngredients.Count; i++)
            {
                if (dish.ingredients[i] != requiredIngredients[i]) return false;
            }
            return true;
        }
    }

    // Dish Class to Track Ingredients
    [System.Serializable]
    public class Dish
    {
        public List<string> ingredients = new List<string>();

        public void AddIngredient(string ingredient)
        {
            ingredients.Add(ingredient);
        }

        public void ClearDish()
        {
            ingredients.Clear();
        }
    }

    public Dish currentDish = new Dish();
    public List<Recipe> recipes = new List<Recipe>();

    // Game Start
    public void StartGame()
    {
        remainingTime = gameDuration; // Reset timer
        GenerateNewOrder(); // Generate first order
        StartCoroutine(GameTimer()); // Start timer coroutine
        currentmoney = 0;

        // Define example recipes
        recipes.Add(new Recipe("Salad", new List<string> { "Lettuce", "Tomato", "Cucumber" }));
        recipes.Add(new Recipe("Sushi", new List<string> { "Rice", "Fish", "Seaweed" }));
    }

    // Game Timer Coroutine
    private IEnumerator GameTimer()
    {
        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            yield return null;
        }

        EndGame(); // End game when time runs out
    }

    private void EndGame()
    {
        Debug.Log("Game Over");
        // Additional end-game handling
    }

    // Generate New Order
    public void GenerateNewOrder()
    {
        currentOrder = (FoodType)Random.Range(0, System.Enum.GetValues(typeof(FoodType)).Length);
        Debug.Log("New Order: " + currentOrder);
        UIManager.instance.UpdateOrderDisplay();
    }

    // Complete the Order (check if dish matches the order)
    public void CompleteOrder()
    {
        // Find the recipe that matches the current order
        Recipe orderRecipe = recipes[(int)currentOrder];
        if (orderRecipe.IsMatch(currentDish))
        {
            Debug.Log("Order Completed: " + currentOrder);
            currentmoney += 1000; // Reward for correct dish
        }
        else
        {
            Debug.Log("Incorrect Order Submitted.");
            currentmoney -= 500; // Penalty for incorrect dish
        }

        // Clear the current dish and generate a new order
        currentDish.ClearDish();
        GenerateNewOrder();
    }

    // Called if the order is missed
    public void MissOrder()
    {
        Debug.Log("Order Missed: " + currentOrder);
        currentmoney -= 500;
        GenerateNewOrder();
    }

    // Returns remaining game time
    public float GetRemainingTime()
    {
        return remainingTime;
    }

    // Adds an ingredient to the current dish
    public void AddIngredientToDish(string ingredient)
    {
        currentDish.AddIngredient(ingredient);
    }
}
