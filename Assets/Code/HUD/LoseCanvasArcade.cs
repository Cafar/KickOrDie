using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using DG.Tweening;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class LoseCanvasArcade : LoseCanvas {

	public Text  kills, best;

	private int totalKills;

	void OnEnable()
	{
		totalKills = Blackboard.totalEnemiesDead;

		kills.text 	= totalKills.ToString();


		if(totalKills > PlayerPrefs.GetInt("TotalKills"))
		{
			PlayerPrefs.SetInt("TotalKills", totalKills);
			//RANKKING
			if(Application.platform == RuntimePlatform.Android)
			{
				GooglePlayManager.Instance.SubmitScoreById(Achievements.RANKING, totalKills);
			}
			else if( Application.platform == RuntimePlatform.IPhonePlayer)
			{
				GameCenterManager.ReportScore(totalKills,Achievements.RANKING);
			}
		}



		best.text	= PlayerPrefs.GetInt("TotalKills").ToString(); 




		CheckAchieve();

		SendAnalitycs();
	}

	private void SendAnalitycs()
	{
		Analytics.CustomEvent("ArcadePoints", new Dictionary<string,object>
			{
				{ "Points", totalKills}
			});
	}

	private void CheckAchieve()
	{
		if(totalKills >= 1000)
		{
			//ACHIEVE
			if(Application.platform == RuntimePlatform.Android)
			{
				GooglePlayManager.Instance.UnlockAchievementById(Achievements.KAGE);
			}
			else if( Application.platform == RuntimePlatform.IPhonePlayer)
			{
				GameCenterManager.SubmitAchievement(100f, Achievements.KAGE);
			}
		}

		if(totalKills >= 500)
		{
			//ACHIEVE
			if(Application.platform == RuntimePlatform.Android)
			{
				GooglePlayManager.Instance.UnlockAchievementById(Achievements.JONIN);
			}
			else if( Application.platform == RuntimePlatform.IPhonePlayer)
			{
				GameCenterManager.SubmitAchievement(100f, Achievements.JONIN);
			}
		}

		if(totalKills >= 200)
		{
			//ACHIEVE
			if(Application.platform == RuntimePlatform.Android)
			{
				GooglePlayManager.Instance.UnlockAchievementById(Achievements.CHUNIN);
			}
			else if( Application.platform == RuntimePlatform.IPhonePlayer)
			{
				GameCenterManager.SubmitAchievement(100f, Achievements.CHUNIN);
			}
		}

		if(totalKills >= 50)
		{
			//ACHIEVE
			if(Application.platform == RuntimePlatform.Android)
			{
				GooglePlayManager.Instance.UnlockAchievementById(Achievements.GENIN);
			}
			else if( Application.platform == RuntimePlatform.IPhonePlayer)
			{
				GameCenterManager.SubmitAchievement(100f, Achievements.GENIN);
			}
		}
	}

	public void RetryArcade()
	{
		if(continuesManager.HaveILife())
		{
			Heart.SetTrigger("Life");
			Invoke("OnFinishAnimationHeartArcade", 2);
			retry.enabled = false;
		}
		else
		{
			Instantiate(Resources.Load("Shop"));
		}
	}

	public void OnFinishAnimationHeartArcade()
	{
		StartCoroutine(Blackboard.Instance.ChangeLevel(Blackboard.nameLoadedlevel));
		Blackboard.currentLifeMainCharacter = 3;
	}

	public void OnClickShowLeader()
	{
		if(Application.platform == RuntimePlatform.Android)
		{
			GooglePlayManager.Instance.ShowLeaderBoardsUI();
		}
		else if( Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if(!GameCenterManager.IsInitialized)
			{
				GameCenterManager.Init();
			}
			else
			{	
				GameCenterManager.ShowLeaderboard(Achievements.RANKING);
			}
		}
	}
}
