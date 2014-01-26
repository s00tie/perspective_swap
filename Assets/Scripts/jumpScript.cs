using UnityEngine;
using System.Collections;

public class jumpScript : MonoBehaviour {

    public KeyCode actionKey;
    private float originalZPosition = 0f;
    private bool stateLocked = false;
    private direction currentDirectionOfMotion;
    private Vector2 moveDirection = Vector2.zero;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(actionKey))
        {
            if (!stateLocked)
            {
                originalZPosition = this.transform.position.z;
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 10);
                stateLocked = true;                
            }
        }

        if (stateLocked)
        {
            switch (currentDirectionOfMotion)
            {
                case direction.forward: moveDirection = new Vector2(0, 0.01f);
                    break;
                case direction.backwards: moveDirection = new Vector2(0, -0.01f);
                    break;
                case direction.left: moveDirection = new Vector2(-0.01f, 0);
                    break;
                case direction.right: moveDirection = new Vector2(0.01f, 0);
                    break;
            }


            transform.Translate(new Vector3(moveDirection.x, moveDirection.y, 0));

            CheckActionKeys();
        }
        else
        {

            if (Input.GetAxis("Horizontal") > 0)
                currentDirectionOfMotion = direction.right;
            else
                if (Input.GetAxis("Horizontal") < 0)
                    currentDirectionOfMotion = direction.left;

            if (Input.GetAxis("Vertical") > 0)
                currentDirectionOfMotion = direction.forward;

            if (Input.GetAxis("Vertical") < 0)
                currentDirectionOfMotion = direction.backwards;
        }
    }

    private void CheckActionKeys()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.rotation *= Quaternion.AngleAxis(180, new Vector3(0, 0, 1));
            stateLocked = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
    }

}


enum direction
{
    left,
    right,
    forward,
    backwards
}
    //Vector3 target;
    //public float jumpDistance = 30;
    //Vector3 trajectoryPath = Vector3.zero;
    //Vector3 projectileVelocity = Vector3.zero;
    //public float angle = 45;
    //public float xAxisVelocity = 10;
    //public float height = 10;
    //private Vector3 initialVelocity = new Vector3();
//{
//        if (trajectoryPath != Vector3.zero)
//        {
//            if(projectileVelocity.x == 0)
//            {
//                projectileVelocity.x = xAxisVelocity;
//            }

//            //float t = Mathf.Sqrt((float)(projectileVelocity.x * projectileVelocity.x * Mathf.Sin(angle) * Mathf.Sin(angle)) - (float)(2 * 9.8 * trajectoryPath.z));
//            float t =  0.5f * 9.8f * Time.fixedDeltaTime * Time.fixedDeltaTime + initialVelocity.x * Time.fixedDeltaTime + initialVelocity.z;

//            if (!double.IsNaN(t))
//                projectileVelocity.z = t;

//            else
//            {
//            }


//            trajectoryPath += projectileVelocity;

//            this.transform.position = trajectoryPath;
//            if (trajectoryPath.x >= target.x)
//            {
//                projectileVelocity = Vector3.zero;
//                trajectoryPath = Vector3.zero;
//            }

//            initialVelocity = projectileVelocity;
//        }
//        if (Input.GetKey(KeyCode.Space))
//        {
//            target = this.transform.position;
//            target.y += jumpDistance;

//            trajectoryPath = this.transform.position;
//        }
//    }