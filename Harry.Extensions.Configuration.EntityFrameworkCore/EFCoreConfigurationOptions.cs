using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.Extensions.Configuration.EntityFrameworkCore
{
    public class EFCoreConfigurationOptions
    {
        /// <summary>
        /// 保存过滤器.为null或返回true时,进行保存操作.
        /// 第一个参数为Key,第二个参数为Value.
        /// </summary>
        public Func<string, string, bool> Filter { get; set; }

        /// <summary>
        /// 延迟保存时间(单位:毫秒)
        /// </summary>
        public int DelayMilliseconds { get; set; } = 200;
    }
}
