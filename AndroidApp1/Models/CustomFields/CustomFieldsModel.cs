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

namespace AndroidApp1.Models.CustomFields
{
    public class RootObject
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }
    public class D
    {
        [JsonProperty("results")]
        public List<Result> Results { get; set; }
    }
    public class Result
    {
        [JsonProperty("__metadata")]
        public Metadata Metadata { get; set; }
        [JsonProperty("EntityType")]
        public EntityType EntityType { get; set; }
        [JsonProperty("LookupEntries")]
        public LookupEntries LookupEntries { get; set; }
        [JsonProperty("LookupTable")]
        public LookupTable LookupTable { get; set; }
        [JsonProperty("AppAlternateId")]
        public string AppAlternateId { get; set; }
        [JsonProperty("Description")]
        public object Description { get; set; }
        [JsonProperty("FieldType")]
        public int FieldType { get; set; }
        [JsonProperty("Formula")]
        public object Formula { get; set; }
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("InternalName")]
        public string InternalName { get; set; }
        [JsonProperty("IsEditableInVisibility")]
        public bool IsEditableInVisibility { get; set; }
        [JsonProperty("IsMultilineText")]
        public bool IsMultilineText { get; set; }
        [JsonProperty("IsRequired")]
        public bool IsRequired { get; set; }
        [JsonProperty("IsWorkflowControlled")]
        public bool IsWorkflowControlled { get; set; }
        [JsonProperty("LookupAllowMultiSelect")]
        public bool LookupAllowMultiSelect { get; set; }
        [JsonProperty("LookupDefaultValue")]
        public string LookupDefaultValue { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("RollsDownToAssignments")]
        public bool RollsDownToAssignments { get; set; }
        [JsonProperty("RollupType")]
        public int RollupType { get; set; }
    }
    public class Metadata
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("uri")]
        public string Uri { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
    public class EntityType
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Deferred
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
    public class LookupEntries
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class LookupTable
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }

}