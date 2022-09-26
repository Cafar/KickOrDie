using UnityEngine;
using System.Collections;

public class ChangeRotation : MonoBehaviour {

	float lastPositionY;

	void Start () 
	{
		lastPositionY = transform.localPosition.y;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(transform.localPosition.y < lastPositionY && transform.localScale.y < 0)
		{
			Vector3 scale = transform.localScale;
			scale.y = 1;
			transform.localScale = scale;
		}
		else if(transform.localPosition.y > lastPositionY && transform.localScale.y > 0)
		{
			Vector3 scale = transform.localScale;
			scale.y = -1;
			transform.localScale = scale;
		}
		else
		{
			lastPositionY = transform.localPosition.y;
		}
	}
}
