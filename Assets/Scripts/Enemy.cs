using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    Animator anim;
    public GameObject MeshParticle;
    public float speed = 1.0f;
    bool CanMove = true;

	void Start()
	{
        anim = GetComponentInChildren<Animator>();
        anim.Play("Walk");
	}
	
	void Update()
	{
        if (CanMove)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            if (!isGrounded() || CheckFront())
            {
                anim.Play("Idle");
                LeanTween.rotateAroundLocal(gameObject, Vector3.up, 180f, 0.5f).setOnComplete(CompleteMove);
                CanMove = false;
            }
        }
	}

    void CompleteMove()
    {
        anim.Play("Walk");
        CanMove = true;
    }

    bool CheckFront()
    {
        return Physics.Raycast(transform.position + transform.forward * 0.4f + transform.up * 0.5f, transform.forward, 1.0f);
    }

    bool isGrounded()
    {
        return Physics.Raycast(transform.position + transform.forward * 0.4f + transform.up * 0.1f, Vector3.down, 0.1f);
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
