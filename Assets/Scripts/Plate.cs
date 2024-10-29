using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    // 재료 존재 여부 확인 변수 (public으로 설정하여 외부에서 접근 가능)
    public bool hasNori = false;
    public bool hasRice = false;

    public bool hasChoppedFish = false;
    public bool hasCookedSalami = false;
    public bool hasChoppedPepper = false;
    public bool hasChoppedCucumber = false;
    public bool hasChoppedCarrot = false;

    // 생성할 김밥 프리팹
    public GameObject kimbapPrefab;

    // 재료 위치 포인트
    public Transform placePointNori;

    void Update()
    {
        // 재료가 준비되면 자동으로 김밥 생성
        if (CheckIngredientsReady())
        {
            CreateKimbap();
        }
    }

    // Plate 상태 초기화 함수
    public void ResetPlate()
    {
        hasNori = false;
        hasRice = false;
        hasChoppedFish = false;
        hasChoppedPepper = false;
        hasChoppedCucumber = false;
        hasChoppedCarrot = false;
    }

    // 모든 재료가 준비되었는지 확인하는 함수
    public bool CheckIngredientsReady()
    {
        // 기본 조건: 김과 밥이 있고, 최소 하나의 메인 재료와 최소 하나의 야채가 있어야 김밥을 만들 수 있음
        bool hasMainIngredient = hasChoppedFish || hasCookedSalami;
        bool hasVegetable = hasChoppedPepper || hasChoppedCucumber || hasChoppedCarrot;

        return hasNori && hasRice && hasMainIngredient && hasVegetable;
    }

    // 김밥 생성 함수
    public void CreateKimbap()
    {
        if (!CheckIngredientsReady())
        {
            Debug.LogWarning("All necessary ingredients are not present on the plate.");
            return;
        }

        // Plate 위의 재료만 삭제하고, 위치 포인트는 남겨둠
        foreach (Transform child in transform)
        {
            if (child.name != "PlatePlacePoint" &&
                child.name != "PlatePlacePointRice" &&
                child.name != "PlatePlacePointMain" &&
                child.name != "PlatePlacePointVeg")
            {
                Destroy(child.gameObject);
            }
        }

        // 김밥 오브젝트 생성
        GameObject kimbap = Instantiate(kimbapPrefab, placePointNori.position, placePointNori.rotation);

        // 김밥 태그 설정 (재료 조합에 따라 김밥1 ~ 김밥6)
        kimbap.tag = DetermineKimbapTag();

        // Plate에 놓이도록 설정
        kimbap.transform.parent = transform;

        // Plate 상태 초기화
        ResetPlate();
    }

    // 김밥 태그 결정 함수
    private string DetermineKimbapTag()
    {
        // 예시로 태그를 결정하는 로직 작성 (재료 조합에 따라 태그 설정)
        if (hasCookedSalami && hasChoppedCucumber)
        {
            Debug.Log("iskimbap1");
            return "Kimbap1";
        }
        else if (hasCookedSalami && hasChoppedPepper)
        {
            Debug.Log("iskimbap2");
            return "Kimbap2";
        }
        else if (hasCookedSalami && hasChoppedCarrot)
        {
            Debug.Log("iskimbap3");
            return "Kimbap3";
        }
        else if (hasChoppedFish && hasChoppedCucumber)
        {
            Debug.Log("iskimbap4");
            return "Kimbap4";
        }
        else if (hasChoppedFish && hasChoppedPepper)
        {
            Debug.Log("iskimbap5");
            return "Kimbap5";
        }
        else if (hasChoppedFish && hasChoppedCarrot)
        {
            Debug.Log("iskimbap6");
            return "Kimbap6";
        }
        else
            return "Kimbap1"; // 기본값
    }
}
