using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Advertisements;

public class MenuController : MonoBehaviour {

	public Button btnTutorial, btnStory, btnArcade, btnNormal, btnHard, btnSettings, btnBackStory, btnBackArcade, btnAchieve, btnRanking, btnDay, btnNight, btnSnow;

	public Text txtChooseMap;

	public MiniPopup miniPopup;

	public Canvas settings;

	public ContinuesManager continuesManager;

	public AudioClip music;

	private Text txtTutorial, txtStory, txtNormal, txtHard, txtBackStory, txtBackArcade, txtArcade, txtDay, txtNight, txtSnow;

	//este es el pop up instanciado
	private MiniPopup myPopUp;

	void Start () 
	{
		txtTutorial = Blackboard.Instance.GetTextFromButton(btnTutorial);
		txtStory = Blackboard.Instance.GetTextFromButton(btnStory);
		txtNormal = Blackboard.Instance.GetTextFromButton(btnNormal);
		txtHard = Blackboard.Instance.GetTextFromButton(btnHard);
		txtBackArcade = Blackboard.Instance.GetTextFromButton(btnBackArcade);
		txtBackStory = Blackboard.Instance.GetTextFromButton(btnBackStory);
		txtArcade = Blackboard.Instance.GetTextFromButton(btnArcade);
		txtDay = Blackboard.Instance.GetTextFromButton(btnDay);
		txtNight = Blackboard.Instance.GetTextFromButton(btnNight);
		txtSnow = Blackboard.Instance.GetTextFromButton(btnSnow);
		

		SwitchButtons(false);
		btnTutorial.gameObject.SetActive(true);
		btnStory.gameObject.SetActive(true);
		btnArcade.gameObject.SetActive(true);
		btnSettings.gameObject.SetActive(true);
		btnAchieve.gameObject.SetActive(true);
		btnRanking.gameObject.SetActive(true);

		btnStory.interactable 		= false;
		btnArcade.interactable 		= false;
		txtStory.color				= Color.grey;
		txtArcade.color 			= Color.grey;
		btnRanking.interactable		= false;
		btnAchieve.interactable		= false;

		SoundManager.PlayImmediately(music,true);

		if(PlayerPrefs.GetString("Comprado") == "Si")
		{
			Blackboard.showVideo = false;
			Blackboard.showIntersi = false;
			continuesManager.gameObject.SetActive(false);
		}


		if(PlayerPrefs.GetInt("TutorialFinish") != 0)
		{
			btnStory.interactable 		= true;
			btnArcade.interactable 		= true;
			txtStory.color				= Color.white;
			txtArcade.color 			= Color.white;
		}

		if(PlayerPrefs.GetInt("BossFinalDead") != 0)
		{
			btnHard.interactable = true;
			txtArcade.color = Color.white;
		}

		HandleOnChangeLanguage();

		GooglePlayConnection.ActionConnectionResultReceived 	+= GooglePlayConnection_ActionConnectionResultReceived;;
		GameCenterManager.OnAuthFinished						+= HandleActionPlayerConnected;
		SettingsController.OnChangeLanguage						+= HandleOnChangeLanguage;
		Shop.OnClose											+= HandleOnCloseShop;

		if(Blackboard.showVideoOnMenu)
		{
			Blackboard.Instance.ShowAd();
		}
		else
		{
			Invoke("ConnectServices",3);
		}

		Blackboard.showVideoOnMenu = true;
	}

	void ConnectServices()
	{
		if(Application.platform == RuntimePlatform.Android)
		{
			if(!GooglePlayConnection.Instance.IsConnected)
			{
				GooglePlayConnection.Instance.Connect();
			}
			else
			{
				HandleActionPlayerConnected();
			}
		}
		else if( Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if(!GameCenterManager.IsInitialized)
			{
				GameCenterManager.Init();
			}
			else
			{
				HandleActionPlayerConnected();
			}
		}
	}

