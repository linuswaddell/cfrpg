using System;
using System.Reflection;
using UnityEngine;

namespace AI.Trees.Nodes
{
    // Repeatedly recreates and runs a Node for the given Task, using the given
    // constructor arguments.
    public class Repeater : Node
    {
        private readonly Task task;
        private Node current;
    
        public Repeater(Task task)
        {
            this.task = task;
        }

        protected override void Init() { }

        protected override void OnCancel()
        {
            if (current != null && !current.Stopped) current.Cancel();
        }

        protected override Status OnUpdate()
        {
            current ??= task.CreateNode();

            if (current.Update() != Status.Running)
            {
                current = null;
            }
            return Status.Running;
        }
    }
}