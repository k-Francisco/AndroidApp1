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

namespace EnterpriseResources
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
        [JsonProperty("Assignments")]
        public Assignments Assignments { get; set; }
        [JsonProperty("BaseCalendar")]
        public BaseCalendar BaseCalendar { get; set; }
        [JsonProperty("CostRateTables")]
        public CostRateTables CostRateTables { get; set; }
        [JsonProperty("CustomFields")]
        public CustomFields CustomFields { get; set; }
        [JsonProperty("DefaultAssignmentOwner")]
        public DefaultAssignmentOwner DefaultAssignmentOwner { get; set; }
        [JsonProperty("Engagements")]
        public Engagements Engagements { get; set; }
        [JsonProperty("UserPermissions")]
        public UserPermissions UserPermissions { get; set; }
        [JsonProperty("ResourceCalendarExceptions")]
        public ResourceCalendarExceptions ResourceCalendarExceptions { get; set; }
        [JsonProperty("TimesheetManager")]
        public TimesheetManager TimesheetManager { get; set; }
        [JsonProperty("User")]
        public User User { get; set; }
        [JsonProperty("CanLevel")]
        public bool CanLevel { get; set; }
        [JsonProperty("Code")]
        public object Code { get; set; }
        [JsonProperty("CostAccrual")]
        public int CostAccrual { get; set; }
        [JsonProperty("CostCenter")]
        public object CostCenter { get; set; }
        [JsonProperty("Created")]
        public DateTime Created { get; set; }
        [JsonProperty("DefaultBookingType")]
        public int DefaultBookingType { get; set; }
        [JsonProperty("Email")]
        public string Email { get; set; }
        [JsonProperty("ExternalId")]
        public object ExternalId { get; set; }
        [JsonProperty("Group")]
        public object Group { get; set; }
        [JsonProperty("HireDate")]
        public DateTime HireDate { get; set; }
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("Initials")]
        public string Initials { get; set; }
        [JsonProperty("IsActive")]
        public bool IsActive { get; set; }
        [JsonProperty("IsBudget")]
        public bool IsBudget { get; set; }
        [JsonProperty("IsCheckedOut")]
        public bool IsCheckedOut { get; set; }
        [JsonProperty("IsGeneric")]
        public bool IsGeneric { get; set; }
        [JsonProperty("IsTeam")]
        public bool IsTeam { get; set; }
        [JsonProperty("MaterialLabel")]
        public object MaterialLabel { get; set; }
        [JsonProperty("Modified")]
        public DateTime Modified { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Phonetics")]
        public object Phonetics { get; set; }
        [JsonProperty("RequiresEngagements")]
        public bool RequiresEngagements { get; set; }
        [JsonProperty("ResourceType")]
        public int ResourceType { get; set; }
        [JsonProperty("TerminationDate")]
        public DateTime TerminationDate { get; set; }
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
    public class Assignments
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Deferred
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
    public class BaseCalendar
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class CostRateTables
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class CustomFields
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class DefaultAssignmentOwner
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Engagements
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class UserPermissions
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class ResourceCalendarExceptions
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class TimesheetManager
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class User
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
}