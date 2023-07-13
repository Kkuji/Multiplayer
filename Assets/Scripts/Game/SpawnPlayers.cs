using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpawnCoins))]
public class SpawnPlayers : MonoBehaviour
{
	[SerializeField] private Image _coinBar;
	[SerializeField] private Image _healthBar;
	[SerializeField] private GameObject _player;
	[SerializeField] private Button _shootButton;
	[SerializeField] private Button _leaveButton;
	[SerializeField] private FixedJoystick _joystick;
	[SerializeField] private Vector2 _minPostion, _maxPosition;
	[SerializeField] private GameObject _winPanel;

	private SpawnCoins _spawnCoins;
	private GameObject _currentPlayer;

	private void Start()
	{
		_spawnCoins = GetComponent<SpawnCoins>();

		Vector2 randomPosition = new Vector2(Random.Range(_minPostion.x, _maxPosition.x), Random.Range(_minPostion.y, _maxPosition.y));
		_currentPlayer = PhotonNetwork.Instantiate(_player.name, randomPosition, Quaternion.identity);

		SetPlayerValues(_currentPlayer);
	}

	private void SetPlayerValues(GameObject player)
	{
		player.GetComponent<PlayerShooter>().SetShootButton(_shootButton);
		player.GetComponent<PlayerMover>().joystick = _joystick;
		player.GetComponent<Player>().spawnCoins = gameObject.GetComponent<SpawnCoins>();
		player.GetComponent<Player>().coinsToSpawn = _spawnCoins.GetAmountCoins();
		player.GetComponent<Player>().SetLeaveButton(_leaveButton);
		player.GetComponent<Player>().healthBar = _healthBar;
		player.GetComponent<Player>().winPanel = _winPanel;
		player.GetComponent<Player>().coinBar = _coinBar;
	}
}