using UnityEngine;
using System.Collections;

public class SimpleMove : MonoBehaviour {

	public Vector3 dir;
		
	public Vector3 startPosition, endPosition;

	// Update is called once per frame
	void Update () 
	{
		if(transform.localPosition.x <= endPosition.x)
			transform.localPosition = startPosition;


		transform.localPosition += dir*Time.deltaTime;
	}
}
