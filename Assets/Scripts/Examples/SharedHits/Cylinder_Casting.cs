using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CRI.HitBoxTemplate.Serial;

namespace CRI.HitBoxTemplate.RayCasting
{
	public class Cylinder_Casting : MonoBehaviour
	{
		[SerializeField]
		int p;
		[SerializeField]
		[Tooltip("Permanent Object to track")]
		private GameObject _permanent;
		[SerializeField]
		[Tooltip("Ephemeral Objects to instantiate")]
		private GameObject[] _hitPrefabs;
		[SerializeField]
		[Tooltip("Switch to hit instanciation")]
		private bool _ephemeral;
		/// <summary>
		/// LED controller => SerialLedController.cs
		/// </summary>
		private SerialLedController controller;
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
		/// Texture to map on the bag
		/// </summary>
		private Texture2D _texture;
		private Vector3 _vecLED_Forward;
		private Vector3 _vecLED_Up;

		// Start is called before the first frame update
		void Start()
		{
			SerialLedController.rayCasting = true; //Deactivating display in SerialLedController when raycasting holograms
			_deltaLine = this.transform.lossyScale.y / 32f;
			_deltaColumn = 360f / 70f;
		}

		private bool Init_SerialLedController()
		{
			if (!controller)
				controller = GameObject.Find("Serial Controller" + p).GetComponent<SerialLedController>();
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
		/// Computing position of LED in world space
		/// </summary>
		/// <param name="i">LED column index.</param>
		/// <param name="j">LED line index.</param>
		/// <returns>Returns the position in Vector3.</returns>
		private Vector3 FindPosLed(float i, float j)
		{
			_vecLED_Forward = RotatePointAroundAxis(_vecGrid, ((-j) - 32) * _deltaColumn, this.transform.up);
			_vecLED_Up = this.transform.up / 52 * (i - 31 * _deltaLine);
			Vector3 _posLED = this.transform.position + _vecLED_Up - (this.transform.up * this.transform.lossyScale.y) + _vecLED_Forward / 2;

			return (_posLED);
		}

		/// <summary>
		/// Initializing a black texture
		/// </summary>
		private void Init_Texture()
		{
			_texture = new Texture2D(64, 64);
			for (int i = 0; i < 64; i++)
			{
				for (int j = 0; j < 64; j++)
				{
					_texture.SetPixel(j, i, Color.black);
				}
			}
			_texture.Apply();
		}

		// Update is called once per frame
		void Update()
		{
			if (_ephemeral) //Switch between permanent objects and hit-instanciated ephemeral objects
				Ephemeral();
			else
				Permanent();
		}

		private void Ephemeral()
		{
			if (!Init_SerialLedController())
				return;
			Init_Texture();

			_vecForward = Vector3.Normalize(this.transform.forward);
			_vecGrid = _vecForward * this.transform.lossyScale.x;

			GameObject[] objs = GameObject.FindGameObjectsWithTag("Cube_To_Render");
			foreach (var obj in objs)
			{
				for (int i = 0; i < 64; i++)
				{
					for (int j = 0; j < 64; j++)
					{
						Vector3 posLed = FindPosLed(i, j);
						Vector3 closest = obj.GetComponent<CapsuleCollider>().ClosestPoint(posLed);
						if (closest == posLed)
						{
							_texture.SetPixel(j, i, obj.GetComponent<MeshRenderer>().material.color);
						}
					}
				}
			}
			_texture.Apply();
			if (controller)
				controller.SetPixelColor(_texture);
		}

		private void Permanent()
		{
			if (!Init_SerialLedController())
				return;
			_texture = new Texture2D(64, 64);

			_vecForward = Vector3.Normalize(this.transform.forward);
			_vecGrid = _vecForward * this.transform.lossyScale.x;

			for (int i = 0; i < 64; i++)
			{
				for (int j = 0; j < 64; j++)
				{
					Color color = new Color(0, 0, 0);
					Vector3 posLed = FindPosLed(i, j);
					Vector3 closest = _permanent.GetComponent<SphereCollider>().ClosestPoint(posLed);
					if (closest == posLed)
					{
						color = _permanent.GetComponent<MeshRenderer>().material.color;
					}
					_texture.SetPixel(j, i, color);
				}
			}
			_texture.Apply();
			if (controller)
				controller.SetPixelColor(_texture);
		}

		private void OnEnable()
		{
			if (_ephemeral)
				ImpactPointControl.onImpact += OnImpact;
		}

		private void OnDisable()
		{
			if (_ephemeral)
				ImpactPointControl.onImpact -= OnImpact;
		}

		private void OnImpact(object sender, ImpactPointControlEventArgs e)
		{
			if (e.playerIndex < _hitPrefabs.Length && e.playerIndex == p && _ephemeral)
			{
				_vecForward = Vector3.Normalize(this.transform.forward);
				_vecGrid = _vecForward * this.transform.lossyScale.x;
				Vector2 pos = e.impactPosition;
				pos.x = (pos.x + 80f) / 2.5f; //calibration for LED position
				pos.y = (pos.y + 100f) / 3.125f; //do not question the math, just accept it
				Vector3 posLed = FindPosLed(pos.y, pos.x);
				Quaternion q = Quaternion.LookRotation(_vecLED_Up, _vecLED_Forward);
				GameObject hitObject = PoolManager.Instance.RequestHitObject(e.playerIndex);
				hitObject.transform.rotation = q;
				hitObject.transform.position = posLed;
			}
		}
	}
}
