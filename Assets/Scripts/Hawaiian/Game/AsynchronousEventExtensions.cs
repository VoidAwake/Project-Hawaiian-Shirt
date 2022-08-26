using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hawaiian.Game
{
    // From https://stackoverflow.com/questions/27761852/how-do-i-await-events-in-c
    public static class AsynchronousEventExtensions
    {
        public static Task Raise<TSource, TEventArgs>(this Func<TSource, TEventArgs, Task> handlers, TSource source, TEventArgs args)
            where TEventArgs : EventArgs
        {
            if (handlers != null)
            {
                return Task.WhenAll(handlers.GetInvocationList()
                    .OfType<Func<TSource, TEventArgs, Task>>()
                    .Select(h => h(source, args)));
            }

            return Task.CompletedTask;
        }
    }
}