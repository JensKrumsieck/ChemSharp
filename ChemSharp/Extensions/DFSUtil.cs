using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChemSharp.Extensions
{
    /// <summary>
    /// A static class for operations on undirected graphs
    /// </summary>
    public static class DFSUtil
    {
        /// <summary>
        /// List all connected figures
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<IEnumerable<T>> ConnectedFigures<T>(IEnumerable<T> list, Func<T, IEnumerable<T>> func)
        {
            var visited = new HashSet<T>();
            foreach (var item in list) if (!visited.Contains(item)) yield return await DFS(item, func, visited);
        }

        /// <summary>
        /// Implementation of recursive DFS
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="vertex"></param>
        /// <param name="func"></param>
        /// <param name="visited">Provide a Hashset if you need to track which figure already has been found</param>
        /// <returns></returns>
        public static async Task<HashSet<T>> DFS<T>(T vertex, Func<T, IEnumerable<T>> func, HashSet<T> visited = null)
        {
            var results = new HashSet<T>();
            await Traverse(vertex, results, func);

            //add results to visited if exists
            visited?.UnionWith(results);
            return results;
        }

        /// <summary>
        /// Traverse Function
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="vertex"></param>
        /// <param name="visited"></param>
        /// <param name="func"></param>
        private static async Task Traverse<T>(T vertex, HashSet<T> visited, Func<T, IEnumerable<T>> func)
        {
            visited.Add(vertex);
            foreach (var neighbor in func(vertex).Where(n => !visited.Contains(n)))
                await Traverse(neighbor, visited, func);
        }

        /// <summary>
        /// Returns all Paths from start to end
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="func">Neighbor function</param>
        /// <param name="length">desired path length</param>
        /// <returns></returns>
        public static HashSet<HashSet<T>> GetAllPaths<T>(T start, T end, Func<T, IEnumerable<T>> func, int length = int.MaxValue)
        {
            var visited = new HashSet<T>();
            var localPaths = new HashSet<T>();
            var output = new HashSet<HashSet<T>>();
            output = AllPaths(start, end, visited, localPaths, func, output, length).AsParallel().ToHashSet();
            return output;
        }

        /// <summary>
        /// Returns all paths
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="visited"></param>
        /// <param name="localPaths"></param>
        /// <param name="func"></param>
        /// <param name="output"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static HashSet<HashSet<T>> AllPaths<T>(T start, T end, ISet<T> visited, HashSet<T> localPaths, Func<T, IEnumerable<T>> func, HashSet<HashSet<T>> output, int length = int.MaxValue)
        {
            visited.Add(start);
            if (localPaths.Count == 0) localPaths.Add(start);
            if (start.Equals(end) && localPaths.Count() == length) BuildPath(localPaths, output);
            foreach (var node in func(start).Where(node => !visited.Contains(node)).Select(node => node).AsParallel())
            {
                localPaths.Add(node);
                AllPaths(node, end, visited, localPaths, func, output, length).AsParallel();
                localPaths.Remove(node);
            }

            visited.Remove(start);
            return output;
        }

        /// <summary>
        /// Copys the localpath to output hashset
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="localPath"></param>
        /// <param name="output"></param>
        private static void BuildPath<T>(HashSet<T> localPath, ISet<HashSet<T>> output)
        {
            var buildPath = new HashSet<T>();
            foreach (var node in localPath) buildPath.Add(node);
            output.Add(buildPath);
        }

        /// <summary>
        /// Calculates the vertex degree (count) of vertex in collection by func(vertex,collection)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="vertex"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static int VertexDegree<T>(IEnumerable<T> collection, T vertex, Func<T, IEnumerable<T>, IEnumerable<T>> func) => func(vertex, collection).Count();
    }
}
