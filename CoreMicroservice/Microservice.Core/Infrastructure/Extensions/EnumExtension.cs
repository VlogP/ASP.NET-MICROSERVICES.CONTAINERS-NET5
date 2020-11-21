using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Microservice.Core.Infrastructure.Extensions
{
    public static class EnumExtension
    {
        public static string GetDisplayName(this Enum value)
        {
            var result = "";

            var enumType = value.GetType();
            var memberName = value.ToString();
            var memberInfo = enumType.GetMember(memberName).FirstOrDefault();

            DisplayAttribute attribute = (DisplayAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(DisplayAttribute));

            if (attribute != null)
            {
                result = attribute.Name;
            }

            return result;
        }
    }
}
