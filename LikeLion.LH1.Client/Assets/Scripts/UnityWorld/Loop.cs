using LikeLion.LH1.Client.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LikeLion.LH1.Client.UnityWorld
{
    public class Loop : MonoBehaviour, ILoop
    {
        public readonly LinkedList<IUpdatable> updatables = new LinkedList<IUpdatable>();

        public void Add(IUpdatable updatable)
        {
            updatables.AddLast(updatable);
        }

        public void Remove(IUpdatable updatable)
        {
            updatables.Remove(updatable);
        }

        private void OnDestroy()
        {
            updatables.Clear();
        }

        private void Update()
        {
            var node = updatables.First;
            while (node != null)
            {
                var current = node;
                node = node.Next;
                current.Value.Update();
            }
        }
    }
}
