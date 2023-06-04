namespace Rule34.us.Downloader.Logic.Extensions
{
    internal static class TaskExtensions
    {
        public static void ForEachCompleted(this Task[] tasks, Action<int, int> action)
        {
            int totalTasksCount = tasks.Length;
            List<Task> tasksList = tasks.ToList();

            Task.Run(async () =>
            {
                int finishedTasksCount = 0;

                while (tasksList.Any())
                {
                    Task finishedTask = await Task.WhenAny(tasksList);
                    _ = tasksList.Remove(finishedTask);
                    finishedTasksCount++;

                    action.Invoke(finishedTasksCount, totalTasksCount);
                }

            }).Wait();
        }
    }
}
