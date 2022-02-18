using UnityEngine;

namespace HauntedModMenu.Utils
{
    // this could be usefull in the future
    public class ObjectTracker : MonoBehaviour
    {
        private float speed = 0f;
        private Vector3 lastPosition = Vector3.zero;
        private Vector3 currentPositon = Vector3.zero;
        private Vector3 rawDirection = Vector3.zero;

        public float Speed {
            get { return speed > 0f ? speed / Time.deltaTime : 0f; }
        }

        public virtual void OnEnable()
        {
            if (RefCache.PlayerTransform != null) {
                lastPosition = this.gameObject.transform.position - RefCache.PlayerTransform.position;

            } else {
                lastPosition = this.gameObject.transform.position;
            }
        }

        public virtual void OnDisable()
        {
            lastPosition = Vector3.zero;
            currentPositon = Vector3.zero;
            rawDirection = Vector3.zero;
            speed = 0f;
        }

        // late update so position changes in Update in other scripts are included
        public virtual void LateUpdate()
        {
            if (RefCache.PlayerTransform != null) {
                currentPositon = this.gameObject.transform.position - RefCache.PlayerTransform.position;

            } else {
                currentPositon = this.gameObject.transform.position;
            }

            rawDirection = currentPositon - lastPosition;
            lastPosition = currentPositon;
            speed = rawDirection.magnitude;

            // Debug.Log("Object Speed = " + Speed);
        }
    }
}