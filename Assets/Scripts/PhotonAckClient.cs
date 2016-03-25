using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;

public class PhotonAckClient : MonoBehaviour, IPhotonPeerListener {

	public PhotonPeer peer;
	private bool connected = false;

	// Use this for initialization
	void Start ()
	{
		// connect to Photon Server
		peer = new PhotonPeer(this, ConnectionProtocol.Udp);
		peer.Connect ("10.0.2.15:5055", "PhotonAckServer");

		StartCoroutine (doService ());
	}

	IEnumerator doService()
	{
		while (true) {
			peer.Service ();
			yield return new WaitForSeconds (0.1f);
		}
	}

	void OnGUI()
	{
		GUILayout.Label ("Connected: " + connected.ToString ());
		if (connected) {
			if (GUILayout.Button ("Send Operation Request")) {
				// send a message to the server
				peer.OpCustom(0, new Dictionary<byte, object>(), true);
			}
		}
	}

#region IPhotonPeerListener Members
	public void DebugReturn(DebugLevel level, string message)
	{
		// log message to console
		Debug.Log(message);
	}

	public void OnEvent(EventData eventData)
	{
		// server raised an event
		Debug.Log("Received event - type: " + eventData.Code.ToString());
	}

	public void OnOperationResponse(OperationResponse operationResponse)
	{
		// server sent operation response
		Debug.Log("Recieved op response - type: " + operationResponse.OperationCode.ToString());
	}

	public void OnStatusChanged(StatusCode statusCode)
	{
		// connected to Photon Server
		if (statusCode == StatusCode.Connect) {
			connected = true;
		}

		// log status change
		Debug.Log("Status change" + statusCode.ToString());
	}
#endregion
}
