using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SkillController : MonoBehaviour {

	public delegate void HandleOnDestroy();
	public static event HandleOnDestroy OnDestroy;

	public delegate void HandleOnInstantiate();
	public static event HandleOnInstantiate OnInstantiate;
	
	public AudioClip fxSoundHits, fxSoundActivate, fxSoundApache;

	private GameObject mainChar;
	private Animator anim;
	public LayerMask ignoreLayer;

	private float bossVelocity;

	void OnEnable()
	{
		if(OnInstantiate != null)
			OnInstantiate();
	}

	void Start () 
	{
		mainChar = GameObject.FindGameObjectWithTag("MainCharacter");


		mainChar.GetComponent<LifeModule>().invulnerable = true;

		anim = GetComponent<Animator>();

		if(Google2u.GameController.Instance != null)
			Google2u.GameController.Instance.pauseSpawnWaves = true;

		foreach(SpriteRenderer sprite in mainChar.GetComponentsInChildren<SpriteRenderer>())
		{
			sprite.enabled = false;
		}

		SoundManager.PlaySFX("ChSpecialAttack");
		//DOTween.To(()=> Camera.main.GetComponent<CameraFit>().UnitsForWidth, x => Camera.main.GetComponent<CameraFit>().UnitsForWidth = x, 7, .3f).SetLoops(2,LoopType.Yoyo);

		Camera.main.GetComponent<CameraShake>().shake = 2f;

		foreach(EnemyController enemy in FindObjectsOfType(typeof(EnemyController)))
		{
			enemy.velocity = 0;
			enemy.Fsm.ChangeState(enemy.idleState);
		}

		foreach(BossController boss in FindObjectsOfType(typeof(BossController)))
		{
			bossVelocity = boss.velocity;
			boss.velocity = 0;
		}

		anim.SetTrigger("Skill");

		Invoke("FinishSkill", 1.7f);
		
	}


	public void FinishSkill()
	{
		Transform pos = mainChar.transform;
		//Coon estas 3 lineas consigo la posición X a la izqeuuierda de la pantalla
		Vector2 targetWidth = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0));
		targetWidth = new Vector2(-targetWidth.x, pos.position.y);
		
		RaycastHit2D[] hits = Physics2D.RaycastAll(targetWidth, Vector2.right, -targetWidth.x * 2);
		for(int i = 0; i< hits.Length; i++)
		{
			RaycastHit2D hit = hits[i];
			
			if(hit.collider.tag == "Enemy")
			{
				EnemyController enemy = hit.transform.GetComponent<EnemyController>();
				enemy.velocity = 0;
				enemy.Fsm.ChangeState(enemy.deathState);
			}
			else if(hit.collider.tag == "Boss")
			{
					hit.transform.GetComponent<LifeModule>().DoDamage(1);
					hit.transform.GetComponent<BossController>().velocity = bossVelocity;
			}
			else if(hit.collider.tag == "BossFinal")
			{
				hit.transform.parent.GetComponent<LifeModule>().DoDamage(1);
			}
		}

		foreach(EnemyController enemy in FindObjectsOfType(typeof(EnemyController)))
		{
			enemy.velocity = enemy.startVelocity;
			if(enemy.Fsm.GetCurrentState() != enemy.deathState)
			{
				if(enemy.name == "Dog(Clone)")
				{
					enemy.GetComponent<DogController>().Fsm.ChangeState(enemy.GetComponent<DogController>().walkState);
				}
				else
				{
					enemy.Fsm.ChangeState(enemy.movingState);
				}
			}

		}

		foreach(BossController boss in FindObjectsOfType(typeof(BossController)))
		{
			boss.velocity = bossVelocity;
		}

		if(Google2u.GameController.Instance != null)
			Google2u.GameController.Instance.pauseSpawnWaves = false;

		if(OnDestroy != null)
			OnDestroy();
		
		foreach(SpriteRenderer sprite in mainChar.GetComponentsInChildren<SpriteRenderer>())
		{
			sprite.enabled = true;
		}

		mainChar.GetComponent<LifeModule>().invulnerable = false;

		Destroy(gameObject);
	}
}
