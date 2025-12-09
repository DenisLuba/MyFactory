namespace MyFactory.Infrastructure.Common;

public class Settings
{
	public int StandardShiftHours { get; set; } = 8;

	public string Currency { get; set; } = "USD";

	public string FileStorageRoot { get; set; } = "storage";

	public bool SeedDemoData { get; set; } = true;
}

