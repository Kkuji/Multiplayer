using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections.Generic;

public class MenuManager : MonoBehaviourPunCallbacks
{
	[SerializeField] private TMP_InputField _createServer;
	[SerializeField] private TMP_InputField _joinServer;
	[SerializeField] private TMP_InputField _nickname;
	[SerializeField] private TextMeshProUGUI _errorText;
	[SerializeField] private byte _maximumPlayers;

	[SerializeField] private int _maxCharactersInput;

	private Dictionary<string, RoomInfo> _roomList = new Dictionary<string, RoomInfo>();

	private void Start()
	{
		SetCharacterLimits();
	}

	private void SetCharacterLimits()
	{
		_createServer.characterLimit = _maxCharactersInput;
		_joinServer.characterLimit = _maxCharactersInput;
		_nickname.characterLimit = _maxCharactersInput;
	}

	private bool CheckNullOrWhiteSpace(string name)
	{
		return string.IsNullOrWhiteSpace(name);
	}

	private bool CheckServerAvaliable(string qwe)
	{
		bool isAvaliable = true;

		foreach (var item in _roomList)
		{
			RoomInfo info = item.Value;

			if (info.Name == qwe)
			{
				isAvaliable = false;
				break;
			}
		}

		return isAvaliable;
	}

	private void SetNickname()
	{
		PhotonNetwork.NickName = _nickname.text;
		PlayerPrefs.SetString("name", _nickname.text);
	}

	private void CreateRoom()
	{
		RoomOptions roomOptions = new() { MaxPlayers = _maximumPlayers };
		PhotonNetwork.CreateRoom(_createServer.text, roomOptions);
	}

	public void TryCreateRoom()
	{
		if (!PhotonNetwork.InLobby)
		{
			_errorText.SetText("You are still connecting to lobby");
		}

		else if (CheckNullOrWhiteSpace(_nickname.text))
		{
			_errorText.SetText("Nickname cannot contain only spaces or be empty");
		}

		else if (!CheckServerAvaliable(_createServer.text))
		{
			_errorText.SetText("The name of the room is occupied");
		}

		else if (CheckNullOrWhiteSpace(_createServer.text))
		{
			_errorText.SetText("The name of the room cannot contain only spaces or be empty");
		}

		else
		{
			SetNickname();
			CreateRoom();
		}
	}

	public void TryJoinRoom()
	{
		if (!PhotonNetwork.InLobby)
		{
			_errorText.SetText("You are still connecting to lobby");
		}

		else if (CheckNullOrWhiteSpace(_nickname.text))
		{
			_errorText.SetText("Nickname cannot contain only spaces or be empty");
		}

		else if (CheckServerAvaliable(_joinServer.text))
		{
			_errorText.SetText("There is no room with such name");
		}

		else
		{
			SetNickname();
			PhotonNetwork.JoinRoom(_joinServer.text);
		}
	}
	private void UpdateCachedRoomList(List<RoomInfo> roomList)
	{
		for (int i = 0; i < roomList.Count; i++)
		{
			RoomInfo info = roomList[i];

			if (info.RemovedFromList)
			{
				_roomList.Remove(info.Name);
			}
			else
			{
				_roomList[info.Name] = info;
			}
		}
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		UpdateCachedRoomList(roomList);
	}

	public override void OnJoinedRoom()
	{
		PhotonNetwork.LoadLevel("Game");
	}

	public override void OnConnectedToMaster()
	{
		if (!PhotonNetwork.InLobby)
		{
			PhotonNetwork.JoinLobby();
		}
	}
}