﻿using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



[RequireComponent(typeof(Sound))]
public class Game : Singleton<Game>
{
    public string CurrentState => m_SceneStateController.m_State.StateName;
    SceneStateController m_SceneStateController = new SceneStateController();
    public Animator transition;
    public float transitionTime = 0.8f;
    public bool Tutorial = false;
    public bool OnTransition = false;
    public bool TestMode = false;
    [SerializeField] Canvas globalCanvas = default;

    protected override void Awake()
    {
        base.Awake();
        if (!alreadyExist)
        {
            Application.targetFrameRate = 60;
            Application.runInBackground = true;
            DontDestroyOnLoad(this.gameObject);
            StaticData.Instance.Initialize();
            TurretSkillFactory.Initialize();
            EnemyBuffFactory.Initialize();
            TurretBuffFactory.Initialize();
            TechnologyFactory.Initialize();
            RuleFactory.Initialize();
        }
    }

    

    private void Start()
    {
        
        //判断当前初始场景在哪里，根据不同场景初始化当前State
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        switch (currentSceneIndex)
        {
            case 0://menu
                StaticData.Instance.ContentFactory.SetDefaultRecipes();//进入游戏时设置为默认配方

                m_SceneStateController.SetState(new MenuState(m_SceneStateController));
                break;
            case 1://battle//测试
                LevelManager.Instance.LoadGame();//直接从战斗场景开始，直接读取存档
                m_SceneStateController.SetState(new BattleState(m_SceneStateController));
                break;
        }
        Sound.Instance.BgVolume = 0.5f;
    }

    private void Update()
    {
        m_SceneStateController.StateUpdate();
        if (TestMode)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                TipsManager.Instance.ShowMessage(GameMultiLang.GetTraduction("TEST1"));
                LevelManager.Instance.SetGameLevel(0);
                LevelManager.Instance.GameExp = 0;
                LevelManager.Instance.PassDiifcutly = 0;
                //LevelManager.Instance.ClearAllSteamStats();
                //LevelManager.Instance.PermitDifficulty = 2;
                PlayerPrefs.DeleteAll();
            }
            if (Input.GetKeyDown(KeyCode.J))//解锁全内容
            {
                TipsManager.Instance.ShowMessage(GameMultiLang.GetTraduction("TEST2"));
                LevelManager.Instance.SetGameLevel(99);
                LevelManager.Instance.GameExp = 0;
                LevelManager.Instance.PassDiifcutly = 9;
                //LevelManager.Instance.PermitDifficulty = 6;
                PlayerPrefs.SetInt("MaxDifficulty", 9);
            }

            //if (Input.GetKeyDown(KeyCode.L))//解锁全内容
            //{
            //    PlayfabManager.Instance.Login();
            //}

        }
    }



    #region 场景读取及转场动画
    //根据ID读取场景
    public void LoadScene(int index)
    {
        StartCoroutine(Transition(index));
    }

    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator Transition(int index)
    {
        OnTransition = true;
        transition.SetTrigger("Start");
        m_SceneStateController.EndState();
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(index, LoadSceneMode.Single);
        OnTransition = false;
        yield return SceneManager.LoadSceneAsync(index);
        switch (index)
        {
            case 0://Menu
                //Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(1);
                m_SceneStateController.SetState(new MenuState(m_SceneStateController));
                break;
            case 1://Battle
                //Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(0);
                m_SceneStateController.SetState(new BattleState(m_SceneStateController));
                break;
        }
        transition.SetTrigger("End");
        globalCanvas.worldCamera = Camera.main;
    }
    #endregion


    public bool InitializeNetworks()
    {
        //SteamManager.Instance.Initialize();
        PlayfabManager.Instance.Login();
        return PlayFabClientAPI.IsClientLoggedIn();
    }

    


    public void QuitGame()
    {
        Application.Quit();
    }







}
