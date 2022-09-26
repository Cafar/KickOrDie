using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DogController : EnemyController {


	public JumpState 	jumpState;
	public WalkState 	walkState;
	public GoOut	 	goOutState;
	
	
	public Transform shadow;

	public override void Awake()
	{
		//if(Random.Range(0,100) >= 95)
		//	velocity *= 2;
		base.Awake();

		rigid = GetComponent<Rigidbody2D>();

		Fsm.AddFSMState(jumpState, walkState,goOutState);
		Fsm.ChangeState(walkState);

		GetComponent<LifeModule>().OnKill += HandleOnKill;

		GetComponent<LifeModule>().OnLifeChange += HandleOnLifeChange;
	}

	void HandleOnLifeChange (LifeModule _who, float _currentLife, float _previous, float _percentage)
	{
		if(_previous > _currentLife)
		{
			SoundManager.PlaySFX("DogHit");
		}
	}

	void HandleOnKill (GameObject _who)
	{
		SoundManager.PlaySFX("DogDie");
		Fsm.ChangeState(deathState);
	}

	[System.Serializable]
	
	public class JumpState : FSM.FSMState
	{
		
		private DogController myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as DogController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			SoundManager.PlaySFX("DogJump");

			myOwner.animator.SetTrigger("Attack");

			Vector2 right = 	-myOwner.transform.TransformDirection(Vector2.right);
			myOwner.rigid.AddForce(new Vector2(right.x * 10,22), ForceMode2D.Impulse);
			myOwner.rigid.AddTorque (-60 * Time.deltaTime, ForceMode2D.Impulse);
		}

		
		public override void Update ()
		{
			base.Update ();
			float distance = Vector2.Distance(myOwner.transform.position, myOwner.mainCharacter.transform.position);

			myOwner.shadow.position = new Vector2(myOwner.transform.position.x, 0f);
			myOwner.shadow.rotation = Quaternion.identity;

			if(distance < 2)
			{
				myOwner.mainCharacter.GetComponent<LifeModule>().DoDamage(myOwner.damage);
				SoundManager.PlaySFX("DogAttack");
				ChangeState(myOwner.goOutState);
			}
		}
	}

	[System.Serializable]
	
	public class WalkState : FSM.FSMState
	{
		
		private DogController myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as DogController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			
			myOwner.animator.SetTrigger("Run");

		}
		
		
		public override void Update ()
		{
			base.Update ();
			
			if(myOwner.transform.localRotation.eulerAngles.y > 100)
				myOwner.GetComponent<Rigidbody2D>().velocity =  new Vector2(myOwner.velocity, myOwner.GetComponent<Rigidbody2D>().velocity.y);
			else
				myOwner.GetComponent<Rigidbody2D>().velocity =  new Vector2(-myOwner.velocity, myOwner.GetComponent<Rigidbody2D>().velocity.y);
			
			float distance = Vector2.Distance(myOwner.transform.position, myOwner.mainCharacter.transform.position);
			if(distance < myOwner.distanceToAttack)
			{
				ChangeState(myOwner.jumpState);
			}
		}
	}

	[System.Serializable]
	
	public class GoOut : FSM.FSMState
	{
		
		private DogController myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as DogController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			
			myOwner.animator.SetTrigger("Run");
			//myOwner.rigid.AddTorque (120 * Time.deltaTime, ForceMode2D.Impulse);
			Destroy(myOwner.gameObject, 2);
		}
		
		
		public override void Update ()
		{
			base.Update ();

			myOwner.shadow.position = new Vector2(myOwner.transform.position.x, 0);
			myOwner.shadow.rotation = Quaternion.identity;
			
			if(myOwner.transform.localRotation.eulerAngles.y > 100)
				myOwner.GetComponent<Rigidbody2D>().velocity =  new Vector2(8, myOwner.GetComponent<Rigidbody2D>().velocity.y);
			else
				myOwner.GetComponent<Rigidbody2D>().velocity =  new Vector2(-8, myOwner.GetComponent<Rigidbody2D>().velocity.y);
		}
	}
}
