using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CRI.HitBoxTemplate.OSC;

namespace CRI.HitBoxTemplate.Example
{
	public class Target_Orientation : MonoBehaviour
	{
		[SerializeField]
		private GameObject _bag;
		private GameObject _target;
		private GameObject _player;

		private void Start()
		{
			_player = GameObject.Find("PlayerTracker");
		}

		// Update is called once per frame
		void Update()
		{
			Vector3 targetDir = Vector3.Normalize(this.transform.position - _bag.transform.position);
			targetDir.y = 0;
			targetDir = Vector3.Normalize(targetDir);
			Vector3 playerDir = Vector3.Normalize(_player.transform.position - _bag.transform.position);
			playerDir.y = 0;
			playerDir = Vector3.Normalize(playerDir);

			float _angle = Vector3.SignedAngle(playerDir, targetDir, Vector3.up);

			OSC_Sender.Instance.SendAngleTarget(_angle);
		}
	}
}
