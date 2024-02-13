using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // [0] : 레벨업, [1] : 판매, ...
    public ParticleSystem[] effects;

    // 일단 index 5 부터 이펙트 프리팹 넣어놨습니다..
    public GameObject[] prefabs;
    public List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
            pools[index] = new List<GameObject>();
    }

    public GameObject GetGameObject(int index)
    {
        GameObject select = null;

        foreach (GameObject gameObj in pools[index])
        {
            // 풀을 돌면서 놀고 있는 게임 오브젝트 찾기
            if (gameObj.activeSelf == false)
            {
                // 찾으면 반환..
                select = gameObj;
                select.SetActive(true);
                break;
            } 
        }

        // 놀고 있는 게임 오브젝트가 없다면..
        if (!select)
        {
            // 새로 생성해서 반환..
            // 새로 생성한 게임 오브젝트는 풀 매니저 하위에 놓을 것.. 그래서 부모에 transform 넣음..
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
