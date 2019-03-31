using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace ATBase.Schedule
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ATBaseJob : IJob
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Execute(IJobExecutionContext context)
        {
            return Execute();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract Task Execute();
    }
}
