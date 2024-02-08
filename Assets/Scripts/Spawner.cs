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

    public void Spawn(int prefabIndex)
    {
        GameObject clay = GameManager.instance.pool.GetGameObject(prefabIndex);
        clay.transform.position = spawnPoints[Random.Range(1, 4)].position;
        

        // 음.. 밑에처럼 아예 이펙트 프리팹을 풀매니저에 넣어서 소환하는 식으로 하려고 했는데,, 소환은 되는데 없애는 걸 어떻게 해야할 지 모르겠어서 그냥,, clay 스크립트에다가 이펙트 프리팹 넣어서 쓰려고 합니다..
        //switch (prefabIndex) { 

        //    //// 프리팹 인덱스가 점토 프리팹 인덱스일 때랑 이펙트 프리팹 인덱스일 때랑 구분 지어서 쓰려고...
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

        //    //    // 이펙트 오브젝트의 부모를, 현재 게임 매니저가 참조하고 있는 점토 게임 오브젝트로 바꿔줌.. 그러면 점토 움직임에 따라 같이 움직일 듯..
        //    //    effectObj.parent = GameManager.instance.clay.transform;
        //    //    effectObj.localPosition = Vector3.zero;
 
        //    //    break;
        //}
    }
}
