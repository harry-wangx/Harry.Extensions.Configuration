using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Threading;

namespace Harry.Extensions.Configuration.EntityFrameworkCore
{
    public class EFCoreConfigurationProvider : ConfigurationProvider
    {
        private readonly EFCoreConfigurationSource _source;

        private static readonly object _locker = new object();

        private static ConfigurationDbContext _saveDb = null;
        private static IDbContextTransaction _tran = null;

        //最后一次更新数据时间
        private static DateTime _lastUpdateTime = DateTime.Now;

        public EFCoreConfigurationProvider(EFCoreConfigurationSource source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
        }

        /// <summary>
        /// 从数据库加载配置
        /// </summary>
        public override void Load()
        {
            using (ConfigurationDbContext db = new ConfigurationDbContext(_source.DbOptions))
            {
                db.Database.EnsureCreated();
                foreach (var item in db.Configurations)
                {
                    //调用基类中的Set方法,以避免进行插入数据库操作
                    base.Set(item.ConfigurationKey, item.ConfigurationValue);
                }
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public override void Set(string key, string value)
        {
            if (string.IsNullOrEmpty(key)) return;

            if (_source.Options.Filter == null || _source.Options.Filter.Invoke(key, value))
            {
                lock (_locker)
                {
                    var needSave = false;//是否需要启动保存程序
                    if (_saveDb == null)
                    {
                        try
                        {
                            _saveDb = new ConfigurationDbContext(_source.DbOptions);
                            _saveDb.Database.EnsureCreated();
                            _tran = _saveDb.Database.BeginTransaction();
                            needSave = true;
                        }
                        catch (Exception ex)
                        {
                            _tran = null;
                            _saveDb = null;
                            needSave = false;
                            throw ex;
                        }
                    }

                    try
                    {
                        if (TryGet(key, out string oldValue))
                        {
                            //更新操作
                            //如果值相等,则不进行操作
                            if (oldValue == value) return;

                            _saveDb.Database.ExecuteSqlInterpolated($"UPDATE Configurations Set ConfigurationValue={value} WHERE ConfigurationKey={key}");
                        }
                        else
                        {
                            //插入操作
                            _saveDb.Configurations.Add(new ConfigurationEntity() { ConfigurationKey = key, ConfigurationValue = value });
                        }

                        //设置内存中的数据.必须放在TryGet后面
                        base.Set(key, value);

                        //添加了新的数据,重新计时
                        _lastUpdateTime = DateTime.Now;
                    }
                    finally
                    {
                        //即使没有添加数据,只要实例化了DbContext,也要启动延迟保存.
                        if (needSave)
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(DelaySaveChanges));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 延时保存
        /// </summary>
        private void DelaySaveChanges(object obj)
        {
            while (true)
            {
                lock (_locker)
                {
                    if (_saveDb == null)
                    {
                        //如果已保存,则直接退出
                        return;
                    }

                    //超过指定延迟时间后,未新增数据,开始保存操作
                    if ((DateTime.Now - _lastUpdateTime).Milliseconds >= _source.Options.DelayMilliseconds)
                    {
                        try
                        {
                            _saveDb.SaveChanges();
                            _tran.Commit();

                            _tran.Dispose();
                            _saveDb.Dispose();

                        }
                        finally
                        {
                            _saveDb = null;
                            _tran = null;
                        }
#if DEBUG
                        Console.WriteLine("保存成功");
#endif
                        return;
                    }
                }

                Thread.Sleep(50);
            }
        }
    }
}
