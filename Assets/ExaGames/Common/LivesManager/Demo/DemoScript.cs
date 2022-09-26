using UnityEngine;
using ExaGames.Common;
using UnityEngine.UI;

public class DemoScript : MonoBehaviour {
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

	/// <summary>
	/// Play button event handler.
	/// </summary>
	public void OnButtonPressed() {
		if(LivesManager.ConsumeLife()) {
			// Go to your game!
		} else {
			// Tell player to buy lives, then:
			// LivesManager.GiveOneLife();
			// or
			// LivesManager.FillLives();
		}
	}

	/// <summary>
	/// Lives changed event handler, changes the label value.
	/// </summary>
	public void OnLivesChanged() {
		LivesText.text = LivesManager.Lives.ToString();
	}

	/// <summary>
	/// Time to next life changed event handler, changes the label value.
	/// </summary>
	public void OnTimeToNextLifeChanged() {
		TimeToNextLifeText.text = LivesManager.RemainingTimeString;
	}
}