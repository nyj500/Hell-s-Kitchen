using UnityEngine;
using System.Collections;

public class RandomObjectSpawner : MonoBehaviour
{
    public Collider spawnArea; // �ݶ��̴��� ������ ���
    public GameObject[] objectsToSpawn; // ������ ������Ʈ ���� (3��)
    public float minSpawnInterval = 5f; // �ּ� ���� ���� (3��)
    public float maxSpawnInterval = 15f; // �ִ� ���� ���� (10��)
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

        // ���� �ȿ� �ִ��� Ȯ���ϱ� ���� ����Ʈ�� �ݶ��̴� �������� �˻�
        if (col.bounds.Contains(randomPoint))
        {
            return randomPoint;
        }
        else
        {
            return GetRandomPointInCollider(col); // ��� ȣ��� ���� ����Ʈ�� ã��
        }
    }
}
