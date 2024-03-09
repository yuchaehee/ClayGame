using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClayAction : MonoBehaviour
{
    SpriteRenderer spriter;
    Rigidbody2D rigid;
    Collider2D coll;

    // 아이템이랑 점토가 상호작용하는 애니메이션이랑, 일반 애니메이션이랑 나눠서 쓰려고..
    
    // 0:normalAnim, 1:itemLevel3Anim, 2:itemLevel2Anim, 3:itemLevel4Anim, 4:itemLevel5Anim
    public RuntimeAnimatorController[] animList;
    public Animator curAnim;

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
        speed = 0.5f; // 고민 중이라 일단 고정으로 해놨습니다..

        // 스크린 상 sellBox 위치..
        SellBoxPos = new Vector3(359.75f, -149, 0);

        isDraging = false;
        isActioned = false;
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
        GameManager.instance.ChangeAc(GameManager.instance.clay.level);
        curAnim.runtimeAnimatorController = animList[0];
        // 다시 활성화 되면 sprite 도 첫번째로 초기화..
        spriter.sprite = firstSprite;
    }

    private void Update()
    {
        // 걸어다니는 애니메이션 실행..
        // 애니메이터가 itemAnim 일 때는 아래 코드 적용 안 되도록 if 문 쓴 것.. 
        //if (curAnim.runtimeAnimatorController != animList[1] && curAnim.runtimeAnimatorController != animList[2])

        if (curAnim.runtimeAnimatorController == animList[0]) {
            if (x == 0 && y == 0)
                curAnim.SetBool("isWalk", false);
            else
                curAnim.SetBool("isWalk", true);
        }

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


        // 애니메이터가 itemAnim 일 때는 아래 코드 적용 안 되도록 if 문 쓴 것.. 
        if (curAnim.runtimeAnimatorController == animList[0])
        {
            GameManager.instance.sound.PlaySound("TOUCH");
            curAnim.SetTrigger("doTouch");
        }
    }

    private void OnMouseDrag()
    {
        // 참조시키기..
        GameManager.instance.clayAction = this.GetComponent<ClayAction>();
        GameManager.instance.clay = this.GetComponent<Clay>();

        // 점토 계속 누르고 있으면 시간 더해짐,,
        touchTime += Time.deltaTime;

        if (touchTime > 0.5f)
        {
            // 점토 드래깅 기능..
            isDraging = true;
            isDraging_ = true;

            transform.position = mousePos;

            // 점토 들어올리면 UICanvas 꺼지도록..
            GameManager.instance.UICanvas.SetActive(false);

            // 점토를 들어올리면 빛이랑 그림자가 켜질 것..
            GameManager.instance.sellLight.SetActive(true);
            GameManager.instance.sellShadow.SetActive(true);

            // 점토를 들어올리면 창문빛 꺼질 것..
            if (GameManager.instance.clayHouseLevel == 4) 
                GameManager.instance.windowLight.SetActive(false);

            // 점토를 들어올리면 셀 버튼의 애니메이션 실행..
            // 애니메이터가 itemAnim 일 때는 아래 코드 적용 안 되도록 if 문 쓴 것.. 
            if (curAnim.runtimeAnimatorController == animList[0])
                GameManager.instance.sellButtonAnim.GetComponent<Animator>().SetBool("isStarted", true);
        }
    }

    private void OnMouseUp()
    {
        isDraging_ = false;

        // 점토 내려놓으면 UICanvas 다시 켜지도록..
        GameManager.instance.UICanvas.SetActive(true);

        // 점토를 아이템이 아닌 그냥 바닥에 놓았을 땐 isDraging 을 false 로 만들어주기..
        if (!GameManager.instance.item.mouseDragToItem)  
            isDraging = false;

        // 점토를 내려놓으면 빛이랑 그림자가 꺼질 것..
        GameManager.instance.sellLight.SetActive(false);
        GameManager.instance.sellShadow.SetActive(false);

        // 점토를 들어올리면 창문빛 켜질 것..
        if (GameManager.instance.clayHouseLevel == 4)
            GameManager.instance.windowLight.SetActive(true);

        // 애니메이션 끄기..
        // 애니메이터가 itemAnim 일 때는 아래 코드 적용 안 되도록 if 문 쓴 것.. 
        if (curAnim.runtimeAnimatorController == animList[0])
            GameManager.instance.sellButtonAnim.GetComponent<Animator>().SetBool("isStarted", false);

        GameManager.instance.clayAction = this.GetComponent<ClayAction>();
        GameManager.instance.clay = this.GetComponent<Clay>();

        // 마우스 커서가 젤리 판매 버튼이랑 닿으면 mouseDragToSellBtn 이 true 가 됨..
        if ((mouseDragToSellBtn || isActioned) && GameManager.instance.clay == null) return; // 게임매니저가 점토를 한 번도 안 누른 상태면 그냥 빠져나오도록..

        if (mouseDragToSellBtn && GameManager.instance.clay.level == maxLevel)
        {
            // 최종 레벨은 일단 4로 해놨습니다..
            // 점토 위치랑 판매창 위치랑 같으면 점토 팔리도록,, (근데 점토가 최종 레벨에 도달했을 때만,,)
            GameManager.instance.SellClay();

            Sell(); // 점토 팔리면 활성화 끄는 함수..

            // 다시 원상태로 해놓기..
            mouseDragToSellBtn = false;
        } else if (mouseDragToSellBtn && GameManager.instance.clay.level != maxLevel)
        {
            GameManager.instance.infoText.text = "성체가 된 점토만 분양할 수 있습니다.";
            GameManager.instance.InfoTextAnimStart();
        }

        if (transform.position.x <= -5.4 || transform.position.x >= 5.4)
            transform.position = prevPos;
        if (transform.position.y <= -2.4 || transform.position.y >= 0.5)
            transform.position = prevPos;
    }

    private void FixedUpdate()
    {
        // 점토를 계속 잡고있으면 밑 코드 아예 실행 안 되도록 나오기..
        if (isDraging_) return;

        if (!isDraging && !delay)
        {   // 점토가 아이템 애니메이션 실행하고 있으면 이거 안 움직이도록..
            // 원근감 주려고 z축 값도 변화시킴
            transform.Translate(new Vector3(x, y, y).normalized * Time.fixedDeltaTime * speed);
        } else if (isDraging && GameManager.instance.item.mouseDragToItem)
        {
            switch (GameManager.instance.curTargetItem) {
                case "level2Item":
                    // level()ItemAnim 애니메이터는 시작하자마자 애니메이션 실행되도록 해놨습니다.. 그래서 setTrigger 이런거 안 썼습니다..
                    curAnim.runtimeAnimatorController = animList[2];
                    isDraging = false; // 애니메이션 실행되면 이제 false 로 바꿔주기..

                    // 아이템과의 상호작용이 끝나면 실행됟도록..
                    Invoke("MakeisActionedToFalse", 3.5f);

                    break;
                case "level3Item":
                    curAnim.runtimeAnimatorController = animList[1];
                    isDraging = false; // 애니메이션 실행되면 이제 false 로 바꿔주기..

                    // 아이템과의 상호작용이 끝나면 실행됟도록..
                    Invoke("MakeisActionedToFalse", 3.5f);

                    break;
                case "level4Item":
                    curAnim.runtimeAnimatorController = animList[3];
                    isDraging = false;

                    // 아이템과의 상호작용이 끝나면 실행됟도록..
                    Invoke("MakeisActionedToFalse", 2f);

                    break;
                case "level5Item":
                    curAnim.runtimeAnimatorController = animList[4];
                    isDraging = false;

                    // 아이템과의 상호작용이 끝나면 실행됟도록..
                    Invoke("MakeisActionedToFalse", 2f);

                    break;
            }
        }
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

    public void MakeisActionedToFalse()
    {
        // Invoke 로 실행시킬 것..
        isActioned = false;
        delay = true;
        isDraging = false;

        Invoke("MakeDelayToFalse", 2f);
        transform.GetComponent<Transform>().position = prevPos;

        // 다시 기본으로 애니메이터 바꿔주기..
        curAnim.runtimeAnimatorController = animList[0];
    }

    void MakeDelayToFalse()
    {
        delay = false;
    }
}