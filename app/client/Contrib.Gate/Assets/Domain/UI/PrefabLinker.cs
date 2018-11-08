using UnityEngine;

namespace UI
{
    public class PrefabLinker : MonoBehaviour
    {
        public GameObject prefab;
        public bool runOnAwake = true;

        private void Awake()
        {
            if (runOnAwake) Run();
        }

        public void Run()
        {
            Instantiate(prefab, transform);
        }

    }
}
