using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        private readonly string _assignmentApi;

        public static object ConfigurationManager { get; private set; }

        static void Main(string[] args)
        {
            _assignmentApi = ConfigurationManager.AppSettings["assignment-api"];
        }
    }
}
