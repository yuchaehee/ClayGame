using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Clay : MonoBehaviour
{
    // ��� ������� �� Clay Ŭ������ ��ӹ��� ���Դϴ�..

    public int rarity; // 0:���� �ܰ�, 1:���� �ܰ�, 2:����� �ܰ�
    public int level;
    public int[] exps; // �����غôµ� ����� �ܰ迡�� ���� �����ϼ��� ������ �Ϸ��� �� ���� ������ �ϴ°� ���� �� �����ϴ�.

    public int touchCount;
    protected float buyPrice;
    protected float sellPrice;
    protected float levelUpPlusLove; // ������ ���ؼ� love ���� ������ ���Դϴ�.. (��͵��� ���� �ٸ�����..)
    public float love; // ��� ��ġ�ϸ� ��ȯ�ϴ� ����..
    public string clayName;

    public virtual void SetClayData()
    {
        level = 0;
        touchCount = 0;
        // love ���� �� Ŭ���� ���� Ŭ�������� �ٸ��� �� ��..
    }

    public virtual float TouchClay()
    {
        if (level == 5) // ���������� �������� �� Ŭ���ϸ� ������ ��ȯ���� �ʵ��� �ϱ� ���� ���ǹ� �޾ҽ��ϴ�..
            return 0;

        return love * GameManager.instance.clayToyLevel;
    }

    public virtual float SellClay()
    {
        return sellPrice;
    }

    public virtual void BuyClay()
    {
        GameManager.instance.gold -= buyPrice;
        // ���� ���� ������ �ش� ������ ������ �� ��..
    }

    public virtual void LevelUp()
    {
        level++;
        touchCount = 0;

        love += levelUpPlusLove * level;

        GameManager.instance.ChangeAc(GetComponent<Animator>(), level);

        // Sound
        GameManager.instance.sound.PlaySound("GROW");
    }
}
