using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using DG.Tweening;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class LoseCanvas : MonoBehaviour {

	public delegate void HandleOnContinue();
	public static event HandleOnContinue OnClickContinue;

	public Button watchVideo, retry, exit, shareVideo;
	public Text hintText;
	public Canvas rate, newFeature;

	public Transform panel;

	public ContinuesManager continuesManager;

	public GameObject canvasShop;

	protected Animator Heart;

	protected bool retry1Time;

	void Awake()
	{
		retry1Time = false;
	}

	public IEnumerator Init () 
	{
		panel.localScale = Vector3.zero;
		yield return new WaitForSeconds(1);
		gameObject.SetActive(true);
		panel.DOScale(Vector3.one,.3f);
		watchVideo.interactable = false;
		watchVideo.gameObject.SetActive(true);

		int num = Random.Range(0,4);
		switch(num)
		{
		case 0: hintText.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Hint1).GetStringData(Blackboard.Language);
			break;
		case 1: hintText.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Hint2).GetStringData(Blackboard.Language);
			break;
		case 2: hintText.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Hint3).GetStringData(Blackboard.Language);
			break;
		case 3: hintText.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Hin4).GetStringData(Blackboard.Language);
			break;
		default: hintText.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Hint1).GetStringData(Blackboard.Language);
			break;
		}

		if(Blackboard.Instance.canIRecord() && Everyplay.IsRecording())
		{
			shareVideo.gameObject.SetActive(true);
			Everyplay.StopRecording();

			if(PlayerPrefs.GetInt("NewFeature") != 1)
			{
				PlayerPrefs.SetInt("NewFeature",1);
				Instantiate(newFeature);
			}
		}
		else
		{
			if(shareVideo != null)
				shareVideo.interactable = false;
		}



		Heart = continuesManager.GetComponent<Animator>();
		//Si no he hecho retry ninguna vez...
		if(!retry1Time)
		{
			if(Advertisement.IsReady("rewardedVideo") && Blackboard.showVideo)
			{
				watchVideo.interactable = true;
				retry1Time = true;
			}
			if(PlayerPrefs.GetString("Comprado") == "Si")
			{
				watchVideo.interactable = true;
				retry1Time = true;
				Heart.gameObject.SetActive(false);
			}
		}

		if(continuesManager.LivesManager.Lives == 5)
		{
			Blackboard.Instance.ShowAd();
			if(rate != null)
				Instantiate(rate);
		}
	}

	public void OnClickShare()
	{
		Everyplay.SetMetadata("score", Blackboard.totalEnemiesDead);
		Everyplay.PlayLastRecording();
	}

	public void PlaySfxUI()
	{
		SoundManager.PlaySFX("UIButton");
	}

	public void OnClickWatchVideo()
	{
		ShowRewardedAd();
	}

	public void OnFinishAnimationHeart()
	{
		StartCoroutine(Blackboard.Instance.ChangeLevel("Level1"+Google2u.GameController.Instance.selectedMode.ToString()));
		Blackboard.currentLifeMainCharacter = 3;
	}

	public void OnClickRetry()
	{
		if(continuesManager.HaveILife())
		{
			Heart.SetTrigger("Life");
			Invoke("OnFinishAnimationHeart", 2);
			retry.enabled = false;
		}
		else
		{
			Instantiate(canvasShop);
		}
	}

	public void OnClickExit()
	{
		StartCoroutine(Blackboard.Instance.ChangeLevel("Menu"));
		Blackboard.currentLifeMainCharacter = 3;
	}

	public void ShowRewardedAd()
	{
		if(PlayerPrefs.GetString("Comprado") == "Si")
		{
			if(OnClickContinue != null)
				OnClickContinue();
		}
		else if (Advertisement.IsReady("rewardedVideo"))
		{
			Analytics.CustomEvent("RewardedVideoLose", new Dictionary<string,object>
				{
					{ "RewardedVideoOnLose", "Yes"}
				});
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("rewardedVideo", options);
		}

	}
	
	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log("The ad was successfully shown.");
			//
			// YOUR CODE TO REWARD THE GAMER
			// Give coins etc.
			if(OnClickContinue != null)
				OnClickContinue();
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			break;
		}
	}
}
