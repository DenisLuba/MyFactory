using System;
using MyFactory.MauiClient.UIModels.Finance;

namespace MyFactory.MauiClient.Models.Finance;

public record RecordOverheadResponse(
    Guid Id,
    OverheadStatus Status);
