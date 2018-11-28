using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using extOSC;

public class ShapePrinter : MonoBehaviour
{

	bool shapeEnabled = false;
	Dictionary<string, float> currentBlendShapes;
    public OSCManager oscManager;
    public double updateInterval = 1;
    public double updateTime = 0;

	// Use this for initialization
	void Start()
	{
		UnityARSessionNativeInterface.ARFaceAnchorAddedEvent += FaceAdded;
		UnityARSessionNativeInterface.ARFaceAnchorUpdatedEvent += FaceUpdated;
		UnityARSessionNativeInterface.ARFaceAnchorRemovedEvent += FaceRemoved;

	}

	/**void OnGUI()
	{
		if (shapeEnabled)
		{

			string blendshapes = "";
			string shapeNames = "";
            string valueNames = "";

			foreach (KeyValuePair<string, float> kvp in currentBlendShapes)
			{
				blendshapes += " [";
				blendshapes += kvp.Key.ToString();
				blendshapes += ":";
				blendshapes += kvp.Value.ToString();
				blendshapes += "]\n";
				shapeNames += "\"";
				shapeNames += kvp.Key.ToString();
				shapeNames += "\",\n";
				valueNames += kvp.Value.ToString();
				valueNames += "\n";
			}

			GUILayout.BeginHorizontal(GUILayout.ExpandHeight(true));
			GUILayout.Box(blendshapes);
			GUILayout.EndHorizontal();

			//Debug.Log(shapeNames);
			//Debug.Log(valueNames);
		}
	}*/

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
             var message = new OSCMessage("/ConductAR/faceBlendShapes");

            foreach (KeyValuePair<string, float> kvp in currentBlendShapes)
            {
                message.AddValue(OSCValue.String("/" + kvp.Key.ToString() + "/" + kvp.Value.ToString()));
            }

            oscManager.transmitter.Send(message);
        }

    }
}
