using UnityEngine;
using System.Collections;

public class RandomObjectSpawner : MonoBehaviour
{
    public Collider spawnArea; // 콜라이더를 범위로 사용
    public GameObject[] objectsToSpawn; // 생성할 오브젝트 종류 (3개)
    public float minSpawnInterval = 5f; // 최소 생성 간격 (3초)
    public float maxSpawnInterval = 15f; // 최대 생성 간격 (10초)
    private GameObject lastSpawnedObject;

    void Start()
    {
        if (spawnArea == null || objectsToSpawn == null || objectsToSpawn.Length < 3)
        {
            Debug.LogError("Spawn area or objects to spawn are not properly assigned or there are fewer than 3 objects.");
            return;
        }

        StartCoroutine(SpawnObjectAtRandomInterval());
    }

    IEnumerator SpawnObjectAtRandomInterval()
    {
        while (true)
        {
            float randomInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(randomInterval);
            SpawnRandomObject();
        }
    }

    void SpawnRandomObject()
    {
        GameObject objectToSpawn;
        do
        {
            objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
        } while (objectToSpawn == lastSpawnedObject);

        lastSpawnedObject = objectToSpawn;
        Vector3 randomPosition = GetRandomPointInCollider(spawnArea);
        Instantiate(objectToSpawn, randomPosition, Quaternion.identity);
    }

    Vector3 GetRandomPointInCollider(Collider col)
    {
        Vector3 boundsMin = col.bounds.min;
        Vector3 boundsMax = col.bounds.max;

        float x = Random.Range(boundsMin.x, boundsMax.x);
        float y = Random.Range(boundsMin.y, boundsMax.y);
        float z = Random.Range(boundsMin.z, boundsMax.z);

        Vector3 randomPoint = new Vector3(x, y, z);

        // 범위 안에 있는지 확인하기 위해 포인트가 콜라이더 내부인지 검사
        if (col.bounds.Contains(randomPoint))
        {
            return randomPoint;
        }
        else
        {
            return GetRandomPointInCollider(col); // 재귀 호출로 내부 포인트를 찾음
        }
    }
}

