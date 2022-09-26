using UnityEngine;
using System.Collections;
using ExaGames.Common;
using UnityEngine.UI;
using System;

public class ContinuesManager : MonoBehaviour {

	/// <summary>
	/// Reference to the LivesManager.
	/// </summary>
	public LivesManager LivesManager;
	/// <summary>
	/// Label to show number of available lives.
	/// </summary>
	public Text LivesText;
	/// <summary>
	/// Label to show time to next life.
	/// </summary>
	public Text TimeToNextLifeText;

	void Start()
	{
		TimeToNextLifeText.text 		= LivesManager.MinutesToRecover.ToString()+":00";
		LivesText.text 					= "x"+LivesManager.Lives.ToString();

		TimeToNextLifeText.text		= Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Full).GetStringData(Blackboard.Language);

		if(PlayerPrefs.GetString("Comprado") == "Si")
		{
			gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// Play button event handler.
	/// </summary>
	public bool HaveILife()
	{
		bool callback = false;
		if(PlayerPrefs.GetString("Comprado") == "Si")
		{
			callback = true;
			SoundManager.PlaySFX("UIStart");
		}
		else
		{
			if(LivesManager.ConsumeLife()) 
			{
				SoundManager.PlaySFX("UIHeart");
				callback = true;
			}
		}

		return callback;
	}
	
	/// <summary>
	/// Lives changed event handler, changes the label value.
	/// </summary>
	public void OnLivesChanged() 
	{
		LivesText.text = "x"+LivesManager.Lives.ToString();
	}
	
	/// <summary>
	/// Time to next life changed event handler, changes the label value.
	/// </summary>
	public void OnTimeToNextLifeChanged() 
	{
		if(TimeToNextLifeText != null)
		{
			if(LivesManager.HasMaxLives)
				TimeToNextLifeText.text		= Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Full).GetStringData(Blackboard.Language);
			else
				TimeToNextLifeText.text = LivesManager.RemainingTimeString;
		}

	}
}
