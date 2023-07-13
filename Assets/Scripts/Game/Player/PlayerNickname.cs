using Photon.Pun;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PlayerNickname : MonoBehaviour
{
	[SerializeField] private float distanceY;

	private RectTransform _textTransform;
	private TextMeshProUGUI _nickname;

	private void Start()
	{
		_nickname = GetComponentInChildren<TextMeshProUGUI>();
		_textTransform = _nickname.GetComponent<RectTransform>();

		_nickname.text = GetComponent<PhotonView>().Owner.NickName;
	}

	private void Update()
	{
		_textTransform.SetPositionAndRotation(new Vector3(transform.position.x,
														  transform.position.y + distanceY,
														  _textTransform.position.z),
														  Quaternion.Euler(Vector3.zero));
	}
}