using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace CameraLogic
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private float rotationAngleX;
        [SerializeField] private float distance;
        [SerializeField] private float offsetY;

        private Transform _following;
        private void LateUpdate()
        {
            if (_following == null)
                return;

            var rotation = Quaternion.Euler(rotationAngleX, 0, 0);
            transform.rotation = rotation;

            var position = rotation * new Vector3(0, 0, -distance) + FollowingPointPosition();

            transform.position = position;
        }

        public void Follow(GameObject following)
        {
            _following = following.transform;
        }

        private Vector3 FollowingPointPosition()
        {
            Vector3 followingPosition = _following.position;
            followingPosition.y += offsetY;
            
            return followingPosition;
        }
    }
}