using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

[RequireComponent(typeof(PhotonView))]
public class Player : MonoBehaviour
{
	[HideInInspector] public float currentHealth;
	[HideInInspector] public float currentCoins;
	[HideInInspector] public float coinsToSpawn;
	[HideInInspector] public Image healthBar;
	[HideInInspector] public Image coinBar;
	[HideInInspector] public SpawnCoins spawnCoins;
	[HideInInspector] public GameObject winPanel;
	[HideInInspector] public Button leaveButton;

	public float maxHealth;

	private float _leaveDelay = 1.5f;
	private float _minClamp = 0f;
	private float _maxClamp = 1f;
	private float _allCoins;

	private PhotonView _view;

	private PlayerMover _playerMover;

	private void Start()
	{
		Player[] players = FindObjectsOfType<MonoBehaviour>().OfType<Player>().ToArray();

		if (players.Length != 0)
		{
			foreach (Player pl in players)
			{
				if (pl != this)
				{
					pl.currentHealth = maxHealth;
					pl.healthBar.fillAmount = 1f;

					pl.currentCoins = 0;
					pl.coinBar.fillAmount = 0f;
					pl._allCoins = pl.coinsToSpawn * Convert.ToSingle(players.Length);
				}
			}
		}

		Coin[] coins = FindObjectsOfType<MonoBehaviour>().OfType<Coin>().ToArray();

		if (coins.Length != 0)
		{
			for (int i = 0; i < coins.Length; i++)
			{
				if (coins[i] != null)
				{
					Destroy(coins[i].gameObject);
				}
			}
		}

		_playerMover = GetComponent<PlayerMover>();

		_view = GetComponent<PhotonView>();
		currentHealth = maxHealth;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.TryGetComponent(out Coin coin))
		{
			Destroy(coin.gameObject);
			currentCoins++;

			coinBar.fillAmount = Mathf.Clamp(currentCoins / _allCoins, _minClamp, _maxClamp);
		}
		if (collision.gameObject.GetComponent<Bullet>() != null)
		{
			Destroy(collision.gameObject);
		}
	}
	private void ShowPanelAndLeave(string lastText)
	{
		winPanel.SetActive(true);
		winPanel.GetComponentInChildren<TextMeshProUGUI>().SetText
			(lastText + "\nCoins amount - " + GetComponent<Player>().currentCoins);

		Invoke(nameof(Leave), _leaveDelay);
	}

	[PunRPC]
	public void Win()
	{
		ShowPanelAndLeave("You won");
	}

	public void GetDamage(float damage)
	{
		currentHealth -= damage;
		healthBar.fillAmount = Mathf.Clamp(currentHealth / maxHealth, _minClamp, _maxClamp);
		CheckIsDead();
	}

	public void CheckIsDead()
	{
		if (currentHealth <= 0.1f)
		{
			if (PhotonNetwork.PlayerList.Length == 2)
			{
				Player winPlayer = FindObjectsOfType<Player>().First(Player => Player.currentHealth > 0);
				PhotonView.Get(winPlayer).RPC(nameof(Win), RpcTarget.Others);
			}

			ShowPanelAndLeave("You lost");
			_playerMover.Die();
		}
	}

	public void SetLeaveButton(Button button)
	{
		leaveButton = button;
		leaveButton.onClick.AddListener(delegate () { Leave(); });
	}

	public void Leave()
	{
		spawnCoins.Clear();

		PhotonNetwork.LeaveRoom();
		SceneManager.LoadScene("Lobby");
	}
}
