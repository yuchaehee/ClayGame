using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonChanger : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    Button uiButton;
    Animator anim;

    // 버튼이 눌린 상태와 아닌 상태를 구분하기 위한 sprite 저장..
    public Sprite buttonSprite1;
    public Sprite buttonSprite2;

    private void Awake()
    {
        uiButton = GetComponent<Button>();
        anim = GetComponent<Animator>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 눌렸으면 IconOver sprite 로 바꿔주기
        uiButton.image.sprite = buttonSprite2;

        anim.SetBool("isClicked", true);
        anim.SetBool("isClosed", false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 이미 버튼이 눌려있는 상태에서 마우스를 가져다대면 위아래로 움직이는 모션 실행되지 않도록 분기 나누기..
        if (!anim.GetBool("isClicked"))
            anim.SetBool("isDraged", true);
        else
        {
            // 버튼이 눌린 상태면 버튼으로 마우스 갖다대도 false 로...
            anim.SetBool("isDraged", false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        anim.SetBool("isDraged", false);
    }

    public void SetBoolButton()
    {
        anim.SetBool("isClosed", true);
        anim.SetBool("isClicked", false);
    }

    public void ResetButtonSprite()
    {
        // 나가기 버튼 누르면 이 함수가 실행되도록 Event Trigger 에 연동할 것임...
        uiButton.image.sprite = buttonSprite1;
        
    }
}
