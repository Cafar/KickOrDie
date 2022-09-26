using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


public class PauseController : FloatingPopUp {

	public Button btnRestart, btnExit;
	public Image imageFx, imageMusic;
	public ContinuesManager continuesManager;
	public Sprite fxOn,fxOff,musicOn,musicOff;
	public Text hintText;

	private Animator Heart;
	private Text restart, exit;

	public override void Start ()
	{
		base.Start ();
		int num = Random.Range(0,4);

		if(Everyplay.IsRecording())
		{
			if(!Everyplay.IsPaused())
				Everyplay.PauseRecording();
		}

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

		restart = Blackboard.Instance.GetTextFromButton(btnRestart);
		exit	= Blackboard.Instance.GetTextFromButton(btnExit);
		
		Blackboard.Instance.CheckImageMusicAndFx(imageMusic,musicOn,musicOff, imageFx, fxOn, fxOff);

		restart.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Restart).GetStringData(Blackboard.Language);
		exit.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Exit).GetStringData(Blackboard.Language);

		Heart = continuesManager.GetComponent<Animator>();
	}

	public void OnFinishAnimationHeart()
	{
		if(Google2u.GameController.Instance.selectedMode == Google2u.GameController.mode.Arcade)
		   StartCoroutine(Blackboard.Instance.ChangeLevel(Blackboard.nameLoadedlevel));
		else           
			StartCoroutine(Blackboard.Instance.ChangeLevel("Level1"+Google2u.GameController.Instance.selectedMode.ToString()));
	}
	
	public void Restart()
	{
		if(continuesManager.HaveILife())
		{
			Time.timeScale = 1;
			Heart.SetTrigger("Life");
			btnRestart.enabled = false;
			Blackboard.currentLifeMainCharacter = 3;
			StopEnemies();
			Invoke("OnFinishAnimationHeart", 1);
		}
		else
		{
			Instantiate(Resources.Load("Shop"));
		}
	}

	public void Exit()
	{
		btnExit.enabled = false;
		Time.timeScale = 1;
		Blackboard.currentLifeMainCharacter = 3;
		StopEnemies();
		if(Everyplay.IsRecording())
		{
			Everyplay.StopRecording();
		}
		StartCoroutine(Blackboard.Instance.ChangeLevel("Menu"));
	}

	private void StopEnemies()
	{
		foreach(EnemyController enemy in FindObjectsOfType(typeof(EnemyController)))
		{
			enemy.velocity = 0;
			enemy.Fsm.ChangeState(enemy.idleState);
		}
		
		foreach(BossController boss in FindObjectsOfType(typeof(BossController)))
		{
			boss.velocity = 0;
		}
	}

	public void fx()
	{
		Blackboard.Instance.ToggleFx(imageFx, fxOn, fxOff);
	}

	public void Music()
	{
		Blackboard.Instance.ToggleMusic(imageMusic, musicOn, musicOff);
	}

	public override void Close ()
	{
		base.Close ();

		if(Everyplay.IsRecording())
		{
			if(Everyplay.IsPaused())
				Everyplay.ResumeRecording();
		}

	}
}
