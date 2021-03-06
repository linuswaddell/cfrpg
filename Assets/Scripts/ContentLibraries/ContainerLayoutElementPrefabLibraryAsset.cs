using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * A ScriptableObject to hold the library of GUI prefabs which are used as
 * layout elements for dynamic containers.
 */
namespace ContentLibraries
{
    [CreateAssetMenu]
    public class ContainerLayoutElementPrefabLibraryAsset : ScriptableObject
    {
        public List<Entry> prefabs;

        [Serializable]
        public class Entry : IContentItem
        {
            public string id;
            public GameObject prefab;

            public string Id => id;
        }
    }
}