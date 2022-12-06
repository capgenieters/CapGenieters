using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CablePhysics : MonoBehaviour
{
    Transform currentTransform;
    Rigidbody previousBody = null;

    private void Start()
    {
        currentTransform = transform;
        SoftJointLimitSpring sjls = new SoftJointLimitSpring();
        sjls.damper = 1;
        sjls.spring = 1;
        JointDrive drive = new JointDrive();
        drive.maximumForce = Mathf.Pow(2, 32);
        drive.positionDamper = 0.1f;
        drive.positionSpring = 120f;

        while (currentTransform.childCount > 0)
        {
            currentTransform = currentTransform.GetChild(0);
            currentTransform.gameObject.AddComponent<Rigidbody>();
            currentTransform.gameObject.AddComponent<CapsuleCollider>();
            currentTransform.gameObject.AddComponent<CharacterJoint>();
            Rigidbody rigidBody = currentTransform.GetComponent<Rigidbody>();
            rigidBody.mass = 0.1f;
            rigidBody.drag = 2.0f;
            rigidBody.angularDrag = 1.0f;
            rigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            CharacterJoint joint = currentTransform.GetComponent<CharacterJoint>();
            joint.lowTwistLimit = GetLimit(-45.0f);
            joint.highTwistLimit = GetLimit(45.0f);
            joint.swing1Limit = GetLimit(120.0f, true);
            joint.swing2Limit = GetLimit(120.0f, true);
            joint.twistLimitSpring = sjls;
            joint.swingLimitSpring = sjls;
            //joint.xDrive = drive;
            //joint.yDrive = drive;
            //joint.zDrive = drive;
            //joint.angularXMotion = ConfigurableJointMotion.Locked;
            //joint.angularYMotion = ConfigurableJointMotion.Locked;
            //joint.angularZMotion = ConfigurableJointMotion.Locked;
            //joint.xMotion = ConfigurableJointMotion.Locked;
            //joint.yMotion = ConfigurableJointMotion.Locked;
            //joint.zMotion = ConfigurableJointMotion.Locked;
            CapsuleCollider collider = currentTransform.GetComponent<CapsuleCollider>();
            collider.radius = 0.001f;
            collider.height = currentTransform.localPosition.y / 4;

            if (previousBody != null)
                joint.connectedBody = previousBody;

            previousBody = currentTransform.GetComponent<Rigidbody>();
        }
    }
    private SoftJointLimit GetLimit(float amt, bool cd = false)
    {
        SoftJointLimit limit = new SoftJointLimit();
        limit.limit = amt;

        if (cd)
            limit.contactDistance = 5.0f;

        return limit;
    }
}
