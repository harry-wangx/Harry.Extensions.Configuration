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

        //���һ�θ�������ʱ��
        private static DateTime _lastUpdateTime = DateTime.Now;

        public EFCoreConfigurationProvider(EFCoreConfigurationSource source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
        }

        /// <summary>
        /// �����ݿ��������
        /// </summary>
        public override void Load()
        {
            using (ConfigurationDbContext db = new ConfigurationDbContext(_source.DbOptions))
            {
                db.Database.EnsureCreated();
                foreach (var item in db.Configurations)
                {
                    //���û����е�Set����,�Ա�����в������ݿ����
                    base.Set(item.ConfigurationKey, item.ConfigurationValue);
                }
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public override void Set(string key, string value)
        {
            if (string.IsNullOrEmpty(key)) return;

            if (_source.Options.Filter == null || _source.Options.Filter.Invoke(key, value))
            {
                lock (_locker)
                {
                    var needSave = false;//�Ƿ���Ҫ�����������
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
                            //���²���
                            //���ֵ���,�򲻽��в���
                            if (oldValue == value) return;

                            _saveDb.Database.ExecuteSqlInterpolated($"UPDATE Configurations Set ConfigurationValue={value} WHERE ConfigurationKey={key}");
                        }
                        else
                        {
                            //�������
                            _saveDb.Configurations.Add(new ConfigurationEntity() { ConfigurationKey = key, ConfigurationValue = value });
                        }

                        //�����ڴ��е�����.�������TryGet����
                        base.Set(key, value);

                        //������µ�����,���¼�ʱ
                        _lastUpdateTime = DateTime.Now;
                    }
                    finally
                    {
                        //��ʹû���������,ֻҪʵ������DbContext,ҲҪ�����ӳٱ���.
                        if (needSave)
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(DelaySaveChanges));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ��ʱ����
        /// </summary>
        private void DelaySaveChanges(object obj)
        {
            while (true)
            {
                lock (_locker)
                {
                    if (_saveDb == null)
                    {
                        //����ѱ���,��ֱ���˳�
                        return;
                    }

                    //����ָ���ӳ�ʱ���,δ��������,��ʼ�������
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
                        Console.WriteLine("����ɹ�");
#endif
                        return;
                    }
                }

                Thread.Sleep(50);
            }
        }
    }
}
