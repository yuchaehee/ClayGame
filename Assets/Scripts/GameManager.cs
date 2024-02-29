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
    // ���� ���ӸŴ����� �ӽ÷� UI �س��Ҿ��µ� �ϰ� �����Ŵ�� ���ֽø� �����ϰڽ��ϴ�..
    [Header("GameData UI")]
    public Text goldText;
    public Text loveText;

    [Header("Game Sound && Exit UI")]
    public GameObject optionPanel;

    [Header("Clay Sell & Unlock UI")]
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
    public GameObject buyClayPanel; // �ϴ� �����̽��� ������ �ߵ��� �س����ϴ�..

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
    public RuntimeAnimatorController[] levelAc;

    public int clayHouseLevel;
    public int clayToyLevel;
    public float gold;
    public float love;

    public static GameManager instance;

    public Spawner spawner;
    public SoundManager sound;
    public PoolManager pool;
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
    }

    public void Unlock()
    {
        // ������ ����� �� �� ���¸� �ر��ϱ� ��ư ������ �ر� �� ��.
        if (isLocked[currentPageIndex])
        {
            Debug.Log("��.. " + pool.prefabs[currentPageIndex].GetComponent<Clay>().rarity + "�ܰ� �߻����� ��ƿ;� �� �� �ϴ�..");
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
        if (pool.GetComponentsInChildren<Clay>().Length == clayHouseLevel) // �� ���� Ű�� �� �ִ� �ִ� ������ ���� 5����
            return;

        if (gold - clayGoldList[currentPageIndex] < 0) // �� ������ �׳� �Լ� ������������..
            return;

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
        clayAction.mouseDragToSellBtn = true;
    }
    public void MouseNotDragToSellBtn()
    {
        // button ���� ����� false �ǵ���..
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

        if (isFirstUpgrade)
        {
            upgradeEffect = Instantiate(upgradeEffectPrefab, GetComponentsInChildren<Transform>()[3].transform);
            isFirstUpgrade = false;
        }
        else
            upgradeEffect.Play();

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
