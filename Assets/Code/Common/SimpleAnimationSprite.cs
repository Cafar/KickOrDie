using UnityEngine;
using System.Collections;

public class SimpleAnimationSprite : MonoBehaviour {

	public Sprite[] sprites;
	public float fps;
	private SpriteRenderer spriteRenderer;
	public float waitRandomTime = 0;
	public bool bucle = true;
	private float velocity;
	// Use this for initialization
	void Start () 
	{
		spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;

		StartCoroutine(Launch());
	}
	void Update()
	{
		velocity = (fps / sprites.Length) * 0.1f;
	}

	IEnumerator Launch()
	{
		while(true)
		{
			for(int i = 0; i <= sprites.Length-1; i++)
			{
				spriteRenderer.sprite = sprites[i];
				yield return new WaitForSeconds(velocity);
			}
			yield return new WaitForSeconds(Random.Range(0,waitRandomTime));

			if(!bucle)
				break;
		}

	}
}
