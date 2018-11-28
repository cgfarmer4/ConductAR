using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using extOSC;
using System.Linq;

public class MeshBroadcast : MonoBehaviour
{
	public OSCManager oscManager;
	public double updateInterval = 1;
    public double updateTime = 1;
    bool faceEnabled = false;

    Vector3[] meshVertices;
    Vector2[] meshUV;
    int[] meshTriangles;

	// Use this for initialization
	void Start()
	{
		UnityARSessionNativeInterface.ARFaceAnchorAddedEvent += FaceAdded;
		UnityARSessionNativeInterface.ARFaceAnchorUpdatedEvent += FaceUpdated;
		UnityARSessionNativeInterface.ARFaceAnchorRemovedEvent += FaceRemoved;
	}

	void FaceAdded(ARFaceAnchor anchorData)
	{
        faceEnabled = true;
	}

	void FaceUpdated(ARFaceAnchor anchorData)
	{
		if (faceEnabled)
		{
			meshVertices = anchorData.faceGeometry.vertices;
			meshUV = anchorData.faceGeometry.textureCoordinates;
			meshTriangles = anchorData.faceGeometry.triangleIndices;
		}

	}

	void FaceRemoved(ARFaceAnchor anchorData)
	{
        faceEnabled = false;
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
		if (faceEnabled)
		{
            System.Guid ID = System.Guid.NewGuid();

			// split on groups with each 100 items
			Vector3[][] vertChunks = meshVertices
								.Select((s, i) => new { Value = s, Index = i })
								.GroupBy(x => x.Index / 150)
								.Select(grp => grp.Select(x => x.Value).ToArray())
								.ToArray();
            
            //
            Vector2[][] uvChunks = meshUV
                                .Select((s, i) => new { Value = s, Index = i })
                                .GroupBy(x => x.Index / 300)
                                .Select(grp => grp.Select(x => x.Value).ToArray())
                                .ToArray();

			// split on groups with each 100 items
			int[][] triangleChunks = meshTriangles
								.Select((s, i) => new { Value = s, Index = i })
								.GroupBy(x => x.Index / 1200)
								.Select(grp => grp.Select(x => x.Value).ToArray())
								.ToArray();


			Dictionary<string, object> meshPacketInfo = new Dictionary<string, object>();
            meshPacketInfo.Add("id", ID);
			meshPacketInfo.Add("verticesChunks", vertChunks.Length);
			meshPacketInfo.Add("uvChunks", uvChunks.Length);
			meshPacketInfo.Add("trianglesChunks", triangleChunks.Length);

			var header = new OSCMessage("/ConductAR/meshHeader");
			//header.AddValue(OSCValue.String(JsonConvert.SerializeObject(meshPacketInfo)));
			oscManager.transmitter.Send(header);

			for (int i = 0; i < vertChunks.Length; i++)
			{
                Dictionary<string, object> meshData = new Dictionary<string, object>();
				meshData.Add("vertID", ID);
                meshData.Add("vertices", vertChunks[i]);
				meshData.Add("verticesPartNum", i);

				var message = new OSCMessage("/ConductAR/meshDetails/vertices");
				//message.AddValue(OSCValue.String(JsonConvert.SerializeObject(meshData)));
				oscManager.transmitter.Send(message);
			}

			for (int i = 0; i < uvChunks.Length; i++)
			{
				Dictionary<string, object> meshData = new Dictionary<string, object>();
                meshData.Add("uvID", ID);
				meshData.Add("uv", uvChunks[i]);
				meshData.Add("uvsPartNum", i);

				var message = new OSCMessage("/ConductAR/meshDetails/uvs");
				//message.AddValue(OSCValue.String(JsonConvert.SerializeObject(meshData)));
				oscManager.transmitter.Send(message);
			}

			for (int i = 0; i < triangleChunks.Length; i++)
			{
				Dictionary<string, object> meshData = new Dictionary<string, object>();
                meshData.Add("trianglesID", ID);
				meshData.Add("triangles", triangleChunks[i]);
				meshData.Add("trianglesPartNum", i);

				var message = new OSCMessage("/ConductAR/meshDetails/triangles");
				//message.AddValue(OSCValue.String(JsonConvert.SerializeObject(meshData)));
				oscManager.transmitter.Send(message);
			}
		}
	}
}