	void HandleOnCloseShop ()
	{
		if(PlayerPrefs.GetString("Comprado") == "Si")
		{
			Blackboard.showVideo = false;
			Blackboard.showIntersi = false;
			continuesManager.gameObject.SetActive(false);
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) 
			Application.Quit();

	}

	void HandleOnChangeLanguage ()
	{	
		txtTutorial.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Tutorial).GetStringData(Blackboard.Language);
		txtArcade.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Arcade).GetStringData(Blackboard.Language);
		txtHard.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Hard).GetStringData(Blackboard.Language);
		txtNormal.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Normal).GetStringData(Blackboard.Language);
		txtStory.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Story).GetStringData(Blackboard.Language);
		txtBackArcade.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Back).GetStringData(Blackboard.Language);
		txtBackStory.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Back).GetStringData(Blackboard.Language);
		txtDay.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Day).GetStringData(Blackboard.Language);
		txtNight.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Night).GetStringData(Blackboard.Language);
		txtSnow.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Snow).GetStringData(Blackboard.Language);
		txtChooseMap.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Select).GetStringData(Blackboard.Language);
	}

	void HandleActionPlayerConnected ()
	{
		btnAchieve.interactable = true;
		btnRanking.interactable = true;
	}

	void GooglePlayConnection_ActionConnectionResultReceived (GooglePlayConnectionResult result)
	{
		if(result.IsSuccess) {
			Debug.Log("Connected!");
			HandleActionPlayerConnected ();
		} else {
			Debug.Log("Cnnection failed with code: " + result.code.ToString());
		}
	}

	void HandleActionPlayerConnected(ISN_Result _result)
	{
		if(_result.IsSucceeded)
			HandleActionPlayerConnected();
	}

	public void ChangeScene(string _nameLevel)
	{
		if(_nameLevel != "Tutorial")
		{
			if(continuesManager.HaveILife())
			{
				StartCoroutine(Blackboard.Instance.ChangeLevel(_nameLevel));
				ToggleButtons(false);
			}
			else
			{
				Instantiate(Resources.Load("Shop"));
			}
		}
		else
		{
			StartCoroutine(Blackboard.Instance.ChangeLevel(_nameLevel));
		}
	}

	public void OnPushDisable	(Transform _btn)
	{
		if(!btnStory.interactable)
		{
			myPopUp = Instantiate(miniPopup);
			myPopUp.Initialize(_btn.localPosition, Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Unloked).GetStringData(Blackboard.Language), _btn);
		}
	}

	public void OnPushDisableHard	(Transform _btn)
	{
		if(!btnHard.interactable)
		{
			myPopUp = Instantiate(miniPopup);
			myPopUp.Initialize(_btn.localPosition, Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_UnlokedHard).GetStringData(Blackboard.Language), _btn);
		}
	}

	public void OnPushDiableConnect()
	{
		if(Application.platform == RuntimePlatform.Android)
		{
			GooglePlayConnection.Instance.Connect();
		}
		else if( Application.platform == RuntimePlatform.IPhonePlayer)
		{
			GameCenterManager.Init();
		}
	}
	
	public void StoryMode()
	{
		SwitchButtons(false);
		btnNormal.gameObject.SetActive(true);
		btnBackStory.gameObject.SetActive(true);
		btnHard.gameObject.SetActive(true);
	}

	public void OnPushBack()
	{
		SwitchButtons(false);
		btnTutorial.gameObject.SetActive(true);
		btnStory.gameObject.SetActive(true);
		btnArcade.gameObject.SetActive(true);
		btnSettings.gameObject.SetActive(true);
		btnAchieve.gameObject.SetActive(true);
		btnRanking.gameObject.SetActive(true);
	}

	public void OnPushArcade()
	{
		SwitchButtons(false);
		btnDay.gameObject.SetActive(true);
		btnNight.gameObject.SetActive(true);
		btnSnow.gameObject.SetActive(true);
		btnBackArcade.gameObject.SetActive(true);
		txtChooseMap.gameObject.SetActive(true);
	}

	public void OnPushAchiev()
	{
		if(Application.platform == RuntimePlatform.Android)
		{
			GooglePlayManager.Instance.ShowAchievementsUI();
		}
		else if( Application.platform == RuntimePlatform.IPhonePlayer)
		{
			GameCenterManager.ShowAchievements();
		}

	}

	public void OnPushLader()
	{
		if(Application.platform == RuntimePlatform.Android)
		{
			GooglePlayManager.Instance.ShowLeaderBoardsUI();
		}
		else if( Application.platform == RuntimePlatform.IPhonePlayer)
		{
			GameCenterManager.ShowLeaderboard(Achievements.RANKING);
		}

	}

	public void PushSettings()
	{
		Instantiate(settings);
	}

	private void SwitchButtons(bool _flag)
	{
		btnTutorial.gameObject.SetActive(_flag);
		btnStory.gameObject.SetActive(_flag);
		btnArcade.gameObject.SetActive(_flag);
		btnNormal.gameObject.SetActive(_flag);
		btnBackStory.gameObject.SetActive(_flag);
		btnBackArcade.gameObject.SetActive(_flag);
		btnHard.gameObject.SetActive(_flag);
		btnAchieve.gameObject.SetActive(_flag);
		btnRanking.gameObject.SetActive(_flag);
		btnSettings.gameObject.SetActive(_flag);
		btnDay.gameObject.SetActive(_flag);
		btnNight.gameObject.SetActive(_flag);
		btnSnow.gameObject.SetActive(_flag);
		txtChooseMap.gameObject.SetActive(_flag);
	}

	public void PlaySfxUI()
	{
		SoundManager.PlaySFX("UIButton");
	}

	private void OnDestroy()
	{
		GooglePlayConnection.ActionConnectionResultReceived 	-= GooglePlayConnection_ActionConnectionResultReceived;;
		GameCenterManager.OnAuthFinished						-= HandleActionPlayerConnected;
		SettingsController.OnChangeLanguage						-= HandleOnChangeLanguage;
		Shop.OnClose											-= HandleOnCloseShop;
	}

	private void ToggleButtons(bool _flag)
	{
		btnDay.enabled = _flag;
		btnAchieve.enabled = _flag;
		btnArcade.enabled = _flag;
		btnBackArcade.enabled = _flag;
		btnBackStory.enabled = _flag;
		btnHard.enabled = _flag;
		btnNight.enabled = _flag;
		btnNormal.enabled = _flag;
		btnRanking.enabled = _flag;
		btnSettings.enabled = _flag;
		btnSnow.enabled = _flag;
		btnStory.enabled = _flag;
		btnTutorial.enabled = _flag;
	}

//	void OnGUI ()
//	{
//		if (GUI.Button(new Rect(10, 100, 80, 30), "Level1Normal"))
//			StartCoroutine( Blackboard.Instance.ChangeLevel("Level1Normal"));
//		if (GUI.Button(new Rect(10, 130, 80, 30), "Level2Normal"))
//			StartCoroutine( Blackboard.Instance.ChangeLevel("Level2Normal"));
//		if (GUI.Button(new Rect(10, 160, 80, 30), "Level3Normal"))
//			StartCoroutine( Blackboard.Instance.ChangeLevel("Level3Normal"));
//		if (GUI.Button(new Rect(10, 190, 80, 30), "Boss1"))
//			StartCoroutine( Blackboard.Instance.ChangeLevel("Boss1"));
//		if (GUI.Button(new Rect(10, 220, 80, 30), "Boss2"))
//			StartCoroutine( Blackboard.Instance.ChangeLevel("Boss2"));
//		if (GUI.Button(new Rect(10, 250, 80, 30), "InfinitasVidas"))
//			PlayerPrefs.SetString("Comprado", "Si");
//	}
//
}
