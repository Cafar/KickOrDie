using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class MiniPopup : MonoBehaviour {

	public Transform panel;
	public Text text;

	public void Initialize (Vector2 _pos, string _text, Transform _parent) 
	{
		panel.localScale = Vector3.zero;
		transform.SetParent(_parent);
		text.text = _text;
		panel.localPosition = _pos;
		panel.DOScale(Vector3.one,.3f);
	}


	public void Update()
	{
		if(Input.GetButtonUp("Fire1"))
			Destroy(1);
	}
	public void Destroy(int _time)
	{
		panel.DOScale(Vector2.zero,.3f);
		Destroy(gameObject,_time);
	}
}
