using System;
using UnityEngine;
using UnityEngine.Events;

namespace ExaGames.Common {
	/// <summary>
	/// Lifes system manager by ExaGames.
	/// </summary>
	/// <coauthor>Eduardo Casillas</coauthor>
	/// <coauthor>Nicolás Michelini</coauthor>
	public class LivesManager : MonoBehaviour {
		#region Constants
		/// <summary>
		/// Key to save the number of currently available lives in the player preferences file.
		/// </summary>
		private const string LIVES_SAVEKEY = "ExaGames.Common.Lives";
		/// <summary>
		/// Key to save the recovery start time in the player preferences file.
		/// </summary>
		private const string RECOVERY_TIME_SAVEKEY = "ExaGames.Common.LivesRecoveryTime";
		#endregion

		#region Fields
		/// <summary>
		/// Maximum number of lives.
		/// </summary>
		public int MaxLives = 5;
		/// <summary>
		/// Time to recover one life in minutes.
		/// </summary>
		public double MinutesToRecover = 30D;

		/// <summary>
		/// Event to be called when the number of lives has changed.
		/// </summary>
		public UnityEvent OnLivesChanged;
		/// <summary>
		/// Evento to be called when the time to recover one life has changed.
		/// </summary>
		public UnityEvent OnRecoveryTimeChanged;

		/// <summary>
		/// Current number of available lives.
		/// </summary>
		private int lives;
		/// <summary>
		/// Timestamp from the last life recovery.
		/// </summary>
		private DateTime recoveryStartTime;
		/// <summary>
		/// Time remaining until the next life in seconds.
		/// </summary>
		private double secondsToNextLife;
		/// <summary>
		/// Specifies whether the timer to next life should be calculated.
		/// </summary>
		private bool calculateSteps;
		#endregion

		#region Properties
		/// <summary>
		/// Current number of available lives.
		/// </summary>
		/// <value>Current number of available lives..</value>
		public int Lives { get { return lives; } }
		/// <summary>
		/// Time remaining until the next life in seconds.
		/// </summary>
		/// <value>Time remaining until the next life in seconds..</value>
		public double SecondsToNextLife { get { return secondsToNextLife; } }
		/// <summary>
		/// Gets a value indicating whether lives are at their maximum number.
		/// </summary>
		/// <value><c>true</c> if lives are at their max; otherwise, <c>false</c>.</value>
		public bool HasMaxLives { get { return (lives >= MaxLives); } }
		/// <summary>
		/// Remaining time for next life formatted as mm:ss
		/// </summary>
		/// <value>Remaining time for next life formatted as mm:ss</value>
		public string RemainingTimeString { get { return TimeSpan.FromSeconds(secondsToNextLife).ToString().Substring(3, 5); } }
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="ExaGames.Common.LivesManager"/> class.
		/// </summary>
		public LivesManager() {
			calculateSteps = false;
		}

		#region Unity Behaviour Methods
		/// <summary>
		/// Initializes lives and recoveryStartTime from local persistance if player is comming back, or
		/// with full lives if this is the first time she plays.
		/// </summary>
		private void Awake() {
			if(FindObjectsOfType<LivesManager>().Length > 1 ) {
				Debug.LogError("More than one LivesManager found in scene.");
			}
			if(PlayerPrefs.HasKey(LIVES_SAVEKEY) && PlayerPrefs.HasKey(RECOVERY_TIME_SAVEKEY)) {
				lives = PlayerPrefs.GetInt(LIVES_SAVEKEY);
				recoveryStartTime = new DateTime(long.Parse(PlayerPrefs.GetString(RECOVERY_TIME_SAVEKEY)));
			} else {
				lives = MaxLives;
				recoveryStartTime = DateTime.Now;
			}
		}

		/// <summary>
		/// On start, calculates the number of lives that must be recovered and the remaining seconds for the next life.
		/// </summary>
		private void Start() {
			initTimer();
		}

		/// <summary>
		/// Every frame calculates the next step of the timer for the next life.
		/// </summary>
		private void Update() {
			if(calculateSteps) {
				stepRecoveryTime();
			}
		}

