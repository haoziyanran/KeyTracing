namespace Walker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Threading;
    using Microsoft.TeamFoundation.VersionControl.Client;
    using System.IO;
    using System.Diagnostics;
    using NLog;

    class Program
    {
        private static ILogger logger = LogManager.GetLogger("Walker");

        static void Main(string[] args)
        {
            VersionControlServer vcs = null;
            try
            {
                var tpc = VCSHelper.GetCollection(args[0]);
                vcs = tpc.GetService<VersionControlServer>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Usage();
                return;
            }

            // initial
            const String ROOT = "$/";
            Queue<String> result = new Queue<string>();
            Queue<String> pending = new Queue<string>();
            // push initial path into the queue
            pending.Enqueue(ROOT);

            Stopwatch timer = new Stopwatch();
            timer.Start();

            var main = Task.Factory.StartNew(() =>
            {
                logger.Info("Main task start.");
                do
                {
                    var path = pending.Dequeue();
                    var itemSet = VCSHelper.GetItemSet(vcs, path);

                    var folders = from item in itemSet.Items
                                  where item.ItemType == ItemType.Folder
                                  select item.ServerItem;
                    var files = from item in itemSet.Items
                                where item.ItemType == ItemType.File
                                select item.ServerItem;

                    foreach (var item in folders)
                    {
                        // ignore first item that is the path of root
                        if (item.Equals(path, StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }
                        pending.Enqueue(item);
                    }
                    foreach (var item in files)
                    {
                        result.Enqueue(item);
                    }

                    logger.Debug("Pending queue current count: {0}", pending.Count);
                    logger.Debug("Result queue current count: {0}", result.Count);

                } while (pending.Count > 0);
            });

            int counter = 0;
            // start record queue to save file list into file
            var record = Task.Factory.StartNew(() =>
            {
                logger.Info("Record task start.");
                using (StreamWriter sw = new StreamWriter(Path.Combine(Environment.CurrentDirectory, "Data.txt")))
                {
                    sw.AutoFlush = true;
                    while (main.Status == TaskStatus.Running)
                    {
                        logger.Info("Main task status: " + main.Status.ToString());

                        while (result.Count > 0)
                        {
                            counter++;
                            sw.WriteLine(result.Dequeue());
                        }
                        // waiting for main task produce data.
                        Thread.Sleep(500);
                    }
                }
            });

            Task.WaitAll(main, record);
            logger.Debug("Record task stopped.");

            timer.Stop();
            logger.Info("Total file count: {0}", counter);
            logger.Info("Total seconds: {0}", timer.Elapsed.TotalSeconds);
        }

        static void Usage()
        {
            Console.WriteLine("Usage: Walker.exe collectionUrl");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
