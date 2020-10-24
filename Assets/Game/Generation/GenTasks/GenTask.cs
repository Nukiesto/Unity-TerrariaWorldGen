using System.Collections.Generic;

namespace Game.Generation.GenTasks
{
    public abstract class GenTask
    {
        public abstract void ModifyWorldGenTasks(List<GenPass> tasks);
    }
}