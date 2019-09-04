using System;
using System.Threading.Tasks;

namespace FESA.SCM.Core.Helpers
{
    public static class TaskExtensions
    {
        public static Task ContinueOnCurrentThread(this Task task, Action<Task> callback)
        {
            return task.ContinueWith(callback, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public static Task<T> ContinueOnCurrentThread<T>(this Task<T> task, Func<Task<T>, T> callback)
        {

            return task.ContinueWith<T>(callback, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public static Task ContinueWith(this Task task, Task continuation)
        {
            return task.ContinueWith(t => continuation).Unwrap();
        }

        public static Task<T> ContinueWith<T>(this Task task, Task<T> continuation)
        {
            return task.ContinueWith(t => continuation).Unwrap();
        }
    }
}