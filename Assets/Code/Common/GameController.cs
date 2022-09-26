using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Google2u
{
	public class GameController : MonoBehaviour {

		public enum mode
		{
			Normal,
			Hard,
			Arcade
		}

		public mode selectedMode;

		public Spawner spawnLeft, spawnRight;
		public int currentWave;

		public AutoDestroy BossPresentation;

		public bool pauseSpawnWaves;

		NormalMode 	normalMode;
		HardMode	hardMode;
		ArcadeMode 	arcadeMode;

		private bool fightingBoss;
		//Esto se utiliza para aumentar la dificultad en el modo arcade, para que vayan saliendo más rápido los enemigos
		private float lessTime;


		private static GameController instance;
		public static GameController Instance 
		{
			get
			{
				return instance;
			}
			
			private set{
				instance = value;
			}
		}

		void Awake()
		{
			instance = this;
			Blackboard.totalEnemiesDead 	= 0;
			Blackboard.eagalBossDeadCount 	= 0;
		}

		void OnEnable()
		{
			MainCharacterController.OnPlayerDead 	+= HandleOnPlayerDead;
			EnemyController.OnEnemyDead 			+= HandleOnEnemyDead;
			BossEagalController.OnEagalDead 		+= HandleOnEagalDead;
			BossMitsuController.OnMitsuDead 		+= HandleOnMitsuDead;
			LoseCanvas.OnClickContinue 				+= ContinueGame;
		}

		void Start () 
		{
			pauseSpawnWaves = true;

			GameObject statsdbobj = GameObject.Find("Google2uDatabase");	
			if ( statsdbobj != null )
			{
				switch(selectedMode)
				{
				case mode.Normal:
					normalMode = statsdbobj.GetComponent<NormalMode>();
					StartCoroutine("SpawnNormalMode");
					break;

				case mode.Hard:
					hardMode = statsdbobj.GetComponent<HardMode>();
					StartCoroutine("SpawnHardMode");
					break;

				case mode.Arcade:
					arcadeMode = statsdbobj.GetComponent<ArcadeMode>();
					StartCoroutine("SpawnArcadeMode");
					break;
				}
			}
				

			fightingBoss 	= false;
			lessTime		= 0;


			

		}

		/// <summary>
		/// Handles the on eagal dead.
		/// </summary>
		void HandleOnEagalDead ()
		{
			Blackboard.eagalBossDeadCount++;
			//Si he matado a los dos gemelos...
			if(Blackboard.eagalBossDeadCount == 2)
			{
				StartCoroutine(Blackboard.Instance.ChangeLevel("Level2"+selectedMode.ToString(),3));
			}
		}

		/// <summary>
		/// Handles the on mitsu dead.
		/// </summary>
		void HandleOnMitsuDead ()
		{
			StartCoroutine(Blackboard.Instance.ChangeLevel("Level3"+selectedMode.ToString()));
		}

		/// <summary>
		/// Handles the on enemy dead.
		/// </summary>
		void HandleOnEnemyDead ()
		{
			//change the time to 0 and then change to 1...this is an effect 
//			Time.timeScale = 0;
//			Blackboard.Instance.StartCoroutine(Blackboard.Instance.ChangeTime(1f,.05f) );
		}

		/// <summary>
		/// Handles the on player dead.
		/// </summary>
		void HandleOnPlayerDead ()
		{
			pauseSpawnWaves = true;
			foreach(EnemyController enemy in FindObjectsOfType(typeof(EnemyController)))
			{
				enemy.Fsm.ChangeState(enemy.idleState);

				//Esto por si les quiero hacer que hagan un fade cuando el personaje muere
//				foreach(SpriteRenderer sprite in enemy.GetComponentsInChildren<SpriteRenderer>())
//				{
//					sprite.DOFade(0,1);
//				}

			}
		}

		IEnumerator SpawnBoss(string nameEnemy)
		{
			if(!pauseSpawnWaves)
			{
				fightingBoss = true;
				//Si el enemigo es un boss, espero el tiempo de la siguiente celda y después recojo el nombre del boss
				pauseSpawnWaves = true;

				Instantiate(BossPresentation);
				SoundManager.PlayImmediately(HUDManager.instance.bossIntro);
				if(nameEnemy == "TwinsEagals")
				{
					//Esperamos hasta que la presentación acabe
					yield return new WaitForSeconds(2);
					spawnLeft.InstanceBoss(nameEnemy);
				}
				if(nameEnemy == "BossMitsu")
				{
					//Esperamos hasta que la presentación acabe
					yield return new WaitForSeconds(2);
					spawnLeft.InstanceBoss(nameEnemy);
				}
				SoundManager.PlayImmediately(HUDManager.instance.bossTheme, true);
			}
			else
			{
				yield return new WaitForSeconds(1);
					StartCoroutine("SpawnBoss", nameEnemy);
			}
		}

/// <summary>
/// Spawns the normal mode.
/// </summary>
		IEnumerator SpawnNormalMode()
		{
			int num = normalMode.Rows[currentWave].Length;
			float timeToNext;
			string nameEnemy = normalMode.GetRow("WAVE_"+currentWave)[0]; //Recojo el nombre del enemigo 
			if(nameEnemy != "BOSS")
			{
				fightingBoss = false;
				for(int i = 0; i < num; i=i+2) //Aumento de 2 en 2 para poder coger el nombre del enemigo
				{
					if(!pauseSpawnWaves)
					{
						nameEnemy = normalMode.GetRow("WAVE_"+currentWave)[i];
						timeToNext = float.Parse( normalMode.GetRow("WAVE_"+currentWave)[i+1]); //Recojo el tiempo que tarda el siguiente enemigo en aparecer
						if( Random.Range(0,100) <= 50)
						{
							spawnLeft.InstanceEnemy(nameEnemy);
						}
						else
						{
							spawnRight.InstanceEnemy(nameEnemy);
						}
						yield return new WaitForSeconds(timeToNext);
					}
					else
					{
						while(pauseSpawnWaves)
							yield return new WaitForFixedUpdate();
					}
				}
				currentWave++;
				//Espero un segundo hasta que empieza la siguiente oleada
				yield return new WaitForSeconds(1);
				StartCoroutine("SpawnNormalMode");
			}
			else
			{
				yield return new WaitForSeconds(float.Parse( normalMode.GetRow("WAVE_"+currentWave)[1])); //La celda 1 corresponde al tiempo que espero hasta que salga el boss
				nameEnemy = normalMode.GetRow("WAVE_"+currentWave)[2]; //La celda 2 siempre corresponde al nombre del boss
				StartCoroutine("SpawnBoss", nameEnemy);
			}
		}

		IEnumerator SpawnHardMode()
		{
			int num = hardMode.Rows[currentWave].Length;
			float timeToNext;
			string nameEnemy = hardMode.GetRow("WAVE_"+currentWave)[0]; //Recojo el nombre del enemigo 
			if(nameEnemy != "BOSS")
			{
				fightingBoss = false;
				for(int i = 0; i < num; i=i+2) //Aumento de 2 en 2 para poder coger el nombre del enemigo
				{
					if(!pauseSpawnWaves)
					{
						nameEnemy = hardMode.GetRow("WAVE_"+currentWave)[i];
						timeToNext = float.Parse( hardMode.GetRow("WAVE_"+currentWave)[i+1]); //Recojo el tiempo que tarda el siguiente enemigo en aparecer
						if( Random.Range(0,100) <= 50)
						{
							spawnLeft.InstanceEnemy(nameEnemy);
						}
						else
						{
							spawnRight.InstanceEnemy(nameEnemy);
						}
						yield return new WaitForSeconds(timeToNext);
					}
					else
					{
						while(pauseSpawnWaves)
							yield return new WaitForFixedUpdate();
					}
				}
				currentWave++;
				//Espero un segundo hasta que empieza la siguiente oleada
				yield return new WaitForSeconds(1);
				StartCoroutine("SpawnHardMode");
			}
			else
			{
				yield return new WaitForSeconds(float.Parse( hardMode.GetRow("WAVE_"+currentWave)[1])); //La celda 1 corresponde al tiempo que espero hasta que salga el boss
				nameEnemy = hardMode.GetRow("WAVE_"+currentWave)[2]; //La celda 2 siempre corresponde al nombre del boss
				StartCoroutine("SpawnBoss", nameEnemy);
			}
		}

		IEnumerator SpawnArcadeMode()
		{
			int num = arcadeMode.Rows[currentWave].Length;
			float timeToNext;
			string nameEnemy = arcadeMode.GetRow("WAVE_"+currentWave)[0]; //Recojo el nombre del enemigo 
			for(int i = 0; i < num; i=i+2) //Aumento de 2 en 2 para poder coger el nombre del enemigo
			{
				if(!pauseSpawnWaves)
				{
					nameEnemy = arcadeMode.GetRow("WAVE_"+currentWave)[i];
					timeToNext = float.Parse( arcadeMode.GetRow("WAVE_"+currentWave)[i+1]); //Recojo el tiempo que tarda el siguiente enemigo en aparecer
					if( Random.Range(0,100) <= 50)
					{
						spawnLeft.InstanceEnemy(nameEnemy);
					}
					else
					{
						spawnRight.InstanceEnemy(nameEnemy);
					}
					yield return new WaitForSeconds(timeToNext - lessTime);
				}
				else
				{
					while(pauseSpawnWaves)
						yield return new WaitForFixedUpdate();
				}
			}
			currentWave++;
			//Espero un segundo hasta que empieza la siguiente oleada
			yield return new WaitForSeconds(1);

			//si llegamos a la oleada 20, volvemos a la oleada 1 y le bajamos la velocidad en la que aparecen
			if(currentWave == 20)
			{
				lessTime -= 0.1f;
				currentWave = 1;
			}

			StartCoroutine("SpawnArcadeMode");
		}

		public void ContinueGame()
		{
			//Si estoy luchando con un boss...
			pauseSpawnWaves = true;
			if(fightingBoss)
			{
				foreach(BossController boss in FindObjectsOfType(typeof(BossController)))
				{
					BossEagalController eagal = boss.GetComponent<BossEagalController>();
					//Cuando resucitamos, si es el boss eagle, le digo que se vaya para atrás
					if(eagal)
					{
						eagal.Fsm.ChangeState(eagal.backingState);
					}
				}
			}
			else
			{
				foreach(EnemyController enemy in FindObjectsOfType(typeof(EnemyController)))
				{
					foreach(SpriteRenderer sprite in enemy.GetComponentsInChildren<SpriteRenderer>())
					{
						sprite.DOFade(1,1);
					}
					enemy.Fsm.ChangeState(enemy.deathState);
				}
			}
		}

		void OnDisable()
		{
			MainCharacterController.OnPlayerDead 	-= HandleOnPlayerDead;
			EnemyController.OnEnemyDead 			-= HandleOnEnemyDead;
			BossEagalController.OnEagalDead 		-= HandleOnEagalDead;
			BossMitsuController.OnMitsuDead			-= HandleOnMitsuDead;
			LoseCanvas.OnClickContinue 				-= ContinueGame;
		}

		void OnGUI ()
		{
//			if (GUI.Button(new Rect(10, 100, 100, 30), "INVULNERABLE"))
//				mainCharacter.invulnerable = true;
//			if (GUI.Button(new Rect(10, 130, 100, 30), "VULNERABLE"))
//				mainCharacter.invulnerable = false;
//			if (GUI.Button(new Rect(10, 160, 100, 30), "MAGIA 100%"))
//				HUDManager.instance.FsmSkill.ChangeState(HUDManager.instance.skillOn);
		}
	}
}