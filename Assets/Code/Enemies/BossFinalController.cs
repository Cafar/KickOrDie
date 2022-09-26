using UnityEngine;
using System.Collections;
using DG.Tweening;


public class BossFinalController : MonoBehaviour {

	public AudioClip fxDead;

	public Transform rightHand, leftHand, head;

	public float timeStuned = 1;

	public float attackVelocity = 5;

	public float distanceToAttack = 5;

	private bool isSkillOn = false;

	private Animator animator;

	public FSM	Fsm{get;set;}

	[HideInInspector]
	public Transform mainCharacter;

	[HideInInspector]
	public LifeModule lifeModule;

	[HideInInspector]
	public bool attackFromBack;
	

	#region FSM

	public PrepareAttack 		prepareAttack;
	
	public Presentation 		presentation;

	public RightAttack	 		rightAttack;

	public LeftAttack	 		leftAttack;
	
	public DoubleAttack 		doubleAttack;

	public Damaged			 	damaged;

	public DeadState			deadState;

	public IdleState			idleState;
	
	
	#endregion

	void Awake()
	{
		animator 	= GetComponent<Animator>();
		mainCharacter = GameObject.FindGameObjectWithTag("MainCharacter").transform;
		lifeModule		= GetComponent<LifeModule>();

	}

	public void Start()
	{

		Fsm = FSM.CreateFSM(this);
		
		Fsm.AddFSMState (idleState,prepareAttack, presentation, rightAttack, leftAttack, doubleAttack, damaged, deadState);
		Fsm.ChangeState(idleState);
		animator.enabled = false;

		SkillController.OnInstantiate += delegate() {
			isSkillOn = true;
		};

		SkillController.OnDestroy += delegate() {
			isSkillOn = false;
		};

		MainCharacterController.OnPlayerDead += HandleOnPlayerDead;

		GetComponent<LifeModule>().OnKill += HandleOnKill;
		GetComponent<LifeModule>().OnLifeChange += HandleOnLifeChange;

		leftHand.FindChild("Sprite").GetComponent<SpriteRenderer>().enabled = false;
		rightHand.FindChild("Sprite").GetComponent<SpriteRenderer>().enabled = false;
		head.gameObject.SetActive(false);


	}
	
	void HandleOnPlayerDead ()
	{
		Fsm.ChangeState(prepareAttack);
	}

	//Cuando me hacen daño, espero timeStuned y cambio de estado a backing
	void HandleOnLifeChange (LifeModule _who, float _currentLife, float _previous, float _percentage)
	{
		if(_currentLife != 0)
		{

			animator.SetTrigger("Damaged");
			animator.SetTrigger("Blink");
			attackVelocity += 5;
			if(attackVelocity >= 20)
				attackVelocity = 20;

			Fsm.ChangeState(damaged);
		}
	}

	/// <summary>
	/// Handles the on kill.
	/// </summary>
	/// <param name="_who">_who.</param>
	void HandleOnKill (GameObject _who)
	{
		Fsm.ChangeState(deadState);
	}

	/// <summary>
	/// Changes the state.
	/// </summary>
	public void ChangeStateAnimator()
	{
		Fsm.ChangeState(prepareAttack);
	}

	IEnumerator ChangeToPresentation()
	{	
		yield return new WaitForSeconds(1);
		animator.enabled = true;
		Fsm.ChangeState(presentation);
		yield return new WaitForSeconds(.5f);
		SoundManager.PlaySFX("BossFinalLaugh");
		leftHand.FindChild("Sprite").GetComponent<SpriteRenderer>().enabled = true;
		rightHand.FindChild("Sprite").GetComponent<SpriteRenderer>().enabled = true;
		head.gameObject.SetActive(true);

	}

	public void OnDestroyEnemy()
	{
		StartCoroutine(Blackboard.Instance.ChangeLevel("Credits"));
	}

	void Update()
	{
		if(isSkillOn)
		{
			Fsm.ChangeState(prepareAttack);
		}
	}


	public void Attack()
	{
		animator.SetBool("Idle", false);

		if(lifeModule.currentLife == 2 || lifeModule.currentLife == 8)
		{
			SoundManager.PlaySFX("BossFinalLaugh");
			animator.SetTrigger("DoubleAttack");
		}
		else
		{
			SoundManager.PlaySFX("BossFinalShout");
			if(Random.Range(0,100) > 50)
			{
				animator.SetTrigger("LeftAttack");
			}
			else
			{
				animator.SetTrigger("RightAttack");
			}
		}
	}

