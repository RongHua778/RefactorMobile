using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : ReusableObject
{
    [SerializeField] Image healthProgress = default;
    [SerializeField] Image frostProgess = default;

    [SerializeField] Vector2 enemyOffset = default;
    //[SerializeField] Vector2 iconStartPos = default;
    //[SerializeField] Vector2 iconOffset = default;

    //private List<GameObject> iconShowList = new List<GameObject>();

    [SerializeField] GameObject[] Icons = default;//1=SLOW,2=DAMAGEMARK,3=SLOWINTENSIFY,4=GOLD
    [SerializeField] Text DmgIntensifyTxt = default;
    [SerializeField] Text FrostIntensifyTxt = default;
    [SerializeField] TextMeshProUGUI BossDialogueTxt = default;
    [SerializeField] Animator BossTextAnim = default;

    public Transform followTrans;

    float fillAmount;
    public float FillAmount
    {
        get => fillAmount;
        set
        {
            fillAmount = value;
            healthProgress.fillAmount = value;
        }
    }

    float frostAmount;
    public float FrostAmount
    {
        get => frostAmount;
        set
        {
            frostAmount = value;
            frostProgess.fillAmount = value;
        }
    }

    float damageIntensify;
    public float DamageIntensify
    {
        get => damageIntensify;
        set
        {
            damageIntensify = value;
            DmgIntensifyTxt.text = Mathf.RoundToInt(damageIntensify * 100) + "%";
        }
    }

    float frostIntensify;

    public float FrostIntensify
    {
        get => frostIntensify;
        set
        {
            frostIntensify = value;
            FrostIntensifyTxt.text = Mathf.RoundToInt(frostIntensify * 100) + "%";
        }
    }

    private void Start()
    {
        GameEvents.Instance.onShowDamageIntensify += ShowDamageIntensify;
    }

    private void OnDestroy()
    {
        GameEvents.Instance.onShowDamageIntensify -= ShowDamageIntensify;
    }

    private void ShowDamageIntensify(bool value)
    {
        DmgIntensifyTxt.gameObject.SetActive(value);
        FrostIntensifyTxt.gameObject.SetActive(value);

    }


    private void OnEnable()
    {
        ShowDamageIntensify(StaticData.ShowIntensify);

    }

    public override void OnUnSpawn()
    {
        FillAmount = 1;
        FrostAmount = 0;
        BossTextAnim.Play("BossDialogue_Default");
    }

    public void ShowIcon(int id, bool value)
    {
        Icons[id].SetActive(value);
    }


    public void ShowBossTxt(EnemyAttribute att,float chance)
    {
        if (Random.value > chance)
            return;
        string text = "";
        if (att.BossDialogues.Length > 0)
        {
            text = GameMultiLang.GetTraduction(att.BossDialogues[Random.Range(0, att.BossDialogues.Length)]);
            StartCoroutine(BossTextCor(text));
        }
    }

    private IEnumerator BossTextCor(string text)
    {
        yield return new WaitForSeconds(0.5f);
        BossDialogueTxt.text = text;
        BossTextAnim.SetBool("Show", true);
        yield return new WaitForSeconds(3f);
        BossTextAnim.SetBool("Show", false);
    }


    private void LateUpdate()
    {
        transform.position = (Vector2)transform.parent.position + enemyOffset;
        transform.rotation = Quaternion.identity;
    }
}
