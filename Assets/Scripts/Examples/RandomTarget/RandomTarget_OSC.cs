using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CRI.HitBoxTemplate.Serial;
using CRI.HitBoxTemplate.OSC;



namespace CRI.HitBoxTemplate.Example
{
	public class RandomTarget_OSC : MonoBehaviour
	{
		private static int _incPlayer = 0;
		/// <summary>
		/// Index of the player that initiated the hit.
		/// </summary>
		[SerializeField]
		private int playerIndex;

		public GameObject bag;
		/// <summary>
		/// Forward Vector for the bag
		/// </summary>
		private Vector3 _vecGrid;
		/// <summary>
		/// Space between lines
		/// </summary>
		private float _deltaLine;
		/// <summary>
		/// Angle between columns
		/// </summary>
		private float _deltaColumn;

		private void Start()
		{
			SerialLedController.rayCasting = true; //Deactivating display in SerialLedController when raycasting holograms
			_deltaLine = bag.transform.lossyScale.y / 32f;
			_deltaColumn = 360f / 70f;
			playerIndex = _incPlayer;
			_incPlayer++;
			Vector3 _vecForward = Vector3.Normalize(bag.transform.forward);
			_vecGrid = _vecForward * bag.transform.lossyScale.x;
			Vector3 newPos = FindPosLed(32, 32);
			this.transform.position = newPos;
			OSC_Sender.Instance.SendTargetPosition(32, 32);
		}

		private void OnEnable()
		{
			ImpactPointControl.onImpact += OnImpact;
		}

		private void OnDisable()
		{
			ImpactPointControl.onImpact -= OnImpact;
		}

		private void OnImpact(object sender, ImpactPointControlEventArgs e)
		{

			OnImpact(e.impactPosition, e.playerIndex);
		}

		private Vector3 RotatePointAroundAxis(Vector3 point, float angle, Vector3 axis)
		{
			Quaternion q = Quaternion.AngleAxis(angle, axis);
			return q * point;
		}

		private Vector3 FindPosLed(float i, float j)
		{
			Vector3 _vecTarget_Forward = RotatePointAroundAxis(_vecGrid, ((-j) - 32) * _deltaColumn, bag.transform.up);
			Vector3 _vecLED_Up = bag.transform.up / 52 * (i - 31 * _deltaLine);
			Vector3 _posLED = bag.transform.position + _vecLED_Up - (bag.transform.up * bag.transform.lossyScale.y) + _vecTarget_Forward / 2;

			return (_posLED);
		}

		private void OnImpact(Vector2 pos, int playerIndex)
		{
			if (this.playerIndex == playerIndex)
			{
				Vector3 _vecForward = Vector3.Normalize(bag.transform.forward);
				_vecGrid = _vecForward * bag.transform.lossyScale.x;
				pos.x = (pos.x + 80f) / 2.5f; //calibration for LED position
				pos.y = (pos.y + 100f) / 3.125f; //do not question the math, just accept it
				Vector3 posHit = FindPosLed(pos.y, pos.x);
				Vector3 closest = this.GetComponent<SphereCollider>().ClosestPoint(posHit);
				if (closest == posHit)
				{
					Debug.Log("Hit");
					int[] newTarget = new int[] { Random.Range(10, 55), Random.Range(10, 55) };
					Vector3 newPos = FindPosLed(newTarget[0], newTarget[1]);
					this.transform.position = newPos;
					OSC_Sender.Instance.SendTargetPosition(newTarget[1], newTarget[0]);
				}
				OSC_Sender.Instance.SendHit(pos);
			}
		}
	}
}

