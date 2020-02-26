using CRI.HitBoxTemplate.Serial;
using UnityEngine;

namespace CRI.HitBoxTemplate.Example
{
	public class SimpleHitController : MonoBehaviour
	{
		[SerializeField]
		[Tooltip("Array of the prefab of the objects that will be put at the position of the impact. Each prefab is associated with its corresponding player index.")]
		private GameObject[] _hitPrefabs;

		private int _playerCount;

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
			if (e.playerIndex < _hitPrefabs.Length)
				Instantiate(_hitPrefabs[e.playerIndex], e.impactPosition, Quaternion.identity);
		}
	}
}
