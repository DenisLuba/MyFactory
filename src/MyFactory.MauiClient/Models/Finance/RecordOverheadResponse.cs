using System;

namespace MyFactory.MauiClient.Models.Finance;

public record RecordOverheadResponse(
    Guid Id,
    OverheadStatus Status);
