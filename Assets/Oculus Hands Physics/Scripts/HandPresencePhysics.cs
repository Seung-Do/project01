using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandPresencePhysics : MonoBehaviour
{
    [SerializeField] private GameObject followObject;
    [SerializeField] private float followSpeed = 50f;
    [SerializeField] private float rotateSpeed = 150f;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;
    private Transform _followTarget;
    private Rigidbody _body;
    void Start()
    {
        _followTarget = followObject.transform;
        _body = GetComponent<Rigidbody>();
        _body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _body.interpolation = RigidbodyInterpolation.Interpolate;
        _body.mass = 5f;

        _body.position = _followTarget.position;
        _body.rotation = _followTarget.rotation;
    }

    private void FixedUpdate()
     {
         PhysicsMove();
     }

    private void PhysicsMove()
    {
        // Position
        var positionWithOffset = _followTarget.TransformPoint(positionOffset);
        var distance = Vector3.Distance(positionWithOffset, transform.position);
        _body.velocity = (positionWithOffset - transform.position).normalized * (followSpeed * distance);

        // Rotation
        var rotationWithOffset = _followTarget.rotation * Quaternion.Euler(rotationOffset);
        var q = rotationWithOffset * Quaternion.Inverse(_body.rotation);
        q.ToAngleAxis(out float angle, out Vector3 axis);
        if (Mathf.Abs(axis.magnitude) != Mathf.Infinity)
        {
            if (angle > 180.0f) { angle -= 360.0f; }
            _body.angularVelocity = axis * (angle * Mathf.Deg2Rad * rotateSpeed);
        }
    }
}
