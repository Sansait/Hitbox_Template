using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CRI.HitBoxTemplate.Example
{
	public class SharedHits : MonoBehaviour
	{
		[SerializeField]
		[Tooltip("Time until the target fades away completely (in sec).")]
		private float _fadeTime;
		private MeshRenderer _renderer;
		private Color _targetColor;
		private float _startTime;
		private Color originalColor;

		private void Start()
		{
			_renderer = GetComponent<MeshRenderer>();
			originalColor = _renderer.material.color;
			_targetColor = Color.clear;
		}

		private void OnEnable()
		{
			_startTime = Time.time;
		}

		private void Update()	
		{
			if (_renderer.material.color.a > 0.005f)
			{
				_renderer.material.color = Color.Lerp(_renderer.material.color, _targetColor, (Time.time - _startTime) / _fadeTime);
			}
			else
			{
				this.gameObject.SetActive(false);
				_renderer.material.color = originalColor;
			}
		}
	}
}