		/// <summary>
		/// When paused, saves the recovery start time to use it when unpaused.
		/// </summary>
		/// <param name="pauseStatus">If set to <c>true</c> pause status.</param>
		private void OnApplicationPause(bool pauseStatus) {
			if(pauseStatus) {
				calculateSteps = false;
				recoveryStartTime = DateTime.Now;
			} else {
				initTimer();
			}
		}

		/// <summary>
		/// On destroy, saves the recovery start time to use it next time the Lives Manager is available 
		/// (on application restart, for example).
		/// </summary>
		private void OnDestroy() {
			PlayerPrefs.SetInt(LIVES_SAVEKEY, lives);
			PlayerPrefs.SetString(RECOVERY_TIME_SAVEKEY, recoveryStartTime.Ticks.ToString());
			try {
				PlayerPrefs.Save();
			} catch(Exception e) {
				Debug.LogError("Could not save LivesManager preferences: " + e.Message);
			}
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Consumes one life if available, and starts counting time for recovery.
		/// </summary>
		/// <returns><c>true</c>, if life was consumed, <c>false</c> otherwise.</returns>
		public bool ConsumeLife() {
			bool result;
			if(lives > 0) {
				result = true;

				// If lifes where full, starts counting time for recovery.
				if(HasMaxLives) {
					recoveryStartTime = DateTime.Now;
					resetSecondsToNextLife();
				}
				lives--;
				informLivesChanged();
			} else {
				result = false;
			}
			return result;
		}

		/// <summary>
		/// Gives one life.
		/// </summary>
		public void GiveOneLife() {
			if(!HasMaxLives) {
				lives++;
				recoveryStartTime = DateTime.Now;
			}
		}

		/// <summary>
		/// Sets the number of available lives to its maximum.
		/// </summary>
		public void FillLives() {
			lives = MaxLives;
			setSecondsToNextLifeToZero();
			informLivesChanged();
			informRecoveryTimeChanged();
		}
		#endregion

		/// <summary>
		/// Initializes the timer for next life.
		/// </summary>
		private void initTimer() {
			secondsToNextLife = calculateLifeRecovery().TotalSeconds;
			calculateSteps = true;
			informRecoveryTimeChanged();
			informLivesChanged();
		}

		#region TimeToNextLife control
		/// <summary>
		/// Calculates the time remaining for the next life, and recovers all possible lives.
		/// </summary>
		/// <returns>Time remaining for the next life.</returns>
		private TimeSpan calculateLifeRecovery() {
			DateTime now = DateTime.Now;
			TimeSpan elapsed = now - recoveryStartTime;
			double minutesElapsed = elapsed.TotalMinutes;
			
			while((!HasMaxLives) && (minutesElapsed >= MinutesToRecover)) {
				lives++;
				recoveryStartTime = DateTime.Now;
				minutesElapsed -= MinutesToRecover;
			}
			
			if(HasMaxLives) {
				return TimeSpan.Zero;
			} else {
				return TimeSpan.FromMinutes(MinutesToRecover - minutesElapsed);
			}
		}

		/// <summary>
		/// Calculates one step in the timer for next life.
		/// </summary>
		private void stepRecoveryTime() {
			if(!HasMaxLives) {
				if(secondsToNextLife > 0) {
					secondsToNextLife -= Time.deltaTime;
					informRecoveryTimeChanged();
				} else {
					GiveOneLife();
					informLivesChanged();
					if(HasMaxLives) {
						setSecondsToNextLifeToZero();
					} else {
						resetSecondsToNextLife();
					}
				}
			}
		}

		/// <summary>
		/// Sets the seconds to next life to zero.
		/// </summary>
		private void setSecondsToNextLifeToZero() {
			secondsToNextLife = 0;
			informRecoveryTimeChanged();
		}

		/// <summary>
		/// Resets the seconds to next life.
		/// </summary>
		private void resetSecondsToNextLife() {
			secondsToNextLife = MinutesToRecover * 60;
			informRecoveryTimeChanged();
		}
		#endregion

		#region Informers
		/// <summary>
		/// Informs on recovery time changed.
		/// </summary>
		private void informRecoveryTimeChanged() {
			OnRecoveryTimeChanged.Invoke();
		}

		/// <summary>
		/// Informs on lives changed.
		/// </summary>
		private void informLivesChanged() {
			OnLivesChanged.Invoke();
		}
		#endregion
	}
}