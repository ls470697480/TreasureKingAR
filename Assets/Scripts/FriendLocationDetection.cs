using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Kudan.AR.Samples;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Unity.Editor;
using System;
using System.Threading.Tasks;

public class FriendLocationDetection : MonoBehaviour
{


	private List<Double> originalLatitude;
	private List<Double> originalLongitude;
	private List<String> userNameList;

	private double currentLongitude;
	private double currentLatitude;

	public TextMesh friendInfoText;


	private double distance;

	//	private bool setOriginalValues = true;

	private Vector3 targetPosition;
	private Vector3 originalPosition;

	private bool startedFirebase = false;

	private static int index;

	private int currentCount;

	private static bool switchFriendClicked = false;
//	private double currentFriendLatitude = 0;
//	private double currentFriendLongitude = 0;
	private Firebase.Auth.FirebaseUser currUser;
	private Firebase.Auth.FirebaseAuth auth;



	//	private float speed = .1f;
	DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

	IEnumerator GetCoordinates()
	{
//		print ("Start StartCoroutine...");

		//while true so this function keeps running once started.
		while (true) {

			// check if user has location service enabled
			if (!Input.location.isEnabledByUser)
				yield break;

			// Start service before querying location
			Input.location.Start (1f,.1f);

			// Wait until service initializes
			int maxWait = 20;
			while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
				yield return new WaitForSeconds (1);
				maxWait--;
			}

			// Service didn't initialize in 20 seconds
			if (maxWait < 1) {
				print ("Timed out");
				yield break;
			}

			// Connection has failed
			if (Input.location.status == LocationServiceStatus.Failed) {
				print ("Unable to determine device location");
				yield break;
			} else {
				// Access granted and location value could be retrieved
				print ("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);

				//if original value has not yet been set save coordinates of player on app start
				//				if (setOriginalValues) {
				//					originalLatitude = Input.location.lastData.latitude;
				//					originalLongitude = Input.location.lastData.longitude;
				//					setOriginalValues = false;
				//				}
				//
				//overwrite current lat and lon everytime
				currentLatitude = Input.location.lastData.latitude;
				currentLongitude = Input.location.lastData.longitude;

				//calculate the distance between where the player was when the app started and where they are now.

//				Calc (originalLatitude[0], originalLongitude [0], currentLatitude, currentLongitude);


			}
			Input.location.Stop();
		}
	}


	public void SwitchFriend() {
//		print ("on clicked");
		switchFriendClicked = true;
//		print (switchFriendClicked);
	}


	//calculates distance between two sets of coordinates, taking into account the curvature of the earth.
	public void Calc(double lat1, double lon1, double lat2, double lon2)
	{

		// Calculate bearing between current location and target location

		// This method calculates distance between two coordinates in metres

		double earthRadius = 6371000; //meters
		double dLat = Mathf.Deg2Rad*(lat1-lat2);
		double dLng = Mathf.Deg2Rad*(lon1-lon2);
		double a = System.Math.Sin(dLat/2) * System.Math.Sin(dLat/2) +
			System.Math.Cos( Mathf.Deg2Rad*(lat2)) * System.Math.Cos( Mathf.Deg2Rad*(lat1)) *
			System.Math.Sin(dLng/2) * System.Math.Sin(dLng/2);
		double c = 2 * System.Math.Atan2(System.Math.Sqrt(a), System.Math.Sqrt(1-a));
		distance = (float) (earthRadius * c);


//		print ("count " + originalLatitude.Count);
//		print ("lat " + originalLatitude[index]);
//		print ("lon " + originalLongitude[index]);
		currentCount = originalLatitude.Count;
//		print ("currCount " + currentCount);



	}

	void Start(){
		//		FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
		//			dependencyStatus = task.Result;
		//			if (dependencyStatus == DependencyStatus.Available) {
		//				InitializeFirebase();
		//			} else {
		//				Debug.LogError(
		//					"Could not resolve all Firebase dependencies: " + dependencyStatus);
		//			}
		//		});

		//		//start GetCoordinate() function 
		//		print(originalLatitude);
		//		distanceText.text = "Ditance: " + originalLatitude;
		//		StartCoroutine ("GetCoordinates");
		//		//initialize target and original position
		//		targetPosition = transform.position;
		//		originalPosition = transform.position;
		//
		// Get current GPS location
		//		lat2 = targetLatitude;
		//		dLon = targetLongitude - camLonitude;
		Input.compass.enabled = true;
		Input.location.Start ();

		index = 0;
		switchFriendClicked = false;


	}

