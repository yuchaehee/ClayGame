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
    // 다 자란 상태의 모습을 저장해놓고,, 레벨이 최종 레벨에 도달하면 스프라이트를 바꿔주기..
    public Sprite lastSprite;
    public int maxLevel;
    public int x;
    public int y;
    public float speed; // 이 스피트를 점토마다 다르게 줄지 말지 고민 중입니다..
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
        speed = 0.5f; // 고민 중이라 일단 고정으로 해놨습니다..

        // 스크린 상 sellBox 위치..
        SellBoxPos = new Vector3(359.75f, -149, 0);
    }

    private void Start()
    {
        // 점토 움직이는 속도 관련 함수..
        inputVec();

        GameManager.instance.clayAction = this.GetComponent<ClayAction>();
        // 클레이가 생성되면 일단 gameManger 의 clay 변수가 참조하도록 했습니다..
        GameManager.instance.clay = this.GetComponent<Clay>();
    }

    private void OnEnable()
    {
        GameManager.instance.clayAction = this.GetComponent<ClayAction>();
        // 다시 활성화 되었을 때도 gameManager 가 clay 를 참조하도록..
        GameManager.instance.clay = this.GetComponent<Clay>();
        // 다시 활성화 되었을 때 level, touchCount 등의 값을 초기화하기 위함..
        GameManager.instance.clay.SetClayData();
        // 다시 활성화 되면 animator 도 첫번째로 초기화..
        GameManager.instance.ChangeAc(GetComponent<Animator>(), GameManager.instance.clay.level);
        // 다시 활성화 되면 sprite 도 첫번째로 초기화..
        spriter.sprite = firstSprite;
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = mousePos.y; // 원근감 주려고.. z 위치를 y 위치랑 같게 해준 것..

        // 방금 클릭한 점토만 스프라이트 바꿀 거라서, 게임매니저가 가리키고 있는 점토가 나 일 때 라는 두번째 조건 추가함.
        if (GameManager.instance.clay.level == maxLevel && GameManager.instance.clay == this.GetComponent<Clay>())
            spriter.sprite = lastSprite;
               

        if (x != 0)
        {
            // 이거는 클레이가 다 자랐을 때 옆을 보고 있어서 방향 바꿔주는 용으로 했습니다.. 다 안 자란 클레이는 정면 보고 있어서 이 경우엔 상관없긴 합니다.. 그래도 일단..
            spriter.flipX = x > 0;
        }
    }

    private void OnMouseDown()
    {
        // 점토의 원래 위치를 기억해놨다가, 만약 점토가 경계 영역을 벗어나면,, 다시 원위치시킴,,
        prevPos = transform.position;
        // 마우스로 점토 클릭했을 때 시간 0으로 만들어놓기..
        touchTime = 0;

        // 점토 클릭하면 게임 매니저의 클레이 변수가 클릭 된 점토를 참조하도록..
        GameManager.instance.clayAction = this.GetComponent<ClayAction>();
        GameManager.instance.clay = this.GetComponent<Clay>();
        GameManager.instance.love += GameManager.instance.clay.TouchClay();
        GameManager.instance.clay.touchCount++;

        GameManager.instance.sound.PlaySound("TOUCH");
        anim.SetTrigger("doTouch");
    }

    private void OnMouseDrag()
    {   // 점토 드래그 기능..

        GameManager.instance.clayAction = this.GetComponent<ClayAction>();
        GameManager.instance.clay = this.GetComponent<Clay>();

        // 점토 계속 누르고 있으면 시간 더해짐,,
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

        // 마우스 커서가 젤리 판매 버튼이랑 닿으면 mouseDragToSellBtn 이 true 가 됨..
        if (mouseDragToSellBtn && GameManager.instance.clay.level==maxLevel)
        {
            // 최종 레벨은 일단 4로 해놨습니다..
            // 점토 위치랑 판매창 위치랑 같으면 점토 팔리도록,, (근데 점토가 최종 레벨에 도달했을 때만,,)
            GameManager.instance.SellClay();

            Sell(); // 점토 팔리면 활성화 끄는 함수..

            // 다시 원상태로 해놓기..
            mouseDragToSellBtn = false;
        }

        if (transform.position.x < -5.4 || transform.position.x > 5.4)
            transform.position = prevPos;
        if (transform.position.y < -1.7 || transform.position.y > 1.3)
            transform.position = prevPos;
    }

    private void FixedUpdate()
    {
        // 원근감 주려고 z축 값도 변화시킴
        transform.Translate(new Vector3(x, y, y).normalized * Time.fixedDeltaTime * speed);
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
