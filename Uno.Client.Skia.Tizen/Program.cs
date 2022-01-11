using Tizen.Applications;
using Uno.UI.Runtime.Skia;

namespace Uno.Client.Skia.Tizen
{
	class Program
{
	static void Main(string[] args)
	{
		var host = new TizenHost(() => new Uno.Client.App(), args);
		host.Run();
	}
}
}
