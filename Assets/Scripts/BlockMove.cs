using UnityEngine;
using System.Collections;

public class BlockMove : MonoBehaviour
{
    Vector3 pos;
    public float TargetX = 0f, HorizonTimer = 2.0f;
    bool onPlayer = false;
    public Valken Player;

	void Start()
	{
        if (HorizonTimer != 0f)
        {
            LeanTween.moveX(gameObject, TargetX, HorizonTimer).setLoopPingPong();
        }
	}

    void Update()
    {
        if (Player.RayGround() == gameObject && !Player.isMoving && onPlayer)
        {
            Player.transform.position = transform.position - pos;
        }
        else
        {
            pos = transform.position - Player.transform.position;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Valken")
        {
            pos = transform.position - Player.transform.position;
            onPlayer = true;
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.name == "Valken")
        {
            onPlayer = false;
        }
    }
}
