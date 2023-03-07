using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;

public class MenuState : ISceneState
{
	public MenuState(SceneStateController Controller) : base(Controller)
	{
		this.StateName = "MenuState";
	}
	// é_Ê¼
	public override void StateBegin()
	{
		//Game.Instance.InitializeNetworks();
		LevelManager.Instance.Initialize();

		MenuManager.Instance.Initinal();
		GameManager.Instance = null;
		Sound.Instance.PlayBg("menu");
		Game.Instance.Tutorial = false;
		TechnologyFactory.ResetAllTech();
		
		StaticData.LockKeyboard = false;
		GameParam.ResetGameParam();
		TipsManager.Instance.SetCanvasCam();

		RuleFactory.Release();
		TaptapManager.Instance.TapLogin();
	}

	// ½YÊø
	public override void StateEnd()
	{
		MenuManager.Instance.Release();
		TipsManager.Instance.HideTips();

	}

	public override void StateUpdate()
	{
		MenuManager.Instance.GameUpdate();
	}

}
