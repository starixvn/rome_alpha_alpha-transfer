namespace Alpha.Transfer.Helpers
{
    using System;
    using Newtonsoft.Json;

    public static class JsonHelper
    {
        [ThreadStatic]
        private static Func<object, string> toJsonFunc;

        public static Func<object, string> ToJsonFunc
        {
            get
            {
                return toJsonFunc ?? ToJson;
            }

            set
            {
                toJsonFunc = value;
            }
        }

        internal static string ToJson(object data) => JsonConvert.SerializeObject(data);
    }
}
