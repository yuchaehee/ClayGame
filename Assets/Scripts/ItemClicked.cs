using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemClicked : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite clickedSprite;
    public Sprite nonClickedSprite;

    public string clickedText;

    public Button button;

    public bool mouseDragToItem;

    private void Awake()
    {
        mouseDragToItem = false;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        // �����ۿ� ���콺 ���� �� ������ ���ӸŴ����� �����ϵ���..
        GameManager.instance.item = this;
        button.image.sprite = clickedSprite;

        // ����� ��ȣ�ۿ� ��ų ������ �����ϱ� ����..
        GameManager.instance.curTargetItem = transform.name;
        mouseDragToItem = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.instance.infoText.text = clickedText;
        GameManager.instance.InfoTextAnimStart();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseDragToItem = false;
        button.image.sprite = nonClickedSprite;
    }
}
