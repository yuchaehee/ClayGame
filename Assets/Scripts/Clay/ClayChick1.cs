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
        clayName = "���Ƹ�";
        clayInformation = "���� ���Ƹ��ӿ�.";
    }

    public override void SetClayData()
    {
        base.SetClayData();
        love = 10; // �ϴ� ���Ƿ� ���� �߽��ϴ�.. �������� ���� ����...
    }

    private void Update()
    {
        // �ε��� ���� �� ����� �Ϸ���.. �̷��� �߽��ϴ�. �׷��� exps[] �� ������ ��Ҹ� -1�� �����س��ҽ��ϴ�.
        if (level >= exps.Length - 1)
            level = exps.Length - 1;
    }
}