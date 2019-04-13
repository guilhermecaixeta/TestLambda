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
        private static int _MAX = 5000000;
        private static int _COMP = 2550000;
        private const int _ITERATOR = 1000;
        private static int _SIZE;
        private static IQueryable<FakeObject> queryable { get; set; }

        public static void CreateList()
        {
            SetCache();
            int maxValue = _MAX * 10;
            List<FakeObject> fakeList = NewFakeList();
            for (int i = 0; i < _MAX; i++)
            {
                fakeList.Add(new FakeObject() { value = new Random().Next(maxValue) });
            }
            queryable = fakeList.AsQueryable();
        }

        public static void SetSize()
        {
            _SIZE = queryable.Count();
        }

        public static string GetHeader()
        {
            SetSize();
            return $"Starting test on {DateTime.Now.ToShortTimeString()} {Environment.NewLine}Total size {_SIZE} - Total iterations - {_ITERATOR}";
        }

        private static List<FakeObject> NewFakeList()
        {
            return new List<FakeObject>();
        }

        private static FilterFakeObject SetFakeFilter()
        {
            FilterFakeObject filter = new FilterFakeObject();
            filter.value = _COMP;
            return filter;
        }

        private static void SetCache()
        {
            Commom.SetSizeByLengthProperties("ConsoleTestApplication", "ConsoleTestApplication");
            Commom.SaveOnCacheIfNonExists<FakeObject>();
            Commom.SaveOnCacheIfNonExists<FilterFakeObject>();
        }

        public static string LambdaDirectCall()
        {
            var timer = new Stopwatch();
            int t = 0;
            timer.Start();
            for (int i = 0; i < _ITERATOR; i++)
                t = queryable.Where(x => x.value >= _COMP).Count();
            timer.Stop();
            return $"Total result {t} {Environment.NewLine}Direct call has executed in {timer.ElapsedMilliseconds} ms.";
        }

        public static string LambdaCompiledCache()
        {
            FilterFakeObject filter = SetFakeFilter();
            var timer = new Stopwatch();
            var func = filter.GenerateLambda<FakeObject, FilterFakeObject>().Compile();
            int t = 0;
            timer.Start();
            for (int i = 0; i < _ITERATOR; i++)
                t = queryable.Where(x => func(x)).Count();
            timer.Stop();

            return $"Total result {t}  {Environment.NewLine}Compiled Cache call has executed in {timer.ElapsedMilliseconds} ms.";
        }


        public static string LambdaCompiled()
        {
            FilterFakeObject filter = SetFakeFilter();

            var timer = new Stopwatch();
            int t = 0;
            timer.Start();
            for (int i = 0; i < _ITERATOR; i++)
                t = queryable.Where(x => filter.GenerateLambda<FakeObject, FilterFakeObject>().Compile()(x)).Count();
            timer.Stop();

            return $"Total result {t} {Environment.NewLine}Compiled call has executed in {timer.ElapsedMilliseconds} ms.";
        }

        public static string LambdaGeneratedCache()
        {
            FilterFakeObject filter = SetFakeFilter();
            var timer = new Stopwatch();
            int t = 0;
            var predicate = filter.GenerateLambda<FakeObject, FilterFakeObject>();
            timer.Start();
            for (int i = 0; i < _ITERATOR; i++)
                t = queryable.Where(predicate).Count();
            timer.Stop();

            return $"Total result {t} {Environment.NewLine}Generated Cache call has executed in {timer.ElapsedMilliseconds} ms.";
        }

        public static string LambdaGenerated()
        {
            FilterFakeObject filter = SetFakeFilter();

            var timer = new Stopwatch();
            int t = 0;
            timer.Start();
            for (int i = 0; i < _ITERATOR; i++)
                t = queryable.Where(filter.GenerateLambda<FakeObject, FilterFakeObject>()).Count();
            timer.Stop();

            return $"Total result {t} {Environment.NewLine}Generated call has executed in {timer.ElapsedMilliseconds} ms.";
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
