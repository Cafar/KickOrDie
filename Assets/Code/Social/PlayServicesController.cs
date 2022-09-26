//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//
//public class PlayServicesController : MonoBehaviour {
//	
//	private const string ACHIEVEMENT_PRUEBA = "411539177333";
//
//	private const string LEADERBOARD_ID = "XXXXX";
//
//	private static PlayServicesController instance;
//	public static PlayServicesController Instance 
//	{
//		get
//		{
//			if(instance == null)
//			{
//				string resourcesPrefabPath = "PlayServicesController";
//				// Search in resources folder for this GameObject
//				PlayServicesController managerPrefab = Resources.Load<PlayServicesController>(resourcesPrefabPath);
//				
//				if(managerPrefab == null)
//				{
//					Debug.LogError("[ERROR] Prefab "+resourcesPrefabPath+" not found in Resources directory");
//					return null;
//				}
//				
//				Instance = Instantiate(managerPrefab) as PlayServicesController;
//			}
//			
//			return instance;
//		}
//		
//		private set{
//			instance = value;
//		}
//	}
//
//	void Start () 
//	{
//		//listen for GooglePlayConnection events
//		GooglePlayConnection.ActionPlayerConnected +=  OnPlayerConnected;
//		GooglePlayConnection.ActionPlayerDisconnected += OnPlayerDisconnected;
//		GooglePlayConnection.ActionConnectionResultReceived += ActionConnectionResultReceived;
//		
//		//listen for GooglePlayManager events
//		GooglePlayManager.ActionAchievementUpdated += OnAchievementUpdated;
//		GooglePlayManager.ActionScoreSubmited += OnScoreSubmited;
//		GooglePlayManager.ActionScoresListLoaded += OnScoreUpdated;
//		
//		GooglePlayManager.ActionOAuthTokenLoaded += ActionOAuthTokenLoaded;
//		GooglePlayManager.ActionAvailableDeviceAccountsLoaded += ActionAvailableDeviceAccountsLoaded;
//		GooglePlayManager.ActionAchievementsLoaded += OnAchievmnetsLoadedInfoListner;
//		
//		if(GooglePlayConnection.State == GPConnectionState.STATE_CONNECTED) {
//			//checking if player already connected
//			OnPlayerConnected ();
//		} 
//
//	}
//	
//	private void OnDestroy() {
//		if(!GooglePlayConnection.IsDestroyed) {
//			
//			GooglePlayConnection.ActionPlayerConnected -=  OnPlayerConnected;
//			GooglePlayConnection.ActionPlayerDisconnected -= OnPlayerDisconnected;
//			GooglePlayConnection.ActionConnectionResultReceived -= ActionConnectionResultReceived;
//		}
//		
//		if(!GooglePlayManager.IsDestroyed) {
//			
//			GooglePlayManager.ActionAchievementUpdated -= OnAchievementUpdated;
//			GooglePlayManager.ActionScoreSubmited -= OnScoreSubmited;
//			GooglePlayManager.ActionScoresListLoaded -= OnScoreUpdated;
//			
//			GooglePlayManager.ActionAvailableDeviceAccountsLoaded -= ActionAvailableDeviceAccountsLoaded;
//			GooglePlayManager.ActionOAuthTokenLoaded -= ActionOAuthTokenLoaded;
//			GooglePlayManager.ActionAchievementsLoaded -= OnAchievmnetsLoadedInfoListner;
//		}
//	}
//
//	public void JustConnect()
//	{
//		if(GooglePlayConnection.State != GPConnectionState.STATE_CONNECTED)
//		{
//			GooglePlayConnection.Instance.Connect ();
//		}
//	}
//
//	public void ConncetButtonPress() {
//		if(GooglePlayConnection.State == GPConnectionState.STATE_CONNECTED) {
//
//			GooglePlayConnection.Instance.Disconnect ();
//		} else {
//			GooglePlayConnection.Instance.Connect ();
//		}
//	}
//	
//	private void GetAccs() {
//		GooglePlayManager.Instance.RetrieveDeviceGoogleAccounts();
//	}
//	
//	private void RetrieveToken() {
//		
//		
//		GooglePlayManager.Instance.LoadToken();
//	}
//	
//	
//	private void showLeaderBoardsUI() {
//		GooglePlayManager.Instance.ShowLeaderBoardsUI ();
//	}
//	
//	private void loadLeaderBoards() {
//		if (GooglePlayManager.Instance.GetLeaderBoard(LEADERBOARD_ID).GetCurrentPlayerScore(GPBoardTimeSpan.ALL_TIME, GPCollectionType.FRIENDS) == null) {
//			//listening for load event 
//			GooglePlayManager.ActionLeaderboardsLoaded += OnLeaderBoardsLoaded;
//			GooglePlayManager.Instance.LoadLeaderBoards ();
//		} else {
//
//		}
//	}
//	
//	private void showLeaderBoard() 
//	{
//		JustConnect();
//		GooglePlayManager.Instance.ShowLeaderBoardById (LEADERBOARD_ID);
//	}
//	
//	private void submitScore() 
//	{
////		score++;
////		GooglePlayManager.Instance.SubmitScore (LEADERBOARD_NAME, score);
//	}
//	
//	
//	private void ResetBoard() 
//	{
//		GooglePlayManager.Instance.ResetLeaderBoard(LEADERBOARD_ID);
//		UpdateBoardInfo();
//	}
//	
//	
//	
//	
//	public void showAchievementsUI() 
//	{
//		JustConnect();
//		GooglePlayManager.Instance.ShowAchievementsUI ();
//	}
//	
//	private void loadAchievements() 
//	{
//		GooglePlayManager.ActionAchievementsLoaded += OnAchievementsLoaded;
//		GooglePlayManager.Instance.LoadAchievements ();
//	}
//	
//	private void reportAchievement() 
//	{
//		GooglePlayManager.Instance.UnlockAchievement ("achievement_simple_achievement_example");
//	
//	}
//
//	private void reportAchievementById() 
//	{
//		GooglePlayManager.Instance.UnlockAchievementById (ACHIEVEMENT_PRUEBA);
//		
//	}
//
//	private void incrementAchievement() 
//	{
//		GooglePlayManager.Instance.IncrementAchievementById (ACHIEVEMENT_PRUEBA, 1);
//	}
//	
//	
//	private void revealAchievement() 
//	{
//		GooglePlayManager.Instance.RevealAchievement ("achievement_hidden_achievement_example");
//	}
//	
//	private void ResetAchievement() {
//		GooglePlayManager.Instance.ResetAchievement(ACHIEVEMENT_PRUEBA);
//
//	}
//	
//	private void ResetAllAchievements() {
//		GooglePlayManager.Instance.ResetAllAchievements();
//		
//	}
//	
//	
//	private void OpenInbox() {
//		GooglePlayManager.Instance.ShowRequestsAccepDialog();
//	}
//	
//	
//	
//	
//	public void clearDefaultAccount() {
//		GooglePlusAPI.Instance.ClearDefaultAccount();
//	}
//	
//	
//	void FixedUpdate() {
//
//	}
//	
//	
//	public void RequestAdvertisingId() {
//		GooglePlayUtils.ActionAdvertisingIdLoaded += ActionAdvertisingIdLoaded;
//		GooglePlayUtils.Instance.GetAdvertisingId();
//	}
//	
//	
//	
//	
//	//--------------------------------------
//	// EVENTS
//	//--------------------------------------
//	
//	private void ActionAdvertisingIdLoaded (GP_AdvertisingIdLoadResult res) {
//		GooglePlayUtils.ActionAdvertisingIdLoaded -= ActionAdvertisingIdLoaded;
//		
//		
//	}
//	
//	private void OnAchievmnetsLoadedInfoListner(GooglePlayResult res) {
//		GPAchievement achievement = GooglePlayManager.Instance.GetAchievement(ACHIEVEMENT_PRUEBA);
//		
//
//	}
//	
//	private void OnAchievementsLoaded(GooglePlayResult result) {
//		GooglePlayManager.ActionAchievementsLoaded -= OnAchievementsLoaded;
//
//		
//	}
//	
//	private void OnAchievementUpdated(GP_AchievementResult result) {
//
//	}
//	
//	
//	
//	private void OnLeaderBoardsLoaded(GooglePlayResult result) {
//
//		
//	}
//	
//	private void UpdateBoardInfo() {
//
//	}
//	
//	private void OnScoreSubmited(GP_LeaderboardResult result) {
//
//	}
//	
//	private void OnScoreUpdated(GooglePlayResult res) {
//		UpdateBoardInfo();
//	}
//	
//	
//	
//	private void OnPlayerDisconnected() {
//
//	}
//	
//	private void OnPlayerConnected() {
//
//	}
//	
//	private void ActionConnectionResultReceived(GooglePlayConnectionResult result) {
//		
//		if(result.IsSuccess) {
//			Debug.Log("Connected!");
//		} else {
//			Debug.Log("Cnnection failed with code: " + result.code.ToString());
//		}
//	}
//			
//	private void ActionAvailableDeviceAccountsLoaded(List<string> accounts) {
//		string msg = "Device contains following google accounts:" + "\n";
//		foreach(string acc in GooglePlayManager.Instance.deviceGoogleAccountList) {
//			msg += acc + "\n";
//		} 
//		
//		AndroidDialog dialog = AndroidDialog.Create("Accounts Loaded", msg, "Sign With Fitst one", "Do Nothing");
//		dialog.ActionComplete += SighDialogComplete;
//		
//	}
//	
//	private void SighDialogComplete (AndroidDialogResult res) {
//		if(res == AndroidDialogResult.YES) {
//			GooglePlayConnection.Instance.Connect(GooglePlayManager.Instance.deviceGoogleAccountList[0]);
//		}
//		
//	}
//	
//	
//	
//	private void ActionOAuthTokenLoaded(string token) 
//	{
//		
//		AN_PoupsProxy.showMessage("Token Loaded", GooglePlayManager.Instance.loadedAuthToken);
//	}
//	
//	
//
//}
