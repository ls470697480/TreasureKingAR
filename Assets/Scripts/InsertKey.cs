using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Unity.Editor;

public class InsertKey : MonoBehaviour {

	public GameObject chest;
//	public GameObject dialog;

//	public Transform treasureContainer;
	public Rigidbody key;
	private int numOfKeys = 10;
	public TextMesh numOfKeysText;
//	public TextMesh itemInfoText;

	public GameObject resetButton;


	private Vector3 initialKeyPosition;
	private Quaternion initialKeyRotation;


	private Vector3 screenPoint;
	private Vector3 offset;

	private AudioSource sound;

//	private static int gold;


	// firebase stuff
//	private bool startedFirebase = false;
//	DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;


	void Start()
	{
//		chest = GameObject.FindGameObjectWithTag("Chest");
//		sound = chest.GetComponent <AudioSource> ();

		numOfKeysText.text = "Keys * " + numOfKeys;

		initialKeyPosition = this.transform.position;
		initialKeyRotation = this.transform.rotation;
//		print (initialKeyPosition);
//		print (initialKeyRotation);

//		if (!startedFirebase) {
//			// first time start
//			FirebaseApp.CheckAndFixDependenciesAsync ().ContinueWith (task => {
//				dependencyStatus = task.Result;
//				if (dependencyStatus == DependencyStatus.Available) {
//					InitializeFirebase ();
//				} else {
//					Debug.LogError (
//						"Could not resolve all Firebase dependencies: " + dependencyStatus);
//				}
//			});
//
//		}
	}

	void Update() {
		
	}

//	protected virtual void InitializeFirebase() {
//		//		print ("Start initialising firebase");
//		FirebaseApp app = FirebaseApp.DefaultInstance;
//		// NOTE: You'll need to replace this url with your Firebase App's database
//		// path in order for the database connection to work correctly in editor.
//		app.SetEditorDatabaseUrl("https://treasure-king-0.firebaseio.com/");
//		if (app.Options.DatabaseUrl != null) {
//			print ("url added");
//			app.SetEditorDatabaseUrl (app.Options.DatabaseUrl);
//		}
//		StartListener();
//		//		print ("count " + originalLatitude.Count);
//
//		startedFirebase = true;
//		//		print ("Stop initialising firebase...");
//
//	}


//	protected void StartListener() {
//		
//		bool hunting = false;
//		string treasureDestroyId;
//
//		FirebaseDatabase.DefaultInstance
//			.GetReference ("Treasure")
//			.ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
//			if (e2.DatabaseError != null) {
//				Debug.LogError (e2.DatabaseError.Message);
//			}
//			if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0) {
//
//				var treasureId = e2.Snapshot.Value as Dictionary<string, object>;
//				foreach (var item in treasureId) {
//					var elements = item.Value as Dictionary<string, object>;
//					foreach(var goldItem in elements){
//						if(goldItem.Key.ToString().Equals("hunting")){
//							if (goldItem.Value.ToString().Equals("true")){
//								treasureDestroyId = item.Key.ToString();
//								hunting = true;
//							}
//						}
//						if(hunting){
//							if(goldItem.Key.ToString().Equals("gold")){
//								gold = int.Parse(goldItem.Value.ToString());
//								print("gold" + gold);
//							}
//						}
//					}
//				}
//			}
//		};
//	}


	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Collider") {

			Destroy (col.gameObject);
			Destroy (this.gameObject);
			chest.GetComponent<Animation>().Play();
//			sound.Play ();

			numOfKeys = numOfKeys - 1;
			numOfKeysText.text = "Keys * " + numOfKeys;

			Rigidbody keyClone = (Rigidbody) Instantiate(key, initialKeyPosition, initialKeyRotation);

			resetButton.SetActive(false);
//			GameObject dialogObject = Instantiate (dialog) as GameObject;
//			dialogObject.transform.SetParent (treasureContainer);
//
//			itemInfoText.text = "YOU GOT " + gold + " gold!";
//			print("YOU GOT " + gold + " gold!");

		}
	}
		

	void OnMouseDown()
	{
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

	}

	void OnMouseDrag()
	{
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
		transform.position = curPosition;
	}

}
	