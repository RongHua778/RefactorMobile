using System;
using UnityEngine;
using System.Collections.Generic;
using Lanuguage;
using Sirenix.OdinInspector;
//using Steamworks;


public class GameMultiLang : MonoBehaviour
{
    public static GameMultiLang Instance;

    public static Dictionary<String, String> Fields;
    public static Dictionary<string, Sprite> SpriteFields;

    [SerializeField] string defaultLang = default;
    [SerializeField] LanguageManager languageData = default;

    [SerializeField] List<SpriteData> SpriteDatas = default;
    public static List<LanguageData> LanguageData { get; set; }



    void Awake()
    {
        if (Instance == null)//首次打开游戏
        {
            Instance = this;
            if (PlayerPrefs.GetString("_language", "") == "")//没有进行过设置
            {
                SetLanguage("ch");  // 设为默认中文
                //if (SteamManager.Initialized)  //如果开了Steam且有特殊语言
                //{
                //    string steamLang = SteamUtils.GetSteamUILanguage();
                //    switch (steamLang)
                //    {
                //        case "schinese":
                //        case "tchinese":
                //            SetLanguage("ch");
                //            break;
                //        case "english":
                //        default:
                //            SetLanguage("en");
                //            break;
                //    }
                //}
                //else
                //{
                //    SetLanguage("en");  // 设为默认英文

                //}

            }

            //DontDestroyOnLoad (gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadLanguage();
    }

    private void SetLanguage(string lang)
    {
        PlayerPrefs.SetString("_language", lang);
    }


    [Button]
    public void LoadLanguage()
    {
        if (Fields == null)
            Fields = new Dictionary<string, string>();

        if (SpriteFields == null)
            SpriteFields = new Dictionary<string, Sprite>();

        Fields.Clear();
        SpriteFields.Clear();

        string lang = PlayerPrefs.GetString("_language", defaultLang);
        PlayerPrefs.SetString("_language", lang);
        switch (lang)
        {
            case "en":
                PlayerPrefs.SetInt("_language_index", 0);
                break;
            case "ch":
                PlayerPrefs.SetInt("_language_index", 1);
                break;
        }



        //string allTexts = (Resources.Load (@"Languages/" + lang) as TextAsset).text; //without (.txt)

        //string[] lines = allTexts.Split (new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        //string key, value;

        //for (int i = 0; i < lines.Length; i++) {
        //	if (lines [i].IndexOf ("=") >= 0 && !lines [i].StartsWith ("#")) {
        //		key = lines [i].Substring (0, lines [i].IndexOf ("="));
        //		value = lines [i].Substring (lines [i].IndexOf ("=") + 1,
        //			lines [i].Length - lines [i].IndexOf ("=") - 1).Replace ("\\n", Environment.NewLine);
        //		Fields.Add (key, value);
        //	}
        //}

        LoadLanguageData();
    }

    internal static string GetTraduction(object p)
    {
        throw new NotImplementedException();
    }

    private void LoadLanguageData()
    {

        //LanguageData = ExcelTool.CreateLanguageArrayWithExcel(ExcelConfig.excelsFolderPath + "LanguageExcel.xlsx");
        LanguageData = languageData.dataArray;
        switch (PlayerPrefs.GetInt("_language_index", 0))
        {
            case 0:
                for (int i = 0; i < LanguageData.Count; i++)
                {
                    if (Fields.ContainsKey(LanguageData[i].Key))
                    {
                        Debug.Log(LanguageData[i].Key);
                    }
                    else
                        Fields.Add(LanguageData[i].Key, LanguageData[i].English);
                }
                //图片
                for (int i = 0; i < SpriteDatas.Count; i++)
                {
                    SpriteFields.Add(SpriteDatas[i].Key, SpriteDatas[i].English);
                }
                break;
            case 1:
                for (int i = 0; i < LanguageData.Count; i++)
                {
                    if (Fields.ContainsKey(LanguageData[i].Key))
                    {
                        Debug.Log(LanguageData[i].Key);
                    }
                    else
                        Fields.Add(LanguageData[i].Key, LanguageData[i].Chinese);
                }
                //图片
                for (int i = 0; i < SpriteDatas.Count; i++)
                {
                    SpriteFields.Add(SpriteDatas[i].Key, SpriteDatas[i].Chinese);
                }
                break;
        }


    }

    public static string GetTraduction(string key)
    {
        if (!Fields.ContainsKey(key))
        {
            Debug.LogError("There is no key with name: [" + key + "] in your text files");
            return null;
        }

        return Fields[key];
    }

    public static Sprite GetSprite(string key)
    {
        if (!SpriteFields.ContainsKey(key))
        {
            Debug.LogError("There is no key with name: [" + key + "] in your sprite files");
            return null;
        }

        return SpriteFields[key];
    }



}
