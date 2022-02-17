using UnityEngine;

namespace HauntedModMenu.Utils
{
    // this could be usefull in the future
    public class ObjectTracker : MonoBehaviour
    {
        private float speed = 0f;
        private Vector3 lastPosition = Vector3.zero;
        private Vector3 direction = Vector3.zero;
        private Vector3 rawDirection = Vector3.zero;

        public float Speed {
            get { return speed > 0f ? speed / Time.deltaTime : 0f; }
        }

        public Vector3 Direction {
            get { return direction; }
        }

        public Vector3 RawDirection {
            get { return rawDirection; }
        }

        public virtual void OnEnable()
        {
            lastPosition = gameObject.transform.position;
        }

        public virtual void OnDisable()
        {
            lastPosition = Vector3.zero;
            rawDirection = Vector3.zero;
            direction = Vector3.zero;
            speed = 0f;
        }

        // late update so position changes in Update in other scripts are included
        public virtual void LateUpdate()
        {
            rawDirection = gameObject.transform.position - lastPosition;
            lastPosition = gameObject.transform.position;
            direction = rawDirection.normalized;
            speed = rawDirection.magnitude;

            // Debug.Log("Object Speed = " + Speed);
        }
    }
}