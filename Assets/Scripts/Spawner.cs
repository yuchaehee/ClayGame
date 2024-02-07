using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;

    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>(); // [0] �� �ڱ� �ڽ��̴ϱ� [1] ���� ����..
    }

    void Spawn()
    {
        // �ϴ� ��͵� 0 ¥�� �ֵ鸸 �����鿡 �־���� 5���� range(0, 5) �� �س��ҽ��ϴ�..
        GameObject jelly = GameManager.instance.pool.GetGameObject(Random.Range(0, 5));
        jelly.transform.position = spawnPoints[Random.Range(1, 4)].position;
    }
}
