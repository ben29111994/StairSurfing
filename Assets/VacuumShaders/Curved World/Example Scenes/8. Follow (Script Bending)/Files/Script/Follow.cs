using UnityEngine;
using System.Collections.Generic;

namespace VacuumShaders.CurvedWorld.Example
{
    [ExecuteInEditMode]
    public class Follow : MonoBehaviour
    {
        public BEND_TYPE bendType;

        public Transform parent;

        public bool recalculateRotation;

        [Space]
        public bool drawDebugLines;
        public float debugLineLength = 2;




        void Start()
        {
            if (parent == null)
            {
                parent = transform.parent;
            }
        }

        public void UpdateStep()
        {

            if (GameManager.Instance != null)
                if (GameManager.Instance.listStickMan[0] != null)
                    if (parent == null) parent = GameManager.Instance.listStickMan[0].transform;

            if (parent == null) return;

            if (parent == null || CurvedWorld_Controller.current == null)
            {
                //Do nothing
            }
            else if (CurvedWorld_Controller.current.enabled == false ||
                     CurvedWorld_Controller.current.gameObject.activeSelf == false ||
                     (CurvedWorld_Controller.current.disableInEditor && Application.isEditor && Application.isPlaying == false))
            {

                transform.position = Vector3.Lerp(transform.position, parent.position, Time.deltaTime * 4.0f);
               // transform.position = parent.position;
                transform.rotation = Quaternion.identity;
            }
            else
            {
                Vector3 tarPos = CurvedWorld_Controller.current.TransformPosition(parent.position, bendType);
                tarPos.x = 0.0f;
                transform.position = Vector3.Lerp(transform.position, tarPos, Time.deltaTime * 4.0f);


                if (recalculateRotation)
                    transform.rotation = CurvedWorld_Controller.current.TransformRotation(parent.position, parent.forward, parent.right, bendType);
            }
        }

        //[ContextMenu("Copy To All")]
        //private void Reset()
        //{
        //    Follow[] scripts = Resources.FindObjectsOfTypeAll<Follow>();

        //    if(scripts != null && scripts.Length > 0)
        //    {
        //        for (int i = 0; i < scripts.Length; i++)
        //        {
        //            if (scripts[i] != null && scripts[i].gameObject != null)
        //                scripts[i].bendType = bendType;
        //        }
        //    }
        //}

        private void OnDrawGizmos()
        {
            if (drawDebugLines)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, transform.position + transform.forward * debugLineLength);


                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, transform.position + transform.up * debugLineLength);


                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, transform.position + transform.right * debugLineLength);
            }
        }
    }
}
