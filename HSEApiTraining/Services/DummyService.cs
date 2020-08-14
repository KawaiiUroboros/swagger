using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HSEApiTraining
{
    public interface IDummyService
    {
        string DummyServiceGenerator(int number);
    }
    public class DummyService : IDummyService
    {
        private static Random rand = new Random();

        public string DummyServiceGenerator(int number)
        {
            return $"Random (0, {number}): {rand.Next(number)}";
        }
    }
}
