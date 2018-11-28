using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using extOSC;

public class EyesTracker : MonoBehaviour
{
	public OSCManager oscManager;
    public bool eyesEnabled;
	public double updateInterval = 1;
	public double updateTime = 1;

    private Vector3 eyePositionL;
    private Vector3 eyePositionR;
    private Quaternion eyeRotationL;
    private Quaternion eyeRotationR;

    void Awake() {
		UnityARSessionNativeInterface.ARFaceAnchorAddedEvent += FaceAdded;
		UnityARSessionNativeInterface.ARFaceAnchorUpdatedEvent += FaceUpdated;
		UnityARSessionNativeInterface.ARFaceAnchorRemovedEvent += FaceRemoved;
    }

	void FaceAdded(ARFaceAnchor anchorData)
	{
        eyesEnabled = true;		
	}

	void FaceUpdated(ARFaceAnchor anchorData)
	{
		eyePositionL = anchorData.leftEyePose.position;
		eyeRotationL = anchorData.leftEyePose.rotation;

		eyePositionR = anchorData.rightEyePose.position;
		eyeRotationR = anchorData.rightEyePose.rotation;
	}

	void FaceRemoved(ARFaceAnchor anchorData)
	{
        eyesEnabled = false;
	}

	void Update()
	{
		// If the next update is reached
		if (Time.time >= updateTime)
		{
			// Change the next update (current second+1)
			updateTime = Mathf.FloorToInt(Time.time) + updateInterval;
			ThrottledUpdate();
		}
	}

	void ThrottledUpdate()
	{
		if (eyesEnabled)
		{
			var message = new OSCMessage("/ConductAR/eyesPose");
            message.AddValue(OSCValue.String("/eyePositionL/x/" + eyePositionL.x));
            message.AddValue(OSCValue.String("/eyePositionL/y/" + eyePositionL.y));
            message.AddValue(OSCValue.String("/eyePositionL/z/" + eyePositionL.z));
            message.AddValue(OSCValue.String("/eyePositionR/x/" + eyePositionR.x));
            message.AddValue(OSCValue.String("/eyePositionR/y/" + eyePositionR.y));
            message.AddValue(OSCValue.String("/eyePositionR/z/" + eyePositionR.z));
            message.AddValue(OSCValue.String("/eyeRotationL/x/" + eyeRotationL.eulerAngles.x));
            message.AddValue(OSCValue.String("/eyeRotationL/y/" + eyeRotationL.eulerAngles.y));
            message.AddValue(OSCValue.String("/eyeRotationL/z/" + eyeRotationL.eulerAngles.z));
            message.AddValue(OSCValue.String("/eyeRotationR/x/" + eyeRotationR.eulerAngles.x));
            message.AddValue(OSCValue.String("/eyeRotationR/y/" + eyeRotationR.eulerAngles.y));
            message.AddValue(OSCValue.String("/eyeRotationR/z/" + eyeRotationR.eulerAngles.z));
            oscManager.transmitter.Send(message);
		}
	}
}
