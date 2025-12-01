using System;

namespace MyFactory.WebApi.Contracts.Finance;

public record RecordOverheadResponse(
	Guid Id,
	OverheadStatus Status
);

