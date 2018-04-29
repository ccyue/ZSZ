using IMyBLL;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBLL
{
    public class DogBLL : IDogBLL
    {
        public void Bark()
        {
            Console.WriteLine("旺旺~");
        }
    }
}
