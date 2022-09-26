using UnityEngine;
using System.Collections;
using DG.Tweening;


public class BossMitsuController : BossController {

	public delegate void HandleOnDeadBoss();
	public static event HandleOnDeadBoss OnMitsuDead;
	
	public AudioClip fxDead;

	public float timeStuned = 1;

	private bool isSkillOn = false;


	[HideInInspector]
	public Transform mainCharacter;

	[HideInInspector]
	public Rigidbody2D rigid;

	[HideInInspector]
	public bool attackFromBack;

	private GhostingContainer ga;

	#region FSM

	
	public BackAttackState 		backAttackState;

	public PrepareState 		prepareState;

	public HittingState 		hittingState;
	
	public FrontAttackState 		frontAttackState;

	public LaughtState	 	laughtState;

	public RunState 		runState;
	
	public DeathState		deathState;

	public BackState		backState;

	
	
	#endregion

	void Awake()
	{
		animator 	= GetComponent<Animator>();
		rigid		= GetComponent<Rigidbody2D>();
		mainCharacter = GameObject.FindGameObjectWithTag("MainCharacter").transform;

	}

	public void Start()
	{
//		if(Random.Range(0,100) >= 95)
//			velocity *= 2;
		Fsm = FSM.CreateFSM(this);
		
		Fsm.AddFSMState (idleState, backAttackState, frontAttackState, hittingState, runState, deathState, laughtState, prepareState, backState);
		
		Fsm.ChangeState(runState);

		SkillController.OnInstantiate += delegate() {
			isSkillOn = true;
		};

		SkillController.OnDestroy += delegate() {
			isSkillOn = false;
		};

		MainCharacterController.OnPlayerDead += HandleOnPlayerDead;

		GetComponent<LifeModule>().OnKill += HandleOnKill;
		GetComponent<LifeModule>().OnLifeChange += HandleOnLifeChange;

		ga = GetComponent<GhostingContainer>();
		
		ga.Init(7,.1f,GetComponent<SpriteRenderer>());

		ga.enabled = false;
	}
	
	void HandleOnPlayerDead ()
	{
		rigid.velocity = Vector2.zero;
		Fsm.ChangeState(laughtState);
	}

	//Cuando me hacen daño, espero timeStuned y cambio de estado a backing
	void HandleOnLifeChange (LifeModule _who, float _currentLife, float _previous, float _percentage)
	{
		if(_currentLife != 0)
		{
			SoundManager.PlaySFX("BossMitsuHit");
			Fsm.ChangeState(hittingState);
			animator.SetTrigger("Blink");
			animator.speed += .1f;
			Invoke("ChangeState", timeStuned);
		}
	}

	/// <summary>
	/// Handles the on kill.
	/// </summary>
	/// <param name="_who">_who.</param>
	void HandleOnKill (GameObject _who)
	{
		Fsm.ChangeState(deathState);
	}

	/// <summary>
	/// Changes the state.
	/// </summary>
	void ChangeState()
	{
		Fsm.ChangeState(backState);
	}

	void Update()
	{
		if(isSkillOn)
		{
			Fsm.ChangeState(idleState);
		}
	}

	/// <summary>
	/// Flip the specified _who.
	/// </summary>
	/// <param name="_who">_who.</param>
	void Flip(Transform _who)
	{
		if(_who.transform.rotation.eulerAngles.y > 100)
			_who.transform.rotation = Quaternion.Euler(Vector3.zero);
		else
			_who.transform.rotation = Quaternion.Euler(new Vector3(0,180,0));

	}

	public void PlaySfxPrepare()
	{
		SoundManager.PlaySFX("BossMitsuShine");
	}

	public void DashAttack()
	{
		float num = Random.Range(0,100);

		ga.enabled = true;

		Vector2 right = 	-transform.TransformDirection(Vector2.right);
		rigid.AddForce(new Vector2( right.x * 50, 0), ForceMode2D.Impulse);

		if(num > 50)
			Fsm.ChangeState(backAttackState);
		else
			Fsm.ChangeState(frontAttackState);
	}

	public void Attack()
	{
		ga.enabled = false;

		SoundManager.PlaySFX("BossMitsuAttack");
		if(mainCharacter.GetComponent<LifeModule>().currentLife > 0)
			mainCharacter.GetComponent<LifeModule>().DoDamage(damage);	
		
		Fsm.ChangeState(laughtState);
	}

	#region states

