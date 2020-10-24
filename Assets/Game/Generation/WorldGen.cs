using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Game.Generation.GenTasks;
using UnityEngine;

namespace Game.Generation
{
    public class WorldGen : MonoBehaviour
    {
        private List<GenPass> genTasks = new List<GenPass>();

        private void Awake()
        {
            Type taskType = typeof(GenTask);
            Assembly assembly = Assembly.GetExecutingAssembly();
            var tasks = assembly.GetTypes().Where(t => t.IsSubclassOf(taskType));

            foreach (Type task in tasks)
            {
                GenTask genTaskObject = Activator.CreateInstance(task) as GenTask;
                task.InvokeMember(
                    "ModifyWorldGenTasks",
                    BindingFlags.InvokeMethod,
                    null,
                    genTaskObject,
                    new object[] { genTasks }
                );
            }
        }

        private void Start()
        {
            foreach (GenPass task in genTasks)
            {
                Debug.Log(task.Name + ": " + task.Func.Method.Name);
                task.Func();
            }
        }
    }
}