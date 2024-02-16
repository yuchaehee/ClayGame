using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClayBook : MonoBehaviour
{
    [Header("Animal Information UI")]
    public GameObject ClayBookPanel;

    // ������ ��� �� �� ����
    public GameObject unlockClayBookPage;
    public Image unlockClayImage;
    public Text unlockClayName;

    // ������ ��� �� ����
    public GameObject lockClayBookPage;
    public Image lockClayImage;

    // ���� �κ�
    public Text ClayInformation;
    public Text ClaySubText;

    public int maxPageIndex;
    public int minPageIndex;
    public int curPageIndex;

    [Header("ClayInformation")]
    public string[] clayInformationList;

    // �ϴ� �⺻ �ټ������� �س����ϴ�..
    public bool[] isRegistered;

    private void Awake()
    {
        maxPageIndex = isRegistered.Length - 1;
        minPageIndex = 0;
        curPageIndex = 0;

        clayInformationList = new string[] { "���� ������ӿ�.", "���� ���Ƹ��ӿ�.", "���� �������ӿ�.", "���� �䳢�ӿ�.", "���� �ٶ����ӿ�." };
    }

    private void Update()
    {
        // �ϴ� ���� ��ư ������ �ߵ��� �س����ϴ�.. ���߿� ��ư���� ������ ��..
        if (Input.GetButtonDown("Horizontal"))
        {
            // �̹� �� ���¿��� �� ������ �׳� â ���ְ� ��������..
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

        ClaySubText.text = "��͵� " + (GameManager.instance.pool.prefabs[curPageIndex].GetComponent<Clay>().rarity + 1);
        ClayInformation.text = clayInformationList[curPageIndex];
    }

    public void pageIndexUp()
    {
        // ���� ������ �ε����� �ִ� ������ �ε������� ũ�ų� ������ ���� ������ �� �Ѿ����..
        if (curPageIndex >= maxPageIndex)
            return;

        curPageIndex++;
    }

    public void pageIndexDown()
    {
        // ���� ������ �ε����� �ּ� ������ �ε������� �۰ų� ������ ���� ������ �� �Ѿ����..
        if (curPageIndex <= minPageIndex)
            return;

        curPageIndex--;
    }
}
