using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace CRI.HitBoxTemplate.Polar
{
	public class HeartBeatDisplay : MonoBehaviour
	{
		public Text scoreText;
		// Update is called once per frame
		private PolarReceiver Polar;

		private void Awake()
		{
			Polar = GameObject.Find("Polar").GetComponent<PolarReceiver>();
		}

		private void Update()
		{
			scoreText.text = "BPM : " + Polar.bpm;
		}
	}
}
