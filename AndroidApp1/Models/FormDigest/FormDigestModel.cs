using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace AndroidApp1.FormDigestModel
{
    public class RootObject
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }
    public class D
    {
        [JsonProperty("GetContextWebInformation")]
        public GetContextWebInformation GetContextWebInformation { get; set; }
    }
    public class GetContextWebInformation
    {
        [JsonProperty("__metadata")]
        public Metadata Metadata { get; set; }
        [JsonProperty("FormDigestTimeoutSeconds")]
        public int FormDigestTimeoutSeconds { get; set; }
        [JsonProperty("FormDigestValue")]
        public string FormDigestValue { get; set; }
        [JsonProperty("LibraryVersion")]
        public string LibraryVersion { get; set; }
        [JsonProperty("SiteFullUrl")]
        public string SiteFullUrl { get; set; }
        [JsonProperty("SupportedSchemaVersions")]
        public SupportedSchemaVersions SupportedSchemaVersions { get; set; }
        [JsonProperty("WebFullUrl")]
        public string WebFullUrl { get; set; }
    }
    public class Metadata
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
    public class SupportedSchemaVersions
    {
        [JsonProperty("__metadata")]
        public Metadata Metadata { get; set; }
        [JsonProperty("results")]
        public List<string> Results { get; set; }
    }

}