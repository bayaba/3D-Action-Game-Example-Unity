using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {

	public float followSpeed = 20;
    public Transform TargetPlayer = null;

	void Update()
    {
        if (TargetPlayer != null)
        {
            Vector3 start = transform.position;
            Vector3 pos = new Vector3(TargetPlayer.position.x, TargetPlayer.position.y + 3.0f, TargetPlayer.position.z);
            Vector3 end = Vector3.MoveTowards(start, pos, followSpeed * Time.deltaTime);

            end.z = start.z;
            transform.position = end;
        }
    }
}
