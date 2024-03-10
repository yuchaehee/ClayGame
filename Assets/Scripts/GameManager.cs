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
    // �̰� clayAction ���� �� ��.. ���� ���ø��� UI ĵ���� ����������..
    public GameObject UICanvas;
    public GameObject exitButton; // ��� ����, ���׷��̵�, ���� �� UI ������ Ȱ��ȭ �� ��..

    [Header("2D Light & SellButtonUIAnim")]
    // ���並 ��Ƶ���� �� �׸��ڶ� �� ȿ���� ��������..
    // �Ʒ� �� �������� ClayAction ���� �� ��..
    public GameObject sellShadow;
    public GameObject sellLight;
    public GameObject sellButtonAnim;
    public GameObject windowLight;
    public GameObject shinLight;


    [Header("GameData UI")]
    public Text goldText;
    public Text loveText;

    // �ȳ����� �ؽ�Ʈ..
    public Text infoText;
    public GameObject infoTextAnim;


    [Header("IteM UI")]
    // ���� �Ͽ콺 ������ �� ������ ������ �߰��ǵ���..
    public GameObject level2Item;
    public GameObject level3Item;
    public GameObject level4Item;
    public GameObject level5Item;
    public string curTargetItem;

    // level 4 �Ǹ� ��������..
    public GameObject windoLight;


    [Header("Game Sound && Exit UI")]
    public GameObject optionPanel;

    [Header("IconButtonUI")]
    public GameObject upgradeButton;
    public GameObject buyButton;
    public GameObject travelButton;

    [Header("Clay SellAndBuy & Unlock UI")]
    // �ر� ���� ��� ���� ���� ����..
    public Text pageText;

    // �ر� �Ϸ� ���¿����� UI ����..
    public Text clayPrice;
    public Text clayName;
    public Image unlockClayImage;

    // �ر� ��Ϸ� ���¿����� UI ����..
    public Image lockClayImage;

    public GameObject unlockPanel; // �ر� �� ���� �� �갡 Ȱ��ȭ �Ǿ��ֱ�..
    public GameObject lockPanel; // �ر� �Ǹ� �갡..(�ر��ϱ� ��ư ������ Ȱ��ȭ on, �ر��ϱ� ��ư�� unlock�� ����.)
    public GameObject buyClayPanel;

    public int finalPageIndex; // ��ư ������ �� �� �ε��� ����� �� �Ѿ����..(�ϴ� �⺻ ���丸 ���Ŷ� 4�� �س��ҽ��ϴ�..)
    public int currentPageIndex;

    public GameObject unlockButton; // ������ ����� �� �� ���¸� �ر��ϱ� ��ư ������ ���� x


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

    // �ϴ� �⺻ ����鸸 �� �Ŷ� ũ�� 5���� �س����ϴ�.
    [Header("BuyClayData")]
    public Sprite[] claySpriteList; // ���� ����
    public string[] clayNameList; // ���� �̸�
    public int[] clayGoldList; // ���� ���Ű���

    // Ȱ��ȭ�� �رݹ�ư�� ������ ���� �����. true�� (�� ������ ���䰡 ����Ǹ�.. �׷��ٸ� ���� ���� Ŭ������ ���� ������..)
    public bool[] isUnlocked; // �� ó�� �⺻ ���� ����� �� false�� �س��� ��..

    [Header("ClayBookData")]
    // �߻����� ���� ��ƿ��� clayBook �� register �Լ��� ���ؼ� �ش� ������ isLocked ���� false �� ��.
    public bool[] isLocked; // ������ ��ϵǾ����� ���� Ȯ�ο�(������ ��ϵǸ� ���� BuyClayData �� isUnlocked ���� true �� �Ǿ �رݹ�ư�� Ȱ��ȭ ��.)


    [Header("UpgradeClayData")]
    public int[] clayHouseUpgradePrice;
    public int[] clayToyUpgradePrice;


    [Header("GameData")]
    // ����� �Ϲ� �ִϸ�����, �������� �����۰� ���䰡 ��ȣ�ۿ� �� �� ���� �ִϸ�����..
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
    public Clay clay; // ������ �̿��Ϸ��� Clay ������ �� �����Դϴ�..

    

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

        // �ʱ�ȭ �س���..
        loveText.text = love + "";
        goldText.text = gold + "";

        unlockPanel.SetActive(true);
    }

    private void Update()
    {
        // ��ġ ī��Ʈ �� ä��� ������..
        if (clay.touchCount == clay.exps[clay.level])
        {
            clay.LevelUp();

            if (isFirstClayLevelUp)
            {
                // ������ �̺�Ʈ�� ó�� ����Ǹ� ����Ʈ ���� ������Ʈ ����..
                isFirstClayLevelUp = false;
                clayLevelUpEffect = Instantiate(clayLevelUpPrefab, GetComponentsInChildren<Transform>()[4].transform);
                clayLevelUpEffect.transform.position = clay.transform.position; 
            } else
            {
                // ó�� ����Ǵ� �� �ƴϸ� �� ���� �ִ� ���� ������Ʈ�� ��ġ�� �ٲ㼭 �����ϱ�..
                clayLevelUpEffect.transform.position = clay.transform.position;
                clayLevelUpEffect.Play();
            }
        }

        if (Input.GetButtonDown("Cancel"))
        {
            // �̹� â�� ���� ���¸� ����..
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
        // ��ġ ��ȯ �ִϸ��̼�
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
        // ����â UI ���� �ڵ�
        if (currentPageIndex < 9)
            pageText.text = "#0" + (currentPageIndex + 1);
        else
            pageText.text = "#" + (currentPageIndex + 1);


        unlockPanel.SetActive(isUnlocked[currentPageIndex]);
        lockPanel.SetActive(!isUnlocked[currentPageIndex]); // �ر� �� ���¸� lockPanel �� ������ �ϴϱ� ! ����..

        clayPrice.text = "" + clayGoldList[currentPageIndex];
        clayName.text = clayNameList[currentPageIndex];
        unlockClayImage.sprite = claySpriteList[currentPageIndex];
        lockClayImage.sprite = claySpriteList[currentPageIndex];


        // ���� ��ǰ ������Ʈ ���� UI �ڵ�
        clayHouseText.text = "���� ���뷮 " + (clayHouseLevel);
        clayToyText.text = "Ŭ�� ���귮 x " + (clayToyLevel);

        if (clayHouseLevel >= clayHouseUpgradePrice.Length)
            houseBtnText.text = "��";
        else
            houseBtnText.text = "" + clayHouseUpgradePrice[clayHouseLevel];

        if (clayToyLevel >= clayToyUpgradePrice.Length)
            toyBtnText.text = "��";
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
        // ������ ����� �� �� ���¸� �ر��ϱ� ��ư ������ �ر� �� ��.
        if (isLocked[currentPageIndex])
        {
            GameManager.instance.infoText.text = "��.. " + pool.prefabs[currentPageIndex].GetComponent<Clay>().rarity + "�ܰ� �߻����� ��ƿ;� �� �� �ϴ�.";
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


        // sound Manager �� playSound �� �����ŵ�ϴ�..
        sound.PlaySound("UNLOCK");
        isUnlocked[currentPageIndex] = true;
    }

    public void BuyClay()
    {   // ��ư�� �����ϱ� ���� �Լ� ����..

        // clayHouseLevel ��ŭ ���� Ű�� �� ����..
        // Lv.0 : 1����, Lv.1 : 2����, ..., Lv.4 : 5����
        if (pool.GetComponentsInChildren<Clay>().Length == clayHouseLevel)
        {   // �� ���� Ű�� �� �ִ� �ִ� ������ ���� 5����
            infoText.text = "���� �ʹ� ���ƿ��_��";

            // â ����������..
            CloseButtonIcon("Buy");

            InfoTextAnimStart();
            return;
        }

        if (gold - clayGoldList[currentPageIndex] < 0)
        {   // �� ������ �׳� �Լ� ������������..
            infoText.text = "��尡 �����ؿ��_��";

            // â ����������..
            CloseButtonIcon("Buy");

            InfoTextAnimStart();
            return;
        }

        gold -= clayGoldList[currentPageIndex];

        // ���� �����ϸ� �����ؼ� ���ƾ��ϴϱ� Ǯ�Ŵ����� GetGameObject �Լ��� ���..
        // poolManager �� ������ ���� �ε����� currentPageIndex �� �Ȱ��� ���並 ����Ŵ.. �׷��� �׳� currentPageIndex ���� ��.
        //pool.GetGameObject(currentPageIndex);
        spawner.Spawn(currentPageIndex);

        // ���� �����ϸ� ����â ����..
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
        // button ���� ������Ʈ�� Event Trigger ���� ������.. �Լ� ����� 

        if (clayAction == null) return; // �ƹ��͵� ���� �� �ϰ� ������ �� �ڵ� ���� �ȵǵ���..
        clayAction.mouseDragToSellBtn = true;
    }
    public void MouseNotDragToSellBtn()
    {
        // button ���� ����� false �ǵ���..
        if (clayAction == null) return; // �ƹ��͵� ���� �� �ϰ� ������ �� �ڵ� ���� �ȵǵ���..
        clayAction.mouseDragToSellBtn = false;
    }

    public void ClayHouseLevelUp()
    {
        if (love - clayHouseUpgradePrice[clayHouseLevel] < 0)
        {
            infoText.text = "������ �����ؿ��_��";

            // â ����������..
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
            infoText.text = "������ �����ؿ��_��";

            // â ����������..
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
            // �̹� â ���� ���ĵ� �� �ѷ��� �ϸ� ó������ �ٽ� �����ϵ���..
            InfoTextAnimClose();
            InfoTextAnimStart(); // ���
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
                // ���� ���� ����
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
                // �߻� ��� ����� ä�� �ֱ�..
                return;
        }
        

        // ��ǰ ���׷��̵� ����

        // ���� ����
    }

    public void ExiteButtonToFalse()
    {
        exitButton.SetActive(false);
    }
}
