using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Player : MonoBehaviour
{
    #region --- helper ---
    private enum RigAnimMode
    {
        off,
        inc,
        dec,
    }
    #endregion
    public float Movespeed = 3.5f;
    public float Turnspeed = 60f;
    public float Punchspeed = 5f;
    public ChainIKConstraint lefthand = null;
    public MultiAimConstraint head = null;
    public Transform target = null;
    public Transform[] box = new Transform[3];
    private RigAnimMode mode = RigAnimMode.off;

    private void Start()
    {
        lefthand.weight = 0;
        head.weight = 0;
    }

    private void Update()
    {
        //move
        float vert = Input.GetAxis("Vertical");
        float horz = Input.GetAxis("Horizontal");
        this.transform.Translate(Vector3.forward * vert * Movespeed * Time.deltaTime);
        this.transform.rotation *= Quaternion.AngleAxis(Turnspeed * horz * Time.deltaTime, Vector3.up);

        //punch
        if (Input.GetKeyDown(KeyCode.Z) == true)
        {
            head.weight = 1;
            lefthand.weight = 0;
            mode = RigAnimMode.inc;
            target.position = new Vector3(box[0].position.x, box[0].position.y, box[0].position.z);
        }
        else if (Input.GetKeyDown(KeyCode.X) == true)
        {
            head.weight = 1;
            lefthand.weight = 0;
            mode = RigAnimMode.inc;
            target.position = new Vector3(box[1].position.x, box[1].position.y, box[1].position.z);
        }
        else if (Input.GetKeyDown(KeyCode.C) == true)
        {
            head.weight = 1;
            lefthand.weight = 0;
            mode = RigAnimMode.inc;
            target.position = new Vector3(box[2].position.x, box[2].position.y, box[2].position.z);
        }
    }
    private void FixedUpdate()
    {
        switch (mode)
        {
            case RigAnimMode.inc:
                lefthand.weight = Mathf.Lerp(lefthand.weight, 1, Punchspeed * Time.deltaTime);
                if (lefthand.weight > 0.95f)
                {
                    lefthand.weight = 1;
                    mode = RigAnimMode.dec;
                }
                break;
            case RigAnimMode.dec:
                lefthand.weight = Mathf.Lerp(lefthand.weight, 0, Punchspeed * Time.deltaTime);
                if (lefthand.weight < 0.1f)
                {
                    head.weight = 0;
                    lefthand.weight = 0;
                    mode = RigAnimMode.off;
                }
                break;
        }
    }
}
