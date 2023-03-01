using ChemSharp.Memory;
using ChemSharp.Spectroscopy.Extensions;

namespace ChemSharp.Spectroscopy.Formats;

public class CSVFormat : FileFormat
{
	private readonly List<List<DataPoint>> MultiXYData = new();
	private readonly List<DataPoint> XYData = new();
	private char _delimiter = ',';
	private bool _firstDataRow = true;
	private bool _multiple;

	private CSVFormat()
	{
		ValidationMethod = FileExtensions.ValidateWithExtensions;
		NeededFiles = new Dictionary<string, Action<string>> {{".csv", ReadCSV}};
	}

	private void ReadCSV(string file)
	{
		using var fs = File.OpenRead(file);
		using var sr = new StreamReader(fs);
		while (sr.Peek() > 0)
		{
			var line = sr.ReadLine().AsSpan();
			ParseLine(line);
		}
	}

	private void ParseLine(ReadOnlySpan<char> line)
	{
		var cols = line.Split(new[] {_delimiter}.AsSpan());
		if (cols.Length == 0) return;
		var firstItem = line.Slice(cols[0].start, cols[0].length);
		if (!double.TryParse(firstItem.ToString(), out _)) return; //is header column
		var lastX = 0d;
		for (var i = 0; i < cols.Length; i++)
		{
			if (i % 2 == 1 && _multiple && _firstDataRow) MultiXYData.Add(new List<DataPoint>());
			var item = line.Slice(cols[i].start, cols[i].length).ToSingle();
			if (i % 2 == 0) lastX = item;
			else if (!_multiple) XYData.Add(new DataPoint(lastX, item));
			else MultiXYData[(i - 1) / 2].Add(new DataPoint(lastX, item));
		}

		_firstDataRow = false;
	}

	public static Spectrum Read(string filename, char delimiter)
	{
		var format = new CSVFormat {_delimiter = delimiter};
		format.Load(filename);
		return new Spectrum(format.XYData) {Title = filename};
	}

	public static List<Spectrum> ReadAll(string filename, char delimiter)
	{
		var format = new CSVFormat {_delimiter = delimiter, _multiple = true};
		format.Load(filename);
		return format.MultiXYData.Select(list => new Spectrum(list) {Title = filename}).ToList();
	}
}
