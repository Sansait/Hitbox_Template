using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CRI.HitBoxTemplate.OSC
{
	public class OSC_Bag_Tilt : MonoBehaviour
	{
		private OSC_Sender _sender;

		// Start is called before the first frame update
		void Start()
		{
			_sender = GameObject.FindGameObjectWithTag("OSC").GetComponent<OSC_Sender>();
		}

		// Update is called once per frame
		void Update()
		{
			_sender.SendTilt(transform);
		}
	}

}
