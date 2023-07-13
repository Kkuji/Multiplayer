using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class PlayerMover : MonoBehaviour
{
	[SerializeField] private float _speepMoving, _speedRotation;

	private Rigidbody2D _rigidbody;

	private Vector2 _direction;

	private PhotonView _view;

	private bool _alive = true;

	[HideInInspector] public FixedJoystick joystick;

	private void Start()
	{
		_view = GetComponent<PhotonView>();
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		if (_view.IsMine && PhotonNetwork.PlayerList.Length > 1 && _alive)
		{
			_rigidbody.velocity = new Vector2(joystick.Horizontal * _speepMoving, joystick.Vertical * _speepMoving);

			RotatePlayer();
		}
	}

	private void RotatePlayer()
	{
		_direction = new Vector2(joystick.Horizontal, joystick.Vertical);

		if (_direction != Vector2.zero)
		{
			Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, _direction);
			_rigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, toRotation, _speedRotation * Time.deltaTime));
		}
	}

	public void Die() => _alive = false;
}