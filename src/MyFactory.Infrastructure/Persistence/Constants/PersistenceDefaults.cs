namespace MyFactory.Infrastructure.Persistence.Constants;

internal static class FieldLengths
{
    public const int Code = 64;
    public const int ShortText = 128;
    public const int Name = 256;
    public const int ContentType = 256;
    public const int Unit = 32;
    public const int Status = 64;
    public const int Description = 2000;
    public const int Contact = 256;
    public const int Email = 320;
    public const int DocumentReference = 128;
    public const int Notes = 512;
    public const int Path = 1024;
}

internal static class ColumnTypes
{
    public const string Quantity = "decimal(18,3)";
    public const string QuantitySmall = "decimal(10,2)";
    public const string Monetary = "decimal(18,2)";
    public const string MonetaryHighPrecision = "decimal(18,4)";
    public const string Percentage = "decimal(5,2)";
}
