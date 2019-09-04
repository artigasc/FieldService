using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace FESA.SCM.UserLoadJob
{
    class Program
    {
        static void Main()
        {
            var host = new JobHost();
            host.Start();
            host.Call(typeof(Functions).GetMethod("LoadUsersFESA"));
            Console.ReadKey();
        }
    }
}
