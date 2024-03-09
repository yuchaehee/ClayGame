using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClayAction : MonoBehaviour
{
    SpriteRenderer spriter;
    Rigidbody2D rigid;
    Collider2D coll;

    // �������̶� ���䰡 ��ȣ�ۿ��ϴ� �ִϸ��̼��̶�, �Ϲ� �ִϸ��̼��̶� ������ ������..
    
    // 0:normalAnim, 1:itemLevel3Anim, 2:itemLevel2Anim, 3:itemLevel4Anim, 4:itemLevel5Anim
    public RuntimeAnimatorController[] animList;
    public Animator curAnim;

    Vector2 dirVec;

    public Sprite firstSprite;
    // �� �ڶ� ������ ����� �����س���,, ������ ���� ������ �����ϸ� ��������Ʈ�� �ٲ��ֱ�..
    public Sprite lastSprite;
    public int maxLevel;
    public int x;
    public int y;
    public float speed; // �� ����Ʈ�� ���丶�� �ٸ��� ���� ���� ��� ���Դϴ�..
    public Vector3 mousePos;
    public Vector3 prevPos;

    Vector3 SellBoxPos;
    float touchTime;
    public bool mouseDragToSellBtn;

    public bool isActioned;
    public bool delay;
    public bool isDraging;
    public bool isDraging_;


    private void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        curAnim = GetComponent<Animator>();
        speed = 0.5f; // ��� ���̶� �ϴ� �������� �س����ϴ�..

        // ��ũ�� �� sellBox ��ġ..
        SellBoxPos = new Vector3(359.75f, -149, 0);

        isDraging = false;
        isActioned = false;
    }

    private void Start()
    {
        // ���� �����̴� �ӵ� ���� �Լ�..
        inputVec();

        GameManager.instance.clayAction = this.GetComponent<ClayAction>();
        // Ŭ���̰� �����Ǹ� �ϴ� gameManger �� clay ������ �����ϵ��� �߽��ϴ�..
        GameManager.instance.clay = this.GetComponent<Clay>();
    }

    private void OnEnable()
    {
        GameManager.instance.clayAction = this.GetComponent<ClayAction>();

        // �ٽ� Ȱ��ȭ �Ǿ��� ���� gameManager �� clay �� �����ϵ���..
        GameManager.instance.clay = this.GetComponent<Clay>();
        // �ٽ� Ȱ��ȭ �Ǿ��� �� level, touchCount ���� ���� �ʱ�ȭ�ϱ� ����..
        GameManager.instance.clay.SetClayData();
        // �ٽ� Ȱ��ȭ �Ǹ� animator �� ù��°�� �ʱ�ȭ..
        GameManager.instance.ChangeAc(GameManager.instance.clay.level);
        curAnim.runtimeAnimatorController = animList[0];
        // �ٽ� Ȱ��ȭ �Ǹ� sprite �� ù��°�� �ʱ�ȭ..
        spriter.sprite = firstSprite;
    }

    private void Update()
    {
        // �ɾ�ٴϴ� �ִϸ��̼� ����..
        // �ִϸ����Ͱ� itemAnim �� ���� �Ʒ� �ڵ� ���� �� �ǵ��� if �� �� ��.. 
        //if (curAnim.runtimeAnimatorController != animList[1] && curAnim.runtimeAnimatorController != animList[2])

        if (curAnim.runtimeAnimatorController == animList[0]) {
            if (x == 0 && y == 0)
                curAnim.SetBool("isWalk", false);
            else
                curAnim.SetBool("isWalk", true);
        }

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = mousePos.y; // ���ٰ� �ַ���.. z ��ġ�� y ��ġ�� ���� ���� ��..

        // ��� Ŭ���� ���丸 ��������Ʈ �ٲ� �Ŷ�, ���ӸŴ����� ����Ű�� �ִ� ���䰡 �� �� �� ��� �ι�° ���� �߰���.
        if (GameManager.instance.clay.level == maxLevel && GameManager.instance.clay == this.GetComponent<Clay>())
            spriter.sprite = lastSprite;


        if (x != 0)
        {
            // �̰Ŵ� Ŭ���̰� �� �ڶ��� �� ���� ���� �־ ���� �ٲ��ִ� ������ �߽��ϴ�.. �� �� �ڶ� Ŭ���̴� ���� ���� �־ �� ��쿣 ������� �մϴ�.. �׷��� �ϴ�..
            spriter.flipX = x > 0;
        }
    }

    private void OnMouseDown()
    {
        // ������ ���� ��ġ�� ����س��ٰ�, ���� ���䰡 ��� ������ �����,, �ٽ� ����ġ��Ŵ,,
        prevPos = transform.position;
        // ���콺�� ���� Ŭ������ �� �ð� 0���� ��������..
        touchTime = 0;

        // ���� Ŭ���ϸ� ���� �Ŵ����� Ŭ���� ������ Ŭ�� �� ���並 �����ϵ���..
        GameManager.instance.clayAction = this.GetComponent<ClayAction>();
        GameManager.instance.clay = this.GetComponent<Clay>();
        GameManager.instance.love += GameManager.instance.clay.TouchClay();
        GameManager.instance.clay.touchCount++;


        // �ִϸ����Ͱ� itemAnim �� ���� �Ʒ� �ڵ� ���� �� �ǵ��� if �� �� ��.. 
        if (curAnim.runtimeAnimatorController == animList[0])
        {
            GameManager.instance.sound.PlaySound("TOUCH");
            curAnim.SetTrigger("doTouch");
        }
    }

    private void OnMouseDrag()
    {
        // ������Ű��..
        GameManager.instance.clayAction = this.GetComponent<ClayAction>();
        GameManager.instance.clay = this.GetComponent<Clay>();

        // ���� ��� ������ ������ �ð� ������,,
        touchTime += Time.deltaTime;

        if (touchTime > 0.5f)
        {
            // ���� �巡�� ���..
            isDraging = true;
            isDraging_ = true;

            transform.position = mousePos;

            // ���� ���ø��� UICanvas ��������..
            GameManager.instance.UICanvas.SetActive(false);

            // ���並 ���ø��� ���̶� �׸��ڰ� ���� ��..
            GameManager.instance.sellLight.SetActive(true);
            GameManager.instance.sellShadow.SetActive(true);

            // ���並 ���ø��� â���� ���� ��..
            if (GameManager.instance.clayHouseLevel == 4) 
                GameManager.instance.windowLight.SetActive(false);

            // ���並 ���ø��� �� ��ư�� �ִϸ��̼� ����..
            // �ִϸ����Ͱ� itemAnim �� ���� �Ʒ� �ڵ� ���� �� �ǵ��� if �� �� ��.. 
            if (curAnim.runtimeAnimatorController == animList[0])
                GameManager.instance.sellButtonAnim.GetComponent<Animator>().SetBool("isStarted", true);
        }
    }

    private void OnMouseUp()
    {
        isDraging_ = false;

        // ���� ���������� UICanvas �ٽ� ��������..
        GameManager.instance.UICanvas.SetActive(true);

        // ���並 �������� �ƴ� �׳� �ٴڿ� ������ �� isDraging �� false �� ������ֱ�..
        if (!GameManager.instance.item.mouseDragToItem)  
            isDraging = false;

        // ���並 ���������� ���̶� �׸��ڰ� ���� ��..
        GameManager.instance.sellLight.SetActive(false);
        GameManager.instance.sellShadow.SetActive(false);

        // ���並 ���ø��� â���� ���� ��..
        if (GameManager.instance.clayHouseLevel == 4)
            GameManager.instance.windowLight.SetActive(true);

        // �ִϸ��̼� ����..
        // �ִϸ����Ͱ� itemAnim �� ���� �Ʒ� �ڵ� ���� �� �ǵ��� if �� �� ��.. 
        if (curAnim.runtimeAnimatorController == animList[0])
            GameManager.instance.sellButtonAnim.GetComponent<Animator>().SetBool("isStarted", false);

        GameManager.instance.clayAction = this.GetComponent<ClayAction>();
        GameManager.instance.clay = this.GetComponent<Clay>();

        // ���콺 Ŀ���� ���� �Ǹ� ��ư�̶� ������ mouseDragToSellBtn �� true �� ��..
        if ((mouseDragToSellBtn || isActioned) && GameManager.instance.clay == null) return; // ���ӸŴ����� ���並 �� ���� �� ���� ���¸� �׳� ������������..

        if (mouseDragToSellBtn && GameManager.instance.clay.level == maxLevel)
        {
            // ���� ������ �ϴ� 4�� �س����ϴ�..
            // ���� ��ġ�� �Ǹ�â ��ġ�� ������ ���� �ȸ�����,, (�ٵ� ���䰡 ���� ������ �������� ����,,)
            GameManager.instance.SellClay();

            Sell(); // ���� �ȸ��� Ȱ��ȭ ���� �Լ�..

            // �ٽ� �����·� �س���..
            mouseDragToSellBtn = false;
        } else if (mouseDragToSellBtn && GameManager.instance.clay.level != maxLevel)
        {
            GameManager.instance.infoText.text = "��ü�� �� ���丸 �о��� �� �ֽ��ϴ�.";
            GameManager.instance.InfoTextAnimStart();
        }

        if (transform.position.x <= -5.4 || transform.position.x >= 5.4)
            transform.position = prevPos;
        if (transform.position.y <= -2.4 || transform.position.y >= 0.5)
            transform.position = prevPos;
    }

    private void FixedUpdate()
    {
        // ���並 ��� ��������� �� �ڵ� �ƿ� ���� �� �ǵ��� ������..
        if (isDraging_) return;

        if (!isDraging && !delay)
        {   // ���䰡 ������ �ִϸ��̼� �����ϰ� ������ �̰� �� �����̵���..
            // ���ٰ� �ַ��� z�� ���� ��ȭ��Ŵ
            transform.Translate(new Vector3(x, y, y).normalized * Time.fixedDeltaTime * speed);
        } else if (isDraging && GameManager.instance.item.mouseDragToItem)
        {
            switch (GameManager.instance.curTargetItem) {
                case "level2Item":
                    // level()ItemAnim �ִϸ����ʹ� �������ڸ��� �ִϸ��̼� ����ǵ��� �س����ϴ�.. �׷��� setTrigger �̷��� �� ����ϴ�..
                    curAnim.runtimeAnimatorController = animList[2];
                    isDraging = false; // �ִϸ��̼� ����Ǹ� ���� false �� �ٲ��ֱ�..

                    // �����۰��� ��ȣ�ۿ��� ������ �����޵���..
                    Invoke("MakeisActionedToFalse", 3.5f);

                    break;
                case "level3Item":
                    curAnim.runtimeAnimatorController = animList[1];
                    isDraging = false; // �ִϸ��̼� ����Ǹ� ���� false �� �ٲ��ֱ�..

                    // �����۰��� ��ȣ�ۿ��� ������ �����޵���..
                    Invoke("MakeisActionedToFalse", 3.5f);

                    break;
                case "level4Item":
                    curAnim.runtimeAnimatorController = animList[3];
                    isDraging = false;

                    // �����۰��� ��ȣ�ۿ��� ������ �����޵���..
                    Invoke("MakeisActionedToFalse", 2f);

                    break;
                case "level5Item":
                    curAnim.runtimeAnimatorController = animList[4];
                    isDraging = false;

                    // �����۰��� ��ȣ�ۿ��� ������ �����޵���..
                    Invoke("MakeisActionedToFalse", 2f);

                    break;
            }
        }
    }

    private void OnDisable()
    {
        // Ȱ��ȭ ������ ���� �ʱ�ȭ..
        GameManager.instance.clay.SetClayData();
    }

    void inputVec()
    {
        // ���� �����̴� �ӵ� ��� �ٲ��ַ��� ���� �Լ��Դϴ�.. start���� �ѹ� ���ϴ�..

        x = Random.Range(-1, 2);
        y = Random.Range(-1, 2);


        Invoke("inputVec", 4);
    }

    public void Sell()
    {
        // ���� �ȸ��� Ȱ��ȭ ���ϴ�..
        gameObject.SetActive(false);
    }

    public void MakeisActionedToFalse()
    {
        // Invoke �� �����ų ��..
        isActioned = false;
        delay = true;
        isDraging = false;

        Invoke("MakeDelayToFalse", 2f);
        transform.GetComponent<Transform>().position = prevPos;

        // �ٽ� �⺻���� �ִϸ����� �ٲ��ֱ�..
        curAnim.runtimeAnimatorController = animList[0];
    }

    void MakeDelayToFalse()
    {
        delay = false;
    }
}