using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
	void Start()
	{
        Invoke("Explode", 2.0f);

        LeanTween.scale(gameObject, new Vector3(6.0f, 6.0f, 6.0f), 0.5f).setEase(LeanTweenType.easeOutExpo).setDelay(2.0f).setDestroyOnComplete(true);
        LeanTween.alpha(gameObject, 0f, 0.5f).setDelay(2.0f);
	}

    void Explode()
    {
        rigidbody.useGravity = false;
        collider.enabled = false;

        GameObject[] Boxes = GameObject.FindGameObjectsWithTag("Box");

        for (int i = 0; i < Boxes.Length; i++)
        {
            Boxes[i].rigidbody.AddExplosionForce(2000f, transform.position, 6.0f);
        }
    }
}
