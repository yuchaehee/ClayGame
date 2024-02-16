using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Clay : MonoBehaviour
{
    // 모든 점토들이 이 Clay 클래스를 상속받을 것입니다..

    public int rarity; // 0:쉬움 단계, 1:보통 단계, 2:어려움 단계
    public int level;
    public int[] exps; // 생각해봤는데 어려움 단계에서 얻은 점토일수록 레벨업 하려면 더 많이 누르게 하는게 좋을 것 같습니다.

    public int touchCount;
    protected float buyPrice;
    protected float sellPrice;
    protected float levelUpPlusLove; // 레벨에 곱해서 love 값에 더해줄 것입니다.. (희귀도에 따라 다를예정..)
    public float love; // 얘는 터치하면 반환하는 애정..
    public string clayName;

    // ClayBook 클래스에서 쓸 변수..
    public string clayInformation;


    public virtual void SetClayData()
    {
        level = 0;
        touchCount = 0;
        // love 값은 각 클레이 하위 클래스마다 다르게 할 것..
    }

    public virtual float TouchClay()
    {
        return love * GameManager.instance.clayToyLevel;
    }

    public virtual float SellClay()
    {
        return sellPrice;
    }

    public virtual void BuyClay()
    {
        GameManager.instance.gold -= buyPrice;
        // 현재 가진 돈에서 해당 점토의 가격을 뺄 것..
    }

    public virtual void LevelUp()
    {
        level++;
        touchCount = 0;

        love += levelUpPlusLove * level;

        GameManager.instance.ChangeAc(GetComponent<Animator>(), level);

        // 이펙트 실행
        //LevelUpEffect.Play();

        // Sound
        GameManager.instance.sound.PlaySound("GROW");
    }
}
