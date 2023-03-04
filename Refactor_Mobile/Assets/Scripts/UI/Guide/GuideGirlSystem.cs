using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuideGirlSystem : MySingleton<GuideGirlSystem>
{
    [SerializeField] int startIndex = default;

    public int CurrentGuideIndex { get; set; }

    private const float DialogueTime = 8f;
    //private bool tempDialoguing = false;

    private Transform m_RootUI;
    [SerializeField] private RectTransform m_GirlTr = default;
    private Animator anim;

    private GameObject backBtn;
    private DialogueData currentDialogue;
    private bool typingSentence = false;
    private TextMeshProUGUI dialogTxt;
    private Queue<string> wordQueue;

    private DialogueData[] m_Dialogues;

    [Header("小姐姐临时对话")]
    [SerializeField] DialogueData RefactorDialogue = default;
    [SerializeField] DialogueData DemoDialogue = default;
    [SerializeField] DialogueData DieProtectDialogue = default;

    [Header("教学物体")]
    [SerializeField] List<GameObject> GuideObjList = new List<GameObject>();

    Dictionary<string, GameObject> GuideDIC = new Dictionary<string, GameObject>();
    [SerializeField] GameObject ClickTip = default;

    [Header("其他")]
    [SerializeField] RectMaskController m_RectMaskController = default;
    [SerializeField] EventPermeater m_EventPermeater = default;
    [SerializeField] GuideBookUI m_GuideBook = default;


    private void Start()
    {
        m_RootUI = transform.Find("Root");
        backBtn = m_RootUI.Find("BackBtn").gameObject;
        dialogTxt = m_RootUI.GetComponentInChildren<TextMeshProUGUI>();
        anim = this.GetComponent<Animator>();
        wordQueue = new Queue<string>();
        m_GuideBook.Initialize();
        GameEvents.Instance.onTempWord += DisplayTempDialogue;
    }

    public void Initialize()
    {
        m_Dialogues = LevelManager.Instance.CurrentLevel.GuideDialogues;

        Game.Instance.Tutorial = m_Dialogues.Length >= 1;//如果有教学对白就表示开始教学

        GuideObjList.Clear();
        GuideDIC.Clear();
        GameEvents.Instance.GuideObjCollect();
    }

    public void ShowGuideGirl(bool value, int posID)
    {
        if (value)
        {
            SetGirlPos(posID);
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void AddGuideObj(GameObject obj)
    {
        foreach (var item in GuideObjList)
        {
            if (item.name == obj.name)
                return;
        }
        GuideObjList.Add(obj);
    }

    private void InitializeGuideDIC()
    {
        foreach (var item in GuideObjList)
        {
            GuideDIC.Add(item.name, item);
        }
    }

    public GameObject GetGuideObj(string name)
    {
        if (GuideDIC.ContainsKey(name))
            return GuideDIC[name];
        else
        {
            Debug.LogWarning("没有可以该教学物体:" + name);
            return null;
        }
    }

    public void PrepareTutorial()
    {
        InitializeGuideDIC();
        if (m_Dialogues != null && m_Dialogues.Length > 0)
            StarTutorial();
    }
    private void StarTutorial()
    {
        GameEvents.Instance.onTutorialTrigger += GuideTrigger;
        CurrentGuideIndex = startIndex;
        currentDialogue = m_Dialogues[CurrentGuideIndex];
        GuideTrigger();
    }



    public void Release()
    {
        m_GuideBook.Hide();
        GameEvents.Instance.onTutorialTrigger -= GuideTrigger;
        GameEvents.Instance.onTempWord -= DisplayTempDialogue;
    }

    #region 临时对话
    private void DisplayTempDialogue(TempWord wordType)
    {
        if (typingSentence || Game.Instance.Tutorial)//教学期间不触发临时对话
            return;
        switch (wordType.WordType)
        {
            case TempWordType.StandardLose:
                StartCoroutine(TempWordCor(LevelManager.Instance.CurrentLevel.LostDialogue));
                break;
            case TempWordType.StandardWin:
                StartCoroutine(TempWordCor(LevelManager.Instance.CurrentLevel.WinDialogue));
                break;
            case TempWordType.EndlessEnd://id为通过波数,30=0,40=1,50=2
                StartCoroutine(TempWordCor(LevelManager.Instance.CurrentLevel.WinDialogue, wordType.ID));
                break;
            case TempWordType.Refactor:
                if (Random.value > 0.95f)//有5%概率触发
                    StartCoroutine(TempWordCor(RefactorDialogue, Random.Range(0, RefactorDialogue.Words.Length - 1)));
                break;
            case TempWordType.Demo:
                StartCoroutine(TempWordCor(DemoDialogue, wordType.ID));
                break;
            case TempWordType.WaveEnd:
                int id = 99;
                switch (wordType.ID)
                {
                    case 31:
                        id = 0;
                        break;
                    case 61:
                        id = 1;
                        break;
                    case 100:
                        id = 2;
                        break;
                        //case 150:
                        //    id = 3;
                        //    break;
                }
                if (id != 99)
                    StartCoroutine(TempWordCor(LevelManager.Instance.CurrentLevel.WaveDialogue[id], Random.Range(0, LevelManager.Instance.CurrentLevel.WaveDialogue[id].Words.Length - 1)));
                break;
            case TempWordType.DieProtect:
                StartCoroutine(TempWordCor(DieProtectDialogue, wordType.ID));
                break;
        }
    }


    IEnumerator TempWordCor(DialogueData dialogue, int id = 0)
    {
        typingSentence = true;
        //tempDialoguing = true;

        SetGirlPos(1);
        //ClickTip.SetActive(false);
        dialogue.TriggerGuideStartEvents();
        Show();
        backBtn.SetActive(false);
        dialogTxt.text = "";
        yield return new WaitForSeconds(0.5f);
        string word = GameMultiLang.GetTraduction(dialogue.Words[id]);
        dialogTxt.text = word;
        dialogTxt.maxVisibleCharacters = 0;
        dialogTxt.ForceMeshUpdate();
        var textInfo = dialogTxt.textInfo;
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            SetCharacterAlpha(i, 0);
        }

        // 按时间逐个显示字符
        var timer = 0f;
        var interval = 0.03f;
        while (dialogTxt.maxVisibleCharacters < textInfo.characterCount)
        {
            timer += Time.deltaTime;
            if (timer >= interval)
            {
                timer = 0;
                dialogTxt.maxVisibleCharacters++;
            }

            yield return null;
        }
        yield return new WaitForSeconds(DialogueTime);
        Hide();
        typingSentence = false;
        //tempDialoguing = false;
    }

    //public void OnGirlClick()//临时对话点击提前结束小姐姐对白
    //{
    //    if (!tempDialoguing)
    //        return;
    //    StopAllCoroutines();
    //    Hide();
    //    typingSentence = false;
    //    tempDialoguing = false;
    //}


    #endregion
    private void StartDialogue()
    {
        backBtn.SetActive(true);
        wordQueue.Clear();

        foreach (var key in currentDialogue.Words)
        {
            string word = GameMultiLang.GetTraduction(key);
            wordQueue.Enqueue(word);
        }
        DisplayNextSentence();
    }

    private void EndDialogue()
    {
        backBtn.SetActive(false);
        currentDialogue.TriggerGuideEndEvents();
        if (CurrentGuideIndex < m_Dialogues.Length - 1)//如果不是最后一个教程，就设置下一个教程
        {
            CurrentGuideIndex++;
            currentDialogue = m_Dialogues[CurrentGuideIndex];
            GuideTrigger(TutorialType.None);
        }
        else
        {
            Hide();
        }
    }

    private void DisplayNextSentence()
    {
        if (wordQueue.Count == 0)
        {
            EndDialogue();
            return;
        }
        string word = wordQueue.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(word));
    }

    private IEnumerator TypeSentence(string word)
    {
        ClickTip.SetActive(true);

        typingSentence = true;
        dialogTxt.text = word;
        dialogTxt.maxVisibleCharacters = 0;
        dialogTxt.ForceMeshUpdate();

        var textInfo = dialogTxt.textInfo;
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            SetCharacterAlpha(i, 0);
        }

        // 按时间逐个显示字符
        var timer = 0f;
        var interval = 0.03f;
        while (dialogTxt.maxVisibleCharacters < textInfo.characterCount)
        {
            timer += Time.deltaTime;
            if (timer >= interval)
            {
                timer = 0;
                dialogTxt.maxVisibleCharacters++;
            }

            yield return null;
        }
        typingSentence = false;
        if (wordQueue.Count == 0)
        {
            if (currentDialogue.DontNeedClickEnd)
                EndDialogue();
        }
    }

    private void SetCharacterAlpha(int index, byte alpha)
    {
        var materialIndex = dialogTxt.textInfo.characterInfo[index].materialReferenceIndex;
        var vertexColors = dialogTxt.textInfo.meshInfo[materialIndex].colors32;
        var vertexIndex = dialogTxt.textInfo.characterInfo[index].vertexIndex;

        vertexColors[vertexIndex + 0].a = alpha;
        vertexColors[vertexIndex + 1].a = alpha;
        vertexColors[vertexIndex + 2].a = alpha;
        vertexColors[vertexIndex + 3].a = alpha;

    }


    public void GuideTrigger(TutorialType triggetType = TutorialType.None)
    {
        if (!Game.Instance.Tutorial)
            return;
        if (currentDialogue.JudgeConditions(triggetType))
        {
            currentDialogue.TriggerGuideStartEvents();
            Invoke(nameof(StartDialogue), currentDialogue.WaitingTime);
        }
    }


    public void NextBtnClick()
    {
        if (!typingSentence)
            DisplayNextSentence();
    }

    public void Show()
    {
        m_RootUI.gameObject.SetActive(true);
        Sound.Instance.PlayUISound("Sound_Guide");
        anim.SetBool("Show", true);
    }
    public void Hide()
    {
        anim.SetBool("Show", false);
    }
    public void HideRoot()
    {
        m_RootUI.gameObject.SetActive(false);
    }

    public void SetGirlPos(int posID)
    {
        switch (posID)
        {
            case 0:
                m_GirlTr.anchorMin = new Vector2(0.5f, 0);
                m_GirlTr.anchorMax = new Vector2(0.5f, 0);
                m_GirlTr.anchoredPosition = new Vector2(0, 250);
                break;
            case 1:
                m_GirlTr.anchorMin = new Vector2(0f, 0);
                m_GirlTr.anchorMax = new Vector2(0f, 0);
                m_GirlTr.anchoredPosition = new Vector2(380, 100);
                break;
        }
    }

    public void ShowGuideBook(int index)
    {
        m_GuideBook.Show();
        m_GuideBook.ShowPage(index);
    }
    public void SetRectMaskObj(GameObject obj, float delayTime)
    {
        m_RectMaskController.SetTarget(obj, delayTime);
    }

    public void SetEventPermeaterTarget(GameObject obj)
    {
        m_EventPermeater.SetTarget(obj);
    }

}
