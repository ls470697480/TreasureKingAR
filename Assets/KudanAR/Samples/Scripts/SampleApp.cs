using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Kudan.AR.Samples
{
	/// <summary>
	/// Script used in the Kudan Samples. Provides functions that switch between different tracking methods and start abitrary tracking.
	/// </summary>
	public class SampleApp : MonoBehaviour
	{
        public KudanTracker _kudanTracker;	// The tracker to be referenced in the inspector. This is the Kudan Camera object.
        
        public TrackingMethodMarkerless _markerlessTracking;	// The reference to the markerless tracking method that lets the tracker know which method it is using

		public Button button;
		public Text buttonText;

		private bool firstTime;

		void Start() {
			firstTime = true;
			buttonText.text = "Dig To Find The Treasure Box";
		}

        public void StartClicked()
        {
			if (!_kudanTracker.ArbiTrackIsTracking ()) 
			{
				// from the floor placer.
				Vector3 floorPosition;			// The current position in 3D space of the floor
				Quaternion floorOrientation;	// The current orientation of the floor in 3D space, relative to the device

				_kudanTracker.FloorPlaceGetPose (out floorPosition, out floorOrientation);	// Gets the position and orientation of the floor and assigns the referenced Vector3 and Quaternion those values
				_kudanTracker.ArbiTrackStart (floorPosition, floorOrientation);				// Starts markerless tracking based upon the given floor position and orientations
			}

			else 
			{
				_kudanTracker.ArbiTrackStop ();
			}
        }

		void Update()
		{
			if (!_kudanTracker.ArbiTrackIsTracking ()) 
			{
				button.enabled = true;
				if (firstTime) {
					buttonText.text = "Dig To Find The Treasure Box";
				} else {
					buttonText.text = "Oh The Treasure Disappeared!\nDig To Find The Treasure Box Again";
				}
			} 
			else 
			{
				buttonText.text = "Use Key to Open the Box";
				firstTime = false;
				button.enabled = false;
			}
		}
	}
}
