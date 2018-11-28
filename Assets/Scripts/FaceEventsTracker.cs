using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using UnityEngine.UI;
using extOSC;

public class FaceEventsTracker : MonoBehaviour
{
	public OSCManager oscManager;
	public double updateInterval = 1;
	public double updateTime = 1;
	bool shapeEnabled = false;
	bool enableTongue = false;
	bool enableBlink = false;
	bool enableSmile = false;
	bool enableFrown = false;
	bool eyebrowsUp = false;
	bool enableLookLeft = false;
	bool enableLookRight = false;
    bool enableMouthOpen = false;
	Dictionary<string, float> currentBlendShapes;

	// Use this for initialization
	void Start()
	{
		UnityARSessionNativeInterface.ARFaceAnchorAddedEvent += FaceAdded;
		UnityARSessionNativeInterface.ARFaceAnchorUpdatedEvent += FaceUpdated;
		UnityARSessionNativeInterface.ARFaceAnchorRemovedEvent += FaceRemoved;
	}

	void OnGUI()
	{

		if (shapeEnabled)
		{
			if (currentBlendShapes.ContainsKey(ARBlendShapeLocation.TongueOut))
			{
				enableTongue = (currentBlendShapes[ARBlendShapeLocation.TongueOut] > 0.3f);
			}

			if (currentBlendShapes.ContainsKey(ARBlendShapeLocation.EyeBlinkLeft) && currentBlendShapes.ContainsKey(ARBlendShapeLocation.EyeBlinkRight))
			{
				enableBlink = (currentBlendShapes[ARBlendShapeLocation.EyeBlinkLeft] > 0.5f && currentBlendShapes[ARBlendShapeLocation.EyeBlinkRight] > 0.5f);
			}

			if (currentBlendShapes.ContainsKey(ARBlendShapeLocation.MouthSmileLeft) && currentBlendShapes.ContainsKey(ARBlendShapeLocation.MouthSmileRight))
			{
				enableSmile = (currentBlendShapes[ARBlendShapeLocation.MouthSmileLeft] > 0.5f && currentBlendShapes[ARBlendShapeLocation.MouthSmileRight] > 0.5f);
			}

			if (currentBlendShapes.ContainsKey(ARBlendShapeLocation.MouthFrownLeft) && currentBlendShapes.ContainsKey(ARBlendShapeLocation.MouthFrownRight))
			{
				enableFrown = (currentBlendShapes[ARBlendShapeLocation.MouthFrownLeft] > 0.3f && currentBlendShapes[ARBlendShapeLocation.MouthFrownRight] > 0.3f);
			}

			if (currentBlendShapes.ContainsKey(ARBlendShapeLocation.BrowOuterUpLeft) && currentBlendShapes.ContainsKey(ARBlendShapeLocation.BrowOuterUpRight))
			{
				eyebrowsUp = (currentBlendShapes[ARBlendShapeLocation.BrowOuterUpLeft] > 0.5f && currentBlendShapes[ARBlendShapeLocation.BrowOuterUpRight] > 0.5f);
			}

			if (currentBlendShapes.ContainsKey(ARBlendShapeLocation.MouthLowerDownLeft) && currentBlendShapes.ContainsKey(ARBlendShapeLocation.MouthLowerDownRight))
			{
				enableMouthOpen = (currentBlendShapes[ARBlendShapeLocation.MouthLowerDownLeft] > 0.3f && currentBlendShapes[ARBlendShapeLocation.MouthLowerDownRight] > 0.3f);
			}

			if (currentBlendShapes.ContainsKey(ARBlendShapeLocation.EyeLookOutLeft))
			{
				enableLookLeft = (currentBlendShapes[ARBlendShapeLocation.EyeLookOutLeft] > 0.5f);
			}

			if (currentBlendShapes.ContainsKey(ARBlendShapeLocation.EyeLookOutRight))
			{
				enableLookRight = (currentBlendShapes[ARBlendShapeLocation.EyeLookOutRight] > 0.5f);
			}
		}

	}

	void FaceAdded(ARFaceAnchor anchorData)
	{
		shapeEnabled = true;
		currentBlendShapes = anchorData.blendShapes;
	}

	void FaceUpdated(ARFaceAnchor anchorData)
	{
		currentBlendShapes = anchorData.blendShapes;
	}

	void FaceRemoved(ARFaceAnchor anchorData)
	{
		shapeEnabled = false;
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
		if (shapeEnabled)
		{
			var message = new OSCMessage("/ConductAR/faceEvents");

            if (enableTongue)
            {
                message.AddValue(OSCValue.String("/tongue/" + 1.0));
            }
            else
            {
                message.AddValue(OSCValue.String("/tongue/" + 0.0));
            }
            if (enableBlink)
            {
                message.AddValue(OSCValue.String("/blink/" + 1.0));
            }
            else
            {
                message.AddValue(OSCValue.String("/blink/" + 0.0));
            }
            if (enableSmile)
            {
                message.AddValue(OSCValue.String("/smile/" + 1.0));
            }
            else
            {
                message.AddValue(OSCValue.String("/smile/" + 0.0));
            }
            if (enableFrown)
            {
                message.AddValue(OSCValue.String("/frown/" + 1.0));
            }
            else
            {
                message.AddValue(OSCValue.String("/frown/" + 0.0));
            }
            if (eyebrowsUp)
            {
                message.AddValue(OSCValue.String("/eyebrows/" + 1.0));
            }
            else
            {
                message.AddValue(OSCValue.String("/eyebrows/" + 0.0));
            }
            if (enableLookLeft)
            {
                message.AddValue(OSCValue.String("/lookLeft/" + 1.0));
            }
            else
            {
                message.AddValue(OSCValue.String("/lookLeft/" + 0.0));
            }
            if (enableLookRight)
            {
                message.AddValue(OSCValue.String("/lookRight/" + 1.0));
            }
            else
            {
                message.AddValue(OSCValue.String("/lookRight/" + 0.0));
            }
            if (enableMouthOpen)
            {
                message.AddValue(OSCValue.String("/mouthOpen/" + 1.0));
            }
            else
            {
                message.AddValue(OSCValue.String("/mouthOpen/" + 0.0));
            }

            oscManager.transmitter.Send(message);
		}
	}
}
