using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
	[SerializeField] private string _region;
	private void Start()
	{
		PhotonNetwork.ConnectUsingSettings();
		PhotonNetwork.ConnectToRegion(_region);
	}

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinLobby();
		SceneManager.LoadScene("Lobby");
	}
}