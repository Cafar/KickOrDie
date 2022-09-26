using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {

	public float waitSeconds;
	public string nameScene;
	public bool change = true;


	public AudioClip musicCredits;

	void Start () 
	{
		if(change)
			StartCoroutine(Blackboard.Instance.ChangeLevel(nameScene, waitSeconds));

		if(Blackboard.nameLoadedlevel == "Credits")
		{
			SoundManager.Play(musicCredits, true);
		}
	}

	void Update()
	{
		if(Input.GetButtonDown("Fire1"))
		{
			StartCoroutine(Blackboard.Instance.ChangeLevel(nameScene, waitSeconds));
		}
	}
}