	void Update(){
		//linearly interpolate from current position to target position
		//		transform.position = Vector3.Lerp(transform.position, targetPosition, speed);
		//rotate by 1 degree about the y axis every frame
		//		transform.eulerAngles += new Vector3 (0, 1f, 0);

		if (!startedFirebase) {


			// first time start
			FirebaseApp.CheckAndFixDependenciesAsync ().ContinueWith (task => {
				dependencyStatus = task.Result;
				if (dependencyStatus == DependencyStatus.Available) {
					InitializeFirebase ();
				} else {
					Debug.LogError (
						"Could not resolve all Firebase dependencies: " + dependencyStatus);
				}
			});

			//start GetCoordinate() function 
//			print (originalLatitude);
			StartCoroutine ("GetCoordinates");
			//initialize target and original position
			targetPosition = transform.position;
			originalPosition = transform.position;
		}

//		print ("index " + index);
//		print (switchFriendClicked);

		if (switchFriendClicked) {
//			print ("clicked!!!!!");

			if (index < currentCount - 1) {
				index++;
			} else {
				index = 0;
			}
			switchFriendClicked = false;
//			print ("index " + index);
		}

		if (originalLatitude.Count > 0 && originalLongitude.Count > 0 && userNameList.Count > 0) {			
			Calc (originalLatitude [index], originalLongitude [index], currentLatitude, currentLongitude);
			friendInfoText.text = userNameList [index];
			friendInfoText.text += "\nDist: " + distance.ToString("#.##") + "m";
		}



		// Rotate the to target location relative to north. Z axis pointing out of screen.
		transform.eulerAngles = new Vector3(0, 0, Mathf.MoveTowardsAngle
			(transform.localEulerAngles.z, (float)(Input.compass.trueHeading + distance), 2 * Mathf.PI));

		//		distanceText.text += "\n" + Input.compass.trueHeading.ToString ();
		//		distanceText.text += "\n" + transform.eulerAngles;
		//		print("lan "+originalLatitude);
		//		print("lon "+originalLongitude);
		//		print("dis "+distance);

	}

	protected virtual void InitializeFirebase() {
		//		print ("Start initialising firebase");
		FirebaseApp app = FirebaseApp.DefaultInstance;
		// NOTE: You'll need to replace this url with your Firebase App's database
		// path in order for the database connection to work correctly in editor.
		app.SetEditorDatabaseUrl("https://treasure-king-0.firebaseio.com/");
		if (app.Options.DatabaseUrl != null) {
			print ("url added");
			app.SetEditorDatabaseUrl (app.Options.DatabaseUrl);
		}
		Debug.Log ("Setting up Firebase Auth");
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		auth.StateChanged += Auth_StateChanged;
		Auth_StateChanged (this, null);

		StartListener();
//		print ("count " + originalLatitude.Count);

		startedFirebase = true;
		//		print ("Stop initialising firebase...");

	}

	void Auth_StateChanged (object sender, EventArgs e)
	{
		if (auth.CurrentUser != currUser) {
			bool signIn = currUser != auth.CurrentUser && auth.CurrentUser != null;
			if (!signIn && currUser != null) {
				Debug.Log ("Signed out" + currUser.UserId);
			}
			currUser = auth.CurrentUser;
			if (signIn) {
				Debug.Log ("Signed in" + currUser.UserId);
			}
		}
	}



	protected void StartListener() {

//		distanceText.text = "";

		originalLatitude = new List<Double> ();
		originalLongitude = new List<Double> ();
		userNameList = new List<String> ();

		FirebaseDatabase.DefaultInstance
			.GetReference ("friends")
			.ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
			if (e2.DatabaseError != null) {
				Debug.LogError (e2.DatabaseError.Message);
			}
			if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0) {
				
				var firendId = e2.Snapshot.Value as Dictionary<string, object>;
				foreach (var item in firendId)
				{	
//					if(item.Key.Equals(FirebaseAuth.DefaultInstance.CurrentUser.UserId)){
					if(item.Key.Equals(currUser.UserId)){
						
						var currentUserId = item.Value as Dictionary<string, object>;
						foreach (var friends in currentUserId)
						{	
							
							FirebaseDatabase.DefaultInstance
								.GetReference ("users")
								.ValueChanged += (object sender3, ValueChangedEventArgs e3) => {
								if (e3.DatabaseError != null) {
									Debug.LogError (e3.DatabaseError.Message);
								}
								if (e3.Snapshot != null && e3.Snapshot.ChildrenCount > 0) {
									
									var firendItem = e3.Snapshot.Value as Dictionary<string, object>;
									foreach (var items in firendItem) {
										if (items.Key.Equals(friends.Key)){
								
											var newItem = items.Value as Dictionary<string,object>;
											foreach (var item2 in newItem){
												if(item2.Key.Equals("Location")){
													
													var newItem2 = item2.Value as Dictionary<string,object>;
													foreach (var item3 in newItem2){
														if(item3.Key.Equals("latitude")){
															originalLatitude.Add(Double.Parse(item3.Value.ToString()));
//															print("add" + item3.Value.ToString());
//															print (originalLatitude.Count);
														}
														if(item3.Key.Equals("longitude")){
															originalLongitude.Add(Double.Parse(item3.Value.ToString()));
//															print("add" + item3.Value.ToString());
//															print (originalLongitude.Count);
														}

													}
												}
												if(item2.Key.Equals("username")){
//													print(item2.Value.ToString());
													userNameList.Add((item2.Value.ToString()));
												}

											}
										}

									}
								}
							};
							
						}
					}

				}
			} 
		} 
				;

//		print (originalLatitude.Count);
//		print (originalLongitude.Count);

//		for (int i = 0;i <= (originalLatitude.Count); i++){
//			print (originalLatitude [i]);

			//			distanceText.text += "\n" + (originalLatitude [i]);
			
//		}
	}


}
