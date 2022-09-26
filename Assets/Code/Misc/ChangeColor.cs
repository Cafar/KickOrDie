using UnityEngine;
using System.Collections;

public class ChangeColor : MonoBehaviour {

	public float timeToSwitch;
	public Color mainColor = Color.white;
	public Color colorToSwitch = Color.green;

	private float timeElapsed;
	private bool switchOn;

	void Start()
	{
		Invoke("Switch", timeToSwitch);
		switchOn = false;
	}

	void Update () 
	{
		if(timeElapsed > 2 && switchOn)
		{
			GetComponent<SpriteRenderer>().color = colorToSwitch;
			Invoke("Switch",.2f);
		}
		timeElapsed += Time.deltaTime;
	}

	void Switch()
	{
		GetComponent<SpriteRenderer>().color = mainColor;
		timeElapsed = 0;
		switchOn = true;
	}
}
