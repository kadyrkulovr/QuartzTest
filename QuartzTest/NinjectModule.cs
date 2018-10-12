using log4net;
using Ninject;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuartzTest
{
    public class NinjectModule
    {
        public IKernel Init()
        {
            var kernel = new StandardKernel();

            kernel.Load(Assembly.GetExecutingAssembly());

            kernel.Bind<ILog>().ToMethod(x => LogManager.GetLogger(x.Request.Target != null
                                    ? x.Request.Target.Member.DeclaringType
                                    : x.Request.Service));

            kernel.Bind<IScheduler>().ToMethod(x =>
            {
                ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
                var sched = schedulerFactory.GetScheduler().Result;
                sched.JobFactory = new NinjectJobFactory(kernel);
                return sched;
            });

            return kernel;
        }
    }
}
