using CRI.HitBoxTemplate.Serial;
using UnityEngine;

namespace CRI.HitBoxTemplate.OSC
{
    public class OSC_SimpleHitSend : MonoBehaviour
    {
		[SerializeField]
		[Tooltip("Array of the prefab of the objects that will be put at the position of the impact. Each prefab is associated with its corresponding player index.")]
		private GameObject[] _hitPrefabs;
		[SerializeField]
		[Tooltip("Activate simple hit effect.")]
		private bool _simpleHit;

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
			{
				if (_simpleHit)
					Instantiate(_hitPrefabs[e.playerIndex], e.impactPosition, Quaternion.identity);
				OSC_Sender.Instance.SendHit(e);
			}
		}
	}
}
