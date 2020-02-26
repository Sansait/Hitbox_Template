using CRI.HitBoxTemplate.Serial;
using UnityEngine;

namespace CRI.HitBoxTemplate.OSC
{
	public class OSC_Sender : MonoBehaviour
	{
		private static OSC_Sender _instance;
		public static OSC_Sender Instance
		{
			get
			{
				if (_instance == null)
					Debug.LogError("OSC Sender is NULL.");

				return (_instance);
			}
		}

		private void Awake()
		{
			_instance = this;
		}

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
		public void SendHit(Vector2 pos)
		{
			OscMessage message;

			message = new OscMessage();
			message.address = "/Hit";
			message.values.Add(pos.x);
			message.values.Add(pos.y);
			osc.Send(message);
		}

		/// <summary>
		/// Sends angle between player's orientation and the bag via OSC
		/// </summary>
		public void SendAnglePlayer(float angle)
		{
			OscMessage message;

			message = new OscMessage();
			message.address = "/AnglePlayer";
			message.values.Add(angle);
			osc.Send(message);
		}

		/// <summary>
		/// Sends angle between player's orientation and the target via OSC
		/// </summary>
		public void SendAngleTarget(float angle)
		{
			OscMessage message;

			message = new OscMessage();
			message.address = "/AngleTarget";
			message.values.Add(angle);
			osc.Send(message);
		}

		/// <summary>
		/// Sends angle between player's orientation and the bag via OSC
		/// </summary>
		public void SendDistance(float dist)
		{
			OscMessage message;

			message = new OscMessage();
			message.address = "/Dist";
			message.values.Add(dist);
			osc.Send(message);
		}
		
		/// <summary>
		/// Sends angle between player's orientation and the bag via OSC
		/// </summary>
		public void SendTargetPosition(int x, int y)
		{
			OscMessage message;

			message = new OscMessage();
			message.address = "/TargetPos";
			message.values.Add(x);
			message.values.Add(y);
			osc.Send(message);
		}
	}
}
