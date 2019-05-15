using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtil
{
    /// <summary>
    /// Json工具类.
    /// </summary>
    public class JsonUtil
    {
        /// <summary>
        /// 初始化json设置。
        ///目前需要做的只有日期，下面备注的是以后需要的部分。
        ///-空值处理
        ///setting.NullValueHandling = NullValueHandling.Ignore;
        ///-高级用法中的Bool类型转换 设置
        ///setting.Converters.Add(new BoolConvert("是,否"));
        /// </summary>
        public static void InitJsonConvert()
        {
            Newtonsoft.Json.JsonSerializerSettings setting = new Newtonsoft.Json.JsonSerializerSettings();
            JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
            {
                //日期类型默认格式化处理
                setting.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
                setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                return setting;
            });
        }
        /// <summary>
        /// 序列化对象, 序列化一个json字符串。
        /// </summary>
        /// <param name="obj">json字符串</param>
        /// <returns></returns>
        public static string SerializeObject(Object obj)
        {
            InitJsonConvert();
            return JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// 反序列化一个json字符串为实例对象。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static object DeserializeObject(string s)
        {
            return JsonConvert.DeserializeObject(s);
        }
        /// <summary>
        /// 反序列化一个json字符串为T类型的实例对象。
        /// </summary>
        /// <typeparam name="T">泛型类型，传入目标类型</typeparam>
        /// <param name="s">json字符串</param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string s)
        {
            return JsonConvert.DeserializeObject<T>(s);
        }
        /// <summary>
        /// 读取一个json属性，当该属性不存在时，返回空字符转转换的default类型实例。
        /// </summary>
        /// <typeparam name="T">泛型类型，传入目标类型</typeparam>
        /// <param name="result">Json 对象</param>
        /// <param name="name">属性名</param>
        /// <returns></returns>
        public static T GetJObjectPropVal<T>(JObject result, string name)
        {
            if (result.Property(name) == null) return CoreUtil.GetObjTranNull<T>("");
            return CoreUtil.GetObjTranNull<T>(result.Property(name).Value);
        }
    }
}
