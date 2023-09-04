using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace SpaceCards.API.PrivateResolvers
{
    public class PrivateResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(
            MemberInfo member,
            MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            if (!property.Writable)
            {
                var propertyInfo = member as PropertyInfo;
                var hasPrivateSetters = propertyInfo?.GetSetMethod(false) != null;
                property.Writable = hasPrivateSetters;
            }

            return property;
        }
    }
}
