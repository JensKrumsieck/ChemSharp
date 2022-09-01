namespace ChemSharp.Files;

/// <summary>
///     Parameter Files are files using a key value storage to store data like the .par or jcamp files
/// </summary>
public class ParameterFile : PlainFile<string>
{
	public char Delimiter;

	/// <summary>
	///     <inheritdoc cref="PlainFile{T}(string)" />
	/// </summary>
	/// <param name="path"></param>
	/// <param name="delimiter"></param>
	public ParameterFile(string path, char delimiter) : base(path) => Delimiter = delimiter;

	/// <summary>
	///     <inheritdoc cref="PlainFile{T}()" />
	/// </summary>
	/// <param name="delimiter"></param>
	public ParameterFile(char delimiter) => Delimiter = delimiter;

	public IDictionary<string, string> Storage { get; set; }

	protected override void ContentChanged()
	{
		base.ContentChanged();
		Storage = ReadStorage().ToDictionary(s => s.Key, s => s.Value);
	}

	private IEnumerable<KeyValuePair<string, string>> ReadStorage() =>
		from line in Content
		select line.Split(new[] {Delimiter}, 2, StringSplitOptions.RemoveEmptyEntries)
		into raw
		where raw.Length == 2
		select new KeyValuePair<string, string>(raw[0].Trim(),
		                                        raw[1].Trim());
}
