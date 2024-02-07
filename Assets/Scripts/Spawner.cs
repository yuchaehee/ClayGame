using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;

    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>(); // [0] 은 자기 자신이니까 [1] 부터 쓰기..
    }

    void Spawn()
    {
        // 일단 희귀도 0 짜리 애들만 프리펩에 넣어놔서 5개라 range(0, 5) 로 해놓았습니다..
        GameObject jelly = GameManager.instance.pool.GetGameObject(Random.Range(0, 5));
        jelly.transform.position = spawnPoints[Random.Range(1, 4)].position;
    }
}
