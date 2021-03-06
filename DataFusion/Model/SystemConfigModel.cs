﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DataFusion.Interfaces;

namespace DataFusion.Model
{
    [Serializable]
    [MetadataType(typeof(SystemConfigSg))]
    public class SystemConfigSg : ObservableObject, IDataErrorInfo
    {
        public SystemConfigSg()
        {


        }
        private string _redisServer = "127.0.0.1";
        private string _redisPwd = "gl";
        [Required(ErrorMessage = "Redis服务器不能为空")]
        public string RedisServer
        {
            get => _redisServer;
            set
            {
                Set(ref _redisServer, value);
            }
        }
        [Required(ErrorMessage = "Redis密码不能为空")]
        public string RedisPwd
        {
            get => _redisPwd;
            set
            {
                Set(ref _redisPwd, value);
            }
        }

        public string Error
        {
            get
            {
                return null;
            }
        }

        public string this[string columnName]
        {
            get
            {
                ValidationContext context = new ValidationContext(this, null, null);
                context.MemberName = columnName;
                var res = new List<ValidationResult>();
                var result = Validator.TryValidateProperty(this.GetType().GetProperty(columnName).GetValue(this, null), context, res);
                if (res.Count > 0)
                {
                    AddDic(dataErrors, context.MemberName);
                    return string.Join(Environment.NewLine, res.Select(r => r.ErrorMessage).ToArray());
                }
                RemoveDic(dataErrors, context.MemberName);
                return null;
            }
        }
        private Dictionary<string, string> dataErrors = new Dictionary<string, string>();

        public bool IsValid
        {
            get => dataErrors.Count == 0;
        }
        #region 附属方法

        /// <summary>
        /// 移除字典
        /// </summary>
        /// <param name="dics"></param>
        /// <param name="dicKey"></param>
        private void RemoveDic(Dictionary<String, String> dics, String dicKey)
        {
            dics.Remove(dicKey);
        }

        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="dics"></param>
        /// <param name="dicKey"></param>
        private void AddDic(Dictionary<String, String> dics, String dicKey)
        {
            if (!dics.ContainsKey(dicKey)) dics.Add(dicKey, "");
        }
        #endregion

        public SystemConfig ToSystemConfig()
        {
            return new SystemConfig()
            {
                RedisServer = RedisServer,
                RedisPwd = RedisPwd
            };
        }
    }
}
