// Copyright ABDULLAH ABDULBARI YOUSEF 2025. All rights reserved.

using UnityEngine;
using System.Collections.Generic;

namespace NeomGameDev
{
    /// <summary>
    /// Script to manage enabling, disabling, or toggling lists of GameObjects.
    /// </summary>
    public class ObjectToggleController : MonoBehaviour
    {
        [Tooltip("Objects to disable when using the method.")]
        public List<GameObject> Close = new List<GameObject>();

        [Tooltip("Objects to enable when using the method.")]
        public List<GameObject> Open = new List<GameObject>();

        [Tooltip("Objects to toggle their active state.")]
        public List<GameObject> Toggle = new List<GameObject>();


        public void RUN()
        {
            OPEN();
            CLOSE();
            Debug.Log("Button pressed. Called RUN()");
        }

        /// <summary>
        /// Disables all GameObjects in the disable list.
        /// </summary>
        public void OPEN() // CALL FIRST
        {
            Debug.Log("Called OPEN()");

            foreach (var obj in Open)
            {
                if (obj != null)
                    obj.SetActive(true);
            }
        }
        public void CLOSE()  // CALL LAST
        {
            Debug.Log("Called CLOSE()");

            foreach (var obj in Close)
            {
                if (obj != null)
                    obj.SetActive(false);
            }
        }

        /// <summary>
        /// Toggles the active state of all GameObjects in the toggle list.
        /// </summary>
        public void TOGGLE()
        {
            Debug.Log("Called TOGGLE()");

            foreach (var obj in Toggle)
            {
                if (obj != null)
                    obj.SetActive(!obj.activeSelf);
            }
        }
    }
}
