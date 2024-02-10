using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayBook : MonoBehaviour
{
    // 일단 기본 다섯마리만 해놨습니다..
    public bool[] isRegistered;


    public void Register(int clayIndex)
    {
        // 이 함수는 야생에서 점토 잡아오면 실행될 것입닏나..

        // 사전에 점토 등록하는 함수..
        // 이 함수가 실행되면 게임매니저의 isLocked[] 속 요소가 false 가 되고..
        // 비로소 게임매니저의 isUnlocked[] 속 요소가 true 가 되어서 구매창의 해금하기 버튼 활성화 됨..

        isRegistered[clayIndex] = true;

        if (isRegistered[clayIndex])
        {
            GameManager.instance.isLocked[clayIndex] = false;
        }
    }
}
