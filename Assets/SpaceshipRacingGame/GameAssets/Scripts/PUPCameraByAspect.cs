using UnityEngine;

namespace Puppeteer
{
    public class PUPCameraByAspect : MonoBehaviour
    {
        public Camera targetCamera;
        public AspectGroup[] aspectGroups;

        [System.Serializable]
        public class AspectGroup
        {
            public float aspectRatio = 1.0f;
            public float fieldOfView = 50;
        }

        public bool updateNow = false;

        // Start is called before the first frame update
        void Start()
        {
            UpdateCamera();

            if (targetCamera == null) targetCamera = Camera.main;
        }

        public void UpdateCamera()
        {
            for (int aspectIndex = 0; aspectIndex < aspectGroups.Length; aspectIndex++)
            {
                if (targetCamera.aspect >= aspectGroups[aspectIndex].aspectRatio)
                {
                    targetCamera.fieldOfView = aspectGroups[aspectIndex].fieldOfView;

                    return;
                }
            }
        }

        private void OnValidate()
        {
            if (updateNow == true)
            {
                updateNow = false;

                UpdateCamera();
            }
        }
    }
}