	public void OnDestroy()
	{
		SoundManager.StopSFX();
		Time.timeScale = 1;
		SoundManager.PlayImmediately(HUDManager.instance.bossEnd);
		MainCharacterController.OnPlayerDead -= HandleOnPlayerDead;
	}

	
	[System.Serializable]
	public class RunState : FSM.FSMState
	{
		private BossMitsuController myOwner;
		private bool flag;
		private AudioSource loop;

		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as BossMitsuController;
			
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			loop = SoundManager.PlaySFX("BossMitsuSteps",true,0,1f);

			if(fsm.GetPreviousState() != myOwner.backState)
				myOwner.animator.SetTrigger("Run");

			flag = true;

		}
		
		public override void Update ()
		{
			base.Update ();
			
			if(myOwner.transform.localRotation.eulerAngles.y > 100)
				myOwner.rigid.velocity =  new Vector2(myOwner.velocity, myOwner.rigid.velocity.y);
			else
				myOwner.rigid.velocity =  new Vector2(-myOwner.velocity, myOwner.rigid.velocity.y);

			//este es el primero estado, compruebo que está a una distancia larga del personaje, cuando llegue a esta distancia se queda parado y hace un ataque aleatorio

			float distance = Vector2.Distance(myOwner.transform.position, myOwner.mainCharacter.transform.position);
			if(distance < 6 && flag)
			{
				ChangeState(myOwner.prepareState);
				//Lanzo con el Animator un evento que diga que ataque va a hacer (DASH ATTACK())
			}
		}
		
