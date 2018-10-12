using log4net;
using Ninject;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzTest
{
    class Program
    {
        static void Main(string[] args)
        {
            NinjectModule ninjectModule = new NinjectModule();
            var kernel = ninjectModule.Init();

            //Uncomment for test
            //WithoutQuartz(kernel);
            WithQuartz(kernel);

            Console.ReadKey();
        }

        /// <summary>
        /// Working example - Ninject + log4net.
        /// </summary>
        public static void WithoutQuartz(IKernel kernel)
        {
            ILog log = kernel.Get<ILog>();
            SimpleJob job = new SimpleJob(log);
            job.ExecuteForTest();
        }

        /// <summary>
        /// Not Working example - Ninject + log4net + Quartz.
        /// </summary>
        /// <param name="kernel"></param>
        public static void WithQuartz(IKernel kernel)
        {
            var scheduler = kernel.Get<IScheduler>();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<SimpleJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()              // создаем триггер
                .WithIdentity("trigger1", "group1")                 // идентифицируем триггер с именем и группой
                .StartNow()                                         // запуск сразу после начала выполнения
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(3) // настраиваем выполнение действия 
                    .RepeatForever())                   // бесконечное повторение
                .Build();                               // создаем триггер

            scheduler.ScheduleJob(job, trigger);  // начинаем выполнение работы
        }
    }
}
