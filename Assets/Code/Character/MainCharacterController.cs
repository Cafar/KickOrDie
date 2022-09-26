using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class MainCharacterController : MonoBehaviour {

	public delegate void MissHandler();
	public static event MissHandler OnMiss;

	public delegate void DeadHandler();
	public static event DeadHandler OnPlayerDead;


	public LayerMask ignoreLayer;
	public float distanceAim;
	public float couldownSkill;
	public Image PanelWhite;
	public GameObject skillPrefab;

	public Material face;
	public float timeBlock;

	[HideInInspector]
	public float timeDoubleAttack;

	[HideInInspector]
	public float lastHited;

	private LifeModule			lifeModule;
	private Animator			animator;
	private Camera				main;


	#region FSM
	
	public FSM	Fsm{get;private set;}
	
	public IdleState 		idleState;
	
	public ShootingState 	shootingState;

	public MissingState 	missingState;

	public SkillState 		skillState;
	
	public DeathState 		deathState;
	
	
	
	#endregion

	void Start()
	{
		animator			= GetComponent<Animator>();
		lifeModule			= GetComponent<LifeModule>();
		main				= Camera.main;

		//Initialize FSM
		Fsm = FSM.CreateFSM(this);
		Fsm.AddFSMState (idleState, shootingState, skillState, missingState, deathState);
		
		Fsm.ChangeState(idleState);

		//Esto lo hago para ignorar las capas que no quiero que golpeen con el rayo...el simbolo ~ hace que coja lo inverso
		ignoreLayer = ~ignoreLayer;

		//si me hacen daño, lo pongo invencible 
		lifeModule.OnLifeChange += delegate(LifeModule _who, float _currentLife, float _previous, float _percentage) {
			//ejecuto la animacion de dañado y lo hago invulnerable durante 1 segundo
			if(_currentLife < _previous)
				StartCoroutine("Invulnerable");
		};


		lifeModule.OnKill += delegate 
		{
			Fsm.ChangeState(deathState);
		};

		LoseCanvas.OnClickContinue += HandleOnClickContinue;


	}

	void OnDisable()
	{
		LoseCanvas.OnClickContinue -= HandleOnClickContinue;
	}

	void HandleOnClickContinue ()
	{
		Fsm.ChangeState(idleState);
		lifeModule.AddLife(1);
		Blackboard.currentLifeMainCharacter++;
	}

	//BORRAR CUANDO SE TERMINE EL JUEGO; SOLO ES PARA VER EL RAYO 
	void Update()
	{
		timeDoubleAttack += Time.deltaTime;
	}


	//THis method is used on animation event
	public void ManualChangeState(string _state)
	{
		switch(_state)
		{
		case "idle" : Fsm.ChangeState(idleState);
			break;
		case "miss" : Fsm.ChangeState(missingState);
			break;
		}

	}

	public void LeftButtonPush()
	{
		if(Fsm.GetCurrentState() == idleState)
		{
			Fsm.ChangeState(shootingState ,new Hashtable(){{"dir", "left"}});
		}
	}

	public void RightButtonPush()
	{
		if(Fsm.GetCurrentState() == idleState)
		{
			Fsm.ChangeState(shootingState ,new Hashtable(){{"dir", "right"}});
		}
	}

	IEnumerator Invulnerable()
	{
		Camera.main.GetComponent<CameraShake>().shake = .5f;
		SoundManager.PlaySFX("ChHurt");
		lifeModule.invulnerable = true;
		if(lifeModule.currentLife >= 1)
		{
			animator.SetTrigger ("Blink");
			main.GetComponent<CameraFit>().enabled = false;
			DOTween.To(()=> main.orthographicSize, x => main.orthographicSize = x, 6, .3f).SetLoops(2,LoopType.Yoyo).OnComplete(() =>{
				main.GetComponent<CameraFit>().enabled = true;
			});
		}
		yield return new WaitForSeconds(1.5f);
		lifeModule.invulnerable = false;
	}

	#region states
	
	
	[System.Serializable]
	public class IdleState : FSM.FSMState
	{

		private MainCharacterController myOwner;

		public override void Init ()
		{
			base.Init ();

			myOwner = owner as MainCharacterController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			if(myOwner.animator != null)
				myOwner.animator.SetBool("Idle",true);
		}

		
		public override void Update ()
		{
			base.Update ();

			//if i push left, change state to shoot and parameter left
			if(Input.GetKeyDown(KeyCode.LeftArrow))
			{
				ChangeState(myOwner.shootingState ,new Hashtable(){{"dir", "left"}});
			}
			
			if(Input.GetKeyDown(KeyCode.RightArrow))
			{
				ChangeState(myOwner.shootingState ,new Hashtable(){{"dir", "right"}});
			}

		}
		
		public override void Exit (FSM.FSMState _nextState)
		{
			myOwner.animator.SetBool("Idle",false);
		}
		
	}
	
	[System.Serializable]
	public class ShootingState : FSM.FSMState
	{
		
		private MainCharacterController myOwner;
		private float couldownAttack;

		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as MainCharacterController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);



			if(myOwner.Fsm.GetPreviousState() != myOwner.deathState)
				Shoot(_parameters["dir"] == "left");

		}
		
		public override void Update ()
		{
			base.Update ();

			
		}
		
		public override void Exit (FSM.FSMState _nextState)
		{
			base.Exit (_nextState);
			
			myOwner.timeDoubleAttack = 0;
		}
		
		public void Shoot(bool _left)
		{

			Quaternion rot = myOwner.transform.rotation;
			//Roto mi personaje hacia el lado que le de
			if(_left)
				rot.y = 180;
			else
				rot.y = 0;
			
			myOwner.transform.rotation = rot;

			//Compruebo si cuando roto, hay algun enemigo en mi rango
			Vector2 right = myOwner.transform.TransformDirection(Vector2.right);
			RaycastHit2D hit = Physics2D.Raycast(myOwner.transform.position, right,myOwner.distanceAim, myOwner.ignoreLayer);

			//Si no golpeo a nadie, MISS
			if(hit.collider == null)
			{
				ChangeState(myOwner.missingState);
			}
			else if(hit.collider.tag == "Enemy" || hit.collider.tag == "Boss" || hit.collider.tag == "BossFinal")
			{
				myOwner.PanelWhite.DOFade(.5f,.03f).SetLoops(2,LoopType.Yoyo).OnComplete(()=>{
					myOwner.PanelWhite.DOFade(0,0);
				});
				Camera.main.GetComponent<CameraShake>().shake = .1f;
				if(hit.collider.tag == "BossFinal")
				{
					hit.transform.parent.GetComponent<LifeModule>().DoDamage(1);
				}
				else
				{
					hit.transform.GetComponent<LifeModule>().DoDamage(1);
				}
				SoundManager.PlaySFX(SoundManager.LoadFromGroup("ChVoice"));
				SoundManager.PlaySFX(SoundManager.LoadFromGroup("ChKick"));
				if(myOwner.timeDoubleAttack < .1f)
				{
					if(myOwner.lastHited == hit.collider.gameObject.GetInstanceID() && hit.collider.name == "ShieldNinja(Clone)")
					{
						//ACHIEVE
						if(Application.platform == RuntimePlatform.Android)
						{
							GooglePlayManager.Instance.UnlockAchievementById(Achievements.PIERNA_RAPIDA);
						}
						else if( Application.platform == RuntimePlatform.IPhonePlayer)
						{
							GameCenterManager.SubmitAchievement(100f, Achievements.PIERNA_RAPIDA);
						}
						myOwner.animator.SetTrigger("Attack");
					}
					SoundManager.PlaySFX("ChShout");
				}
				else
				{
					myOwner.animator.SetTrigger("Attack");
				}


				myOwner.lastHited = hit.collider.gameObject.GetInstanceID();

				ChangeState(myOwner.idleState);
			}
		}
		
	}

	[System.Serializable]
	public class MissingState : FSM.FSMState
	{
		
		private MainCharacterController myOwner;
		
		public override void Init ()
		{
			base.Init ();

			myOwner = owner as MainCharacterController;
		
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			if(OnMiss != null)
				OnMiss();
			myOwner.animator.SetTrigger("Miss");
			SoundManager.PlaySFX("ChFail");
		}
		
		public override void Update ()
		{
			base.Update ();
			
			
		}
		
		public override void Exit (FSM.FSMState _nextState)
		{
			base.Exit (_nextState);
			
			
		}
	}

	[System.Serializable]
	public class SkillState : FSM.FSMState
	{
		
		private MainCharacterController myOwner;
		public override void Init ()
		{
			base.Init ();
			myOwner = owner as MainCharacterController;
			
			
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			SkillController.OnDestroy += delegate() {
				ChangeState(myOwner.idleState);
			};

			Instantiate(myOwner.skillPrefab);
			
		}

		public override void Exit (FSM.FSMState _nextState)
		{
			base.Exit (_nextState);
			SkillController.OnDestroy -= delegate() {
				ChangeState(myOwner.idleState);
			};
		}

	}

	[System.Serializable]
	public class DeathState : FSM.FSMState
	{
		
		private MainCharacterController myOwner;
		public override void Init ()
		{
			base.Init ();
			myOwner = owner as MainCharacterController;
			
			
			
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			
			myOwner.animator.SetTrigger("Dead");

			if(OnPlayerDead != null)
				OnPlayerDead();

			//Time.timeScale = .5f;
			Camera.main.GetComponent<CameraShake>().shake = .5f;
			SoundManager.PlaySFX("ChDie");
			//myOwner.GetComponent<Rigidbody2D>().isKinematic = false;
			//myOwner.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,10),ForceMode2D.Impulse);

		}
		
	}
	
	
	
	
	#endregion
}
