using UnityEngine;

public class UIMenu : IUserInterface
{
    [SerializeField] GameObject m_ContinueGameBtn = default;


    public override void Initialize()
    {
        base.Initialize();
        m_ContinueGameBtn.SetActive(LevelManager.Instance.LastGameSave.HasLastGame);
    }

    public override void Show()
    {
        base.Show();
        anim.SetBool("Show", true);
    }

    public override void ClosePanel()
    {
        base.ClosePanel();
        anim.SetBool("Show", false);

    }

    public void ContinueBtnClick()
    {
        MenuManager.Instance.ContinueGame();
    }

    public void StartGameBtnClick()
    {
        MenuManager.Instance.OpenMode();

    }

    public void TujianBtnClick()
    {
        MenuManager.Instance.OpenTujian();
    }
    public void AchievementBtnClick()
    {
        MenuManager.Instance.OpenAchievement();
    }
    public void SettingBtnClick()
    {
        MenuManager.Instance.OpenSetting();
    }

    public void SteamPage()
    {
        Application.OpenURL("https://store.steampowered.com/app/1664670/_Refactor");

    }

    public void JoinDiscord()
    {
        Application.OpenURL("https://discord.gg/bPgMZ6kgBH");

    }

    public void JoinQQ()
    {
        Application.OpenURL("https://jq.qq.com/?_wv=1027&k=wuuN4Bll");
    }

    public void JoinWechat()
    {
        MenuManager.Instance.OpenWechat();
    }

    public void QuitGameBtnClick()
    {
        Game.Instance.QuitGame();
    }
}
