using System;

namespace Harry.Extensions.Configuration.EntityFrameworkCore
{
    public class ConfigurationEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// 键
        /// </summary>
        public string ConfigurationKey { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string ConfigurationValue { get; set; }
		
        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

    }
}
