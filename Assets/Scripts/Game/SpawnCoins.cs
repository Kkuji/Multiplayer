using Photon.Pun;
using UnityEngine;
using System.Linq;

public class SpawnCoins : MonoBehaviour
{
	[SerializeField] private float _amount;
	[SerializeField] private GameObject _coin;
	[SerializeField] private Vector2 _minPostion, _maxPosition;

	public void Spawn()
	{
		if (PhotonNetwork.PlayerList.Length > 1)
		{
			for (int i = 0; i < _amount; i++)
			{
				Vector2 randomPosition = new Vector2(Random.Range(_minPostion.x, _maxPosition.x), Random.Range(_minPostion.y, _maxPosition.y));
				PhotonNetwork.Instantiate(_coin.name, randomPosition, Quaternion.identity);
			}
		}
	}

	public void Clear()
	{
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
	}

	public float GetAmountCoins() => _amount;
}