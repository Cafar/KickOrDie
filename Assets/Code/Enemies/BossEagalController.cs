using UnityEngine;
using System.Collections;
using DG.Tweening;


public class BossEagalController : BossController {

	public delegate void HandleOnDeadBoss();
	public static event HandleOnDeadBoss OnEagalDead;
	
	public AudioClip fxDead;

	public float timeStuned = 1;

	private bool isSkillOn = false;


	[HideInInspector]
	public Transform mainCharacter;

	[HideInInspector]
	public Rigidbody2D rigid;

	[HideInInspector]
	public AudioSource loop;

	#region FSM

	
	public RunAttackState 	runAttackState;

	public HittingState 	hittingState;
	
	public FightingState 	fightingState;

	public LaughtState	 	laughtState;

	public BackingState 	backingState;
	
	public DeathState		deathState;

	//En este modo, el personaje va directamente corriento hacia a ti, sencillo
	public ModeAttack1 		attack1State;

	//En este modo, el personaje va corriendo y antes de llegar se para y vuelve a arrancar
	public ModeAttack2 		attack2State;


	public ModeAttack3 		attack3State;
	
	
	
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
		Fsm.AddFSMState (fightingState, hittingState, runAttackState, deathState, laughtState, backingState, attack1State, attack2State, attack3State);
		
		Fsm.ChangeState(runAttackState);

		SkillController.OnInstantiate += delegate() {
			isSkillOn = true;
		};

		SkillController.OnDestroy += delegate() {
			isSkillOn = false;
		};

		MainCharacterController.OnPlayerDead += HandleOnPlayerDead;

		GetComponent<LifeModule>().OnKill += HandleOnKill;
		GetComponent<LifeModule>().OnLifeChange += HandleOnLifeChange;
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
			Fsm.ChangeState(hittingState);
			animator.SetTrigger("Blink");
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
		Fsm.ChangeState(backingState);
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


	#region states

	public void OnDestroy()
	{
		SoundManager.StopSFX();
		Time.timeScale = 1;
		if(Blackboard.eagalBossDeadCount > 1)
			SoundManager.PlayImmediately(HUDManager.instance.bossEnd);
		MainCharacterController.OnPlayerDead -= HandleOnPlayerDead;
	}

	
	[System.Serializable]
	public class RunAttackState : FSM.FSMState
	{
		private BossEagalController myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as BossEagalController;
			
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			if(myOwner.Fsm.GetPreviousState() != myOwner.backingState)
			{
				myOwner.animator.SetBool("RunAttack", true);	
			}

			myOwner.loop = SoundManager.PlaySFX("TwinsEagalsNunchakuLOOP",true,0,0.05f);
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
			if(distance < 5)
			{
				myOwner.rigid.velocity = Vector2.zero;
				//Me quedo parado con la animacion ejecutandose
				myOwner.animator.SetBool("RunAttack", false);	
				float num = Random.Range(0,100);


				if(num <= 40)
					ChangeState(myOwner.attack1State);
				else if(num > 40 && num <= 70)
					ChangeState(myOwner.attack2State);
				else
					ChangeState(myOwner.attack3State);
			}
		}
		
