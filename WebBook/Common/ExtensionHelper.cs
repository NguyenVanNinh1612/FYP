using System.Text.Json;

namespace WebBook.Common
{
    public static class ExtensionHelper
    {

        public static string Status(this int value)
        {
            if (value == 0)
                return "Chờ xác nhận";
            if (value == 1)
                return "Đã xác nhận";
            if (value == 2)
                return "Sẵn sàng giao hàng";
            if (value == 3)
                return "Đang giao hàng";
            if (value == 4)
                return "Hoàn tất";
            if (value == 5)
                return "Đã hủy";
            return "";
            
        }

        public static string ToVnd(this decimal value)
        {
            return $"{value: #,##0.00} đ";
        }

        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
