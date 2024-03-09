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
        // 아이템에 마우스 갔다 댈 때마다 게임매니저가 참조하도록..
        GameManager.instance.item = this;
        button.image.sprite = clickedSprite;

        // 점토랑 상호작용 시킬 아이템 구별하기 위해..
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
