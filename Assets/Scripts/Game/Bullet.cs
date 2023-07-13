using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView), typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
	[SerializeField] private float _damage;
	[SerializeField] private float _speed;

	private Rigidbody2D _rigidbody;

	private void Start()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_rigidbody.velocity = transform.up * _speed;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.GetComponent<Border>() != null)
		{
			Destroy(gameObject);
		}

		if (collision.gameObject.TryGetComponent(out Player player))
		{
			player.GetDamage(_damage);
		}
	}
}