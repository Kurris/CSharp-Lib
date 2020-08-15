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
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static string _msmappingConfigPath = string.Empty;

        /// <summary>
        /// 记录上次配置文件路径
        /// </summary>
        private static string _mtempPath = string.Empty;

        #region Instance

        private static Configuration _mconfigurationInstance = null;

        /// <summary>
        /// 获取映射主程序配置文件Configuration实例
        /// </summary>
        /// <returns>Configuration实例</returns>
        public static Configuration ConfigurationInstance
        {
            get
            {
                if (_mconfigurationInstance == null)
                {
                    //不为空的情况可能外部先设置了配置文件的值
                    if (string.IsNullOrEmpty(_msmappingConfigPath))
                    {
                        _msmappingConfigPath = string.Empty;//初始化配置文件地址
                    }

                    _mconfigurationInstance = GetConfiguration(_msmappingConfigPath);
                }
                return _mconfigurationInstance;
            }
        }

        /// <summary>
        /// 获取映射主程序配置文件Configuration实例
        /// </summary>
        /// <returns></returns>
        public static Configuration GetConfiguration() => ConfigurationInstance;


        /// <summary>
        /// 获取映射指定配置文件Configuration实例
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static Configuration GetConfiguration(string FilePath)
        {
            return ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap()
            {
                ExeConfigFilename = FilePath
            }, ConfigurationUserLevel.None);
        }

        #endregion

        #region FilePath

        /// <summary>
        /// 设置配置文件路径(全局路径)
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        public static void SetConfigFile(string FilePath)
        {
            //记录当前的配置文件地址
            _mtempPath = _msmappingConfigPath;
            //设置新的配置文件地址
            _msmappingConfigPath = FilePath;
            //将静态实例置空(为空会重新获取)
            _mconfigurationInstance = null;
        }

        /// <summary>
        /// 恢复上次使用的配置文件
        /// </summary>
        public static void RestoreConfigFile()
        {
            //记录当前配置文件地址
            string sTempPath = _msmappingConfigPath;
            //恢复上次的值
            _msmappingConfigPath = _mtempPath;
            //记录配置文件的值
            _mtempPath = sTempPath;
            //将静态实例置空(为空会重新获取)
            _mconfigurationInstance = null;
        }

        #endregion

        #region Check Method

        /// <summary>
        /// 检查是否存在Key
        /// </summary>
        /// <param name="Key">键</</param>
        /// <returns>存在结果</returns>
        public static bool ContainsKey(string Key) => ContainsKey(ConfigurationInstance, Key);

        /// <summary>
        /// 检查是否存在Key
        /// </summary>
        /// <param name="Config">配置文件实例</param>
        /// <param name="Key">键</param>
        /// <returns>存在结果</returns>
        public static bool ContainsKey(Configuration Config, string Key) => Config.AppSettings.Settings.AllKeys.Contains(Key);



        #endregion

        #region Add Method

        /// <summary>
        /// 添加键值对
        /// </summary>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        public static void AddConfigKeyValue(string Key, string Value)
        {
            AddConfigKeyValue(ConfigurationInstance, Key, Value);
        }

        /// <summary>
        /// 添加键值对
        /// </summary>
        /// <param name="Config">配置文件实例</param>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        public static void AddConfigKeyValue(Configuration Config, string Key, string Value)
        {
            Config.AppSettings.Settings.Add(Key, Value);
            Config.Save();
        }

        /// <summary>
        /// 尝试添加键值对
        /// </summary>
        /// <param name="Config">配置文件实例</param>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        /// <returns>添加结果</returns>
        public static bool TryAddConfigKeyValue(string Key, string Value)
        {
            return TryAddConfigKeyValue(ConfigurationInstance, Key, Value);
        }

        /// <summary>
        /// 尝试添加键值对
        /// </summary>
        /// <param name="Config">配置文件实例</param>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        /// <returns>添加结果</returns>
        public static bool TryAddConfigKeyValue(Configuration Config, string Key, string Value)
        {
            if (ContainsKey(Config, Key))
            {
                return false;
            }

            AddConfigKeyValue(Config, Key, Value);

            return true;
        }

        #endregion

        #region Set Method

        /// <summary>
        /// 设置键的值
        /// </summary>
        /// <param name="Config">配置文件实例</param>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        public static void SetConfigValue(string Key, string Value) => SetConfigValue(ConfigurationInstance, Key, Value);

        /// <summary>
        /// 设置键的值
        /// </summary>
        /// <param name="Config">配置文件实例</param>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        public static void SetConfigValue(Configuration Config, string Key, string Value)
        {
            Config.AppSettings.Settings[Key].Value = Value;
            Config.Save();
        }

        /// <summary>
        /// 尝试设置键的值
        /// </summary>
        /// <param name="Config">配置文件实例</param>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        /// <returns>设置结果</returns>
        public static bool TrySetConfigValue(string Key, string Value)
        {
            return TrySetConfigValue(ConfigurationInstance, Key, Value);
        }

        /// <summary>
        /// 尝试设置键的值
        /// </summary>
        /// <param name="Config">配置文件实例</param>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        /// <returns>设置结果></returns>
        public static bool TrySetConfigValue(Configuration Config, string Key, string Value)
        {
            if (!ContainsKey(Config, Key))
            {
                return false;
            }
            SetConfigValue(Config, Key, Value);
            return true;
        }

        #endregion

        #region Save Method

        /// <summary>
        /// 保存键值
        /// </summary>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        /// <param name="Create">如果不存在是否自动创建</param>
        public static void SaveKeyValue(string Key, string Value, bool Create = false)
        {
            SaveKeyValue(ConfigurationInstance, Key, Value, Create);
        }

        /// <summary>
        /// 保存键值
        /// </summary>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        /// <param name="Create">如果不存在是否自动创建</param>
        public static void SaveKeyValue(Configuration Config, string Key, string Value, bool Create = false)
        {
            SaveKeyValue(Config, new Dictionary<string, object>()
            {
                [Key] = Value
            }, Create);
        }


        /// <summary>
        /// 保存键值
        /// </summary>
        /// <param name="KeyValues">键值</param>
        /// <param name="Create">如果不存在是否自动创建</param>
        public static void SaveKeyValue(IDictionary<string, object> KeyValues, bool Create = false)
        {
            SaveKeyValue(ConfigurationInstance, KeyValues, Create);
        }

        /// <summary>
        /// 保存键值
        /// </summary>
        /// <param name="Config">配置文件实例</param>
        /// <param name="KeyValues">键值</param>
        /// <param name="Create">如果不存在是否自动创建</param>
        public static void SaveKeyValue(Configuration Config, IDictionary<string, object> KeyValues, bool Create = false)
        {
            foreach (KeyValuePair<string, object> kv in KeyValues)
            {
                string key = kv.Key;


                if (!ContainsKey(Config, key))
                {
                    if (Create)
                    {
                        AddConfigKeyValue(Config, key, kv.Value + string.Empty);
                    }
                }
                else
                {
                    SetConfigValue(Config, key, kv.Value + string.Empty);
                }
            }
        }

        /// <summary>
        /// 保存键值
        /// </summary>
        /// <param name="KeyValues">键值</param>
        /// <param name="FailFiles">保存失败的Key</param>
        public static void TrySaveKeyValue(IDictionary<string, object> KeyValues, out List<string> FailFiles)
        {
            TrySaveKeyValue(ConfigurationInstance, KeyValues, out FailFiles);
        }

        /// <summary>
        /// 保存键值
        /// </summary>
        /// <param name="Config">配置文件实例</param>
        /// <param name="KeyValues">键值</param>
        /// <param name="FailFiles">保存失败的Key</param>
        public static bool TrySaveKeyValue(Configuration Config, IDictionary<string, object> KeyValues, out List<string> FailFiles)
        {
            FailFiles = new List<string>(KeyValues.Count);

            foreach (KeyValuePair<string, object> kv in KeyValues)
            {
                string key = kv.Key;


                if (!ContainsKey(Config, key))
                {
                    FailFiles.Add(key);
                }
                else
                {
                    SetConfigValue(Config, key, kv.Value + string.Empty);
                }
            }

            return FailFiles.Count == 0;
        }


        #endregion

        #region Get Method

        /// <summary>
        /// 通过键获取值
        /// </summary>
        /// <param name="Key">键</param>
        /// <param name="Create">如果不存在是否自动创建</param>
        /// <param name="Default">默认值</param>
        public static string GetValueByKey(string Key, bool Create = false, string Default = null)
        {
            return GetValueByKey(ConfigurationInstance, Key, Create, Default);
        }

        /// <summary>
        /// 通过键获取值
        /// </summary>
        /// <param name="Config">配置文件实例</param>
        /// <param name="Key">键</param>
        /// <param name="Create">如果不存在是否自动创建</param>
        /// <param name="Default">默认值</param>
        /// <returns></returns>
        public static string GetValueByKey(Configuration Config, string Key, bool Create = false, string Default = null)
        {
            if (ContainsKey(Config, Key))
            {
                return Config.AppSettings.Settings[Key].Value;
            }
            else
            {
                if (Create)
                {
                    AddConfigKeyValue(Config, Key, Default);
                    return Default;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 通过字典获取键值
        /// </summary>
        /// <param name="KeyValues">键值</param>
        /// <param name="Create">如果不存在是否自动创建</param>
        /// <param name="Default">默认值</param>
        public static void GetValueByDic(IDictionary<string, string> KeyValues, bool Create = false, string Default = null)
        {
            GetValueByDic(ConfigurationInstance, KeyValues, Create, Default);
        }

        /// <summary>
        /// 通过字典获取键值
        /// </summary>
        /// <param name="Config">配置文件实例</param>
        /// <param name="KeyValues">键值</param>
        /// <param name="Create">如果不存在是否自动创建</param>
        /// <param name="Default">默认值</param>
        public static void GetValueByDic(Configuration Config, IDictionary<string, string> KeyValues, bool Create = false, string Default = null)
        {
            string[] arrStr = KeyValues.Select(x => x.Key).ToArray();

            for (int i = 0; i < arrStr.Length; i++)
            {
                string key = arrStr[i];

                if (ContainsKey(Config, key))
                {
                    KeyValues[key] = Config.AppSettings.Settings[key].Value;
                }
                else
                {
                    if (Create)
                    {
                        AddConfigKeyValue(Config, key, string.IsNullOrEmpty(Default) ? string.Empty : Default);
                    }
                }
            }
        }



        /// <summary>
        /// 通过ValueItem获取键值
        /// </summary>
        /// <param name="KeyValues">键值</param>
        /// <param name="Create">如果不存在是否自动创建</param>
        /// <param name="Default">默认值</param>
        public static void GetValueByValueItem(IEnumerable<ValueItem<string>> KeyValues, bool Create = false, string Default = null)
        {
            GetValueByValueItem(ConfigurationInstance, KeyValues, Create, Default);
        }

        /// <summary>
        /// 通过ValueItem获取键值
        /// </summary>
        /// <param name="Config">配置文件实例</param>
        /// <param name="KeyValues">ValueItem集合</param>
        /// <param name="Create">如果不存在是否自动创建</param>
        /// <param name="Default">默认值</param>
        public static void GetValueByValueItem(Configuration Config, IEnumerable<ValueItem<string>> KeyValues, bool Create = false, string Default = null)
        {
            foreach (var item in KeyValues)
            {
                string Key = item.Key;
                string Value = item.Value;

                if (!ContainsKey(Config, item.Key))
                {
                    if (Create)
                    {
                        Value = string.IsNullOrEmpty(Default) ? string.Empty : Default;
                        AddConfigKeyValue(Config, Key, Value);
                    }
                }

                item.Value = Value;
            }
        }


        #endregion

        #region Remove Method

        /// <summary>
        /// 移除一个键
        /// </summary>
        /// <param name="Key">键</param>
        /// <returns></returns>
        public static bool RemoveByKey(string Key)
        {
            return RemoveByKey(ConfigurationInstance, Key);
        }

        /// <summary>
        /// 移除一个键
        /// </summary>
        /// <param name="Config">配置文件实例</param>
        /// <param name="Key">键</param>
        /// <returns></returns>
        public static bool RemoveByKey(Configuration Config, string Key)
        {
            if (!ContainsKey(Config, Key))
            {
                return false;
            }

            RemoveByIEnum(Config, new string[] { Key });
            return true;
        }

        /// <summary>
        /// 移除一组
        /// </summary>
        /// <param name="IEnum">一组Key</param>
        public static void RemoveByIEnum(IEnumerable<string> IEnum)
        {
            RemoveByIEnum(ConfigurationInstance, IEnum);
        }

        /// <summary>
        /// 移除一组
        /// </summary>
        /// <param name="Config">配置文件实例</param>
        /// <param name="IEnum">一组Key</param>
        public static void RemoveByIEnum(Configuration Config, IEnumerable<string> IEnum)
        {
            foreach (string Key in IEnum)
            {
                Config.AppSettings.Settings.Remove(Key);
            }

            Config.Save();
        }

        #endregion

    }
}