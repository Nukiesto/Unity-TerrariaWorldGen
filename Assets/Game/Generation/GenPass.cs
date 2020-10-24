using System;

namespace Game.Generation
{
    public class GenPass
    {
        public string Name;
        public Func<bool> Func;

        public GenPass(string name, Func<bool> func)
        {
            Name = name;
            Func = func;
        }
    }
}