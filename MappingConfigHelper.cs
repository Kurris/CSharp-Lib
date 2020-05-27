using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace CSharpLib
{
    /// <summary>
    /// 映射主程序配置文件
    /// </summary>
    public class MappingConfigHelper
    {
        private static readonly string _msmappingConfigPath = string.Empty;//Path.Combine(Application.StartupPath, "PackageInfo.config");

        /// <summary>
        /// 映射主程序配置文件
        /// </summary>
        /// <returns>Configuration实例</returns>
        public static Configuration GetConfiguration() => ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap()
        {
            ExeConfigFilename = _msmappingConfigPath
        }, ConfigurationUserLevel.None);

        /// <summary>
        /// 检查是否存在Key
        /// </summary>
        /// <param name="config">配置文件实例</param>
        /// <param name="key">键</param>
        /// <returns>存在(true) 不存在(false)</returns>
        private static bool CheckConfigKeyExist(Configuration config, string key) => config.AppSettings.Settings.AllKeys.Contains(key);

        /// <summary>
        /// 添加键值对
        /// </summary>
        /// <param name="config">配置文件实例</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        private static void AddConfigKeyValue(Configuration config, string key, string value) => config.AppSettings.Settings.Add(key, value);

        /// <summary>
        /// 设置Key的Value
        /// </summary>
        /// <param name="config">配置文件实例</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        private static void SetConfigValue(Configuration config, string key, string value) => config.AppSettings.Settings[key].Value = value;


        private static string GetConfigValue(Configuration config, string key) => config.AppSettings.Settings[key].Value;

        /// <summary>
        /// 通过键获取主程序配置文件中的值(key不存在则返回默认值)
        /// </summary>
        /// <param name="config">配置文件实例</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        private static string GetValueByKey(Configuration config, string key, bool createKey, string defaultString = "")
        {
            //存在
            if (CheckConfigKeyExist(config, key))
            {
                return GetConfigValue(config, key);
            }
            else
            {
                if (createKey)
                {
                    AddConfigKeyValue(config, key, defaultString);

                    config.Save();

                    //在不存在Key的情况下,默认值不为空则返回默认值
                    return defaultString;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 通过传入字典获取主程序配置文件中的值
        /// </summary>
        /// <param name="keyValues">字典</param>
        /// <param name="createKeyAndSetDefaultValue">是否创建Key</param>
        /// <returns></returns>
        public static void GetValueByDic(ref Dictionary<string, string> keyValues, bool createKeyAndSetDefaultValue = false)
        {
            Configuration config = GetConfiguration();

            if (config == null) throw new ArgumentNullException("配置文件读取失败");

            string[] arrStr = keyValues.Select(x => x.Key).ToArray();

            for (int i = 0; i < arrStr.Length; i++)
            {
                string key = arrStr[i];
                keyValues[key] = GetValueByKey(config, key, createKeyAndSetDefaultValue, keyValues[key]);
            }
        }


        /// <summary>
        /// 保存键值到主配置文件 20200423 ligy add
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public static bool SaveKeyValueInConfig(IDictionary<string, object> keyValues)
        {
            var config = GetConfiguration();

            foreach (KeyValuePair<string, object> kv in keyValues)
            {
                string key = kv.Key;
                if (CheckConfigKeyExist(config, key))
                {
                    SetConfigValue(config, key, kv.Value + "");
                }
                else
                {
                    AddConfigKeyValue(config, key, kv.Value + "");
                }
            }

            config.Save();

            return true;
        }

        /// <summary>
        /// 通过键获取主程序配置文件中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="createKey">是否创建(当键不存在)</param>
        /// <returns></returns>
        public static string GetValueByKey(string key, bool createKey = false, string defaultString = "")
        {
            Configuration config = GetConfiguration();

            return config == null
                ? throw new ArgumentNullException("配置文件读取失败")
                : GetValueByKey(config, key, createKey, defaultString);
        }
    }
}