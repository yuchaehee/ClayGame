using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClayBook : MonoBehaviour
{
    [Header("Animal Information UI")]
    public GameObject ClayBookPanel;

    // 사전에 등록 안 된 상태
    public GameObject unlockClayBookPage;
    public Image unlockClayImage;
    public Text unlockClayName;

    // 사전에 등록 된 상태
    public GameObject lockClayBookPage;
    public Image lockClayImage;

    // 공통 부분
    public Text ClayInformation;
    public Text ClaySubText;

    public int maxPageIndex;
    public int minPageIndex;
    public int curPageIndex;

    [Header("ClayInformation")]
    public string[] clayInformationList;

    // 일단 기본 다섯마리만 해놨습니다..
    public bool[] isRegistered;

    private void Awake()
    {
        maxPageIndex = isRegistered.Length - 1;
        minPageIndex = 0;
        curPageIndex = 0;

        clayInformationList = new string[] { "저는 고양이임요.", "저는 병아리임요.", "저는 강아지임요.", "저는 토끼임요.", "저는 다람쥐임요." };
    }

    private void Update()
    {
        // 일단 수평 버튼 누르면 뜨도록 해놨습니다.. 나중에 버튼으로 변경할 것..
        if (Input.GetButtonDown("Horizontal"))
        {
            // 이미 켠 상태에서 또 누르면 그냥 창 없애고 나가도록..
            if (ClayBookPanel.activeSelf) {
                ClayBookPanel.SetActive(false);
                return;
            }
            else
                ClayBookPanel.SetActive(true);
        }

        UIUpdate();
    }

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

    public void UIUpdate()
    {
        if (isRegistered[curPageIndex])
        {
            unlockClayBookPage.SetActive(true);
            lockClayBookPage.SetActive(false);
        }
        else
        {
            unlockClayBookPage.SetActive(false);
            lockClayBookPage.SetActive(true);
        }

        unlockClayImage.sprite = GameManager.instance.claySpriteList[curPageIndex];
        unlockClayName.text = GameManager.instance.clayNameList[curPageIndex];

        lockClayImage.sprite = GameManager.instance.claySpriteList[curPageIndex];

        ClaySubText.text = "희귀도 " + (GameManager.instance.pool.prefabs[curPageIndex].GetComponent<Clay>().rarity + 1);
        ClayInformation.text = clayInformationList[curPageIndex];
    }

    public void pageIndexUp()
    {
        // 현재 페이지 인덱스가 최대 페이지 인덱스보다 크거나 같으면 다음 장으로 안 넘어가도록..
        if (curPageIndex >= maxPageIndex)
            return;

        curPageIndex++;
    }

    public void pageIndexDown()
    {
        // 현재 페이지 인덱스가 최소 페이지 인덱스보다 작거나 같으면 다음 장으로 안 넘어가도록..
        if (curPageIndex <= minPageIndex)
            return;

        curPageIndex--;
    }
}
