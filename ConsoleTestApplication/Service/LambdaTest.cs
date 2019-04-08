using Generic.Service.Attributes.Lambda;
using Generic.Service.Entity.IFilter;
using Generic.Service.Extensions.Commom;
using Generic.Service.Extensions.Filter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ConsoleTestApplication.Service
{
    public static class LambdaTest
    {
        private static int _MAX = 1000000;
        private static int _COMP = 150000;
        public static List<FakeObject> testList { get; set; }


        public static void CreateList()
        {
            testList = new List<FakeObject>();
            foreach (int val in Enumerable.Range(1, _MAX))
            {
                testList.Add(new FakeObject() { value = val });
            }
        }

        public static string LambdaDirectCall()
        {
            var timer = new Stopwatch();
            int t = 0;
            timer.Start();
            for (int i = 0; i < 10000; i++)
                t = testList.Where(x => x.value >= _COMP).Count();
            timer.Stop();
            return $"Total size {testList.Count()} - Total result {t} {Environment.NewLine}Direct call has executed in {timer.ElapsedMilliseconds} ms.";
        }

        public static string LambdaCompiled()
        {
            FilterFakeObject filter = new FilterFakeObject();
            filter.value = _COMP;
            Commom.SetSizeByLengthProperties("ConsoleTestApplication", "ConsoleTestApplication");
            Commom.SaveOnCacheIfNonExists<FakeObject>();
            Commom.SaveOnCacheIfNonExists<FilterFakeObject>();

            var timer = new Stopwatch();
            var queryable = testList.AsQueryable();
            int t = 0;
            var func = filter.GenerateLambda<FakeObject, FilterFakeObject>().Compile();
            timer.Start();
            for (int i = 0; i < 10000; i++)
                t = queryable.Where(x => func(x)).Count();
            timer.Stop();
            return $"Total size {testList.Count()} - Total result {t} {Environment.NewLine}Compiled call has executed in {timer.ElapsedMilliseconds} ms.";
        }

        public static string LambdaGenerated()
        {
            FilterFakeObject filter = new FilterFakeObject();
            filter.value = _COMP;
            Commom.SetSizeByLengthProperties("ConsoleTestApplication", "ConsoleTestApplication");
            Commom.SaveOnCacheIfNonExists<FakeObject>();
            Commom.SaveOnCacheIfNonExists<FilterFakeObject>();

            var timer = new Stopwatch();
            int t = 0;
            var queryable = testList.AsQueryable();
            timer.Start();
            for (int i = 0; i < 10000; i++)
                t = queryable.Where(filter.GenerateLambda<FakeObject, FilterFakeObject>()).Count();
            timer.Stop();
            return $"Total size {testList.Count()} - Total result {t} {Environment.NewLine}Generated call has executed in {timer.ElapsedMilliseconds} ms.";
        }
    }


    public class FakeObject
    {
        public int value { get; set; }
    }

    public class FilterFakeObject : IFilter
    {
        [LambdaGenerate(MethodOption = Generic.Service.Enums.Lambda.LambdaMethod.GreaterThanOrEqual)]
        public int value { get; set; }
    }
}
