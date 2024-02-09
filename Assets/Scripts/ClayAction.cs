using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClayAction : MonoBehaviour
{
    SpriteRenderer spriter;
    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;

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

    private void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        speed = 0.5f; // ��� ���̶� �ϴ� �������� �س����ϴ�..

        // ��ũ�� �� sellBox ��ġ..
        SellBoxPos = new Vector3(359.75f, -149, 0);
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
        GameManager.instance.ChangeAc(GetComponent<Animator>(), GameManager.instance.clay.level);
        // �ٽ� Ȱ��ȭ �Ǹ� sprite �� ù��°�� �ʱ�ȭ..
        spriter.sprite = firstSprite;
    }

    private void Update()
    {
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

        GameManager.instance.sound.PlaySound("TOUCH");
        anim.SetTrigger("doTouch");
    }

    private void OnMouseDrag()
    {   // ���� �巡�� ���..

        GameManager.instance.clayAction = this.GetComponent<ClayAction>();
        GameManager.instance.clay = this.GetComponent<Clay>();

        // ���� ��� ������ ������ �ð� ������,,
        touchTime += Time.deltaTime;

        if (touchTime > 0.5f)
        {
            transform.position = mousePos;
        }
    }

    private void OnMouseUp()
    {
        GameManager.instance.clayAction = this.GetComponent<ClayAction>();
        GameManager.instance.clay = this.GetComponent<Clay>();

        // ���콺 Ŀ���� ���� �Ǹ� ��ư�̶� ������ mouseDragToSellBtn �� true �� ��..
        if (mouseDragToSellBtn && GameManager.instance.clay.level==maxLevel)
        {
            // ���� ������ �ϴ� 4�� �س����ϴ�..
            // ���� ��ġ�� �Ǹ�â ��ġ�� ������ ���� �ȸ�����,, (�ٵ� ���䰡 ���� ������ �������� ����,,)
            GameManager.instance.SellClay();

            Sell(); // ���� �ȸ��� Ȱ��ȭ ���� �Լ�..

            // �ٽ� �����·� �س���..
            mouseDragToSellBtn = false;
        }

        if (transform.position.x < -5.4 || transform.position.x > 5.4)
            transform.position = prevPos;
        if (transform.position.y < -1.7 || transform.position.y > 1.3)
            transform.position = prevPos;
    }

    private void FixedUpdate()
    {
        // ���ٰ� �ַ��� z�� ���� ��ȭ��Ŵ
        transform.Translate(new Vector3(x, y, y).normalized * Time.fixedDeltaTime * speed);
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
}