	public void AttackLeft()
	{
		Fsm.ChangeState(leftAttack);
	}

	public void AttackRight()
	{
		Fsm.ChangeState(rightAttack);	
	}

	public void AtackDouble()
	{
		Fsm.ChangeState(doubleAttack);
	}
	#region states

	public void OnDestroy()
	{
		Time.timeScale = 1;
		MainCharacterController.OnPlayerDead -= HandleOnPlayerDead;
	}
	
	[System.Serializable]
	public class IdleState : FSM.FSMState
	{

		private BossFinalController myOwner;


		public override void Init ()
		{
			base.Init ();

			myOwner = owner as BossFinalController;
		}

		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
	
		}

		public override void Update ()
		{
			base.Update ();

			if(Input.GetButtonDown("Fire1"))
			{
				myOwner.StartCoroutine(myOwner.ChangeToPresentation());
			}


		}

		public override void Exit (FSM.FSMState _nextState)
		{
			base.Exit (_nextState);

			myOwner.animator.SetBool("Idle", false);
		}
	}


	[System.Serializable]
	public class PrepareAttack : FSM.FSMState
	{
		
		private BossFinalController myOwner;

		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as BossFinalController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			myOwner.animator.SetBool("Idle", true);
			SoundManager.PlaySFX(SoundManager.LoadFromGroup("Boss3Voices"));
			myOwner.Invoke("Attack", Random.Range(3,6));
			
		}

		public override void Update ()
		{
			base.Update ();

		}
		
		public override void Exit (FSM.FSMState _nextState)
		{
			base.Exit (_nextState);

			myOwner.animator.SetBool("Idle", false);
		}
	}


	[System.Serializable]
	public class Presentation : FSM.FSMState
	{
		
		private BossFinalController myOwner;
		private AudioSource loop;

		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as BossFinalController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			Camera.main.GetComponent<CameraShake>().shake = 4f;

			loop = SoundManager.PlaySFX("BossFinalEarthquakeLOOP",true,0,0.1f);
		}

		
		public override void Exit (FSM.FSMState _nextState)
		{
			base.Exit (_nextState);

			HUDManager.instance.FsmSkill.ChangeState(HUDManager.instance.skillOff);
			loop.Stop();
		}
	}

	[System.Serializable]
	public class RightAttack : FSM.FSMState
	{
		
		private BossFinalController myOwner;
		private bool flag;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as BossFinalController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			//myOwner.animator.enabled = false;
			flag = true;
		}

		public override void Update ()
		{
			base.Update ();


			float distance = Vector2.Distance(myOwner.rightHand.position, myOwner.mainCharacter.transform.position);
			if(distance < myOwner.distanceToAttack)
			{
				if(flag)
				{
					SoundManager.PlaySFX("BossFinalAttack");
					flag = false;
					if(myOwner.mainCharacter.GetComponent<LifeModule>().currentLife > 0)
						myOwner.mainCharacter.GetComponent<LifeModule>().DoDamage(1);	
				}
			}
			else
			{
				myOwner.rightHand.Translate(-Vector2.right * Time.deltaTime * myOwner.attackVelocity);
			}
		}
		
		public override void Exit (FSM.FSMState _nextState)
		{
			base.Exit (_nextState);
			myOwner.animator.enabled = true;
		}
	}

	[System.Serializable]
	public class LeftAttack : FSM.FSMState
	{
		
		private BossFinalController myOwner;
		
		private bool flag;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as BossFinalController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			flag = true;
		}
		
		public override void Update ()
		{
			base.Update ();
			float distance = Vector2.Distance(myOwner.leftHand.position, myOwner.mainCharacter.transform.position);
			if(distance < myOwner.distanceToAttack)
			{
				if(flag)
				{
					SoundManager.PlaySFX("BossFinalAttack");
					flag = false;
					if(myOwner.mainCharacter.GetComponent<LifeModule>().currentLife > 0)
						myOwner.mainCharacter.GetComponent<LifeModule>().DoDamage(1);	
				}
			}
			else
			{
				myOwner.leftHand.Translate(-Vector2.right * Time.deltaTime * myOwner.attackVelocity);
			}
		}

		public override void Exit (FSM.FSMState _nextState)
		{
			base.Exit (_nextState);
		}
	}

	[System.Serializable]
	public class DoubleAttack : FSM.FSMState
	{
		
		private BossFinalController myOwner;
		private bool flag;
		private int num;
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as BossFinalController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			
			num = 0;
			myOwner.leftHand.DOLocalMove(new Vector2( -6f, -2.36f), .5f).OnComplete(()=>{
				num++;
			}
			);
			myOwner.rightHand.DOLocalMove(new Vector2( 6f, -2.36f), .5f).OnComplete(()=>{
				num++;
			}
			);

			flag = true;
		}

		public override void Update ()
		{
			base.Update ();

			float distance = Vector2.Distance(myOwner.leftHand.position, myOwner.mainCharacter.transform.position);
			if(distance < myOwner.distanceToAttack-1.8f)
			{
				if(flag)
				{
					flag = false;
					if(myOwner.mainCharacter.GetComponent<LifeModule>().currentLife > 0)
						myOwner.mainCharacter.GetComponent<LifeModule>().DoDamage(1);	
				}
			}
			else
			{
				if(num == 2)
				{
					myOwner.leftHand.Translate(-Vector2.right * Time.deltaTime * 20);
					myOwner.rightHand.Translate(-Vector2.right * Time.deltaTime * 20);
				}
			}
		}
		
		public override void Exit (FSM.FSMState _nextState)
		{
			base.Exit (_nextState);
		}
	}

	[System.Serializable]
	public class Damaged : FSM.FSMState
	{
		
		private BossFinalController myOwner;
		private bool flag;
		
		private int num = 0;
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as BossFinalController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			flag = true;
			num = 0;
			SoundManager.PlaySFX("BossFinalHit");
			myOwner.lifeModule.invulnerable = true;

			Camera.main.GetComponent<CameraShake>().shake = 1f;
		}
		
		public override void Update ()
		{
			base.Update ();

			if(flag)
			{
				myOwner.leftHand.DOLocalMove(new Vector2( -7.33f, -2.36f), 1).OnComplete(()=>{
					num++;
				}
				);
				myOwner.rightHand.DOLocalMove(new Vector2( 6.17f, -2.36f), 1).OnComplete(()=>{
					num++;
				}
				);
				flag = false;
			}
			
			if(num == 2)
			{
				ChangeState(myOwner.prepareAttack);
			}
		}

		public override void Exit (FSM.FSMState _nextState)
		{
			base.Exit (_nextState);
			myOwner.lifeModule.invulnerable = false;
		}
	}

	[System.Serializable]
	public class DeadState : FSM.FSMState
	{
		
		private BossFinalController myOwner;
		
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as BossFinalController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			SoundManager.PlaySFX("BossFinalDie");
			myOwner.animator.SetTrigger("Dead");
			Camera.main.GetComponent<CameraShake>().shake = 4f;
			myOwner.Invoke("OnDestroyEnemy", 6);

			PlayerPrefs.SetInt("BossFinalDead",1);

			if(Blackboard.nameLoadedlevel == "Level3Hard")
			{
				//ACHIEVE
				if(Application.platform == RuntimePlatform.Android)
				{
					GooglePlayManager.Instance.UnlockAchievementById(Achievements.MAESTRO);
				}
				else if( Application.platform == RuntimePlatform.IPhonePlayer)
				{
					GameCenterManager.SubmitAchievement(100f, Achievements.MAESTRO);
				}
			}

			//ACHIEVE
			if(Application.platform == RuntimePlatform.Android)
			{
				GooglePlayManager.Instance.UnlockAchievementById(Achievements.COMIENZO);
			}
			else if( Application.platform == RuntimePlatform.IPhonePlayer)
			{
				GameCenterManager.SubmitAchievement(100f, Achievements.COMIENZO);
			}

			//Achieve
			if(Blackboard.currentLifeMainCharacter == 3)
			{
				if(Application.platform == RuntimePlatform.Android)
				{
					GooglePlayManager.Instance.UnlockAchievementById(Achievements.INTOCABLE);
				}
				else if( Application.platform == RuntimePlatform.IPhonePlayer)
				{
					GameCenterManager.SubmitAchievement(100f, Achievements.INTOCABLE);
				}
			}
		}
		
		
		public override void Exit (FSM.FSMState _nextState)
		{
			base.Exit (_nextState);
		}
	}

	#endregion
}
