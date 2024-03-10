using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UICanvase")]
    // 이거 clayAction 에서 쓸 것.. 점토 들어올리면 UI 캔버스 꺼버리도록..
    public GameObject UICanvas;
    public GameObject exitButton; // 얘는 구매, 업그레이드, 설정 등 UI 켜지면 활성화 될 것..

    [Header("2D Light & SellButtonUIAnim")]
    // 점토를 잡아들었을 때 그림자랑 빛 효과가 켜지도록..
    // 아래 네 변수들은 ClayAction 에서 쓸 것..
    public GameObject sellShadow;
    public GameObject sellLight;
    public GameObject sellButtonAnim;
    public GameObject windowLight;
    public GameObject shinLight;


    [Header("GameData UI")]
    public Text goldText;
    public Text loveText;

    // 안내문구 텍스트..
    public Text infoText;
    public GameObject infoTextAnim;


    [Header("IteM UI")]
    // 점토 하우스 레벨업 할 때마다 아이템 추가되도록..
    public GameObject level2Item;
    public GameObject level3Item;
    public GameObject level4Item;
    public GameObject level5Item;
    public string curTargetItem;

    // level 4 되면 켜지도록..
    public GameObject windoLight;


    [Header("Game Sound && Exit UI")]
    public GameObject optionPanel;

    [Header("IconButtonUI")]
    public GameObject upgradeButton;
    public GameObject buyButton;
    public GameObject travelButton;

    [Header("Clay SellAndBuy & Unlock UI")]
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
    public GameObject buyClayPanel;

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

    [Header("Effect")]
    public ParticleSystem unlockEffectPrefab;
    public ParticleSystem sellEffectPrefab;
    public ParticleSystem upgradeEffectPrefab;
    public ParticleSystem clayLevelUpPrefab;

    public ParticleSystem unLockEffect;
    public ParticleSystem sellEffect;
    public ParticleSystem upgradeEffect;
    public ParticleSystem clayLevelUpEffect;

    public bool isFirstUnlock;
    public bool isFirstSell;
    public bool isFirstUpgrade;
    public bool isFirstClayLevelUp;

    // 일단 기본 점토들만 쓸 거라 크기 5개로 해놨습니다.
    [Header("BuyClayData")]
    public Sprite[] claySpriteList; // 점토 사진
    public string[] clayNameList; // 점토 이름
    public int[] clayGoldList; // 점토 구매가격

    // 활성화된 해금버튼을 누르면 값이 변경됨. true로 (즉 사전에 점토가 저장되면.. 그렇다면 사전 점토 클래스를 새로 만들어야..)
    public bool[] isUnlocked; // 맨 처음 기본 점토 말고는 다 false로 해놓을 것..

    [Header("ClayBookData")]
    // 야생에서 점토 잡아오면 clayBook 의 register 함수를 통해서 해당 점토의 isLocked 값이 false 가 됨.
    public bool[] isLocked; // 사전에 등록되었는지 여부 확인용(사전에 등록되면 이제 BuyClayData 의 isUnlocked 값이 true 가 되어서 해금버튼이 활성화 됨.)


    [Header("UpgradeClayData")]
    public int[] clayHouseUpgradePrice;
    public int[] clayToyUpgradePrice;


    [Header("GameData")]
    // 노멀은 일반 애니메이터, 아이템은 아이템과 점토가 상호작용 할 때 쓰는 애니메이터..
    public RuntimeAnimatorController[] normalLevelAc;
    public RuntimeAnimatorController[] itemLevel2Ac;
    public RuntimeAnimatorController[] itemLevel3Ac;
    public RuntimeAnimatorController[] itemLevel4Ac;
    public RuntimeAnimatorController[] itemLevel5Ac;

    public int clayHouseLevel;
    public int clayToyLevel;
    public float gold;
    public float love;

    public static GameManager instance;

    public Spawner spawner;
    public SoundManager sound;
    public PoolManager pool;
    public ItemClicked item;
    public ClayAction clayAction;
    public Clay clay; // 다형성 이용하려고 Clay 변수로 쓸 예정입니다..

    

    private void Awake()
    {
        instance = this;

        isFirstUnlock = true;
        isFirstSell = true;
        isFirstUpgrade = true;
        isFirstClayLevelUp = true;

        clayPrice.text = "" + clayGoldList[0];
        clayName.text = clayNameList[0];
        unlockClayImage.sprite = claySpriteList[0];

        // 초기화 해놓기..
        loveText.text = love + "";
        goldText.text = gold + "";

        unlockPanel.SetActive(true);
    }

    private void Update()
    {
        // 터치 카운트 다 채우면 레벨업..
        if (clay.touchCount == clay.exps[clay.level])
        {
            clay.LevelUp();

            if (isFirstClayLevelUp)
            {
                // 레벨업 이벤트가 처음 실행되면 이펙트 게임 오브젝트 생성..
                isFirstClayLevelUp = false;
                clayLevelUpEffect = Instantiate(clayLevelUpPrefab, GetComponentsInChildren<Transform>()[4].transform);
                clayLevelUpEffect.transform.position = clay.transform.position; 
            } else
            {
                // 처음 실행되는 거 아니면 걍 원래 있는 게임 오브젝트의 위치만 바꿔서 재사용하기..
                clayLevelUpEffect.transform.position = clay.transform.position;
                clayLevelUpEffect.Play();
            }
        }

        if (Input.GetButtonDown("Cancel"))
        {
            // 이미 창이 켜진 상태면 끄기..
            if (optionPanel.activeSelf)
            {
                optionPanel.SetActive(false);
                return;
            }

            optionPanel.SetActive(true);
        }

        UIUpdate();
    }

    private void LateUpdate()
    {
        // 수치 변환 애니메이션
        loveText.text = string.Format("{0:n0}", Mathf.SmoothStep(float.Parse(loveText.text), love, 0.6f));
        goldText.text = string.Format("{0:n0}", Mathf.SmoothStep(float.Parse(goldText.text), gold, 0.6f));
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

    public void UIIndexReset()
    {
        currentPageIndex = 0;
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


        // ItemUI
        if (clayHouseLevel > 5) return;
        else
        {
            switch (clayHouseLevel)
            {
                case 2:
                    level2Item.SetActive(true);
                    break;
                case 3:
                    level3Item.SetActive(true);
                    break;
                case 4:
                    level4Item.SetActive(true);
                    windoLight.SetActive(true);
                    shinLight.SetActive(false);

                    break;
                case 5:
                    level5Item.SetActive(true);
                    break;
            }
        }
    }

    public void Unlock()
    {
        // 사전에 등록이 안 된 상태면 해금하기 버튼 눌러도 해금 안 됨.
        if (isLocked[currentPageIndex])
        {
            GameManager.instance.infoText.text = "흠.. " + pool.prefabs[currentPageIndex].GetComponent<Clay>().rarity + "단계 야생에서 잡아와야 할 듯 하다.";
            CloseButtonIcon("Buy");
            InfoTextAnimStart();
            
            return;
        }

        if (isFirstUnlock)
        {
            unLockEffect = Instantiate(unlockEffectPrefab, GetComponentsInChildren<Transform>()[1].transform);
            isFirstUnlock = false;
        }
        else
            unLockEffect.Play();


        // sound Manager 의 playSound 를 실행시킵니다..
        sound.PlaySound("UNLOCK");
        isUnlocked[currentPageIndex] = true;
    }

    public void BuyClay()
    {   // 버튼에 적용하기 위해 함수 제작..

        // clayHouseLevel 만큼 점토 키울 수 있음..
        // Lv.0 : 1마리, Lv.1 : 2마리, ..., Lv.4 : 5마리
        if (pool.GetComponentsInChildren<Clay>().Length == clayHouseLevel)
        {   // 한 번에 키울 수 있는 최대 점토의 수는 5마리
            infoText.text = "집이 너무 좁아요ㅠ_ㅠ";

            // 창 꺼버리도록..
            CloseButtonIcon("Buy");

            InfoTextAnimStart();
            return;
        }

        if (gold - clayGoldList[currentPageIndex] < 0)
        {   // 돈 없으면 그냥 함수 빠져나가도록..
            infoText.text = "골드가 부족해요ㅠ_ㅠ";

            // 창 꺼버리도록..
            CloseButtonIcon("Buy");

            InfoTextAnimStart();
            return;
        }

        gold -= clayGoldList[currentPageIndex];

        // 점토 구매하면 생성해서 놓아야하니까 풀매니저의 GetGameObject 함수를 사용..
        // poolManager 의 프리펩 점토 인덱스랑 currentPageIndex 가 똑같은 점토를 가리킴.. 그래서 그냥 currentPageIndex 쓰면 됨.
        //pool.GetGameObject(currentPageIndex);
        spawner.Spawn(currentPageIndex);

        // 점토 구매하면 구매창 꺼짐..
        sound.PlaySound("BUY");
        buyClayPanel.SetActive(false);
    }

    public void SellClay()
    {
        if (isFirstSell)
        {
            sellEffect = Instantiate(sellEffectPrefab, GetComponentsInChildren<Transform>()[2].transform);
            isFirstSell = false;
        }
        else
            sellEffect.Play();

        sound.PlaySound("SELL");
        gold += clay.SellClay();
    }

    public void MouseDragToSellBtn()
    {
        // button 게임 오브젝트의 Event Trigger 에서 쓰도록.. 함수 만들기 

        if (clayAction == null) return; // 아무것도 참조 안 하고 있으면 밑 코드 실행 안되도록..
        clayAction.mouseDragToSellBtn = true;
    }
    public void MouseNotDragToSellBtn()
    {
        // button 에서 벗어나면 false 되도록..
        if (clayAction == null) return; // 아무것도 참조 안 하고 있으면 밑 코드 실행 안되도록..
        clayAction.mouseDragToSellBtn = false;
    }

    public void ClayHouseLevelUp()
    {
        if (love - clayHouseUpgradePrice[clayHouseLevel] < 0)
        {
            infoText.text = "애정이 부족해요ㅠ_ㅠ";

            // 창 꺼버리도록..
            CloseButtonIcon("Upgrade");

            InfoTextAnimStart();
            return;
        }

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

        if (isFirstUpgrade)
        {
            upgradeEffect = Instantiate(upgradeEffectPrefab, GetComponentsInChildren<Transform>()[3].transform);
            isFirstUpgrade = false;
        }
        else
            upgradeEffect.Play();

        sound.PlaySound("BUY");
    }

    public void ClayToyLevelUp()
    {
        if (love - clayToyUpgradePrice[clayToyLevel] < 0)
        {
            infoText.text = "애정이 부족해요ㅠ_ㅠ";

            // 창 꺼버리도록..
            CloseButtonIcon("Upgrade");

            InfoTextAnimStart();
            return;
        }

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

        if (isFirstUpgrade)
        {
            upgradeEffect = Instantiate(upgradeEffectPrefab, GetComponentsInChildren<Transform>()[3].transform);
            isFirstUpgrade = false;
        }
        else
            upgradeEffect.Play();

        sound.PlaySound("BUY");
    }

    public void ChangeAc(int level)
    {
        GameManager.instance.clayAction.animList[0] = normalLevelAc[level];
        GameManager.instance.clayAction.animList[1] = itemLevel3Ac[level];
        GameManager.instance.clayAction.animList[2] = itemLevel2Ac[level];
        GameManager.instance.clayAction.animList[3] = itemLevel4Ac[level];
        GameManager.instance.clayAction.animList[4] = itemLevel5Ac[level];
    }

    public void GameEixt()
    {
        Application.Quit();
    }

    public void InfoTextAnimStart()
    {
        if (infoTextAnim.activeSelf)
        {
            // 이미 창 켜진 상탠데 또 켜려고 하면 처음부터 다시 시작하도록..
            InfoTextAnimClose();
            InfoTextAnimStart(); // 재귀
            return;
        }

        infoTextAnim.SetActive(true);
        infoTextAnim.GetComponent<Animator>().SetTrigger("isStarted");

        Invoke("InfoTextAnimClose", 3f);
    }

    void InfoTextAnimClose()
    {
        infoTextAnim.SetActive(false);
        infoTextAnim.GetComponent<Animator>().SetTrigger("isClosed");
    }

    void CloseButtonIcon(string icon)
    {
        switch (icon)
        {
            case "Buy":
                // 점토 구매 관련
                buyClayPanel.SetActive(false);
                buyButton.GetComponent<ButtonChanger>().ResetButtonSprite();
                buyButton.GetComponent<ButtonChanger>().SetBoolButton();
                return;
            case "Upgrade":
                plantPanel.SetActive(false);
                upgradeButton.GetComponent<ButtonChanger>().ResetButtonSprite();
                upgradeButton.GetComponent<ButtonChanger>().SetBoolButton();
                return;
            case "Travel":
                // 야생 기능 생기면 채워 넣기..
                return;
        }
        

        // 용품 업그레이드 관련

        // 여행 관련
    }

    public void ExiteButtonToFalse()
    {
        exitButton.SetActive(false);
    }
}
