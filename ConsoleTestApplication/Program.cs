using ConsoleTestApplication.Service;
using System;

namespace ConsoleTestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            LambdaTest.CreateList();
            var t1 = LambdaTest.LambdaDirectCall();
            var t2 = LambdaTest.LambdaCompiled();
            var t3 = LambdaTest.LambdaGenerated();
            Console.WriteLine($"{t1}{Environment.NewLine}{t2}{Environment.NewLine}{t3}");
            Console.ReadKey();
        }
    }
}
