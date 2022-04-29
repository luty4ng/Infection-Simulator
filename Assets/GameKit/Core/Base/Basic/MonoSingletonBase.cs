using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameKit;

namespace GameKit
{
    public abstract class MonoSingletonBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T Current;
        public static T current
        {
            get
            {
                return Current;
            }
            set
            {
                Current = value;
            }
        }
        private void Awake()
        {
            Current = this as T;
            OnAwake();
        }
        
        protected virtual void OnAwake() {}
    }
}