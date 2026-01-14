using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityUtils {
    public static class ListExtensions {
        static Random rng;
        
        /// <summary>
        /// Determines whether a collection is null or has no elements判断集合是否为 null 或不包含任何元素。
        /// without having to enumerate the entire collection to get a count.无需遍历整个集合即可获取元素数量。
        ///
        /// Uses LINQ's Any() method to determine if the collection is empty,使用LINQ's Any()方法判断集合是否为空。
        /// so there is some GC overhead.因此会产生一些垃圾回收开销。
        /// </summary>
        /// <param name="list">List to evaluate</param>
        public static bool IsNullOrEmpty<T>(this IList<T> list) {
            return list == null || !list.Any();
        }

        /// <summary>
        /// Creates a new list that is a copy of the original list.创建一个新的列表，它是原始列表的副本。
        /// </summary>
        /// <param name="list">The original list to be copied.需要复制的原始列表。</param>
        /// <returns>A new list that is a copy of the original list.返回复制列表</returns>
        public static List<T> Clone<T>(this IList<T> list) {
            List<T> newList = new List<T>();
            foreach (T item in list) {
                newList.Add(item);
            }

            return newList;
        }

        /// <summary>
        /// Swaps two elements in the list at the specified indices.交换列表中指定索引处的两个元素。
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="indexA">The index of the first element.第一个元素的索引。</param>
        /// <param name="indexB">The index of the second element.第二个元素的索引。</param>
        public static void Swap<T>(this IList<T> list, int indexA, int indexB) {
            (list[indexA], list[indexB]) = (list[indexB], list[indexA]);
        }

        /// <summary>
        /// Shuffles the elements in the list using the Durstenfeld implementation of the Fisher-Yates algorithm.使用 Fisher-Yates 算法的 Durstenfeld 实现方法对列表中的元素进行随机排序。
        /// This method modifies the input list in-place, ensuring each permutation is equally likely, and returns the list for method chaining.此方法会就地修改输入列表，确保每个排列出现的概率均等，并返回修改后的列表，以便进行方法链式调用。
        /// Reference: http://en.wikipedia.org/wiki/Fisher-Yates_shuffle
        /// </summary>
        /// <param name="list">The list to be shuffled.需要打乱顺序的列表。</param>
        /// <typeparam name="T">The type of the elements in the list.列表中元素的类型。</typeparam>
        /// <returns>The shuffled list.打乱顺序后的列表。</returns>
        public static IList<T> Shuffle<T>(this IList<T> list) {
            if (rng == null) rng = new Random();
            int count = list.Count;
            while (count > 1) {
                --count;
                int index = rng.Next(count + 1);
                (list[index], list[count]) = (list[count], list[index]);
            }
            return list;
        }

        /// <summary>
        /// Filters a collection based on a predicate and returns a new list根据条件过滤集合并返回一个新的列表。
        /// containing the elements that match the specified condition.包含符合指定条件的元素。
        /// </summary>
        /// <param name="source">The collection to filter.需要过滤的集合</param>
        /// <param name="predicate">The condition that each element is tested against.用于测试每个元素的条件。</param>
        /// <returns>A new list containing elements that satisfy the predicate.一个包含满足条件的元素的新列表。</returns>
        public static IList<T> Filter<T>(this IList<T> source, Predicate<T> predicate) {
            List<T> list = new List<T>();
            foreach (T item in source) {
                if (predicate(item)) {
                    list.Add(item);
                }
            }
            return list;
        }
        /// <summary>
        /// 更新列表内容
        /// </summary>
        /// <param name="list"></param>
        /// <param name="items"></param>
        /// <typeparam name="T"></typeparam>
        public static void RefreshWith<T>(this List<T> list, IEnumerable<T> items)
        {
            list.Clear();
            list.AddRange(items);
        }
    }
}
