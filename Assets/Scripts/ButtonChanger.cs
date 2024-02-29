using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonChanger : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    Button uiButton;
    Animator anim;

    // ��ư�� ���� ���¿� �ƴ� ���¸� �����ϱ� ���� sprite ����..
    public Sprite buttonSprite1;
    public Sprite buttonSprite2;

    private void Awake()
    {
        uiButton = GetComponent<Button>();
        anim = GetComponent<Animator>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // �������� IconOver sprite �� �ٲ��ֱ�
        uiButton.image.sprite = buttonSprite2;

        anim.SetBool("isClicked", true);
        anim.SetBool("isClosed", false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // �̹� ��ư�� �����ִ� ���¿��� ���콺�� �����ٴ�� ���Ʒ��� �����̴� ��� ������� �ʵ��� �б� ������..
        if (!anim.GetBool("isClicked"))
            anim.SetBool("isDraged", true);
        else
        {
            // ��ư�� ���� ���¸� ��ư���� ���콺 ���ٴ뵵 false ��...
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
        // ������ ��ư ������ �� �Լ��� ����ǵ��� Event Trigger �� ������ ����...
        uiButton.image.sprite = buttonSprite1;
        
    }
}
