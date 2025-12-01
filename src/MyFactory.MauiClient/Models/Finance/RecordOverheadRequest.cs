using System;

namespace MyFactory.MauiClient.Models.Finance;

public record RecordOverheadRequest(
    DateTime Date,
    string Article,
    decimal Amount,
    string Comment);
