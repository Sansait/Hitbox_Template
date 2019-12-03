using UnityEngine;
using CRI.HitBoxTemplate.OSC;

namespace Valve.VR
{
	public class Player_Orientation : MonoBehaviour
	{
		[SerializeField]
		[Tooltip("Tracked object to point toward")]
		private GameObject bag;


		// Update is called once per frame
		void Update()
		{
			Vector3 _bagDir = bag.transform.position - transform.position; //Computing vector between player and bag
			float dist = Vector3.Magnitude(_bagDir); //Distance between player and bag tracker
			_bagDir = Vector3.Normalize(new Vector3(_bagDir.x, 0, _bagDir.z)); //Getting rid of the height for the vector and normalizing
			Vector3 _playerOrientation = Vector3.Normalize(transform.up); //Normalizing player orientation

			float _angle = Vector3.SignedAngle(_bagDir, _playerOrientation, Vector3.up);

			OSC_Sender.Instance.SendAnglePlayer(_angle);
			OSC_Sender.Instance.SendDistance(dist);
		}
	}
}
