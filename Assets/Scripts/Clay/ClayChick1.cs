using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClayChick1 : Clay
{
    private void Awake()
    {
        levelUpPlusLove = 10;
        buyPrice = 500;
        sellPrice = 2000;
        love = 10;
        clayName = "병아리";
        clayInformation = "저는 병아리임요.";
    }

    public override void SetClayData()
    {
        base.SetClayData();
        love = 10; // 일단 임의로 설정 했습니다.. 언제든지 변경 가능...
    }

    private void Update()
    {
        // 인덱스 범위 안 벗어나게 하려고.. 이렇게 했습니다. 그래서 exps[] 의 마지막 요소를 -1로 설정해놓았습니다.
        if (level >= exps.Length - 1)
            level = exps.Length - 1;
    }
}