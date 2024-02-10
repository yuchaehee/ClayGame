using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayBook : MonoBehaviour
{
    // �ϴ� �⺻ �ټ������� �س����ϴ�..
    public bool[] isRegistered;


    public void Register(int clayIndex)
    {
        // �� �Լ��� �߻����� ���� ��ƿ��� ����� ���Ԉ���..

        // ������ ���� ����ϴ� �Լ�..
        // �� �Լ��� ����Ǹ� ���ӸŴ����� isLocked[] �� ��Ұ� false �� �ǰ�..
        // ��μ� ���ӸŴ����� isUnlocked[] �� ��Ұ� true �� �Ǿ ����â�� �ر��ϱ� ��ư Ȱ��ȭ ��..

        isRegistered[clayIndex] = true;

        if (isRegistered[clayIndex])
        {
            GameManager.instance.isLocked[clayIndex] = false;
        }
    }
}
