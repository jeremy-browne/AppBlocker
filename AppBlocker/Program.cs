using System;
using System.Diagnostics;
using System.Management;
using System.Collections.Generic;

namespace AppBlocker
{
    class AppBlocker
    {
        static void Main(string[] args)
        {
            // Get all processes running on the local computer.
            Process[] localAll = RemoveDuplicates(Process.GetProcesses());
            foreach (var process in localAll)
            {
                Console.WriteLine(process.Id + " | " + process.ProcessName);
            }
        }

        private static Process[] RemoveDuplicates(Process[] s)
        {
            HashSet<Process> set = new HashSet<Process>(s);
            Process[] result = new Process[set.Count];
            set.CopyTo(result);
            return result;
        }

        private static void KillAllProcessesSpawnedBy(UInt32 parentProcessId)
        {
            Console.WriteLine("Finding processes spawned by process with Id [" + parentProcessId + "]");

            // NOTE: Process Ids are reused!
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                "SELECT * " +
                "FROM Win32_Process " +
                "WHERE ParentProcessId=" + parentProcessId);
            ManagementObjectCollection collection = searcher.Get();
            if (collection.Count > 0)
            {
                Console.WriteLine("Killing [" + collection.Count + "] processes spawned by process with Id [" + parentProcessId + "]");
                foreach (var item in collection)
                {
                    UInt32 childProcessId = (UInt32)item["ProcessId"];
                    if ((int)childProcessId != Process.GetCurrentProcess().Id)
                    {
                        KillAllProcessesSpawnedBy(childProcessId);

                        Process childProcess = Process.GetProcessById((int)childProcessId);
                        Console.WriteLine("Killing child process [" + childProcess.ProcessName + "] with Id [" + childProcessId + "]");
                        childProcess.Kill();
                    }
                }
            }
        }
    }
}
