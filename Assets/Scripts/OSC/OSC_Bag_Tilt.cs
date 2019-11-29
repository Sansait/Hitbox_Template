using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CRI.HitBoxTemplate.OSC
{
	public class OSC_Bag_Tilt : MonoBehaviour
	{
		// Update is called once per frame
		void Update()
		{
			OSC_Sender.Instance.SendTilt(transform);
		}
	}

}