		public override void Exit (FSM.FSMState _nextState)
		{
			if(loop != null)
				loop.Stop();
		}
		
	}


	[System.Serializable]
	public class PrepareState : FSM.FSMState
	{
		
		private BossMitsuController myOwner;
		
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as BossMitsuController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			myOwner.rigid.velocity = Vector2.zero;
			myOwner.animator.SetTrigger("Prepare");
			
		}

		
		public override void Exit (FSM.FSMState _nextState)
		{
			base.Exit (_nextState);
		}
	}

	
	// Cuando es golpeado
	[System.Serializable]
	public class HittingState : FSM.FSMState
	{
		
		private BossMitsuController myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as BossMitsuController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			myOwner.ga.enabled = false;

			myOwner.animator.SetTrigger("Hit");
			Vector2 right = Vector2.zero;

			if(myOwner.attackFromBack)
			{
				right = -myOwner.transform.TransformDirection(Vector2.right);
			}
			else
			{	
				right = myOwner.transform.TransformDirection(Vector2.right);
			}

			myOwner.rigid.velocity = Vector2.zero;
			myOwner.rigid.AddForce(new Vector2( right.x * 5, 10), ForceMode2D.Impulse);


			myOwner.GetComponent<LifeModule>().invulnerable = true;

		}

		public override void Exit (FSM.FSMState _nextState)
		{
			base.Exit (_nextState);
		}
	}

	[System.Serializable]
	public class LaughtState : FSM.FSMState
	{
		
		private BossMitsuController myOwner;

		private AudioSource laughloop;

		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as BossMitsuController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			
			myOwner.animator.SetBool("Laught", true);
			
			myOwner.rigid.velocity = Vector2.zero;

			
			laughloop = SoundManager.PlaySFX("BossMitsuLaughLOOP", true,0,0.6f);

			
		}
		
		public override void Exit (FSM.FSMState _nextState)
		{
			base.Exit (_nextState);
			myOwner.animator.SetBool("Laught", false);
			if(laughloop != null)
				laughloop.Stop();
			
		}
	}

	[System.Serializable]
	public class DeathState : FSM.FSMState
	{
		
		private BossMitsuController myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as BossMitsuController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			SoundManager.PlaySFX("BossMitsuDie");
			Rigidbody2D rigid = myOwner.rigid ;
			//SoundManager.Instance.Play2DSound(myOwner.fxDead);
			myOwner.animator.SetTrigger("Dead");
			//myOwner.GetComponent<BoxCollider2D>().enabled = false;
			//myOwner.GetComponent<CircleCollider2D>().enabled = false;
			Vector2 right = 	myOwner.transform.TransformDirection(Vector2.right);


			Time.timeScale = 0.3f;
			rigid.velocity = Vector2.zero;
			rigid.AddForce(new Vector2( right.x * 3, right.y + 6), ForceMode2D.Impulse);
			rigid.gravityScale = 3;
			foreach(SpriteRenderer sprite in myOwner.GetComponentsInChildren<SpriteRenderer>())
				sprite.DOFade(0,2);

			if(OnMitsuDead != null)
				OnMitsuDead();

			//ACHIEVE
			if(Application.platform == RuntimePlatform.Android)
			{
				GooglePlayManager.Instance.UnlockAchievementById(Achievements.MITSU);
			}
			else if( Application.platform == RuntimePlatform.IPhonePlayer)
			{
				GameCenterManager.SubmitAchievement(100f, Achievements.MITSU);
			}

			Destroy(myOwner.gameObject, 2);
		}
		
	}

	[System.Serializable]
	public class BackAttackState : FSM.FSMState
	{
		
		private BossMitsuController myOwner;
		private bool flag;
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as BossMitsuController;
			
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			flag = true;
			myOwner.attackFromBack = true;
			SoundManager.PlaySFX("BossMitsuMoveLong");
		}
		
		public override void Update ()
		{
			base.Update ();
			if(flag)
			{
				if(myOwner.transform.rotation.eulerAngles.y > 100 )
				{
					if(myOwner.transform.position.x > 2 && myOwner.transform.position.x > 0)
					{
						myOwner.rigid.velocity = Vector2.zero;
						myOwner.transform.localPosition = new Vector2(2.5f,myOwner.transform.localPosition.y);
						myOwner.animator.SetTrigger("BackAttack");
						flag = false;
					}
				}
				else
				{
					if(myOwner.transform.position.x < -2 && myOwner.transform.position.x < 0)
					{
						myOwner.rigid.velocity = Vector2.zero;
						myOwner.transform.localPosition = new Vector2(-2.5f,myOwner.transform.localPosition.y);
						myOwner.animator.SetTrigger("BackAttack");
						flag = false;
					}
				}
			}
		}
		
		public override void Exit (FSM.FSMState _nextState)
		{
			base.Exit (_nextState);
		}
		
	}
	
	[System.Serializable]
	public class FrontAttackState : FSM.FSMState
	{
		
		private BossMitsuController myOwner;
		private bool flag;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as BossMitsuController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			flag = true;
			myOwner.attackFromBack = false;
			SoundManager.PlaySFX("BossMitsuMoveShort");
		}

		public override void Update ()
		{
			base.Update ();

			if(((myOwner.transform.localPosition.x <= myOwner.distanceToAttack && myOwner.transform.localPosition.x > 0) || (myOwner.transform.localPosition.x >= -myOwner.distanceToAttack && myOwner.transform.localPosition.x < 0)) && flag)
			{
				flag = false;
				myOwner.rigid.velocity = Vector2.zero;
				myOwner.animator.SetTrigger("FrontAttack");
			}
		}
		
	}

	//Cuando entro en este estado es porque me han golpeado y me voy para atrás
	[System.Serializable]
	public class BackState : FSM.FSMState
	{
		
		private BossMitsuController myOwner;
		private bool flag;

		private AudioSource loop;

		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as BossMitsuController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			flag = true;
			SoundManager.PlaySFX("BossMitsuAngry");
			loop = SoundManager.PlaySFX("BossMitsuSteps",true,0,1f);
			myOwner.animator.SetTrigger("Run");
		}
		
		public override void Update ()
		{
			base.Update ();

			if(!myOwner.attackFromBack && flag)
			{
				myOwner.Flip(myOwner.transform);
				flag = false;
			}

			if(myOwner.transform.localRotation.eulerAngles.y > 100)
				myOwner.rigid.velocity =  new Vector2(myOwner.velocity+5, myOwner.rigid.velocity.y);
			else
				myOwner.rigid.velocity =  new Vector2(-myOwner.velocity-5, myOwner.rigid.velocity.y);


			
			float distance = Vector2.Distance(myOwner.transform.position, myOwner.mainCharacter.transform.position);
			if(distance > 8)
			{
				myOwner.GetComponent<LifeModule>().invulnerable = false;
				if(Random.Range(0,100) <= 50)
				{
					myOwner.transform.position = new Vector3(-myOwner.transform.position.x, myOwner.transform.position.y, 0);
				}
				else
				{
					myOwner.Flip(myOwner.transform);	
				}
				ChangeState(myOwner.runState);
			}
		}
		public override void Exit (FSM.FSMState _nextState)
		{
			base.Exit (_nextState);
			if(loop != null)
				loop.Stop();
		}
	}
	#endregion
}
