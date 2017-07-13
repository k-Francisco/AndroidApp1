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

namespace Custom_Lists
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
        [JsonProperty("FirstUniqueAncestorSecurableObject")]
        public FirstUniqueAncestorSecurableObject FirstUniqueAncestorSecurableObject { get; set; }
        [JsonProperty("RoleAssignments")]
        public RoleAssignments RoleAssignments { get; set; }
        [JsonProperty("Activities")]
        public Activities Activities { get; set; }
        [JsonProperty("ContentTypes")]
        public ContentTypes ContentTypes { get; set; }
        [JsonProperty("CreatablesInfo")]
        public CreatablesInfo CreatablesInfo { get; set; }
        [JsonProperty("DefaultView")]
        public DefaultView DefaultView { get; set; }
        [JsonProperty("DescriptionResource")]
        public DescriptionResource DescriptionResource { get; set; }
        [JsonProperty("EventReceivers")]
        public EventReceivers EventReceivers { get; set; }
        [JsonProperty("Fields")]
        public Fields Fields { get; set; }
        [JsonProperty("Forms")]
        public Forms Forms { get; set; }
        [JsonProperty("InformationRightsManagementSettings")]
        public InformationRightsManagementSettings InformationRightsManagementSettings { get; set; }
        [JsonProperty("Items")]
        public Items Items { get; set; }
        [JsonProperty("ParentWeb")]
        public ParentWeb ParentWeb { get; set; }
        [JsonProperty("RootFolder")]
        public RootFolder RootFolder { get; set; }
        [JsonProperty("Subscriptions")]
        public Subscriptions Subscriptions { get; set; }
        [JsonProperty("TitleResource")]
        public TitleResource TitleResource { get; set; }
        [JsonProperty("UserCustomActions")]
        public UserCustomActions UserCustomActions { get; set; }
        [JsonProperty("Views")]
        public Views Views { get; set; }
        [JsonProperty("WorkflowAssociations")]
        public WorkflowAssociations WorkflowAssociations { get; set; }
        [JsonProperty("AllowContentTypes")]
        public bool AllowContentTypes { get; set; }
        [JsonProperty("BaseTemplate")]
        public int BaseTemplate { get; set; }
        [JsonProperty("BaseType")]
        public int BaseType { get; set; }
        [JsonProperty("ContentTypesEnabled")]
        public bool ContentTypesEnabled { get; set; }
        [JsonProperty("CrawlNonDefaultViews")]
        public bool CrawlNonDefaultViews { get; set; }
        [JsonProperty("Created")]
        public DateTime Created { get; set; }
        [JsonProperty("CurrentChangeToken")]
        public CurrentChangeToken CurrentChangeToken { get; set; }
        [JsonProperty("CustomActionElements")]
        public CustomActionElements CustomActionElements { get; set; }
        [JsonProperty("DefaultContentApprovalWorkflowId")]
        public string DefaultContentApprovalWorkflowId { get; set; }
        [JsonProperty("DefaultItemOpenUseListSetting")]
        public bool DefaultItemOpenUseListSetting { get; set; }
        [JsonProperty("Description")]
        public string Description { get; set; }
        [JsonProperty("Direction")]
        public string Direction { get; set; }
        [JsonProperty("DocumentTemplateUrl")]
        public object DocumentTemplateUrl { get; set; }
        [JsonProperty("DraftVersionVisibility")]
        public int DraftVersionVisibility { get; set; }
        [JsonProperty("EnableAttachments")]
        public bool EnableAttachments { get; set; }
        [JsonProperty("EnableFolderCreation")]
        public bool EnableFolderCreation { get; set; }
        [JsonProperty("EnableMinorVersions")]
        public bool EnableMinorVersions { get; set; }
        [JsonProperty("EnableModeration")]
        public bool EnableModeration { get; set; }
        [JsonProperty("EnableVersioning")]
        public bool EnableVersioning { get; set; }
        [JsonProperty("EntityTypeName")]
        public string EntityTypeName { get; set; }
        [JsonProperty("ExemptFromBlockDownloadOfNonViewableFiles")]
        public bool ExemptFromBlockDownloadOfNonViewableFiles { get; set; }
        [JsonProperty("FileSavePostProcessingEnabled")]
        public bool FileSavePostProcessingEnabled { get; set; }
        [JsonProperty("ForceCheckout")]
        public bool ForceCheckout { get; set; }
        [JsonProperty("HasExternalDataSource")]
        public bool HasExternalDataSource { get; set; }
        [JsonProperty("Hidden")]
        public bool Hidden { get; set; }
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("ImagePath")]
        public ImagePath ImagePath { get; set; }
        [JsonProperty("ImageUrl")]
        public string ImageUrl { get; set; }
        [JsonProperty("IrmEnabled")]
        public bool IrmEnabled { get; set; }
        [JsonProperty("IrmExpire")]
        public bool IrmExpire { get; set; }
        [JsonProperty("IrmReject")]
        public bool IrmReject { get; set; }
        [JsonProperty("IsApplicationList")]
        public bool IsApplicationList { get; set; }
        [JsonProperty("IsCatalog")]
        public bool IsCatalog { get; set; }
        [JsonProperty("IsPrivate")]
        public bool IsPrivate { get; set; }
        [JsonProperty("ItemCount")]
        public int ItemCount { get; set; }
        [JsonProperty("LastItemDeletedDate")]
        public DateTime LastItemDeletedDate { get; set; }
        [JsonProperty("LastItemModifiedDate")]
        public DateTime LastItemModifiedDate { get; set; }
        [JsonProperty("LastItemUserModifiedDate")]
        public DateTime LastItemUserModifiedDate { get; set; }
        [JsonProperty("ListExperienceOptions")]
        public int ListExperienceOptions { get; set; }
        [JsonProperty("ListItemEntityTypeFullName")]
        public string ListItemEntityTypeFullName { get; set; }
        [JsonProperty("MajorVersionLimit")]
        public int MajorVersionLimit { get; set; }
        [JsonProperty("MajorWithMinorVersionsLimit")]
        public int MajorWithMinorVersionsLimit { get; set; }
        [JsonProperty("MultipleDataList")]
        public bool MultipleDataList { get; set; }
        [JsonProperty("NoCrawl")]
        public bool NoCrawl { get; set; }
        [JsonProperty("ParentWebPath")]
        public ParentWebPath ParentWebPath { get; set; }
        [JsonProperty("ParentWebUrl")]
        public string ParentWebUrl { get; set; }
        [JsonProperty("ParserDisabled")]
        public bool ParserDisabled { get; set; }
        [JsonProperty("ServerTemplateCanCreateFolders")]
        public bool ServerTemplateCanCreateFolders { get; set; }
        [JsonProperty("TemplateFeatureId")]
        public string TemplateFeatureId { get; set; }
        [JsonProperty("Title")]
        public string Title { get; set; }
    }
    public class Metadata
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("uri")]
        public string Uri { get; set; }
        [JsonProperty("etag")]
        public string Etag { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
    public class FirstUniqueAncestorSecurableObject
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Deferred
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
    public class RoleAssignments
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Activities
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class ContentTypes
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class CreatablesInfo
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class DefaultView
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class DescriptionResource
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class EventReceivers
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Fields
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Forms
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class InformationRightsManagementSettings
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Items
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class ParentWeb
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class RootFolder
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Subscriptions
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class TitleResource
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class UserCustomActions
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Views
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class WorkflowAssociations
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class CurrentChangeToken
    {
        [JsonProperty("__metadata")]
        public Metadata1 Metadata { get; set; }
        [JsonProperty("StringValue")]
        public string StringValue { get; set; }
    }
    public class Metadata1
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
    public class CustomActionElements
    {
        [JsonProperty("__metadata")]
        public Metadata2 Metadata { get; set; }
        [JsonProperty("Items")]
        public Items1 Items { get; set; }
    }
    public class Metadata2
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
    public class Items1
    {
        [JsonProperty("__metadata")]
        public Metadata3 Metadata { get; set; }
        [JsonProperty("results")]
        public List<Result1> Results { get; set; }
    }
    public class Metadata3
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
    public class Result1
    {
        [JsonProperty("ClientSideComponentId")]
        public string ClientSideComponentId { get; set; }
        [JsonProperty("ClientSideComponentProperties")]
        public string ClientSideComponentProperties { get; set; }
        [JsonProperty("CommandUIExtension")]
        public object CommandUIExtension { get; set; }
        [JsonProperty("EnabledScript")]
        public object EnabledScript { get; set; }
        [JsonProperty("ImageUrl")]
        public object ImageUrl { get; set; }
        [JsonProperty("Location")]
        public string Location { get; set; }
        [JsonProperty("RegistrationId")]
        public string RegistrationId { get; set; }
        [JsonProperty("RegistrationType")]
        public int RegistrationType { get; set; }
        [JsonProperty("RequireSiteAdministrator")]
        public bool RequireSiteAdministrator { get; set; }
        [JsonProperty("Rights")]
        public Rights Rights { get; set; }
        [JsonProperty("Title")]
        public string Title { get; set; }
        [JsonProperty("UrlAction")]
        public string UrlAction { get; set; }
    }
    public class Rights
    {
        [JsonProperty("__metadata")]
        public Metadata4 Metadata { get; set; }
        [JsonProperty("High")]
        public string High { get; set; }
        [JsonProperty("Low")]
        public string Low { get; set; }
    }
    public class Metadata4
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
    public class ImagePath
    {
        [JsonProperty("__metadata")]
        public Metadata5 Metadata { get; set; }
        [JsonProperty("DecodedUrl")]
        public string DecodedUrl { get; set; }
    }
    public class Metadata5
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
    public class ParentWebPath
    {
        [JsonProperty("__metadata")]
        public Metadata6 Metadata { get; set; }
        [JsonProperty("DecodedUrl")]
        public string DecodedUrl { get; set; }
    }
    public class Metadata6
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }

}