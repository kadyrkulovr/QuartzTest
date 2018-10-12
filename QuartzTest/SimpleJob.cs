using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzTest
{
    internal class SimpleJob : IJob
    {
        private readonly ILog log;

        public SimpleJob(ILog log)
        {
            this.log = log;
        }

        public Task Execute(IJobExecutionContext context)
        {
            log.Error("Before start.");

            return Task.Run(() => log.Debug("Job started."));
        }

        public void ExecuteForTest()
        {
            log.Error("Hello!");
        }
    }
}
