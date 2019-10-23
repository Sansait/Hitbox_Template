using CRI.HitBoxTemplate.Serial;
using UnityEngine;

namespace CRI.HitBoxTemplate.OSC
{
	public class OSC_Sender : MonoBehaviour
	{
		[SerializeField]
		[Tooltip("Link OSC script for configuration")]
		private OSC osc;

		/// <summary>
		/// Sends bag tilt informations via OSC
		/// </summary>
		public void SendTilt(Transform trans)
		{
			OscMessage message;

			message = new OscMessage();
			message.address = "/BagTilt";
			message.values.Add(trans.localEulerAngles.x);
			message.values.Add(trans.localEulerAngles.y);
			message.values.Add(trans.localEulerAngles.z);
			osc.Send(message);
		}

		/// <summary>
		/// Sends bag acceleration informations via OSC
		/// </summary>
		public void SendAcceleration(Vector3 acceleration)
		{
			OscMessage message;

			message = new OscMessage();
			message.address = "/Accelerometre";
			message.values.Add(acceleration.x);
			message.values.Add(acceleration.y);
			osc.Send(message);
		}

		/// <summary>
		/// Sends hit informations via OSC
		/// </summary>
		public void SendHit(ImpactPointControlEventArgs e)
		{
			OscMessage message;

			message = new OscMessage();
			message.address = "/Hit";
			message.values.Add(e.impactPosition.x);
			message.values.Add(e.impactPosition.y);
			osc.Send(message);
		}

		/// <summary>
		/// Sends angle between player's orientation and the bag via OSC
		/// </summary>
		public void SendAngle(float angle)
		{
			OscMessage message;

			message = new OscMessage();
			message.address = "/Angle";
			message.values.Add(angle);
			osc.Send(message);
		}
	}
}
