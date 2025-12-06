using System;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Entities.Specifications;

namespace MyFactory.Domain.Entities.Shifts;

/// <summary>
/// Aggregate root describing a planned manufacturing shift for a specific employee and specification.
/// </summary>
public sealed class ShiftPlan : BaseEntity
{
	private ShiftPlan()
	{
	}

	public ShiftPlan(Guid employeeId, Guid specificationId, DateOnly shiftDate, string shiftType, decimal plannedQuantity)
	{
		Guard.AgainstEmptyGuid(employeeId, nameof(employeeId));
		Guard.AgainstEmptyGuid(specificationId, nameof(specificationId));
		EnsureValidDate(shiftDate);
		Guard.AgainstNullOrWhiteSpace(shiftType, nameof(shiftType));
		Guard.AgainstNegative(plannedQuantity, nameof(plannedQuantity));

		EmployeeId = employeeId;
		SpecificationId = specificationId;
		ShiftDate = shiftDate;
		ShiftType = shiftType.Trim();
		PlannedQuantity = plannedQuantity;
	}

	public Guid EmployeeId { get; private set; }
	public Employee? Employee { get; private set; }
	public Guid SpecificationId { get; private set; }
	public Specification? Specification { get; private set; }
	public DateOnly ShiftDate { get; private set; }
	public string ShiftType { get; private set; } = string.Empty;
	public decimal PlannedQuantity { get; private set; }

	public void UpdatePlannedQuantity(decimal quantity)
	{
		Guard.AgainstNegative(quantity, nameof(quantity));
		PlannedQuantity = quantity;
	}

	public void UpdateShiftType(string shiftType)
	{
		Guard.AgainstNullOrWhiteSpace(shiftType, nameof(shiftType));
		ShiftType = shiftType.Trim();
	}

	private static void EnsureValidDate(DateOnly date)
	{
		if (date == default)
		{
			throw new DomainException("Shift date must be specified.");
		}
	}
}

/// <summary>
/// Aggregate root capturing actual results achieved during a shift.
/// </summary>
public sealed class ShiftResult : BaseEntity
{
	private ShiftResult()
	{
	}

	public ShiftResult(Guid shiftPlanId, decimal actualQuantity, decimal hoursWorked, DateTime recordedAt)
	{
		Guard.AgainstEmptyGuid(shiftPlanId, nameof(shiftPlanId));
		Guard.AgainstNegative(actualQuantity, nameof(actualQuantity));
		Guard.AgainstNegative(hoursWorked, nameof(hoursWorked));
		Guard.AgainstDefaultDate(recordedAt, nameof(recordedAt));

		ShiftPlanId = shiftPlanId;
		ActualQuantity = actualQuantity;
		HoursWorked = hoursWorked;
		RecordedAt = recordedAt;
	}

	public Guid ShiftPlanId { get; private set; }
	public ShiftPlan? ShiftPlan { get; private set; }
	public decimal ActualQuantity { get; private set; }
	public decimal HoursWorked { get; private set; }
	public DateTime RecordedAt { get; private set; }

	public void UpdateActualQuantity(decimal quantity)
	{
		Guard.AgainstNegative(quantity, nameof(quantity));
		ActualQuantity = quantity;
	}

	public void UpdateHoursWorked(decimal hours)
	{
		Guard.AgainstNegative(hours, nameof(hours));
		HoursWorked = hours;
	}

	public void UpdateRecordedAt(DateTime recordedAt)
	{
		Guard.AgainstDefaultDate(recordedAt, nameof(recordedAt));
		RecordedAt = recordedAt;
	}
}


