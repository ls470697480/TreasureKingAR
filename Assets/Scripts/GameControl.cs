using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

	public Text friendButtonText;
	public GameObject nextFriendButton;
	public Transform UIContainer;

//	private GameObject nextFriendButtonObject;

	public GameObject arrow;

	private FriendButtonStatus status;

//	private bool switchFriendClicked;

	enum FriendButtonStatus
	{
		FIRST_TIME, STOPPED, ONGOING
	};


	// Use this for initialization
	void Start () {
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		status = FriendButtonStatus.STOPPED;
		friendButtonText.text = "Start To Find Friends";
//		switchFriendClicked = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
	}


	public void FindFriendClicked() {
		GameObject nextFriendButtonObject = null;
		GameObject arrowObject = null;
		if (status == FriendButtonStatus.STOPPED) {
			// StartFindFriend
//			friendButtonText.text = "Stop Finding Friends";
			friendButtonText.text = "Finding Friends";
			status = FriendButtonStatus.ONGOING;
			arrowObject = Instantiate (arrow) as GameObject;
			nextFriendButtonObject = Instantiate (nextFriendButton) as GameObject;
			nextFriendButtonObject.transform.SetParent (UIContainer);
			nextFriendButtonObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3 (206, 477, 0);
			nextFriendButtonObject.GetComponent<RectTransform> ().localScale = new Vector3(1, 1, 1);
		} /*else if (status == FriendButtonStatus.ONGOING) {
//			StopFindFriend
			friendButtonText.text = "Start To Find Friends";
			status = FriendButtonStatus.STOPPED;
//			DestroyImmediate (arrowObject, true);
//			DestroyImmediate (nextFriendButtonObject, true);
			print("destroy arrow");
			GameObject.Destroy (arrowObject);
			print("destroy button");
			nextFriendButtonObject.SetActive(false);
		}
		print(status);
		*/
	}

	//	private void FriendTextUpdate() {
	//		if (status == FriendButtonStatus.STOPPED) {
	//			
	//			status = FriendButtonStatus.ONGOING;
	//		} else if (status == FriendButtonStatus.ONGOING) {
	//			
	//			status = FriendButtonStatus.STOPPED;
	//		}
	//	}


//	public void SwitchFriendClicked() {
//		if (switchFriendClicked) {
//			print ("clicked!!!!!");
//			switchFriendClicked = false;
//			print (switchFriendClicked);
//		}
//	}
//
//	public bool GetSwitchFriendClicked () {
//		return switchFriendClicked;
//	}
}
