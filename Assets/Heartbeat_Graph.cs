using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace CRI.HitBoxTemplate.Polar
{
	public class Heartbeat_Graph : MonoBehaviour
	{
		[SerializeField]
		private Sprite circleSprite;
		private RectTransform graphContainer;
		private PolarReceiver polarReceiver;
		private float _graphHeight;
		private float _ymax;
		private float _xsize;

		private void Start()
		{
			graphContainer = GameObject.Find("GraphContainer").GetComponent<RectTransform>();
			List<byte> newList = new List<byte>() { 75, 89, 80, 80, 84, 87, 85, 110, 115, 120, 180 };
			polarReceiver = GameObject.Find("Polar").GetComponent<PolarReceiver>();
			_graphHeight = graphContainer.sizeDelta.y;
			_ymax = 200f;
			_xsize = graphContainer.sizeDelta.x / 30f;
			ShowGraph(polarReceiver.polarData);
		}

		private GameObject CreateCircle(Vector2 anchoredPosition)
		{
			GameObject gameobject = new GameObject("circle", typeof(Image));
			gameobject.transform.SetParent(graphContainer, false);
			gameobject.tag = "graph_point";
			gameobject.GetComponent<Image>().sprite = circleSprite;
			RectTransform rectTransform = gameobject.GetComponent<RectTransform>();
			rectTransform.anchoredPosition = anchoredPosition;
			rectTransform.sizeDelta = new Vector2(11, 11);
			rectTransform.anchorMin = new Vector2(0, 0);
			rectTransform.anchorMax = new Vector2(0, 0);
			return gameobject;
		}

		private void ShowGraph(List<byte> heartbeatData)
		{
			GameObject lastCircleGameObject = null;
			for (int i = 0; i < heartbeatData.Count; i++)
			{
				float xpos = _xsize + i * _xsize;
				float ypos = (heartbeatData[i] / _ymax) * _graphHeight;
				GameObject circleGameObject = CreateCircle(new Vector2(xpos, ypos));
				if (lastCircleGameObject)
					CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
				lastCircleGameObject = circleGameObject;
			}
		}

		private void CreateDotConnection(Vector2 posA, Vector2 posB)
		{
			GameObject gameObject = new GameObject("dotConnection", typeof(Image));
			gameObject.transform.SetParent(graphContainer, false);
			gameObject.tag = "graph_line";
			gameObject.GetComponent<Image>().color = new Color(0, 0, 0, .5f);
			RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
			Vector2 dir = (posB - posA).normalized;
			float dist = Vector2.Distance(posA, posB);
			rectTransform.anchorMin = new Vector2(0, 0);
			rectTransform.anchorMax = new Vector2(0, 0);
			rectTransform.sizeDelta = new Vector2(dist, 3f);
			rectTransform.anchoredPosition = posA + dir * dist * .5f;
			rectTransform.localRotation = Quaternion.Euler(0, 0, (float)(Math.Atan2(dir.y, dir.x) * Mathf.Rad2Deg));
		}

		private void RefreshDotConnection(Vector2 posA, Vector2 posB, GameObject line)
		{
			RectTransform rectTransform = line.GetComponent<RectTransform>();
			Vector2 dir = (posB - posA).normalized;
			float dist = Vector2.Distance(posA, posB);
			rectTransform.anchorMin = new Vector2(0, 0);
			rectTransform.anchorMax = new Vector2(0, 0);
			rectTransform.sizeDelta = new Vector2(dist, 3f);
			rectTransform.anchoredPosition = posA + dir * dist * .5f;
			rectTransform.localRotation = Quaternion.Euler(0, 0, (float)(Math.Atan2(dir.y, dir.x) * Mathf.Rad2Deg));
		}

		private void RefreshGraph(List<byte> heartbeatData)
		{
			GameObject[] graph_points = GameObject.FindGameObjectsWithTag("graph_point");
			GameObject[] graph_lines = GameObject.FindGameObjectsWithTag("graph_line");
			if (graph_points != null)
			{
				GameObject lastCircleGameObject = null;
				int i = 0;
				foreach (var circleGameObject in graph_points)
				{
					if (i >= heartbeatData.Count)
						break;
					float xpos = _xsize + i * _xsize;
					float ypos = (heartbeatData[i] / _ymax) * _graphHeight;
					circleGameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(xpos, ypos);
					if (lastCircleGameObject)
						RefreshDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition, graph_lines[i - 1]);
					lastCircleGameObject = circleGameObject;
					i++;
				}
				while (i < heartbeatData.Count)
				{
					float xpos = _xsize + i * _xsize;
					float ypos = (heartbeatData[i] / _ymax) * _graphHeight;
					GameObject circleGameObject = CreateCircle(new Vector2(xpos, ypos));
					if (lastCircleGameObject)
						CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
					lastCircleGameObject = circleGameObject;
					i++;
				}
			}
			else
			{
				ShowGraph(heartbeatData);
			}
		}

		private void Update()
		{
			RefreshGraph(polarReceiver.polarData);
		}
	}
}
