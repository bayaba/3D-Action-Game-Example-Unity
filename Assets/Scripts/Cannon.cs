using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour
{
    public Valken Player;
    public GameObject MeshParticle;

	void Update()
	{
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Player.transform.position - transform.position), 2.0f * Time.deltaTime);
	}

    void OnParticleCollision(GameObject tar)
    {
        if (tar.name == "Muzzle")
        {
            Damage();
        }
    }

    void Damage()
    {
        Instantiate(MeshParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
