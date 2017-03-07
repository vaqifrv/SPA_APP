using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using App.Core.Infrastructure.Json;

namespace App.Web.UI.Infrastructure
{
    public class JsonNetFormatter : MediaTypeFormatter
    {

        private JsonSerializerSettings _jsonSerializerSettings;

        public JsonNetFormatter(JsonSerializerSettings jsonSerializerSettings = null)
        {
            _jsonSerializerSettings = jsonSerializerSettings ?? new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };

            //_jsonSerializerSettings.DateFormatString = "dd-mm-yyyy HH:mm:ss";
            _jsonSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            _jsonSerializerSettings.Culture = new CultureInfo("az-Latn-AZ");
            // Fill out the mediatype and encoding we support
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            SupportedEncodings.Add(new UTF8Encoding(false, true));
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            // Create a serializer
            JsonSerializer serializer = JsonSerializer.Create(_jsonSerializerSettings);

            // Create task reading the content
            return Task.Factory.StartNew(() =>
            {
                using (StreamReader streamReader = new StreamReader(readStream, SupportedEncodings[0]))
                {
                    using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
                    {
                        try

                        {
                            return serializer.Deserialize(jsonTextReader, type);
                        }
                        catch (Exception ex)
                        {
                            return null;
                        }
                    }
                }
            });
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            string NameOfSet = "";
            ObjectWrapperWithNameOfSet obj = value as ObjectWrapperWithNameOfSet;
            if (obj != null)
            {
                NameOfSet = obj.NameOfSet;
                value = obj.WrappedObject;
            }

            _jsonSerializerSettings.ContractResolver = new CustomContractResolver(NameOfSet);

            // Create a serializer
            JsonSerializer serializer = JsonSerializer.Create(_jsonSerializerSettings);

            // Create task writing the serialized content
            return Task.Factory.StartNew(() =>
            {
                using (JsonTextWriter jsonTextWriter = new JsonTextWriter(new StreamWriter(writeStream, SupportedEncodings[0])) { CloseOutput = false })
                {
                    try
                    {
                        serializer.Serialize(jsonTextWriter, value);
                        jsonTextWriter.Flush();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            });
        }
    }

    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            base.DateTimeFormat = "dd-MM-yyyy";
        }
    }
}
