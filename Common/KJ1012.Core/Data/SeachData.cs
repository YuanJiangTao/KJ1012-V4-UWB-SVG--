using System;
using System.Collections.Generic;

namespace KJ1012.Core.Data
{
    public class SearchData : Dictionary<string,string>
    {
        public SearchData():base(StringComparer.OrdinalIgnoreCase)
        {
        }
        /// <summary>
        /// 根据字段名获取某个字段的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            TryGetValue(key, out string o);
            return o;
        }
        /// <summary>
        ///  判断某个字段是否已设置
        /// </summary>
        /// <param name="key"></param>
        /// <returns>若字段key已被设置，则返回true，否则返回false</returns>
        public bool IsSet(string key)
        {
            TryGetValue(key, out string o);
            return !string.IsNullOrEmpty(o);
        }

        public Pager ToPager()
        {
            Pager pager=new Pager();
            if (IsSet("pageSize"))
            {
                int.TryParse(GetValue("pageSize"), out int pageSize);
                pager.PageSize = pageSize;
            }

            if (!IsSet("currentPage")) return pager;

            int.TryParse(GetValue("currentPage"), out int currentPage);
            pager.CurrentPage = currentPage;
            return pager;
        }
    }
}