using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
	private static PoolManager _instance;
	public static PoolManager Instance
	{
		get
		{
			if (_instance == null)
				Debug.LogError("Pool Manager is NULL.");
			return _instance;
		}
	}
	[SerializeField]
	private GameObject _objectContainer;
	[SerializeField]
	private GameObject _p1Prefab;
	[SerializeField]
	private List<GameObject> _p1Pool;
	[SerializeField]
	private GameObject _p2Prefab;
	[SerializeField]
	private List<GameObject> _p2Pool;
	[SerializeField]
	private int _poolQuantity;

	private void Awake()
	{
		_instance = this;
	}

    // Start is called before the first frame update
    void Start()
    {
		_p1Pool = GenerateHitsP1(_poolQuantity);
		_p2Pool = GenerateHitsP2(_poolQuantity);
    }

	List<GameObject> GenerateHitsP1(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			GameObject hit = Instantiate(_p1Prefab);
			hit.transform.parent = _objectContainer.transform;
			hit.SetActive(false);
			_p1Pool.Add(hit);
		}
		return _p1Pool;
	}

	List<GameObject> GenerateHitsP2(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			GameObject hit = Instantiate(_p2Prefab);
			hit.transform.parent = _objectContainer.transform;
			hit.SetActive(false);
			_p2Pool.Add(hit);
		}
		return _p2Pool;
	}

	public GameObject RequestHitObject(int playerIndex)
	{
		if (playerIndex == 0)
		{
			foreach(var hit in _p1Pool)
			{
				if (hit.activeInHierarchy == false)
				{
					hit.SetActive(true);
					return hit;
				}
			}
			GameObject newHit = Instantiate(_p1Prefab);
			newHit.transform.parent = _objectContainer.transform;
			newHit.SetActive(false);
			_p1Pool.Add(newHit);
			return newHit;
		}
		else if (playerIndex == 1)
		{
			foreach (var hit in _p2Pool)
			{
				if (hit.activeInHierarchy == false)
				{
					hit.SetActive(true);
					return hit;
				}
			}
			GameObject newHit = Instantiate(_p2Prefab);
			newHit.transform.parent = _objectContainer.transform;
			newHit.SetActive(false);
			_p2Pool.Add(newHit);
			return newHit;
		}
		return null;
	}
}
