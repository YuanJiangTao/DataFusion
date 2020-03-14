using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFusion.Interfaces.Extensions
{
    /// <summary>
    /// ConcurrentBag的扩展方法
    /// </summary>
    public static class ConcurrentBagExtensions
    {

        private static Object locker = new object();

        public static void Clear<T>(this ConcurrentBag<T> bag)
        {
            try
            {
                var listTemp = bag.ToList();

                while (!bag.IsEmpty)
                {
                    bag.TryTake(out T t);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        public static void Remove<T>(this ConcurrentBag<T> bag, List<T> itemlist)
        {
            try
            {
                lock (locker)
                {
                    List<T> removelist = bag.ToList();

                    Parallel.ForEach(itemlist, currentitem =>
                    {
                        removelist.Remove(currentitem);
                    });

                    bag = new ConcurrentBag<T>();


                    Parallel.ForEach(removelist, currentitem =>
                    {
                        bag.Add(currentitem);
                    });
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static void Remove<T>(this ConcurrentBag<T> bag, T removeitem)
        {
            try
            {
                lock (locker)
                {
                    List<T> removelist = bag.ToList();
                    removelist.Remove(removeitem);

                    bag = new ConcurrentBag<T>();

                    Parallel.ForEach(removelist, currentitem =>
                    {
                        bag.Add(currentitem);
                    });
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public static void Remove<T>(this ConcurrentBag<T> bag, Predicate<T> match)
        {
            try
            {
                lock (locker)
                {
                    var listTemp = bag.ToList();
                    listTemp.RemoveAll(match);

                    while (!bag.IsEmpty)
                    {
                        bag.TryTake(out T t);
                    }
                    listTemp.ForEach(o => bag.Add(o));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
