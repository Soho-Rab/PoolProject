/***************************************************************************
 *
 * Copyright (c) 2018 苏州云链信息咨询有限公司 All Rights Reserved.
 * 机器名称：      DESKTOP-DF8B3JQ
 * 公司名称：      苏州云链信息咨询有限公司
 * 命名空间：      Pool.Common.Cache
 * 文件名：        Class1
 * 唯一标识：      5f945698-db0c-46f7-9413-6c5563422632
 * 当前的用户域：  DESKTOP-DF8B3JQ
 * 创建人：        DevC
 * 创建时间：      2018/1/18 18:30:45
 * 描述：          
 *
 *=====================================================================*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Redis;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Pool.Common.Cache
{
    public class RedisCacheService : ICacheService
    {
        protected IDatabase _cache;
        private ConnectionMultiplexer _connection;
        private readonly string _instance;

        public RedisCacheService(RedisCacheOptions options, int database = 0)
        {
            _connection = ConnectionMultiplexer.Connect(options.Configuration);
            _cache = _connection.GetDatabase(database);
            _instance = options.InstanceName;

            //var cache = new RedisCache(options);
            /*options = new RedisCacheOptions
            {
                Configuration = "实例ID.redis.rds.aliyuncs.com:6379,password=实例ID:密码",
                InstanceName="CCS_"
            };*/
            //https://www.cnblogs.com/soundcode/p/6214287.html
            //await _cache.(cacheKey, cacheValue, new DistributedCacheEntryOptions());
        }

        public string GetKeyForRedis(string key)
        {
            return _instance + key.Trim();
        }

        public void Dispose()
        {
            if (_connection != null)
                _connection.Dispose();
            GC.SuppressFinalize(this);
        }

        #region 实现验证接口
        /// <summary>
        /// 验证缓存项是否存在
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }
            return _cache.KeyExists(GetKeyForRedis(key));
        }

        /// <summary>
        /// 验证缓存项是否存在
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public Task<bool> ExistsAsync(string key, int sleep = 0)
        {

            if (string.IsNullOrWhiteSpace(key))
            {
                return Task<bool>.Run(() =>
                {
                    if (sleep > 0) Thread.Sleep(sleep);
                    return false;
                });
            }
            return _cache.KeyExistsAsync(GetKeyForRedis(key));
        }

        #endregion


        #region 添加缓存

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <returns></returns>
        public bool Add(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }
            return _cache.StringSet(GetKeyForRedis(key), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)));
        }

        /// <summary>
        /// 添加缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="sleep">睡眠时长</param>
        /// <returns></returns>
        public Task<bool> AddAsync(string key, object value, int sleep = 0) {
            if (string.IsNullOrWhiteSpace(key))
            {
                return Task<bool>.Run(() =>
                {
                    if (sleep > 0) Thread.Sleep(sleep);
                    return false;
                });
            }
            return _cache.StringSetAsync(GetKeyForRedis(key), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)));
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间,Redis中无效）</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <returns></returns>
        public bool Add(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }
            return _cache.StringSet(GetKeyForRedis(key), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)), expiressAbsoulte);
        }

        /// <summary>
        /// 添加缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <param name="sleep">睡眠时长</param>
        /// <returns></returns>
        public Task<bool> AddAsync(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte, int sleep = 0)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return Task<bool>.Run(() =>
                {
                    if (sleep > 0) Thread.Sleep(sleep);
                    return false;
                });
            }
            return _cache.StringSetAsync(GetKeyForRedis(key), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)), expiressAbsoulte);
        }


        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresIn">缓存时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间,Redis中无效）</param>
        /// <returns></returns>
        public bool Add(string key, object value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }
            return _cache.StringSet(GetKeyForRedis(key), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)), expiresIn);
        }

        /// <summary>
        /// 添加缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresIn">缓存时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <param name="sleep">睡眠时长</param>
        /// <returns></returns>
        public Task<bool> AddAsync(string key, object value, TimeSpan expiresIn, bool isSliding = false, int sleep = 0)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return Task<bool>.Run(() =>
                {
                    if (sleep > 0) Thread.Sleep(sleep);
                    return false;
                });
            }
            return _cache.StringSetAsync(GetKeyForRedis(key), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)), expiresIn);
        }

        #endregion

        #region 删除缓存
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }
            return _cache.KeyDelete(GetKeyForRedis(key));
        }

        /// <summary>
        /// 删除缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="sleep">睡眠时长</param>
        /// <returns></returns>
        public Task<bool> RemoveAsync(string key, int sleep = 0) {
            if (string.IsNullOrWhiteSpace(key))
            {
                return Task<bool>.Run(() =>
                {
                    return false;
                });
            }
            return _cache.KeyDeleteAsync(GetKeyForRedis(key));
        }

        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <param name="keys">缓存Key集合</param>
        /// <returns></returns>
        public void RemoveAll(IEnumerable<string> keys)
        {
            if (keys != null)
            {
                keys.ToList().ForEach(item => Remove(GetKeyForRedis(item)));
            }
        }

        /// <summary>
        /// 批量删除缓存（异步方式）
        /// </summary>
        /// <param name="keys">缓存Key集合</param>
        /// <param name="sleep">睡眠时长</param>
        /// <returns></returns>
        public Task RemoveAllAsync(IEnumerable<string> keys, int sleep = 0) {
            return Task.Run(() =>
            {
                if (sleep > 0) Thread.Sleep(sleep);
                if (keys != null)
                {
                    keys.ToList().ForEach(item => RemoveAsync(GetKeyForRedis(item)));
                }
            });
        }

        #endregion

        #region 获取缓存
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }
            var value = _cache.StringGet(GetKeyForRedis(key));
            if (!value.HasValue)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// 获取缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="sleep">睡眠时长</param>
        /// <returns></returns>
        public Task<T> GetAsync<T>(string key, int sleep = 0) where T : class
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return Task<T>.Run(() =>
                {
                    if (sleep > 0) Thread.Sleep(sleep);
                    return default(T);
                });
            }
            var valueT = _cache.StringGetAsync(GetKeyForRedis(key));
            valueT.Start();
            var value = valueT.Result;
            if (!value.HasValue)
            {
                return Task<T>.Run(() =>
                {
                    if (sleep > 0) Thread.Sleep(sleep);
                    return default(T);
                });
            }
            return Task<T>.Run(() =>
            {
                if (sleep > 0) Thread.Sleep(sleep);
                return JsonConvert.DeserializeObject<T>(value);
            }); 
        }


        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public object Get(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }
            var value = _cache.StringGet(GetKeyForRedis(key));
            if (!value.HasValue)
            {
                return null;
            }
            return JsonConvert.DeserializeObject(value);
        }

        /// <summary>
        /// 获取缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="sleep">睡眠时长</param>
        /// <returns></returns>
        public Task<object> GetAsync(string key, int sleep = 0) {
            if (string.IsNullOrWhiteSpace(key))
            {
                return Task<object>.Run(() =>
                {
                    if (sleep > 0) Thread.Sleep(sleep);
                    return default(object);
                });
            }
            var valueT = _cache.StringGetAsync(GetKeyForRedis(key));
            valueT.Start();
            var value = valueT.Result;
            if (!value.HasValue)
            {
                return Task<object>.Run(() =>
                {
                    if (sleep > 0) Thread.Sleep(sleep);
                    return default(object);
                });
            }
            return Task<object>.Run(() =>
            {
                if (sleep > 0) Thread.Sleep(sleep);
                return JsonConvert.DeserializeObject(value);
            });
        }

        /// <summary>
        /// 获取缓存集合
        /// </summary>
        /// <param name="keys">缓存Key集合</param>
        /// <returns></returns>
        public IDictionary<string, object> GetAll(IEnumerable<string> keys)
        {
            if (keys == null)
            {
                return null;
            }
            var dict = new Dictionary<string, object>();
            keys.ToList().ForEach(item => dict.Add(item, Get(GetKeyForRedis(item))));
            return dict;
        }

        /// <summary>
        /// 获取缓存集合（异步方式）
        /// </summary>
        /// <param name="keys">缓存Key集合</param>
        /// <param name="sleep">睡眠时长</param>
        /// <returns></returns>
        public Task<IDictionary<string, object>> GetAllAsync(IEnumerable<string> keys, int sleep = 0)
        {
            return Task<IDictionary<string, object>>.Run(() =>
            {
                if (sleep > 0) Thread.Sleep(sleep);
                return GetAll(keys);
            });
        }
        #endregion

        #region 修改缓存
        /// <summary>
        /// 修改缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <returns></returns>
        public bool Replace(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }
            if (value == null)
            {
                return false;
            }
            if (Exists(key))
            {
                bool ismove = Remove(key);
                if(!ismove) return false;
            }
            return Add(key, value);
        }

        /// <summary>
        /// 修改缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <param name="sleep">睡眠时长</param>
        /// <returns></returns>
        public Task<bool> ReplaceAsync(string key, object value, int sleep = 0) {
            return Task<bool>.Run(() =>
            {
                if (sleep > 0) Thread.Sleep(sleep);
                return Replace(key, value);
            });
        }

        /// <summary>
        /// 修改缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <returns></returns>
        public bool Replace(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }
            if (value == null)
            {
                return false;
            }
            if (Exists(key))
            {
                bool ismove = Remove(key);
                if (!ismove) return false;
            }
            return Add(key, value, expiresSliding, expiressAbsoulte);
        }

        /// <summary>
        /// 修改缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <param name="sleep">睡眠时长</param>
        /// <returns></returns>
        public Task<bool> ReplaceAsync(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte, int sleep = 0) {
            return Task<bool>.Run(() =>
            {
                if (sleep > 0) Thread.Sleep(sleep);
                return Replace(key, value, expiresSliding, expiressAbsoulte);
            });
        }

        /// <summary>
        /// 修改缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <param name="expiresIn">缓存时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <returns></returns>
        public bool Replace(string key, object value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }
            if (value == null)
            {
                return false;
            }
            if (Exists(key))
            {
                bool ismove = Remove(key);
                if (!ismove) return false;
            }
            return Add(key, value, expiresIn, isSliding);
        }

        /// <summary>
        /// 修改缓存（异步方式）
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <param name="expiresIn">缓存时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <param name="sleep">睡眠时长</param>
        /// <returns></returns>
        public Task<bool> ReplaceAsync(string key, object value, TimeSpan expiresIn, bool isSliding = false, int sleep = 0) {
            return Task<bool>.Run(() =>
            {
                if (sleep > 0) Thread.Sleep(sleep);
                return Replace(key, value, expiresIn, isSliding);
            });
        }
        #endregion
    }
}
