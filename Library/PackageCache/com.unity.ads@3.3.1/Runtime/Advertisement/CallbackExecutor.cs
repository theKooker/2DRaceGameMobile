using System;
using System.Collections.Generic;

namespace UnityEngine.Advertisements
{
    [AddComponentMenu("")]
    sealed internal class CallbackExecutor : MonoBehaviour
    {
        readonly Queue<Action<CallbackExecutor>> s_Queue = new Queue<Action<CallbackExecutor>>();

        public void Post(Action<CallbackExecutor> action)
        {
            lock (s_Queue)
            {
                s_Queue.Enqueue(action);
            }
        }

        void Update()
        {
            lock (s_Queue)
            {
                while (s_Queue.Count > 0)
                {
                    s_Queue.Dequeue()(this);
                }
            }
        }
    }
}
