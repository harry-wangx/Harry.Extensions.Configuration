using System;
using System.IO;
using Harry.Extensions.Configuration.Yaml;
using Microsoft.Extensions.FileProviders;

namespace Microsoft.Extensions.Configuration
{

    public static class YamlConfigurationExtensions
    {
        /// <summary>
        /// 添加Yaml文件配置源
        /// </summary>
        /// <param name="path">配置文件路径</param>
        /// <returns></returns>
        public static IConfigurationBuilder AddYamlFile(this IConfigurationBuilder builder, string path)
        {
            return AddYamlFile(builder, provider: null, path: path, optional: false, reloadOnChange: false);
        }

        /// <summary>
        /// 添加Yaml文件配置源
        /// </summary>
        /// <param name="path">配置文件路径</param>
        /// <param name="optional">是否可选</param>
        /// <returns></returns>
        public static IConfigurationBuilder AddYamlFile(this IConfigurationBuilder builder, string path, bool optional)
        {
            return AddYamlFile(builder, provider: null, path: path, optional: optional, reloadOnChange: false);
        }

        /// <summary>
        /// 添加Yaml文件配置源
        /// </summary>
        /// <param name="path">配置文件路径</param>
        /// <param name="optional">是否可选</param>
        /// <param name="reloadOnChange">文件改变时是否重新加载</param>
        /// <returns></returns>
        public static IConfigurationBuilder AddYamlFile(this IConfigurationBuilder builder, string path, bool optional, bool reloadOnChange)
        {
            return AddYamlFile(builder, provider: null, path: path, optional: optional, reloadOnChange: reloadOnChange);
        }

        /// <summary>
        /// 添加Yaml文件配置源
        /// </summary>
        /// <param name="path">配置文件路径</param>
        /// <param name="optional">是否可选</param>
        /// <param name="reloadOnChange">文件改变时是否重新加载</param>
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
        /// 添加Yaml文件配置源
        /// </summary>
        /// <returns></returns>
        public static IConfigurationBuilder AddYamlFile(this IConfigurationBuilder builder, Action<YamlConfigurationSource> configureSource)
            => builder.Add(configureSource);


        /// <summary>
        /// 添加Yaml文件流配置源
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
