using UnityEngine;
using System.Collections;

enum Direction
{
    LEFT,
    RIGHT,
}

public class Valken : MonoBehaviour
{
    Animator anim;
    Direction dir = Direction.RIGHT;

    public GameObject Bomb;
    public ParticleSystem RightMuzzle, LeftMuzzle, RightFire, LeftFire;
    public Transform LeftArm, RightArm;
    public ParticleSystem Boost;
    public Transform MissilePoint;


    public JoyStick MyStick;
    public GameButton Abutton, Bbutton;

    public Light LeftLight, RightLight;

    public float speed = 4.0f;
    public bool isMoving = false;
    float JumpTimer = 0f;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        anim.Play("Walk");
        RightMuzzle.emissionRate = RightFire.emissionRate = LeftMuzzle.emissionRate = LeftFire.emissionRate = 0;
    }

    IEnumerator LightControl()
    {
        while (true)
        {
            LeftLight.intensity = RightLight.intensity = 1.0f;
            yield return new WaitForSeconds(0.03f);
            LeftLight.intensity = RightLight.intensity = 0f;
            yield return new WaitForSeconds(0.03f);
        }
    }

    bool isGrounded()
    {
        return Physics.Raycast(transform.position + transform.forward * 0.4f + transform.up * 0.1f, Vector3.down, 0.1f);
    }

    public GameObject RayGround()
    {
        GameObject temp = null;
        RaycastHit hit;
        Physics.Raycast(transform.position + transform.forward * 0.4f + transform.up * 0.1f, Vector3.down, out hit, 0.1f);
        if (hit.collider != null) temp = hit.collider.gameObject;
        return temp;
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || (MyStick.isClick && MyStick.Degree > 180f))
        {
            if (!LeanTween.isTweening(gameObject))
            {
                if (isGrounded()) anim.Play("Walk");
                else anim.Play("Idle");

                if (dir != Direction.LEFT)
                {
                    LeanTween.rotateAroundLocal(gameObject, Vector3.up, 180f, 0.3f).setOnComplete(TurnLeft);
                }
                else
                {
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);
                }
                isMoving = true;
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow) || (MyStick.isClick && MyStick.Degree <= 180f))
        {
            if (!LeanTween.isTweening(gameObject))
            {
                if (isGrounded()) anim.Play("Walk");
                else anim.Play("Idle");

                if (dir != Direction.RIGHT)
                {
                    LeanTween.rotateAroundLocal(gameObject, Vector3.up, -180f, 0.3f).setOnComplete(TurnRight);
                }
                else
                {
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);
                }
                isMoving = true;
            }
        }
        else
        {
            anim.Play("Idle");
            isMoving = false;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            RightArm.Rotate(Vector3.back * 200f * Time.deltaTime);
            LeftArm.Rotate(Vector3.back * 200f * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            RightArm.Rotate(Vector3.forward * 200f * Time.deltaTime);
            LeftArm.Rotate(Vector3.forward * 200f * Time.deltaTime);
        }

        if (MyStick.isClick)
        {
            if (dir == Direction.RIGHT)
            {
                RightArm.localRotation = Quaternion.Euler(new Vector3(0f, 90f, MyStick.Degree - 90f));
                LeftArm.localRotation = Quaternion.Euler(new Vector3(0f, 90f, MyStick.Degree + 90f));
            }
            else
            {
                RightArm.localRotation = Quaternion.Euler(new Vector3(0f, 90f, 270f - MyStick.Degree));
                LeftArm.localRotation = Quaternion.Euler(new Vector3(0f, 90f, 90f - MyStick.Degree));
            }
        }

        if (Input.GetKey(KeyCode.Z) || Bbutton.isClick)
        {
            rigidbody.constantForce.force = Vector3.zero;
            if (rigidbody.velocity.y < 4f) rigidbody.AddRelativeForce(Vector3.up * 20f);

            if (!Boost.loop)
            {
                Boost.Play();
                Boost.loop = true;
            }
            isMoving = true;
        }
        else
        {
            rigidbody.constantForce.force = new Vector3(0f, -10f, 0f);
            Boost.loop = false;
        }

        if (Input.GetKey(KeyCode.X) || Abutton.isClick)
        {
            if (!audio.isPlaying)
            {
                audio.Play();
                StartCoroutine("LightControl");
            }
            RightMuzzle.emissionRate = LeftMuzzle.emissionRate = 10;
            RightFire.emissionRate = LeftFire.emissionRate = 30;
        }
        else
        {
            audio.Stop();
            RightMuzzle.emissionRate = RightFire.emissionRate = LeftMuzzle.emissionRate = LeftFire.emissionRate = 0;
            LeftLight.intensity = RightLight.intensity = 0f;
            StopCoroutine("LightControl");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            LaunchMissile();
        }
        rigidbody.velocity = new Vector3(0f, rigidbody.velocity.y, 0f);
    }

    void LaunchMissile()
    {
        if (!LeanTween.isTweening(gameObject))
        {
            Vector3 pos = Vector3.zero;
            if (dir == Direction.RIGHT) pos = new Vector3(transform.position.x + 1.0f, transform.position.y + 1.0f, transform.position.z);
            if (dir == Direction.LEFT) pos = new Vector3(transform.position.x - 1.0f, transform.position.y + 1.0f, transform.position.z);

            for (int i = 0; i < 5; i++)
            {
                Vector3 origin = pos + Vector3.up * Random.Range(-1f, 1f) + Vector3.left * Random.Range(-1f, 1f);
                GameObject temp = Instantiate(Bomb, origin, Quaternion.AngleAxis(dir == Direction.RIGHT ? 0f : 180f, Vector3.up)) as GameObject;
                Vector3 tarPos = MissilePoint.position + MissilePoint.forward * 20f + MissilePoint.up * Random.Range(-1f, 1f);
                temp.SendMessage("LaunchMissile", tarPos);
            }
        }
    }

    void TurnLeft()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        dir = Direction.LEFT;
    }

    void TurnRight()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        dir = Direction.RIGHT;
    }
}
