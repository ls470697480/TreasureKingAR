using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Kudan.AR.Samples;

public class DetectLocation : MonoBehaviour {

	// location marker show
	private Vector2 targetCoordinates;
	// device location variable
	private Vector2 deviceCoordinates;
	// distance allowed from device to target
//	private float distanceFromTarget = 0.00004f;
	private float distanceFromTarget = 100f;
	// distance between device to target coordinates
	private float proximity = 0.001f;
	// values for latitude and longitude get from device gps
	private float sLatitude, sLongitude;
	// set value for target location
	public float dLatitude = -38.884687f, dLongitude = 146.605945f;
	// var for location request
	private bool enableByRequest = true;
	LocationService service;

	public int maxWait = 10;
	public bool ready = false;
	public Text text;
	// sampleapp script
	public SampleApp sampleApp;

	public GameObject kudanBundle;

	public Text camPosText;


	// get last update location
	IEnumerator getLocation() {
		service = Input.location;
		if (!enableByRequest && !service.isEnabledByUser) {
			Debug.Log ("Location Services not enabled by user");
			yield break;
		}
		service.Start ();
		while (service.status == LocationServiceStatus.Initializing && maxWait > 0) {
			yield return new WaitForSeconds (1);
			maxWait--;
		}
		if (maxWait < 1) {
			Debug.Log ("Timed out");
			yield break;
		}
		if (service.status == LocationServiceStatus.Failed) {
			Debug.Log("Unable to determine device location");
			yield break;
		} else {
			text.text = "Target Location: " + dLatitude + ", " + dLongitude + 
				"\nMy Location: " + service.lastData.latitude + ", " + service.lastData.longitude;
			// here we pass latitude and longitude from device
			sLatitude = service.lastData.latitude;
			sLongitude = service.lastData.longitude;
		}
		// stop service if you want
		// service.Stop();
		ready = true;
		startCalculate ();
	}


	// method to calculate distance between device location and target location
	public void startCalculate() {
		// create vector2 from device lat and lon
		deviceCoordinates = new Vector2 (sLatitude, sLongitude);
		// priximity: calculate distance
		proximity = Vector2.Distance (targetCoordinates, deviceCoordinates);
		// if proximity <= target
		if (proximity <= distanceFromTarget) {
			text.text += "\nDistance: " + proximity.ToString ();
			text.text += "\nTarget Detected";
			// show 3d object by calling SampleApp script
			sampleApp.StartClicked ();
		} else {
			// else, show warning
			text.text += "\nDistance: " + proximity.ToString ();
			text.text += "\ntarget not detected, too far!";
		}
	}
			

	// call getlocation from start
	void Start () {
		// create vector2 coordinates from given lat and lon
		targetCoordinates = new Vector2(dLatitude, dLongitude);
		// start get location
		StartCoroutine (getLocation());
	}
	
	// Update is called once per frame
	void Update () {
		
		text.text = "Target Location: " + dLatitude + ", " + dLongitude + 
			"\nMy Location: " + service.lastData.latitude + ", " + service.lastData.longitude;
		// here we pass latitude and longitude from device
		sLatitude = service.lastData.latitude;
		sLongitude = service.lastData.longitude;

		camPosText.text = "Cam Pos: " + kudanBundle.GetComponent<Camera>().transform.position + "; " +
			kudanBundle.GetComponent<Camera>().transform.rotation;
	}
}
