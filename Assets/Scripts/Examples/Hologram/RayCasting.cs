using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CRI.HitBoxTemplate.Serial;

namespace CRI.HitBoxTemplate.RayCasting
{
	public class RayCasting : MonoBehaviour
	{
		[SerializeField]
		private GameObject bag;
		/// <summary>
		/// LED controller => SerialLedController.cs
		/// </summary>
		private SerialLedController controller;
		/// <summary>
		/// Player position
		/// </summary>
		private Vector3 _posPlayer;
		/// <summary>
		/// Unit Forward Vector for the bag
		/// </summary>
		private Vector3 _vecForward;
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
		/// <summary>
		/// Distance between tracker and LED
		/// </summary>
		private float _distLED;
		/// <summary>
		/// Texture to map on the bag
		/// </summary>
		private Texture2D _texture;

		// Start is called before the first frame update
		void Start()
		{
			SerialLedController.rayCasting = true; //Deactivating display in SerialLedController when raycasting holograms
			_deltaLine = bag.transform.lossyScale.y / 32f;
			_deltaColumn = 360f / 70f;
		}

		private bool Init_SerialLedController()
		{
			if (!controller)
				controller = GameObject.FindGameObjectWithTag("SerialLedController").GetComponent<SerialLedController>();
			if (controller)
				return true;
			else
				return false;
		}

		private Vector3 RotatePointAroundAxis(Vector3 point, float angle, Vector3 axis)
		{
			Quaternion q = Quaternion.AngleAxis(angle, axis);
			return q * point;
		}

		/// <summary>
		/// Computing ray values
		/// </summary>
		/// <param name="i">LED column index.</param>
		/// <param name="j">LED line index.</param>
		/// <returns>The computed ray.</returns>
		private Ray Init_Ray(int i, int j)
		{
			Vector3 _vecLED_Forward = RotatePointAroundAxis(_vecGrid, ((-j) + 32) * _deltaColumn, bag.transform.up);
			Vector3 _vecLED_Up = bag.transform.up / 52 * (i - 31 * _deltaLine);
			Vector3 _posLED = bag.transform.position + _vecLED_Up - (bag.transform.up * bag.transform.lossyScale.y) + _vecLED_Forward / 2;
			Vector3 _dir = _posLED - _posPlayer;
			_distLED = Vector3.Magnitude(_dir);
			Ray _ray = new Ray(_posPlayer, _dir);

			return _ray;
		}

		// Update is called once per frame
		void Update()
		{
			if (!Init_SerialLedController())
				return;
			_texture = new Texture2D(64, 64);

			_vecForward = Vector3.Normalize(bag.transform.forward);
			_posPlayer = transform.position;
			_vecGrid = _vecForward * bag.transform.lossyScale.x;

			for (int i = 0; i < 64; i++)
			{
				for (int j = 0; j < 64; j++)
				{
					Color color = new Color(0, 0, 0);
					float _dist = Mathf.Infinity;
					Ray _ray = Init_Ray(i, j);

					RaycastHit[] hits = Physics.RaycastAll(_ray, 100, 1 << 11);
					for (int k = 0; k < hits.Length; k++)
					{
						RaycastHit hit = hits[k];
						if (hit.collider.tag == "Cube_To_Render" && hit.distance < _dist && hit.distance > _distLED)
						{
							_dist = hit.distance;
							color = hit.collider.GetComponent<MeshRenderer>().material.color;
						}
					}
					_texture.SetPixel(j, i, color);
				}
			}
			_texture.Apply();
			controller.SetPixelColor(_texture);
		}
	}
}
