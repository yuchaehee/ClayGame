using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // [0] : ������, [1] : �Ǹ�, ...
    public ParticleSystem[] effects;

    // �ϴ� index 5 ���� ����Ʈ ������ �־�����ϴ�..
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
            pools[index].Add(select);

            // effects[0] : ������, effects[1] : �Ǹ�, ... 
            select.GetComponent<Clay>().LevelUpEffect = Instantiate(effects[0], select.transform);
            select.GetComponent<Clay>().SellEffect = Instantiate(effects[1], select.transform);
        }

        return select;
    }
}
