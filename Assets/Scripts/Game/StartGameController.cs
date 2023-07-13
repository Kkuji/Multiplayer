using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Linq;
using System;

[RequireComponent(typeof(SpawnCoins))]
public class StartGameController : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _waitingText;
	[SerializeField] private float _delayForPlayersConnection = 0.4f;

	private bool _gameStarted = false;

	private SpawnCoins _spawnCoins;

	private void Start()
	{
		_spawnCoins = GetComponent<SpawnCoins>();
	}

	private void Update()
	{
		if (PhotonNetwork.PlayerList.Length > 1 && !_gameStarted)
		{
			SetValues(false);
			_spawnCoins.Clear();
			Invoke(nameof(SpawnCoins), _delayForPlayersConnection);
		}

		if (PhotonNetwork.PlayerList.Length < 2 && _gameStarted)
		{
			SetValues(true);
			_spawnCoins.Clear();
		}
	}

	private void SpawnCoins()
	{
		_spawnCoins.Spawn();
	}

	private void SetValues(bool value)
	{
		_waitingText.gameObject.SetActive(value);
		_gameStarted = !value;
	}
}