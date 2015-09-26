using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour
{
    public GameObject Explosion;

	void LaunchMissile(Vector3 tarPos)
	{
        Invoke("SetActive", 0.9f);
        LeanTween.move(gameObject, tarPos, 1.6f).setEase(LeanTweenType.easeInBack).setOnComplete(Explode);
	}

    void SetActive()
    {
        collider.enabled = true;
    }

    void Explode()
    {
        Instantiate(Explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (col.tag == "Box")
            {
                col.rigidbody.AddExplosionForce(2000f, transform.position, 6.0f);
            }
            else if (col.tag == "Enemy")
            {
                col.SendMessage("Damage");
            }
            Explode();
        }
    }
}
