using ConsoleTestApplication.Service;
using System;

namespace ConsoleTestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            LambdaTest.CreateList();
            Console.Write(LambdaTest.GetHeader());
            Console.WriteLine($"{Environment.NewLine}First test finalized on  {DateTime.Now.ToShortTimeString()}  {Environment.NewLine}" +
                $"RESULT> {Environment.NewLine}{LambdaTest.LambdaDirectCall()}");

            Console.WriteLine($"{Environment.NewLine}Second test finalized on  {DateTime.Now.ToShortTimeString()}  {Environment.NewLine}" +
                $"RESULT> {Environment.NewLine}{LambdaTest.LambdaCompiledCache()}");

            //Console.WriteLine($"{Environment.NewLine}Third test finalized on  {DateTime.Now.ToShortTimeString()}  {Environment.NewLine}" +
            //    $"RESULT> {Environment.NewLine}{LambdaTest.LambdaCompiled()}");

            Console.WriteLine($"{Environment.NewLine}Four test finalized on  {DateTime.Now.ToShortTimeString()}  {Environment.NewLine}" +
                $"RESULT> {Environment.NewLine}{LambdaTest.LambdaGeneratedCache()}");

            Console.WriteLine($"{Environment.NewLine}Fifth test finalized on  {DateTime.Now.ToShortTimeString()}  {Environment.NewLine}" +
                $"RESULT> {Environment.NewLine}{LambdaTest.LambdaGenerated()}");

            Console.ReadKey();
        }
    }
}
