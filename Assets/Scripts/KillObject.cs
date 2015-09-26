using UnityEngine;
using System.Collections;

public class KillObject : MonoBehaviour
{
    public float DelayTime = 1.0f;

	void Start()
	{
        Destroy(gameObject, DelayTime);
	}
}
