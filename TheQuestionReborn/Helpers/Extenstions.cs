using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TheQuestionReborn.Helpers
{
    public static class Extensions
    {
        public static HtmlNode GetNode(this HtmlNode nodes, string name, string attribute, string attributeValue)
        {
            return nodes.Descendants().FirstOrDefault(x => (x.Name == name && x.Attributes[attribute] != null && x.Attributes[attribute].Value.Contains(attributeValue)));
        }

        public static string TranslateMonth(this string str)
        {
            var months = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("January", "Января"),
                new Tuple<string, string>("February","Февраля"),
                new Tuple<string, string>("March","Марта"),
                new Tuple<string, string>("April","Апреля"),
                new Tuple<string, string>("May","Мая"),
                new Tuple<string, string>("June","Июня"),
                new Tuple<string, string>("July","Июля"),
                new Tuple<string, string>("August","Августя"),
                new Tuple<string, string>("September","Сентября"),
                new Tuple<string, string>("October","Октября"),
                new Tuple<string, string>("November","Ноября"),
                new Tuple<string, string>("December", "Декабря")
            };

            foreach(var mon in months)
            {
                if (str.Contains(mon.Item1))
                    str = str.Replace(mon.Item1, mon.Item2);
            }

            return str;
        }
    }
}
