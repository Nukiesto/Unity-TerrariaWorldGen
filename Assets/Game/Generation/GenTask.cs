using System.Collections.Generic;

namespace Game.Generation
{
    public abstract class GenTask
    {
        public abstract void ModifyWorldGenTasks(List<GenPass> tasks);
    }
}