using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


public class SettingsController : FloatingPopUp {

	public delegate void HandleOnChangeLanguage();
	public static event HandleOnChangeLanguage OnChangeLanguage;

	public Button btnCredits, btnLanguaje, btnFx, btnMusic, btnRestorePurchase, btnShop;

	public GameObject canvasShop;

	public Image imageMusic, imageFx, imageFlag;

	public Sprite fxOn,fxOff,musicOn,musicOff, flagSpanish, flagEnglish;

	public Text txtShop, txtRate, txtSettings;

	private Text txtCredits, txtLanguage, txtMusic, txtRestore;
	
	public override void Start ()
	{
		base.Start ();

		Blackboard.Instance.CheckImageMusicAndFx(imageMusic,musicOn,musicOff, imageFx, fxOn, fxOff);
		Blackboard.Instance.CheckImageLanguage(flagEnglish, flagSpanish, imageFlag);

		txtCredits 		= Blackboard.Instance.GetTextFromButton(btnCredits);
		txtLanguage 	= Blackboard.Instance.GetTextFromButton(btnLanguaje);
		txtMusic 		= Blackboard.Instance.GetTextFromButton(btnMusic);
		txtRestore 		= Blackboard.Instance.GetTextFromButton(btnRestorePurchase);

		if(PlayerPrefs.GetString("Comprado") == "Si")
		{
			btnShop.interactable = false;
		}

		ChangeLanguage();
		
	}

	void ChangeLanguage ()
	{
		txtCredits.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Credits).GetStringData(Blackboard.Language);
		txtLanguage.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Lang).GetStringData(Blackboard.Language);
		txtMusic.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Music).GetStringData(Blackboard.Language);
		txtRestore.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Restore).GetStringData(Blackboard.Language);
		txtShop.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Shop).GetStringData(Blackboard.Language);
		txtRate.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Rate).GetStringData(Blackboard.Language);
		txtSettings.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Settings).GetStringData(Blackboard.Language);
	}

	public void Credits()
	{
		StartCoroutine(Blackboard.Instance.ChangeLevel("Credits"));
	}

	public void Languaje()
	{
		Blackboard.Instance.ToggleLanguage(flagEnglish, flagSpanish, imageFlag);
		if(OnChangeLanguage != null)
			OnChangeLanguage();

		ChangeLanguage();
	}

	public void fx()
	{
		Blackboard.Instance.ToggleFx(imageFx, fxOn, fxOff);
	}

	public void Music()
	{
		Blackboard.Instance.ToggleMusic(imageMusic, musicOn, musicOff);
	}

	public void PushShop()
	{
		Instantiate(canvasShop);
	}

	public void Rate()
	{
		//ACHIEVE
		if(Application.platform == RuntimePlatform.Android)
		{
			Application.OpenURL("https://play.google.com/store/apps/details?id=com.boomfiregames.kickordie");
			GooglePlayManager.Instance.UnlockAchievementById(Achievements.GRACIAS);

		}
		else if( Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Application.OpenURL("https://itunes.apple.com/us/app/kick-or-die/id1058498355?l=es&ls=1&mt=8");
			GameCenterManager.SubmitAchievement(100f, Achievements.GRACIAS);
		}

	}

	public void Twitter()
	{
		Application.OpenURL("https://twitter.com/BoomfireGames");
	}

	public void Facebook()
	{
		Application.OpenURL("https://www.facebook.com/Boomfiregames/");

		//ACHIEVE
		if(Application.platform == RuntimePlatform.Android)
		{
			GooglePlayManager.Instance.UnlockAchievementById(Achievements.FACEBOOK);
		}
		else if( Application.platform == RuntimePlatform.IPhonePlayer)
		{
			GameCenterManager.SubmitAchievement(100f, Achievements.FACEBOOK);
		}
	}

	public void RestorePurchase()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer)
		{
			IOSInAppPurchaseManager.Instance.RestorePurchases();
		}
	}
}
