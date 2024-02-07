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

    // 다 자란 상태의 모습을 저장해놓고,, 레벨이 최종 레벨에 도달하면 스프라이트를 바꿔주기..
    public Sprite maxSprite;
    public int maxLevel;
    public int x;
    public int y;
    public float speed; // 이 스피트를 점토마다 다르게 줄지 말지 고민 중입니다..

    private void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        speed = 0.5f; // 고민 중이라 일단 고정으로 해놨습니다..
    }

    private void Start()
    {
        // 점토 움직이는 속도 관련 함수..
        inputVec();

        // 클레이가 생성되면 일단 gameManger 의 clay 변수가 참조하도록 했습니다..
        GameManager.instance.clay = this.GetComponent<Clay>();
    }

    private void OnEnable()
    {
        // 다시 활성화 되었을 때도 gameManager 가 clay 를 참조하도록..
        GameManager.instance.clay = this.GetComponent<Clay>();
        // 다시 활성화 되었을 때 level, touchCount 등의 값을 초기화하기 위함..
        GameManager.instance.clay.SetClayData();
    }

    private void Update()
    {
        if (GameManager.instance.clay.level == maxLevel)
            spriter.sprite = maxSprite;

        if (x != 0)
        {
            // 이거는 클레이가 다 자랐을 때 옆을 보고 있어서 방향 바꿔주는 용으로 했습니다.. 다 안자란 클레이는 정면 보고 있어서 이 경우엔 상관없긴 합니다.. 그래도 일단..
            spriter.flipX = x > 0;
        }
    }

    private void OnMouseDown()
    { 
        // maxLevel 은 일단 4로 해놨습니다...
        if (GameManager.instance.clay.level == maxLevel)
        {
            GameManager.instance.gold += GameManager.instance.clay.SellClay();
            Sell(); // 점토 팔리면 활성화 끄는 함수..
        }

        // 점토 클릭하면 게임 매니저의 클레이 변수가 클릭 된 점토를 참조하도록..
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
        // 활성화 꺼지면 정보 초기화..
        GameManager.instance.clay.SetClayData();
    }

    void inputVec()
    {
        // 점토 움직이는 속도 계속 바꿔주려고 만든 함수입니다.. start에서 한번 씁니다..

        x = Random.Range(-1, 2);
        y = Random.Range(-1, 2);


        Invoke("inputVec", 4);
    }

    public void Sell()
    {
        // 점토 팔리면 활성화 끕니다..
        gameObject.SetActive(false);
    }
}
