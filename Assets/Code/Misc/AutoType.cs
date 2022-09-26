using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AutoType : MonoBehaviour {

	public delegate void FinishTextHandler();
	public event FinishTextHandler OnFinishText;

	public float letterPause = 0.2f;
	public AudioClip sound;
	public Text myText;
	public string message;

	private IEnumerator co;
	
	
	// Use this for initialization
	void OnEnable () 
	{
		myText.text = "";
		co = TypeText ();
		StartCoroutine(co);
	}
	
	IEnumerator TypeText () {
		foreach (char letter in message.ToCharArray()) {
			myText.text += letter;
			if (sound)
				//SoundManager.Instance.Play2DSound(sound);
			yield return 0;
			yield return new WaitForSeconds (letterPause);
		}      
		if(OnFinishText != null)
			OnFinishText();
	}

	void Update()
	{
		if(Input.GetButtonDown("Fire1"))
		{
			StopCoroutine(co);
			if(myText.text != message)
			{
				myText.text = message;
			}
			else
			{
				if(OnFinishText != null)
					OnFinishText();
			}
		}
	}
}
