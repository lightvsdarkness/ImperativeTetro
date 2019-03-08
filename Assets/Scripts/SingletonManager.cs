using System.Collections;
using UnityEngine;

namespace General
{
    public class SingletonManager<T> : MonoBehaviour where T : Component
    {
        protected static T instance;
        public static T I {
            get {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        instance = obj.AddComponent<T>();
                    }
                }
                return instance;
            }
        }

        [SerializeField]
        [Tooltip("Tick to log debug info to the Console window")]
        protected bool Debugging = false;
        public bool DebugLogging { get { return Debugging || Debug.isDebugBuild; } }

        protected virtual void Awake() {
            if (instance == null)
            {
                instance = this as T;
            }
            else
            {
                // ACHTUNG if(this is ManagerGame) return; // ACHTUNG Dirtiest of hacks!
                Destroy(gameObject);
            }
        }

    }
}
