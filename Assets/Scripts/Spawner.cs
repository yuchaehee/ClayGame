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

    public void Spawn(int prefabIndex)
    {
        GameObject clay = GameManager.instance.pool.GetGameObject(prefabIndex);
        clay.transform.position = spawnPoints[Random.Range(1, 4)].position;
        

        // ��.. �ؿ�ó�� �ƿ� ����Ʈ �������� Ǯ�Ŵ����� �־ ��ȯ�ϴ� ������ �Ϸ��� �ߴµ�,, ��ȯ�� �Ǵµ� ���ִ� �� ��� �ؾ��� �� �𸣰ھ �׳�,, clay ��ũ��Ʈ���ٰ� ����Ʈ ������ �־ ������ �մϴ�..
        //switch (prefabIndex) { 

        //    //// ������ �ε����� ���� ������ �ε����� ���� ����Ʈ ������ �ε����� ���� ���� ��� ������...
        //    //case 0:
        //    //case 1:
        //    //case 2:
        //    //case 3:
        //    //case 4:
        //    //    GameObject clay = GameManager.instance.pool.GetGameObject(prefabIndex);
        //    //    clay.transform.position = spawnPoints[Random.Range(1, 4)].position;
        //    //    break;
        //    //case 5:
        //    //    Transform effectObj = GameManager.instance.pool.GetGameObject(prefabIndex).transform;

        //    //    // ����Ʈ ������Ʈ�� �θ�, ���� ���� �Ŵ����� �����ϰ� �ִ� ���� ���� ������Ʈ�� �ٲ���.. �׷��� ���� �����ӿ� ���� ���� ������ ��..
        //    //    effectObj.parent = GameManager.instance.clay.transform;
        //    //    effectObj.localPosition = Vector3.zero;
 
        //    //    break;
        //}
    }
}
