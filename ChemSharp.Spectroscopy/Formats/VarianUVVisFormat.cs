using ChemSharp.Spectroscopy.Extensions;

namespace ChemSharp.Spectroscopy.Formats;

public class VarianUVVisFormat : FileFormat
{
	private readonly List<DataPoint> XYData = new();


	private VarianUVVisFormat()
	{
		ValidationMethod = FileExtensions.ValidateWithExtensions;
		NeededFiles = new Dictionary<string, Action<string>> {{".dsw", ReadDSW}};
	}

	private void ReadDSW(string file)
	{
		const int byteOffset = 0x459;
		var byteArray = File.ReadAllBytes(file);
		var cutOffLength = BitConverter.ToInt32(byteArray, 0x6d) * 8; //read length;
		var data = new float[cutOffLength / sizeof(float)];
		Buffer.BlockCopy(byteArray, byteOffset, data, 0, cutOffLength);
		if (data.Length % 2 != 0) throw new InvalidDataException("X and Y Axis Length mismatch");
		for (var i = 0; i < data.Length; i += 2)
			XYData.Add(new DataPoint(data[i], data[i + 1]));
	}

	public static Spectrum Read(string filename)
	{
		var format = new VarianUVVisFormat();
		format.Load(filename);
		return new Spectrum(format.XYData) {Title = filename};
	}
}
