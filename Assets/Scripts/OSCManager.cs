using System;
using System.Collections;
using System.Collections.Generic;
using extOSC;
using UnityEngine;

public class OSCManager : MonoBehaviour {
	public String remoteHost;
    public Int16 remotePort;
    public OSCTransmitter transmitter;

	// Use this for initialization
	void Start () {
        // Creating a transmitter.
        transmitter = gameObject.AddComponent<OSCTransmitter>();

        // Set remote host address.
        transmitter.RemoteHost = remoteHost;

        // Set remote port;
        transmitter.RemotePort = remotePort;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void updateHost(string host) {
        transmitter.RemoteHost = host;
    }

	public void updatePort(string port)
	{
        transmitter.RemotePort = Convert.ToInt32(port);
	}


}
