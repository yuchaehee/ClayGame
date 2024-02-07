using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    public List<GameObject>[] pool;

    private void Awake()
    {
        pool = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pool.Length; index++)
            pool[index] = new List<GameObject>();
    }

    public GameObject GetGameObject(int index)
    {
        GameObject select = null;

        foreach (GameObject gameObj in pool[index])
        {
            // Ǯ�� ���鼭 ��� �ִ� ���� ������Ʈ ã��
            if (gameObj.activeSelf == false)
            {
                // ã���� ��ȯ..
                select = gameObj;
                select.SetActive(true);
                break;
            } 
        }

        // ��� �ִ� ���� ������Ʈ�� ���ٸ�..
        if (!select)
        {
            // ���� �����ؼ� ��ȯ..
            // ���� ������ ���� ������Ʈ�� Ǯ �Ŵ��� ������ ���� ��.. �׷��� �θ� transform ����..
            select = Instantiate(prefabs[index], transform);
            pool[index].Add(select);
        }

        return select;
    }
}
