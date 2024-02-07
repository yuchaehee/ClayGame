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

    // �� �ڶ� ������ ����� �����س���,, ������ ���� ������ �����ϸ� ��������Ʈ�� �ٲ��ֱ�..
    public Sprite maxSprite;
    public int maxLevel;
    public int x;
    public int y;
    public float speed; // �� ����Ʈ�� ���丶�� �ٸ��� ���� ���� ��� ���Դϴ�..

    private void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        speed = 0.5f; // ��� ���̶� �ϴ� �������� �س����ϴ�..
    }

    private void Start()
    {
        // ���� �����̴� �ӵ� ���� �Լ�..
        inputVec();

        // Ŭ���̰� �����Ǹ� �ϴ� gameManger �� clay ������ �����ϵ��� �߽��ϴ�..
        GameManager.instance.clay = this.GetComponent<Clay>();
    }

    private void OnEnable()
    {
        // �ٽ� Ȱ��ȭ �Ǿ��� ���� gameManager �� clay �� �����ϵ���..
        GameManager.instance.clay = this.GetComponent<Clay>();
        // �ٽ� Ȱ��ȭ �Ǿ��� �� level, touchCount ���� ���� �ʱ�ȭ�ϱ� ����..
        GameManager.instance.clay.SetClayData();
    }

    private void Update()
    {
        if (GameManager.instance.clay.level == maxLevel)
            spriter.sprite = maxSprite;

        if (x != 0)
        {
            // �̰Ŵ� Ŭ���̰� �� �ڶ��� �� ���� ���� �־ ���� �ٲ��ִ� ������ �߽��ϴ�.. �� ���ڶ� Ŭ���̴� ���� ���� �־ �� ��쿣 ������� �մϴ�.. �׷��� �ϴ�..
            spriter.flipX = x > 0;
        }
    }

    private void OnMouseDown()
    { 
        // maxLevel �� �ϴ� 4�� �س����ϴ�...
        if (GameManager.instance.clay.level == maxLevel)
        {
            GameManager.instance.gold += GameManager.instance.clay.SellClay();
            Sell(); // ���� �ȸ��� Ȱ��ȭ ���� �Լ�..
        }

        // ���� Ŭ���ϸ� ���� �Ŵ����� Ŭ���� ������ Ŭ�� �� ���並 �����ϵ���..
        GameManager.instance.clay = this.GetComponent<Clay>();
        GameManager.instance.love += GameManager.instance.clay.TouchClay();
        GameManager.instance.clay.touchCount++;

        GameManager.instance.sound.PlaySound("SELL");
        anim.SetTrigger("doTouch");
    }

    private void FixedUpdate()
    {
        Vector2 moveVec = Vector2.zero;
        Vector2 moveVecX = Vector2.zero;
        Vector2 moveVecY  = Vector2.zero;

        if (x < 0)
            moveVecX = Vector2.left;
        else if (x > 0)
            moveVecX = Vector2.right;

        if (y < 0)
            moveVecY = Vector2.down;
        else if (y > 0)
            moveVecY = Vector2.up;

        moveVec = moveVecX + moveVecY;
        rigid.MovePosition(rigid.position + (moveVec.normalized * Time.fixedDeltaTime * speed));
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
