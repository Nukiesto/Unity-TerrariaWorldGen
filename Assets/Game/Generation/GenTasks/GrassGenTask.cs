using System.Collections.Generic;
using UnityEngine;

namespace Game.Generation.GenTasks
{
    public class GrassGenTask : GenTask
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks)
        {
            tasks.Insert(0, new GenPass("Surface Creation", SurfaceGeneration));
        }

        private static bool SurfaceGeneration()
        {
            Debug.Log("Created Surface");
            return false;
        }
    }
}