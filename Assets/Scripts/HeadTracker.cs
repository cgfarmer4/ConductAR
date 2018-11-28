using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using extOSC;


public class HeadTracker : MonoBehaviour
{
	public OSCManager oscManager;
	public bool headEnabled;
	public double updateInterval = 1;
	public double updateTime = 1;

    public GameObject backgroundPlane;

	private Vector3 headPosition;
    private Vector3 lookAtPoint;
	private Quaternion headRotation;	

	void Start()
	{
		UnityARSessionNativeInterface.ARFaceAnchorAddedEvent += FaceAdded;
		UnityARSessionNativeInterface.ARFaceAnchorUpdatedEvent += FaceUpdated;
		UnityARSessionNativeInterface.ARFaceAnchorRemovedEvent += FaceRemoved;
	}

	void FaceAdded(ARFaceAnchor anchorData)
	{
		headEnabled = true;
	}

	void FaceUpdated(ARFaceAnchor anchorData)
	{
		if (headEnabled)
		{
            //backgroundPlane.transform.localPosition = UnityARMatrixOps.GetPosition(anchorData.transform);
			//backgroundPlane.transform.localPosition = new Vector3(backgroundPlane.transform.localPosition.x, backgroundPlane.transform.localPosition.y, backgroundPlane.transform.localPosition.z);
			//backgroundPlane.transform.localRotation = UnityARMatrixOps.GetRotation(anchorData.transform);
			headPosition = UnityARMatrixOps.GetPosition(anchorData.transform);
			headRotation = UnityARMatrixOps.GetRotation(anchorData.transform);
            lookAtPoint = anchorData.lookAtPoint;
		}
	}

	void FaceRemoved(ARFaceAnchor anchorData)
	{
		headEnabled = false;
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
		if (headEnabled)
		{
			var message = new OSCMessage("/ConductAR/headPose");

            message.AddValue(OSCValue.String("/headPosition/x/" + headPosition.x));
            message.AddValue(OSCValue.String("/headPosition/y/" + headPosition.y));
            message.AddValue(OSCValue.String("/headPosition/z/" + headPosition.z));

            message.AddValue(OSCValue.String("/headRotation/x/" + headRotation.eulerAngles.x));
            message.AddValue(OSCValue.String("/headRotation/y/" + headRotation.eulerAngles.y));
            message.AddValue(OSCValue.String("/headRotation/z/" + headRotation.eulerAngles.z));

            message.AddValue(OSCValue.String("/lookAtPoint/x/" + lookAtPoint.x));
            message.AddValue(OSCValue.String("/lookAtPoint/y/" + lookAtPoint.y));
            message.AddValue(OSCValue.String("/lookAtPoint/z/" + lookAtPoint.z));

            oscManager.transmitter.Send(message);
		}
	}
}
