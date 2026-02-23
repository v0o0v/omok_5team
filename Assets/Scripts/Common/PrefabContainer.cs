using System;
using UnityEngine;

namespace Omok
{
    /// <summary>
    /// A container class that associates a unique ID string with a specific Prefab.
    /// </summary>
    [Serializable]
    public class PrefabContainer
    {
        /// <summary> Unique identifier for the prefab. </summary>
        [SerializeField]
        public string ID;

        /// <summary> The GameObject prefab asset. </summary>
        [SerializeField]
        public GameObject Prefab;
    }
}
