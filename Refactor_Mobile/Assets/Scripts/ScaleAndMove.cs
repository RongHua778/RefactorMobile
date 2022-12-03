using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ScaleAndMove : IGameSystem
{
    MainUI m_MainUI;
    Vector2 m_ScreenPos = new Vector2();
    Vector3 oldPosition;

    private Vector3 Origin;
    private Vector3 Difference;
    private float SmoothSpeed = 0.6f;
    private bool Drag = false;

    Camera cam;
    private float scrollSpeed = 2.5f;
    private float maximum = 12;
    private float minmum = 3;

    private float maxY = 11;
    private float minY = -11;
    private float minX = -11;
    private float maxX = 11;
    Vector2 CamMovement;
    // Start is called before the first frame update

    //新手引导
    Vector3 camInitPos;
    Vector3 CamViewPos;
    float CamInitialSize;
    public bool MoveTurorial = false;
    public bool SizeTutorial = false;
    public bool CanControl = true;
    //



    [Header("视差控制")]
    [SerializeField] Transform[] backGrounds = default;
    [SerializeField] float parallaxScale = default;
    [SerializeField] float parallaxReductionFactor = default;
    [SerializeField] float smoothing;

    [Header("摄像机晃动")]
    [SerializeField] private float shakeDuration = default;
    [SerializeField] private float shakeStrength = default;


    private Transform camTr;

    protected static float current = 0;
    //private float min = 0;
    //private float max = 3.7f;
    private float last = -1;

    public void Initialize(MainUI mainUI)
    {
        m_MainUI = mainUI;
        cam = this.GetComponent<Camera>();
        camTr = cam.transform;
        CamViewPos = cam.transform.position;
        camInitPos = cam.transform.position;
        CamInitialSize = 6f;
        oldPosition = cam.transform.position;
        Input.multiTouchEnabled = true;
        MoveTurorial = false;
        SizeTutorial = false;
        StartCamAnim();
    }

    private void StartCamAnim()
    {
        cam.DOOrthoSize(CamInitialSize, 2f);
    }

    private IEnumerator ShakeCor()
    {
        Time.timeScale = 0.3f;
        cam.DOShakePosition(shakeDuration, shakeStrength);
        yield return new WaitForSecondsRealtime(1.2f);
        Time.timeScale = GameRes.GameSpeed;
    }

    public void ShakeCam()
    {
        StartCoroutine(ShakeCor());
    }

    public override void GameUpdate()
    {
        //DesktopInput();
        MobileInput();
        TutorialCounter();
        BackgroundUpdate();
        //RTSView();
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), Mathf.Clamp(transform.position.y, minY, maxY), transform.position.z);
    }

    IEnumerator MoveCor()
    {
        CanControl = false;
        MoveTurorial = false;
        cam.transform.DOMove(new Vector3(0, 0, cam.transform.position.z), 1f);
        cam.DOOrthoSize(CamInitialSize, 1f);
        yield return new WaitForSeconds(1f);
        GameEvents.Instance.TutorialTrigger(TutorialType.MouseMove);
    }
    private void TutorialCounter()
    {
        if (MoveTurorial)
        {
            if (Vector2.Distance(transform.position, oldPosition) > 4f)
            {
                StartCoroutine(MoveCor());
            }
        }
        if (SizeTutorial)
        {
            if (Mathf.Abs(cam.orthographicSize - CamInitialSize) > 1f)
            {
                SizeTutorial = false;
                cam.transform.DOMove(new Vector3(0, 0, cam.transform.position.z), 1f);
                cam.DOOrthoSize(CamInitialSize, 1f);
                GameEvents.Instance.TutorialTrigger(TutorialType.WheelMove);
            }
        }
    }



    private void DesktopInput()
    {
        if (!CanControl)
            return;
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minmum, maximum);
            cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;

        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1, StaticData.GetSelectLayer);

            if (hit.collider != null)
            {
                hit.collider.GetComponent<TileBase>().TileDown();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1, StaticData.GetSelectLayer);
            if (hit.collider != null)
            {
                hit.collider.GetComponent<TileBase>().TileUp();
            }
        }
        if (DraggingActions.DraggingThis == null && Input.GetMouseButton(0))
        {
            Difference = cam.ScreenToWorldPoint(Input.mousePosition) - cam.transform.position;
            if (Drag == false)
            {
                Drag = true;
                Origin = cam.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            Drag = false;
        }
        if (Drag)
        {
            Vector3 desirePos = Origin - Difference;
            Vector3 smoothPos = Vector3.Lerp(cam.transform.position, desirePos, SmoothSpeed);
            cam.transform.position = smoothPos;
        }


        //speedVertical = Input.GetAxisRaw("Horizontal") * Vector3.right * slideSpeed * Time.deltaTime;
        //speedHorizon = Input.GetAxisRaw("Vertical") * Vector3.up * slideSpeed * Time.deltaTime;
        //speed = speedHorizon + speedVertical;
        //transform.Translate(speed / GameRes.GameSpeed, Space.World);


    }
    private void MobileInput()
    {
        if (!CanControl)
            return;

        if (Input.touchCount <= 0)
        {
            last = -1;//不触发逻辑时
            return;
        }
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            return;
        }
        if (Input.touchCount == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                m_ScreenPos = Input.touches[0].position;
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(m_ScreenPos), Vector2.zero, 1, StaticData.GetSelectLayer);
                if (hit.collider != null)
                {
                    hit.collider.GetComponent<TileBase>().TileDown();
                }
            }
            else if (Input.touches[0].phase == TouchPhase.Ended)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.touches[0].position), Vector2.zero, 1, StaticData.GetSelectLayer);
                if (hit.collider != null)
                {
                    hit.collider.GetComponent<TileBase>().TileUp();
                }
                Drag = false;
            }
            if (Input.touches[0].phase == TouchPhase.Moved)
            {
                if (DraggingActions.DraggingThis == null)
                {
                    Difference = cam.ScreenToWorldPoint(Input.touches[0].position) - cam.transform.position;
                    if (Drag == false)
                    {
                        Drag = true;
                        Origin = cam.ScreenToWorldPoint(Input.touches[0].position);
                    }
                }
                if (Drag)
                {
                    Vector3 desirePos = Origin - Difference;
                    Vector3 smoothPos = Vector3.Lerp(cam.transform.position, desirePos, SmoothSpeed);
                    cam.transform.position = smoothPos;
                }
            }

        }
        else if (Input.touchCount == 2)
        {
            float dis = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);//两指之间的距离
            if (-1 == last)
                last = dis;
            float result = dis - last;//与上一帧比较变化
            //if (result + current < min)//区间限制：最小
            //    result = min - current;
            //else if (result + current > max)//区间限制：最大
            //    result = max - current;
            result *= 0.025f;//系数
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, cam.orthographicSize - result, SmoothSpeed);
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minmum, maximum);

            //current -= result;//累计当前
            last = dis;//记录为上一帧的值
        }

    }

    public void LocatePos(Vector2 pos)
    {
        Vector3 newPos = new Vector3(pos.x, pos.y, cam.transform.position.z);
        cam.transform.DOMove(newPos, 0.5f);
    }

    private void BackgroundUpdate()
    {
        Vector2 parallax = (CamViewPos - camTr.position) * parallaxScale;
        for (int i = 0; i < backGrounds.Length; i++)
        {
            float backgroundTargetPosX = backGrounds[i].position.x + parallax.x * (i * parallaxReductionFactor + 1);
            float backgroundTargetPosY = backGrounds[i].position.y + parallax.y * (i * parallaxReductionFactor + 1);
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgroundTargetPosY, backGrounds[i].position.z);
            backGrounds[i].position = Vector3.Lerp(backGrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }
        CamViewPos = camTr.position;
    }
}
