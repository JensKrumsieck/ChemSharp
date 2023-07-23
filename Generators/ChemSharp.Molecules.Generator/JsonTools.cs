using System.Globalization;
using System.Text.Json;

namespace ChemSharp.Molecules.Generator;

public static class JsonTools
{
	public static string TryGetString(this JsonElement jsonObject, string propertyName)
	{
		if (!jsonObject.TryGetProperty(propertyName, out var value))
			return "";
		return value.GetString() ?? "";
	}

	public static string TryGetInt(this JsonElement jsonObject, string propertyName)
	{
		if (!jsonObject.TryGetProperty(propertyName, out var value))
			return 0.ToString(CultureInfo.InvariantCulture);
		if (value.ValueKind != JsonValueKind.Number)
			return 0.ToString(CultureInfo.InvariantCulture);
		if (!value.TryGetInt32(out var @int))
			return 0.ToString(CultureInfo.InvariantCulture);
		return @int.ToString(CultureInfo.InvariantCulture);
	}

	public static string TryGetDouble(this JsonElement jsonObject, string propertyName)
	{
		if (!jsonObject.TryGetProperty(propertyName, out var value))
			return 0d.ToString(CultureInfo.InvariantCulture);
		if (value.ValueKind != JsonValueKind.Number)
			return 0d.ToString(CultureInfo.InvariantCulture);
		if (!value.TryGetDouble(out var @double))
			return 0d.ToString(CultureInfo.InvariantCulture);
		return @double.ToString(CultureInfo.InvariantCulture);
	}
}
