using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 원래 게임매니저에 임시로 UI 해놓았었는데 하고 싶으신대로 해주시면 감사하겠습니다..
    [Header("GameData UI")]
    public Text goldText;
    public Text loveText;

    [Header("Game Sound && Exit UI")]
    public GameObject optionPanel;

    [Header("Clay Sell & Unlock UI")]
    // 해금 여부 상관 없는 공통 정보..
    public Text pageText;

    // 해금 완료 상태에서의 UI 정보..
    public Text clayPrice;
    public Text clayName;
    public Image unlockClayImage;

    // 해금 비완료 상태에서의 UI 정보..
    public Image lockClayImage;

    public GameObject unlockPanel; // 해금 안 됐을 땐 얘가 활성화 되어있기..
    public GameObject lockPanel; // 해금 되면 얘가..(해금하기 버튼 누르면 활성화 on, 해금하기 버튼은 unlock에 있음.)
    public GameObject buyClayPanel; // 일단 스페이스바 누르면 뜨도록 해놨습니다..

    public int finalPageIndex; // 버튼 눌렀을 때 이 인덱스 벗어나면 안 넘어가도록..(일단 기본 점토만 쓸거라 4로 해놓았습니다..)
    public int currentPageIndex;

    public GameObject unlockButton; // 사전에 등록이 안 된 상태면 해금하기 버튼 눌러도 실행 x


    [Header("UpgradeClayDataUI")]
    public GameObject plantPanel;
    public Button clayHouseUpBtn;
    public Button clayToyUpBtn;
    public Text clayHouseText;
    public Text clayToyText;
    public Text houseBtnText;
    public Text toyBtnText;


    // 일단 기본 점토들만 쓸 거라 크기 5개로 해놨습니다.
    [Header("BuyClayData")]
    public Sprite[] claySpriteList; // 점토 사진
    public string[] clayNameList; // 점토 이름
    public int[] clayGoldList; // 점토 구매가격

    // 활성화된 해금버튼을 누르면 값이 변경됨. true로 (즉 사전에 점토가 저장되면.. 그렇다면 사전 점토 클래스를 새로 만들어야..)
    public bool[] isUnlocked; // 맨 처음 기본 점토 말고는 다 false로 해놓을 것..

    [Header("ClayBookData")]
    // 야생에서 점토 잡아오면 해당 점토의 isLocked 값이 false 가 됨.
    public bool[] isLocked; // 사전에 등록되었는지 여부 확인용(사전에 등록되면 이제 BuyClayData 의 isUnlocked 값이 true 가 되어서 해금버튼이 활성화 됨.)


    [Header("UpgradeClayData")]
    public int[] clayHouseUpgradePrice;
    public int[] clayToyUpgradePrice;


    [Header("GameData")]
    public RuntimeAnimatorController[] levelAc;

    public int clayHouseLevel;
    public int clayToyLevel;
    public float gold;
    public float love;

    public static GameManager instance;

    public SoundManager sound;
    public PoolManager pool;
    public ClayAction clayAction;
    public Clay clay; // 다형성 이용하려고 Clay 변수로 쓸 예정입니다..

    private void Awake()
    {
        instance = this;

        clayPrice.text = "" + clayGoldList[0];
        clayName.text = clayNameList[0];
        unlockClayImage.sprite = claySpriteList[0];

        unlockPanel.SetActive(true);
    }

    private void Update()
    {
        goldText.text = "" + gold;
        loveText.text = "" + love;

        // 터치 카운트 다 채우면 레벨업..
        if (clay.touchCount == clay.exps[clay.level])
        {
            clay.LevelUp();
        }

        // 일단 Esc 버튼 누르면 뜨도록 해놨습니다..
        if (Input.GetButtonDown("Cancel"))
        {
            if (optionPanel.activeSelf == true)
                optionPanel.SetActive(false);
            else
                optionPanel.SetActive(true);
        }

        // 일단 스페이스바 누르면 구매창 뜨도록 해놨습니다.. 원하시는대로 변경 가능합니다..
        if (Input.GetButtonDown("Jump"))
        {
            if (buyClayPanel.activeSelf == true)
                buyClayPanel.SetActive(false);
            else
                buyClayPanel.SetActive(true);
        }

        // 일단 vertical 버튼 누르면 구매창 뜨도록 해놨습니다.. 원하시는대로 변경 가능합니다..
        if (Input.GetButtonDown("Vertical"))
        {
            if (plantPanel.activeSelf == true)
                plantPanel.SetActive(false);
            else
                plantPanel.SetActive(true);
        }

        UIUpdate();
    }

    public void PageLeftBtn()
    {
        if (currentPageIndex <= 0)
            return;

        currentPageIndex--;
    }

    public void PageRightBtn()
    {
        if (currentPageIndex >= claySpriteList.Length-1)
            return;

        currentPageIndex++;
    }

    public void UIUpdate()
    {
        // 구매창 UI 관련 코드
        if (currentPageIndex < 9)
            pageText.text = "#0" + (currentPageIndex + 1);
        else
            pageText.text = "#" + (currentPageIndex + 1);


        unlockPanel.SetActive(isUnlocked[currentPageIndex]);
        lockPanel.SetActive(!isUnlocked[currentPageIndex]); // 해금 된 상태면 lockPanel 은 꺼져야 하니까 ! 써줌..

        clayPrice.text = "" + clayGoldList[currentPageIndex];
        clayName.text = clayNameList[currentPageIndex];
        unlockClayImage.sprite = claySpriteList[currentPageIndex];
        lockClayImage.sprite = claySpriteList[currentPageIndex];


        // 점토 용품 업데이트 관련 UI 코드
        clayHouseText.text = "점토 수용량 " + (clayHouseLevel);
        clayToyText.text = "클릭 생산량 x " + (clayToyLevel);

        if (clayHouseLevel >= clayHouseUpgradePrice.Length)
            houseBtnText.text = "끝";
        else
            houseBtnText.text = "" + clayHouseUpgradePrice[clayHouseLevel];

        if (clayToyLevel >= clayToyUpgradePrice.Length)
            toyBtnText.text = "끝";
        else
            toyBtnText.text = "" + clayToyUpgradePrice[clayToyLevel];
    }

    public void Unlock()
    {
        // 사전에 등록이 안 된 상태면 해금하기 버튼 눌러도 해금 안 됨.
        if (isLocked[currentPageIndex])
        {
            Debug.Log("흠.. " + pool.prefabs[currentPageIndex].GetComponent<Clay>().rarity + "단계 야생에서 잡아와야 할 듯 하다..");
            return;
        }

        // sound Manager 의 playSound 를 실행시킵니다..
        sound.PlaySound("UNLOCK");
        isUnlocked[currentPageIndex] = true;
    }

    public void BuyClay()
    {   // 버튼에 적용하기 위해 함수 제작..

        // clayHouseLevel 만큼 점토 키울 수 있음..
        // Lv.0 : 1마리, Lv.1 : 2마리, ..., Lv.4 : 5마리
        if (pool.GetComponentsInChildren<Clay>().Length == clayHouseLevel) // 한 번에 키울 수 있는 최대 점토의 수는 5마리
            return;

        if (gold - clayGoldList[currentPageIndex] < 0) // 돈 없으면 그냥 함수 빠져나가도록..
            return;

        gold -= clayGoldList[currentPageIndex];

        // 점토 구매하면 생성해서 놓아야하니까 풀매니저의 GetGameObject 함수를 사용..
        // poolManager 의 프리펩 점토 인덱스랑 currentPageIndex 가 똑같은 점토를 가리킴.. 그래서 그냥 currentPageIndex 쓰면 됨.
        pool.GetGameObject(currentPageIndex);

        // 점토 구매하면 구매창 꺼짐..
        sound.PlaySound("BUY");
        buyClayPanel.SetActive(false);
    }

    public void MouseDragToSellBtn()
    {
        // button 게임 오브젝트의 Event Trigger 에서 쓰도록.. 함수 만들기 
        clayAction.mouseDragToSellBtn = true;
    }
    public void MouseNotDragToSellBtn()
    {
        // button 에서 벗어나면 false 되도록..
        clayAction.mouseDragToSellBtn = false;
    }

    public void ClayHouseLevelUp()
    {
        if (love - clayHouseUpgradePrice[clayHouseLevel] < 0)
            return;

        if (clayHouseLevel + 1 == clayHouseUpgradePrice.Length)
        {
            clayHouseLevel++;
            return;
        }
        else
        {
            love -= clayHouseUpgradePrice[clayHouseLevel];
            clayHouseLevel++;
        }

        sound.PlaySound("BUY");
    }

    public void ClayToyLevelUp()
    {
        if (love - clayToyUpgradePrice[clayToyLevel] < 0)
            return;

        if (clayToyLevel + 1 == clayToyUpgradePrice.Length)
        {
            clayToyLevel++;
            return;
        }
        else
        {
            love -= clayToyUpgradePrice[clayToyLevel];
            clayToyLevel++;   
        }

        sound.PlaySound("BUY");
    }

    public void ChangeAc(Animator anim, int level)
    {
        anim.runtimeAnimatorController = levelAc[level];
    }

    public void GameEixt()
    {
        Application.Quit();
    }
}
