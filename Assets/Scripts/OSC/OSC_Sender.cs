using System.Collections;
using System.Collections.Generic;
using CRI.HitBoxTemplate.Serial;
using UnityEngine;

namespace CRI.HitBoxTemplate.OSC
{
	public class OSC_Sender : MonoBehaviour
	{
		public OSC osc;

		public void SendTilt(Transform trans)
		{
			OscMessage message;

			message = new OscMessage();
			message.address = "/BagTilt";
			message.values.Add(trans.localRotation.x);
			message.values.Add(trans.localRotation.y);
			message.values.Add(trans.localRotation.z);
			osc.Send(message);
		}

		public void SendAcceleration(Vector3 acceleration)
		{
			OscMessage message;

			message = new OscMessage();
			message.address = "/Accelerometre";
			message.values.Add(acceleration.x);
			message.values.Add(acceleration.y);
			osc.Send(message);
		}

		public void SendHit(ImpactPointControlEventArgs e)
		{
			OscMessage message;

			message = new OscMessage();
			message.address = "/Hit";
			message.values.Add(e.impactPosition.x);
			message.values.Add(e.impactPosition.y);
			osc.Send(message);
		}
	}
}
