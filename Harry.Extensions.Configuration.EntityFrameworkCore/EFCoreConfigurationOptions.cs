using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.Extensions.Configuration.EntityFrameworkCore
{
    public class EFCoreConfigurationOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public Func<string,string ,bool> Filter { get; set; }
    }
}
