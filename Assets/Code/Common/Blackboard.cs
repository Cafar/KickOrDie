using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using Google2u;
using UnityEngine.Advertisements;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class Blackboard : MonoBehaviour {
	
	public static int totalEnemiesDead;
	public static int eagalBossDeadCount;
	public static float currentLifeMainCharacter = 3;
	public static string selectedMap;
	public static string nameLoadedlevel;
	public static bool showVideoOnMenu;

	public static string Language;

	public static Google2u.MyLocalization localization;

	public static bool showIntersi, showVideo;


	private static Blackboard instance;
	public static Blackboard Instance 
	{
		get
		{
			if(instance == null)
			{
				string resourcesPrefabPath = "Blackboard";
				// Search in resources folder for this GameObject
				Blackboard managerPrefab = Resources.Load<Blackboard>(resourcesPrefabPath);
				
				if(managerPrefab == null)
				{
					Debug.LogError("[ERROR] Prefab "+resourcesPrefabPath+" not found in Resources directory");
					return null;
				}
				
				Instance = Instantiate(managerPrefab) as Blackboard;
			}
			
			return instance;
		}
		
		private set{
			instance = value;
		}
	}

	void Awake()
	{
		DOTween.Init();

		instance = this;

		showVideo 		= true;
		showIntersi 	= true;



		if(PlayerPrefs.GetString("Language") == "")
		{
			if(Application.systemLanguage == SystemLanguage.Spanish)
			{
				PlayerPrefs.SetString("Language","Spanish");
				Language = "es";
			}
			else
			{
				PlayerPrefs.SetString("Language","English");
				Language = "en";
			}
		}

		localization = Google2u.MyLocalization.Instance;
		checkMusicAndFx();

		CheckLanguage();
	}

	public bool canIRecord()
	{
		bool flag = false;
		if(Everyplay.IsSupported())
		{
			flag = true;
		}

		return flag;

	}

	public IEnumerator ChangeLevel(string sceneName, float time  = 0)
	{
		Analytics.CustomEvent("SceneLoaded", new Dictionary<string,object>
			{
				{ "Scene", sceneName}
			});
		nameLoadedlevel = sceneName;

		//ACHIEVE
		if(sceneName == "ArcadeModeNight")
		{
			if(Application.platform == RuntimePlatform.Android)
			{
				GooglePlayManager.Instance.UnlockAchievementById(Achievements.TROTA);

			}
			else if( Application.platform == RuntimePlatform.IPhonePlayer)
			{
				GameCenterManager.SubmitAchievement(100f, Achievements.TROTA);
			}
		}
		yield return new WaitForSeconds(time);

		SoundManager.SetCrossDuration(2);
		SoundManager.StopMusic();
		ChangeSceneManager.Instance.ChangeScene(sceneName);
	}

	/// <summary>
	/// Change the timeScale of the game
	/// </summary>
	/// <returns>The time.</returns>
	public IEnumerator ChangeTime(float time, float delay = 0)
	{
		yield return  StartCoroutine( WaitForRealSeconds( delay ) );
		Time.timeScale = time;
	}

	public void ShowAd()
	{
		if (Advertisement.IsReady() && showIntersi)
		{
			Advertisement.Show();
		}
	}

	/// <summary>
	/// Esto lo hago por que no funciona el WaitfOrseconds cuando hago un time.timescale a 0
	/// </summary>
	/// <returns>The for real seconds.</returns>
	/// <param name="delay">Delay.</param>
	public static IEnumerator WaitForRealSeconds( float delay )
	{
		float start = Time.realtimeSinceStartup;
		while( Time.realtimeSinceStartup < start + delay )
		{
			yield return null;
		}
	}

	public Text GetTextFromButton(Button _btn)
	{
		Text text = null;
		if(_btn.transform.FindChild("Text") != null)
		{
			text = _btn.transform.GetComponentInChildren<Text>();
		}
		else
		{
			Debug.LogError("El btn no tiene como hijo ninguno text");
		}
		return text;
	}

	public void checkMusicAndFx()
	{
		if(PlayerPrefs.GetInt("Music") == 0)
		{
			SoundManager.SetVolumeMusic(1);
		}
		else
		{
			SoundManager.SetVolumeMusic(0);
		}

		if(PlayerPrefs.GetInt("Fx") == 0)
		{
			SoundManager.SetVolumeSFX(1);
		}
		else
		{
			SoundManager.SetVolumeSFX(0);
		}
	}

	public void CheckImageMusicAndFx(Image _imageMusic, Sprite _musicOn, Sprite _musicOff,Image _imageFx, Sprite _fxOn, Sprite _fxOff)
	{
		if(PlayerPrefs.GetInt("Music") == 0)
		{
			_imageMusic.sprite = _musicOn;
		}
		else
		{
			_imageMusic.sprite = _musicOff;
		}
		_imageMusic.SetNativeSize();

		if(PlayerPrefs.GetInt("Fx") == 0)
		{
			_imageFx.sprite = _fxOn;
		}
		else
		{
			_imageFx.sprite = _fxOff;
		}
		_imageFx.SetNativeSize();
	}

	public void ToggleMusic(Image _image, Sprite _imageOn, Sprite _imageOff)
	{

		bool flag = PlayerPrefs.GetInt("Music") == 0 ? true : false;
		if(flag)
		{
			PlayerPrefs.SetInt("Music", 1);
			SoundManager.SetVolumeMusic(0);
			_image.sprite = _imageOff;
		}
		else
		{
			PlayerPrefs.SetInt("Music", 0);
			SoundManager.SetVolumeMusic(1);
			_image.sprite = _imageOn;
		}
		_image.SetNativeSize();
	}

	public void ToggleFx(Image _image, Sprite _imageOn, Sprite _imageOff)
	{
		bool flag = PlayerPrefs.GetInt("Fx") == 0 ? true : false;
		if(flag)
		{
			PlayerPrefs.SetInt("Fx", 1);
			SoundManager.SetVolumeSFX(0);
			_image.sprite = _imageOff;
		}
		else
		{
			PlayerPrefs.SetInt("Fx", 0);
			SoundManager.SetVolumeSFX(1);
			_image.sprite = _imageOn;
		}
		_image.SetNativeSize();
	}

	public void CheckLanguage()
	{
		if(PlayerPrefs.GetString("Language") == "Spanish")
		{
			Language = "es";
		}
		else
		{
			Language = "en";
		}
	}

	public void CheckImageLanguage(Sprite _en, Sprite _es, Image _flag)
	{
		if(PlayerPrefs.GetString("Language") == "Spanish")
		{
			_flag.sprite = _es;
		}
		else
		{
			_flag.sprite = _en;
		}
	}

	public void ToggleLanguage(Sprite _en, Sprite _es, Image _flag)
	{
		if(PlayerPrefs.GetString("Language") == "Spanish")
		{
			_flag.sprite = _en;
			PlayerPrefs.SetString("Language","English");
		}
		else
		{
			_flag.sprite = _es;
			PlayerPrefs.SetString("Language","Spanish");
		}
		CheckLanguage();
	}
}