		public override void Exit (FSM.FSMState _nextState)
		{
		}
		
	}
	
	[System.Serializable]
	public class FightingState : FSM.FSMState
	{
		
		private BossEagalController myOwner;

		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as BossEagalController;
			
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			
			myOwner.rigid.velocity = Vector2.zero;
			myOwner.rigid.angularVelocity = 0;
			myOwner.loop.Stop();
			if(!myOwner.isSkillOn)
				Attack();
		}
		
		public override void Update ()
		{
			base.Update ();
		}
		
		public override void Exit (FSM.FSMState _nextState)
		{
			base.Exit (_nextState);
		}
		
		private void Attack()
		{	
			if(myOwner.mainCharacter.GetComponent<LifeModule>().currentLife > 0)
				myOwner.mainCharacter.GetComponent<LifeModule>().DoDamage(myOwner.damage);	

			ChangeState(myOwner.laughtState);
		}
		
	}
	


	// Cuando es golpeado
	[System.Serializable]
	public class HittingState : FSM.FSMState
	{
		
		private BossEagalController myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as BossEagalController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			myOwner.loop.Stop();
			myOwner.animator.SetTrigger("Hit");
			SoundManager.PlaySFX("TwinsEagalsHit");
			Vector2 right = myOwner.transform.TransformDirection(Vector2.right);
			myOwner.rigid.velocity = Vector2.zero;
			myOwner.rigid.AddForce(new Vector2( right.x * 5, 0), ForceMode2D.Impulse);
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
		
		private BossEagalController myOwner;
		private AudioSource laughloop;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as BossEagalController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			
			myOwner.animator.SetBool("Laught",true);
			laughloop = SoundManager.PlaySFX("TwinsEagalsLaugh", true,0,0.3f);
			myOwner.loop.Stop();
			myOwner.rigid.velocity = Vector2.zero;
			myOwner.rigid.angularVelocity = 0;
			
		}
		
		public override void Exit (FSM.FSMState _nextState)
		{
			base.Exit (_nextState);
			myOwner.animator.SetBool("Laught", false);
			laughloop.Stop();
		}
	}

	[System.Serializable]
	public class DeathState : FSM.FSMState
	{
		
		private BossEagalController myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as BossEagalController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			myOwner.loop.Stop();
			Rigidbody2D rigid = myOwner.rigid ;
			myOwner.animator.SetTrigger("Dead");
			if(Blackboard.eagalBossDeadCount == 0)
				SoundManager.PlaySFX("TwinsEagalsDie");
			else
				SoundManager.PlaySFX("TwinsEagalsDieFinal");

			Vector2 right = 	myOwner.transform.TransformDirection(Vector2.right);

			if(Blackboard.eagalBossDeadCount == 1)
			{
				Time.timeScale = 0.3f;
			}
			rigid.velocity = Vector2.zero;
			rigid.AddForce(new Vector2( right.x * 3, right.y + 6), ForceMode2D.Impulse);
			rigid.gravityScale = 3;
			foreach(SpriteRenderer sprite in myOwner.GetComponentsInChildren<SpriteRenderer>())
				sprite.DOFade(0,2);

			if(OnEagalDead != null)
				OnEagalDead();

			//ACHIEVE
			if(Application.platform == RuntimePlatform.Android)
			{
				GooglePlayManager.Instance.UnlockAchievementById(Achievements.GEMELOS);
			}
			else if( Application.platform == RuntimePlatform.IPhonePlayer)
			{
				GameCenterManager.SubmitAchievement(100f, Achievements.GEMELOS);
			}


			Destroy(myOwner.gameObject, 2f);
		}
		
	}


	//Cuando entro en este estado es porque me han golpeado y me voy para atrás
	[System.Serializable]
	public class BackingState : FSM.FSMState
	{
		
		private BossEagalController myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as BossEagalController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			//Si tiene la rotacion mayor que 0 lo giro y al revés
			myOwner.Flip(myOwner.transform);
			myOwner.loop = SoundManager.PlaySFX("TwinsEagalsNunchakuLOOP",true,0,0.05f);
			
		}

		public override void Update ()
		{
			base.Update ();


			if(myOwner.transform.localRotation.eulerAngles.y > 100)
				myOwner.rigid.velocity =  new Vector2(myOwner.velocity * 2, myOwner.rigid.velocity.y);
			else
				myOwner.rigid.velocity =  new Vector2(-myOwner.velocity * 2, myOwner.rigid.velocity.y);
			
			//este es el primero estado, compruebo que está a una distancia larga del personaje, cuando llegue a esta distancia se queda parado y hace un ataque aleatorio
			
			float distance = Vector2.Distance(myOwner.transform.position, myOwner.mainCharacter.transform.position);
			if(distance > 8)
			{
				myOwner.Flip(myOwner.transform);
				myOwner.GetComponent<LifeModule>().invulnerable = false;
				ChangeState(myOwner.runAttackState);
			}
		}

		public override void Exit (FSM.FSMState _nextState)
		{
			myOwner.loop.Stop();
		}
		
	}

	[System.Serializable]
	public class ModeAttack1 : FSM.FSMState
	{
		private float couldownAttack;
		private bool firstTime;

		private BossEagalController myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as BossEagalController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			couldownAttack = 0;
			myOwner.velocity += .5f;
			firstTime = true;
		}

		public override void Update ()
		{
			base.Update ();
			
			couldownAttack += Time.deltaTime;


			//Espero 2 segundos y hago el ataque correspondiente
			if(couldownAttack >= .5f)
			{
				//si es la primera vezque entro, activo la animación de andar
				if(firstTime)
				{
					myOwner.animator.SetBool("RunAttack", true);	
					firstTime = false;
				}

				float distance = Vector2.Distance(myOwner.transform.position, myOwner.mainCharacter.transform.position);
				if(distance < myOwner.distanceToAttack)
				{
					ChangeState(myOwner.fightingState);
				}
				if(!myOwner.isSkillOn)
					Attack();
			}
		}

		void Attack()
		{
			if(myOwner.transform.localRotation.eulerAngles.y > 100)
				myOwner.rigid.velocity =  new Vector2(myOwner.velocity, myOwner.rigid.velocity.y);
			else
				myOwner.rigid.velocity =  new Vector2(-myOwner.velocity, myOwner.rigid.velocity.y);
		}
	}

	[System.Serializable]
	public class ModeAttack2 : FSM.FSMState
	{
		private float couldownAttack;
		private bool firstTime;
		
		private BossEagalController myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as BossEagalController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			
			couldownAttack = 0;
			firstTime = true;
		}
		
		public override void Update ()
		{
			base.Update ();
			
			couldownAttack += Time.deltaTime;
			
			
			//Espero 2 segundos y hago el ataque correspondiente
			if(couldownAttack >= .5f)
			{
				//si es la primera vezque entro, activo la animación de andar
				if(firstTime)
				{
					myOwner.animator.SetBool("RunAttack", true);	
					firstTime = false;
				}

				float distance = Vector2.Distance(myOwner.transform.position, myOwner.mainCharacter.transform.position);
				if(distance < myOwner.distanceToAttack)
				{
					ChangeState(myOwner.fightingState);
				}
				else
				{
					if(!myOwner.isSkillOn)
						Attack();
				}
			}
		}
		
		void Attack()
		{
			if(myOwner.transform.localRotation.eulerAngles.y > 100)
				myOwner.rigid.velocity =  new Vector2(myOwner.velocity * 3, myOwner.rigid.velocity.y);
			else
				myOwner.rigid.velocity =  new Vector2(-myOwner.velocity * 3, myOwner.rigid.velocity.y);
		}
		
	}

	[System.Serializable]
	public class ModeAttack3 : FSM.FSMState
	{
		
		private float couldownAttack, couldownStopped, timeStoped;
		private bool firstTime;
		
		private BossEagalController myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as BossEagalController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			
			couldownAttack = 0;
			couldownStopped = 0;
			timeStoped = (float)Random.Range(.5f,1f);
			firstTime = true;
		}
		
		public override void Update ()
		{
			base.Update ();
			
			couldownAttack += Time.deltaTime;
			
			
			//Espero 2 segundos y hago el ataque correspondiente
			if(couldownAttack >= 2)
			{
				//si es la primera vezque entro, activo la animación de andar
				if(firstTime)
				{
					myOwner.animator.SetBool("RunAttack", true);	
					firstTime = false;
				}

				float distance = Vector2.Distance(myOwner.transform.position, myOwner.mainCharacter.transform.position);
				//Si la distancia es menor de 4, paro al personaje y hago que es espere de 1 a 3 segundos, después ataca
				if(distance < 4)
				{

					couldownStopped += Time.deltaTime;

					//han pasado x segundos y puedo pararme?
					if(couldownStopped >= timeStoped)
					{
						myOwner.animator.SetBool("RunAttack", true);	
						if(!myOwner.isSkillOn)
							Attack();
					}
					else
					{
						myOwner.rigid.velocity = Vector2.zero;
						myOwner.animator.SetBool("RunAttack", false);
					}
				}
				else
				{
					if(!myOwner.isSkillOn)
						Attack();
				}

				//y si la distancia de ataque es la del boss, pues voy al estado de ataque
				if(distance < myOwner.distanceToAttack)
				{
					ChangeState(myOwner.fightingState);
				}
			
			}
		}
		
		void Attack()
		{
			if(myOwner.transform.localRotation.eulerAngles.y > 100)
				myOwner.rigid.velocity =  new Vector2(5, myOwner.rigid.velocity.y);
			else
				myOwner.rigid.velocity =  new Vector2(-5, myOwner.rigid.velocity.y);
		}
		
	}



	#endregion
}
