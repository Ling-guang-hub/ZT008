using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace zeta_framework
{
    public class ResourceCtrl : ResourceDB, ICtrl
    {
        public static ResourceCtrl Instance;

        public ResourceCtrl()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        /// <summary>
        /// 初始化存档数据
        /// </summary>
        /// <param name="data"></param>
        public void Init(JsonData data)
        {
            foreach (PropertyInfo propertyInfo in GetType().GetProperties())
            {
                object item = propertyInfo.GetValue(this);
                MethodInfo methodInfo = item.GetType().GetMethod("SetData");
                string key = propertyInfo.Name;
                methodInfo.Invoke(item, new object[] { data != null && data.ContainsKey(key) ? data[key] : null });
            }
        }

        /// <summary>
        /// 需要存档的数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetSerializeData()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            foreach (PropertyInfo property in GetType().GetProperties())
            {
                Item item = (Item)property.GetValue(this);
                data.Add(property.Name, item.data);
            }
            return data;
        }

        /// <summary>
        /// 修改资源数值
        /// </summary>
        /// <param name="item">要修改的资源实例</param>
        /// <param name="_value">变化值</param>
        /// <param name="checkMax">是否检查最大值</param>
        /// <returns></returns>
        public bool AddItemValue(Item item, double _value, bool checkMax = false)
        {
            bool addSuccess;
            addSuccess = item.AddValue(_value, checkMax);
            if (addSuccess)
            {
                MessageCenterLogic.GetInstance().Send(CConfig.mg_ItemChange_ + item.id, new MessageData(_value));
            }
            return addSuccess;
        }
        public bool AddItemValue(string item_id, double _value, bool checkMax = false)
        {
            return AddItemValue(GetItemById(item_id), _value, checkMax);
        }

        /// <summary>
        /// 直接设置属性值
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newValue"></param>
        /// <param name="checkValue"></param>
        public bool SetItemValue(Item item, double newValue, bool checkValue = false)
        {
            double oldValue = item.currentValue;
            bool success = item.SetValue(newValue, checkValue);
            if (success)
            {
                MessageCenterLogic.GetInstance().Send(CConfig.mg_ItemChange_ + item.id, new MessageData(newValue - oldValue));
            }
            return success;
        }
        public bool SetItemValue(string item_id, int newValue, bool checkValue = false)
        {
            Item item = GetItemById(item_id);
            return SetItemValue(item, newValue, checkValue);
        }

        /// <summary>
        /// 根据item_id获取资源对象
        /// </summary>
        /// <param name="item_id"></param>
        /// <returns></returns>
        public Item GetItemById(string item_id)
        {
            return (Item)GetType().GetProperty(item_id).GetValue(this);
        }

    }
}
