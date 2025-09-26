using UnityEngine;

namespace PlanA
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        private static bool applicationIsQuitting = false;

        public static T Instance
        {
            get
            {
                if (applicationIsQuitting)
                {
                    Debug.LogWarning("[Singleton] Instância de " + typeof(T) +
                                     " já foi destruída. Retornando null.");
                    return null;
                }

                return _instance;
            }
        }

        public virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Debug.LogWarning("[Singleton] Mais de uma instância de " + typeof(T) + " encontrada. Destruindo o GameObject adicional.");
                Destroy(gameObject);
            }
        }

        public virtual void OnApplicationQuit()
        {
            applicationIsQuitting = true;
        }

        public virtual void OnDestroy()
        {
            if (_instance == this)
            {
                applicationIsQuitting = true;
            }
        }
    }
}