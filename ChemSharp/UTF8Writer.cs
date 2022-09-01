using System.Text;

namespace ChemSharp;

public class UTF8Writer : StringWriter
{
	public override Encoding Encoding => Encoding.UTF8;
}
