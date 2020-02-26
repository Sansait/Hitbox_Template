using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube_Rotation : MonoBehaviour
{
	[SerializeField]
	private GameObject bag;
	// Start is called before the first frame update
	void Start()
    {
	}

	// Update is called once per frame
	void Update()
    {
		transform.RotateAround(bag.transform.position, transform.up, 45 * Time.deltaTime);
    }
}
