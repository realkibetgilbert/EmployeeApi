using Newtonsoft.Json;
using System.Globalization;
using System.Text.Json.Serialization;

namespace EmployeeApi.Helpers
{
    public class ErrorResponseData
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public string Path  { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
