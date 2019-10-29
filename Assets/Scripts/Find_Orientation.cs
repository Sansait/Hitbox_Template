using UnityEngine;
using CRI.HitBoxTemplate.OSC;

namespace Valve.VR
{
	public class Find_Orientation : MonoBehaviour
	{
		[SerializeField]
		[Tooltip("Tracked object to point toward")]
		private SteamVR_TrackedObject_OnlyTilt bag;

		private OSC_Sender _sender;


		// Start is called before the first frame update
		void Start()
		{
			_sender = GameObject.FindGameObjectWithTag("OSC").GetComponent<OSC_Sender>();
		}

		// Update is called once per frame
		void Update()
		{
			Vector3 _bagDir = bag.posTracker - transform.position; //Computing vector between player and bag
			_bagDir = Vector3.Normalize(new Vector3(_bagDir.x, 0, _bagDir.z)); //Getting rid of the height for the vector and normalizing
			Vector3 _playerOrientation = Vector3.Normalize(transform.up); //Normalizing player orientation

			float _angle = Vector3.SignedAngle(_bagDir, _playerOrientation, Vector3.up);
			//Debug.Log("Bag position " + bag.posTracker + " Player position " + transform.position + " Angle " + _angle + " BagDir " + _bagDir + " PlayerDir " + _playerOrientation);

			//Debug.DrawRay(transform.position, transform.up, Color.green);
			//Debug.DrawRay(transform.position, _bagDir, Color.red);
			_sender.SendAngle(_angle);
			
		}
	}
}
