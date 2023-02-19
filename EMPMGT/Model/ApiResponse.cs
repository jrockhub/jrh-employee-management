using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EMPMGT.Model
{
    public class ApiResponse<T>
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ResponseCodeType ResponseCode { get; set; }
        public string Message { get; set; }

        public T ResponseObject { get; set; }
    }

    public enum ResponseCodeType
    {
        Success,
        Error
    }
}
