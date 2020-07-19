using System;
using System.IO;
using Harry.Extensions.Configuration.Yaml;
using Microsoft.Extensions.FileProviders;

namespace Microsoft.Extensions.Configuration
{

    public static class YamlConfigurationExtensions
    {
        /// <summary>
        /// ���Yaml�ļ�����Դ
        /// </summary>
        /// <param name="path">�����ļ�·��</param>
        /// <returns></returns>
        public static IConfigurationBuilder AddYamlFile(this IConfigurationBuilder builder, string path)
        {
            return AddYamlFile(builder, provider: null, path: path, optional: false, reloadOnChange: false);
        }

        /// <summary>
        /// ���Yaml�ļ�����Դ
        /// </summary>
        /// <param name="path">�����ļ�·��</param>
        /// <param name="optional">�Ƿ��ѡ</param>
        /// <returns></returns>
        public static IConfigurationBuilder AddYamlFile(this IConfigurationBuilder builder, string path, bool optional)
        {
            return AddYamlFile(builder, provider: null, path: path, optional: optional, reloadOnChange: false);
        }

        /// <summary>
        /// ���Yaml�ļ�����Դ
        /// </summary>
        /// <param name="path">�����ļ�·��</param>
        /// <param name="optional">�Ƿ��ѡ</param>
        /// <param name="reloadOnChange">�ļ��ı�ʱ�Ƿ����¼���</param>
        /// <returns></returns>
        public static IConfigurationBuilder AddYamlFile(this IConfigurationBuilder builder, string path, bool optional, bool reloadOnChange)
        {
            return AddYamlFile(builder, provider: null, path: path, optional: optional, reloadOnChange: reloadOnChange);
        }

        /// <summary>
        /// ���Yaml�ļ�����Դ
        /// </summary>
        /// <param name="path">�����ļ�·��</param>
        /// <param name="optional">�Ƿ��ѡ</param>
        /// <param name="reloadOnChange">�ļ��ı�ʱ�Ƿ����¼���</param>
        /// <returns></returns>
        public static IConfigurationBuilder AddYamlFile(this IConfigurationBuilder builder, IFileProvider provider, string path, bool optional, bool reloadOnChange)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("File path must be a non-empty string.", nameof(path));
            }

            return builder.AddYamlFile(s =>
            {
                s.FileProvider = provider;
                s.Path = path;
                s.Optional = optional;
                s.ReloadOnChange = reloadOnChange;
                s.ResolveFileProvider();
            });
        }

        /// <summary>
        /// ���Yaml�ļ�����Դ
        /// </summary>
        /// <returns></returns>
        public static IConfigurationBuilder AddYamlFile(this IConfigurationBuilder builder, Action<YamlConfigurationSource> configureSource)
            => builder.Add(configureSource);


        /// <summary>
        /// ���Yaml�ļ�������Դ
        /// </summary>
        /// <returns></returns>
        public static IConfigurationBuilder AddYamlStream(this IConfigurationBuilder builder, Stream stream)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.Add<YamlStreamConfigurationSource>(s => s.Stream = stream);
        }
    }
}
