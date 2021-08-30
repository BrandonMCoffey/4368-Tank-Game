using Assets.Scripts.Interfaces;
using Assets.Scripts.Utility.CustomFloats;
using UnityEngine;

namespace Assets.Scripts.Collectibles {
    public class SpeedIncrease : CollectibleBase {
        [Header("Effect Settings")]
        [SerializeField] private ValueAdjustType _speedIncreaseType = ValueAdjustType.AddBase;
        [SerializeField] private float _amount = 1f;

        protected override bool OnCollect(GameObject other)
        {
            IMoveable moveableObject = other.GetComponent<IMoveable>();
            if (moveableObject == null) {
                return false;
            }
            // Permanently Increase the speed of the other object
            moveableObject.OnSpeedIncrease(_amount, _speedIncreaseType);

            return true;
        }

        protected override void Movement(Rigidbody rb)
        {
            Quaternion turnOffset = Quaternion.Euler(RotationSpeed, RotationSpeed, RotationSpeed);
            rb.MoveRotation(rb.rotation * turnOffset);
        }
    }
}