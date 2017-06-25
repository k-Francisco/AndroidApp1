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

namespace ProjectModel
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
        //_api/ProjectServer/Projects
        [JsonProperty("__metadata")]
        public Metadata Metadata { get; set; }
        [JsonProperty("CheckedOutBy")]
        public CheckedOutBy CheckedOutBy { get; set; }
        [JsonProperty("CustomFields")]
        public CustomFields CustomFields { get; set; }
        [JsonProperty("Engagements")]
        public Engagements Engagements { get; set; }
        [JsonProperty("EnterpriseProjectType")]
        public EnterpriseProjectType EnterpriseProjectType { get; set; }
        [JsonProperty("UserPermissions")]
        public UserPermissions UserPermissions { get; set; }
        [JsonProperty("Phase")]
        public Phase Phase { get; set; }
        [JsonProperty("ProjectSummaryTask")]
        public ProjectSummaryTask ProjectSummaryTask { get; set; }
        [JsonProperty("QueueJobs")]
        public QueueJobs QueueJobs { get; set; }
        [JsonProperty("Stage")]
        public Stage Stage { get; set; }
        [JsonProperty("Assignments")]
        public Assignments Assignments { get; set; }
        [JsonProperty("Calendar")]
        public Calendar Calendar { get; set; }
        [JsonProperty("Draft")]
        public Draft Draft { get; set; }
        [JsonProperty("EntityLinks")]
        public EntityLinks EntityLinks { get; set; }
        [JsonProperty("IncludeCustomFields")]
        public IncludeCustomFields IncludeCustomFields { get; set; }
        [JsonProperty("Owner")]
        public Owner Owner { get; set; }
        [JsonProperty("ProjectResources")]
        public ProjectResources ProjectResources { get; set; }
        [JsonProperty("TaskLinks")]
        public TaskLinks TaskLinks { get; set; }
        [JsonProperty("Tasks")]
        public Tasks Tasks { get; set; }
        //
        [JsonProperty("ApprovedEnd")]
        public DateTime ApprovedEnd { get; set; }
        [JsonProperty("ApprovedStart")]
        public DateTime ApprovedStart { get; set; }
        [JsonProperty("CalculateActualCosts")]
        public bool CalculateActualCosts { get; set; }
        [JsonProperty("CalculatesActualCosts")]
        public bool CalculatesActualCosts { get; set; }
        [JsonProperty("CheckedOutDate")]
        public DateTime CheckedOutDate { get; set; }
        [JsonProperty("CheckOutDescription")]
        public string CheckOutDescription { get; set; }
        [JsonProperty("CheckOutId")]
        public string CheckOutId { get; set; }
        [JsonProperty("CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [JsonProperty("CriticalSlackLimit")]
        public int CriticalSlackLimit { get; set; }
        [JsonProperty("DefaultFinishTime")]
        public DateTime DefaultFinishTime { get; set; }
        [JsonProperty("DefaultOvertimeRateUnits")]
        public int DefaultOvertimeRateUnits { get; set; }
        [JsonProperty("DefaultStandardRateUnits")]
        public int DefaultStandardRateUnits { get; set; }
        [JsonProperty("DefaultStartTime")]
        public DateTime DefaultStartTime { get; set; }
        [JsonProperty("HonorConstraints")]
        public bool HonorConstraints { get; set; }
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("IsCheckedOut")]
        public bool IsCheckedOut { get; set; }
        [JsonProperty("LastPublishedDate")]
        public DateTime LastPublishedDate { get; set; }
        [JsonProperty("LastSavedDate")]
        public DateTime LastSavedDate { get; set; }
        [JsonProperty("MoveActualIfLater")]
        public bool MoveActualIfLater { get; set; }
        [JsonProperty("MoveActualToStatus")]
        public bool MoveActualToStatus { get; set; }
        [JsonProperty("MoveRemainingIfEarlier")]
        public bool MoveRemainingIfEarlier { get; set; }
        [JsonProperty("MoveRemainingToStatus")]
        public bool MoveRemainingToStatus { get; set; }
        [JsonProperty("MultipleCriticalPaths")]
        public bool MultipleCriticalPaths { get; set; }
        [JsonProperty("OptimizerDecision")]
        public int OptimizerDecision { get; set; }
        [JsonProperty("PercentComplete")]
        public int PercentComplete { get; set; }
        [JsonProperty("PlannerDecision")]
        public int PlannerDecision { get; set; }
        [JsonProperty("ProjectType")]
        public int ProjectType { get; set; }
        [JsonProperty("ScheduledFromStart")]
        public bool ScheduledFromStart { get; set; }
        [JsonProperty("SplitInProgress")]
        public bool SplitInProgress { get; set; }
        [JsonProperty("SpreadActualCostsToStatus")]
        public bool SpreadActualCostsToStatus { get; set; }
        [JsonProperty("SpreadPercentCompleteToStatus")]
        public bool SpreadPercentCompleteToStatus { get; set; }
        [JsonProperty("SummaryTaskId")]
        public string SummaryTaskId { get; set; }
        [JsonProperty("CurrencyCode")]
        public string CurrencyCode { get; set; }
        [JsonProperty("CurrencyDigits")]
        public int CurrencyDigits { get; set; }
        [JsonProperty("CurrencyPosition")]
        public int CurrencyPosition { get; set; }
        [JsonProperty("CurrencySymbol")]
        public string CurrencySymbol { get; set; }
        [JsonProperty("CurrentDate")]
        public DateTime CurrentDate { get; set; }
        [JsonProperty("DaysPerMonth")]
        public int DaysPerMonth { get; set; }
        [JsonProperty("DefaultEffortDriven")]
        public bool DefaultEffortDriven { get; set; }
        [JsonProperty("DefaultEstimatedDuration")]
        public bool DefaultEstimatedDuration { get; set; }
        [JsonProperty("DefaultFixedCostAccrual")]
        public int DefaultFixedCostAccrual { get; set; }
        [JsonProperty("DefaultOvertimeRate")]
        public int DefaultOvertimeRate { get; set; }
        [JsonProperty("DefaultStandardRate")]
        public int DefaultStandardRate { get; set; }
        [JsonProperty("DefaultTaskType")]
        public int DefaultTaskType { get; set; }
        [JsonProperty("DefaultWorkFormat")]
        public int DefaultWorkFormat { get; set; }
        [JsonProperty("Description")]
        public string Description { get; set; }
        [JsonProperty("FinishDate")]
        public DateTime FinishDate { get; set; }
        [JsonProperty("FiscalYearStartMonth")]
        public int FiscalYearStartMonth { get; set; }
        [JsonProperty("MinutesPerDay")]
        public int MinutesPerDay { get; set; }
        [JsonProperty("MinutesPerWeek")]
        public int MinutesPerWeek { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("NewTasksAreManual")]
        public bool NewTasksAreManual { get; set; }
        [JsonProperty("NumberFiscalYearFromStart")]
        public bool NumberFiscalYearFromStart { get; set; }
        [JsonProperty("ProjectIdentifier")]
        public string ProjectIdentifier { get; set; }
        [JsonProperty("ProtectedActualsSynch")]
        public bool ProtectedActualsSynch { get; set; }
        [JsonProperty("ShowEstimatedDurations")]
        public bool ShowEstimatedDurations { get; set; }
        [JsonProperty("StartDate")]
        public DateTime StartDate { get; set; }
        [JsonProperty("StatusDate")]
        public DateTime StatusDate { get; set; }
        [JsonProperty("TrackingMode")]
        public int TrackingMode { get; set; }
        [JsonProperty("UtilizationDate")]
        public DateTime UtilizationDate { get; set; }
        [JsonProperty("UtilizationType")]
        public int UtilizationType { get; set; }
        [JsonProperty("WeekStartDay")]
        public int WeekStartDay { get; set; }
        [JsonProperty("WinprojVersion")]
        public string WinprojVersion { get; set; }

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
    public class CheckedOutBy
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Deferred
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
    public class CustomFields
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Engagements
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class EnterpriseProjectType
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class UserPermissions
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Phase
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class ProjectSummaryTask
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class QueueJobs
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Stage
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Assignments
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Calendar
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Draft
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class EntityLinks
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class IncludeCustomFields
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Owner
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class ProjectResources
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class TaskLinks
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Tasks
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }


}
