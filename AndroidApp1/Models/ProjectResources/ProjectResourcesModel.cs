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

namespace ProjectResources
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
        [JsonProperty("CustomFields")]
        public CustomFields CustomFields { get; set; }
        [JsonProperty("EnterpriseResource")]
        public EnterpriseResource EnterpriseResource { get; set; }
        [JsonProperty("Assignments")]
        public Assignments Assignments { get; set; }
        [JsonProperty("DefaultAssignmentOwner")]
        public DefaultAssignmentOwner DefaultAssignmentOwner { get; set; }
        [JsonProperty("ActualCost")]
        public int ActualCost { get; set; }
        [JsonProperty("ActualCostWorkPerformed")]
        public string ActualCostWorkPerformed { get; set; }
        [JsonProperty("ActualCostWorkPerformedMilliseconds")]
        public int ActualCostWorkPerformedMilliseconds { get; set; }
        [JsonProperty("ActualCostWorkPerformedTimeSpan")]
        public string ActualCostWorkPerformedTimeSpan { get; set; }
        [JsonProperty("ActualOvertimeCost")]
        public int ActualOvertimeCost { get; set; }
        [JsonProperty("ActualOvertimeWork")]
        public string ActualOvertimeWork { get; set; }
        [JsonProperty("ActualOvertimeWorkMilliseconds")]
        public int ActualOvertimeWorkMilliseconds { get; set; }
        [JsonProperty("ActualOvertimeWorkTimeSpan")]
        public string ActualOvertimeWorkTimeSpan { get; set; }
        [JsonProperty("ActualWork")]
        public string ActualWork { get; set; }
        [JsonProperty("ActualWorkMilliseconds")]
        public int ActualWorkMilliseconds { get; set; }
        [JsonProperty("ActualWorkTimeSpan")]
        public string ActualWorkTimeSpan { get; set; }
        [JsonProperty("AvailableFrom")]
        public DateTime AvailableFrom { get; set; }
        [JsonProperty("AvailableTo")]
        public DateTime AvailableTo { get; set; }
        [JsonProperty("BaselineCost")]
        public int BaselineCost { get; set; }
        [JsonProperty("BaselineWork")]
        public string BaselineWork { get; set; }
        [JsonProperty("BaselineWorkMilliseconds")]
        public int BaselineWorkMilliseconds { get; set; }
        [JsonProperty("BaselineWorkTimeSpan")]
        public string BaselineWorkTimeSpan { get; set; }
        [JsonProperty("BudetCostWorkPerformed")]
        public int BudetCostWorkPerformed { get; set; }
        [JsonProperty("BudgetedCost")]
        public int BudgetedCost { get; set; }
        [JsonProperty("BudgetedCostWorkScheduled")]
        public int BudgetedCostWorkScheduled { get; set; }
        [JsonProperty("BudgetedWork")]
        public string BudgetedWork { get; set; }
        [JsonProperty("BudgetedWorkMilliseconds")]
        public int BudgetedWorkMilliseconds { get; set; }
        [JsonProperty("BudgetedWorkTimeSpan")]
        public string BudgetedWorkTimeSpan { get; set; }
        [JsonProperty("Cost")]
        public int Cost { get; set; }
        [JsonProperty("CostVariance")]
        public int CostVariance { get; set; }
        [JsonProperty("CostVarianceAtCompletion")]
        public int CostVarianceAtCompletion { get; set; }
        [JsonProperty("Created")]
        public DateTime Created { get; set; }
        [JsonProperty("CurrentCostVariance")]
        public int CurrentCostVariance { get; set; }
        [JsonProperty("Finish")]
        public DateTime Finish { get; set; }
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("IsBudgeted")]
        public bool IsBudgeted { get; set; }
        [JsonProperty("IsGenericResource")]
        public bool IsGenericResource { get; set; }
        [JsonProperty("IsOverAllocated")]
        public bool IsOverAllocated { get; set; }
        [JsonProperty("Modified")]
        public DateTime Modified { get; set; }
        [JsonProperty("Notes")]
        public object Notes { get; set; }
        [JsonProperty("OvertimeCost")]
        public int OvertimeCost { get; set; }
        [JsonProperty("OvertimeWork")]
        public string OvertimeWork { get; set; }
        [JsonProperty("OvertimeWorkMilliseconds")]
        public int OvertimeWorkMilliseconds { get; set; }
        [JsonProperty("OvertimeWorkTimeSpan")]
        public string OvertimeWorkTimeSpan { get; set; }
        [JsonProperty("PeakWork")]
        public string PeakWork { get; set; }
        [JsonProperty("PeakWorkMilliseconds")]
        public int PeakWorkMilliseconds { get; set; }
        [JsonProperty("PeakWorkTimeSpan")]
        public string PeakWorkTimeSpan { get; set; }
        [JsonProperty("PercentWorkComplete")]
        public int PercentWorkComplete { get; set; }
        [JsonProperty("RegularWork")]
        public string RegularWork { get; set; }
        [JsonProperty("RegularWorkMilliseconds")]
        public int RegularWorkMilliseconds { get; set; }
        [JsonProperty("RegularWorkTimeSpan")]
        public string RegularWorkTimeSpan { get; set; }
        [JsonProperty("RemainingCost")]
        public int RemainingCost { get; set; }
        [JsonProperty("RemainingOvertimeCost")]
        public int RemainingOvertimeCost { get; set; }
        [JsonProperty("RemainingOvertimeWork")]
        public string RemainingOvertimeWork { get; set; }
        [JsonProperty("RemainingOvertimeWorkMilliseconds")]
        public int RemainingOvertimeWorkMilliseconds { get; set; }
        [JsonProperty("RemainingOvertimeWorkTimeSpan")]
        public string RemainingOvertimeWorkTimeSpan { get; set; }
        [JsonProperty("RemainingWork")]
        public string RemainingWork { get; set; }
        [JsonProperty("RemainingWorkMilliseconds")]
        public int RemainingWorkMilliseconds { get; set; }
        [JsonProperty("RemainingWorkTimeSpan")]
        public string RemainingWorkTimeSpan { get; set; }
        [JsonProperty("ScheduleCostVariance")]
        public int ScheduleCostVariance { get; set; }
        [JsonProperty("Start")]
        public DateTime Start { get; set; }
        [JsonProperty("Work")]
        public string Work { get; set; }
        [JsonProperty("WorkMilliseconds")]
        public int WorkMilliseconds { get; set; }
        [JsonProperty("WorkTimeSpan")]
        public string WorkTimeSpan { get; set; }
        [JsonProperty("WorkVariance")]
        public string WorkVariance { get; set; }
        [JsonProperty("WorkVarianceMilliseconds")]
        public int WorkVarianceMilliseconds { get; set; }
        [JsonProperty("WorkVarianceTimeSpan")]
        public string WorkVarianceTimeSpan { get; set; }
        [JsonProperty("CanLevel")]
        public bool CanLevel { get; set; }
        [JsonProperty("Code")]
        public object Code { get; set; }
        [JsonProperty("CostAccrual")]
        public int CostAccrual { get; set; }
        [JsonProperty("CostCenter")]
        public object CostCenter { get; set; }
        [JsonProperty("CostPerUse")]
        public int CostPerUse { get; set; }
        [JsonProperty("DefaultBookingType")]
        public int DefaultBookingType { get; set; }
        [JsonProperty("Email")]
        public object Email { get; set; }
        [JsonProperty("Group")]
        public object Group { get; set; }
        [JsonProperty("Initials")]
        public string Initials { get; set; }
        [JsonProperty("MaterialLabel")]
        public object MaterialLabel { get; set; }
        [JsonProperty("MaximumCapacity")]
        public int MaximumCapacity { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("OvertimeRate")]
        public int OvertimeRate { get; set; }
        [JsonProperty("OvertimeRateUnits")]
        public int OvertimeRateUnits { get; set; }
        [JsonProperty("Phonetics")]
        public object Phonetics { get; set; }
        [JsonProperty("StandardRate")]
        public int StandardRate { get; set; }
        [JsonProperty("StandardRateUnits")]
        public int StandardRateUnits { get; set; }
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
    public class CustomFields
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Deferred
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
    public class EnterpriseResource
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Assignments
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class DefaultAssignmentOwner
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }

}