using UnityEngine;
using System.Collections;
using DG.Tweening;

public class EnemyController : MonoBehaviour {

	public delegate void HandleOnDead();
	public static event HandleOnDead OnEnemyDead;

	public float velocity;
	public float damage = 1;
	public float distanceToAttack = 1;
	protected Animator animator;
	protected Rigidbody2D rigid;
	protected BoxCollider2D box;
	protected SpriteRenderer sprite;
	public Transform bloodParticle;

	[HideInInspector]
	public float startVelocity;

	[HideInInspector]
	public Transform mainCharacter;
	
	#region FSM
	
	public FSM	Fsm{get;set;}

	public IdleState 		idleState;

	public MovingState 		movingState;
	
	public DeathState 		deathState;
	
	
	
	#endregion

	public virtual void Awake()
	{
		Fsm = FSM.CreateFSM(this);
		Fsm.AddFSMState (idleState, movingState, deathState);

		animator	= GetComponent<Animator>();
		rigid		= GetComponent<Rigidbody2D>();
		sprite		= GetComponent<SpriteRenderer>();
		box			= GetComponent<BoxCollider2D>();
		mainCharacter = GameObject.FindGameObjectWithTag("MainCharacter").transform;


	}

	public virtual void Start()
	{
		startVelocity = velocity;

	}

	public virtual void Attack()
	{
		if(mainCharacter.GetComponent<LifeModule>().currentLife > 0)
			mainCharacter.GetComponent<LifeModule>().DoDamage(damage);	
	}

	#region states
	

	[System.Serializable]
	public class IdleState : FSM.FSMState
	{
		private EnemyController myOwner;

		private Vector3 pos;
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as EnemyController;
			
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			
			myOwner.animator.SetTrigger("Idle");

			//When is idle, velocity is 0 and the enemies make a movement like Street Or Rage ñaaaaa
			myOwner.rigid.velocity =  Vector2.zero;
			myOwner.rigid.angularVelocity = 0;
			pos = myOwner.transform.position;
		}
		
		public override void Update ()
		{
			base.Update ();


			myOwner.transform.position = pos;

		}
		
		public override void Exit (FSM.FSMState _nextState)
		{
			
		}
		
	}

	[System.Serializable]
	public class MovingState : FSM.FSMState
	{
		private EnemyController myOwner;

		private bool flag;

		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as EnemyController;
			
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			myOwner.animator.SetTrigger("Run");

			flag = true;
		}
		
		public override void Update ()
		{
			base.Update ();

			if(flag)
			{
				if(myOwner.transform.localRotation.eulerAngles.y > 100)
					myOwner.rigid.velocity =  new Vector2(myOwner.velocity, myOwner.rigid.velocity.y);
				else
					myOwner.rigid.velocity =  new Vector2(-myOwner.velocity, myOwner.rigid.velocity.y);

				float distance = Vector2.Distance(myOwner.transform.position, myOwner.mainCharacter.transform.position);
				if(distance < myOwner.distanceToAttack)
				{
					flag = false;
					myOwner.rigid.velocity = Vector2.zero;
					myOwner.animator.SetTrigger("Attack");	
					
				}
			}
		}
		
		public override void Exit (FSM.FSMState _nextState)
		{
			
		}
		
	}

	[System.Serializable]
	
	public class DeathState : FSM.FSMState
	{
		
		private EnemyController myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as EnemyController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			
			if(OnEnemyDead != null)
				OnEnemyDead();

			myOwner.animator.SetTrigger("Dead");
			myOwner.box.enabled = false;
			//myOwner.GetComponent<CircleCollider2D>().enabled = false;
			
			Vector2 right = 	myOwner.transform.TransformDirection(Vector2.right);
			//esto es para hacer que parezca que caen por delante
			//myOwner.transform.DOScale(new Vector2(2,2),2);
			//myOwner.GetComponent<SpriteRenderer>().sortingOrder = 20;
			//rigid.constraints = false;
			myOwner.rigid.velocity 	= Vector2.zero;
			myOwner.rigid.mass		= 1;
			myOwner.rigid.AddForce(new Vector2( right.x * Random.Range(6,10), right.y +Random.Range(10,20)), ForceMode2D.Impulse);
			myOwner.rigid.gravityScale = 3;
			myOwner.sprite.DOFade(0,2);
			Destroy(myOwner.gameObject, 2);
		}
		
	}
	
	
	
	#endregion
}
