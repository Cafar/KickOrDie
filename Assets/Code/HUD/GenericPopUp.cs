using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GenericPopUp : FloatingPopUp {

	public string key;
	public Text myText;

	public override void Start ()
	{
		base.Start ();

		myText.text = Blackboard.localization.GetRow(key).GetStringData(Blackboard.Language);
	}
	
	public void Rate()
	{
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
}
