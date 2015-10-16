using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saleae.SocketApi
{
	class Program
	{
		static void Main( string[] args )
		{
			Program app = new Program();

			app.UnitTestRawExport();
		}

		public void UnitTestRawExport()
		{
			ExportUnitTests tests = new ExportUnitTests();

			//unit tests not all gaunteed to run. This is primarily used for internal testing of the new export command.
			//This also demonstrates almost every major export combination.
			//This set of tests were designed to be used with a Logic 8, Logic Pro 8, or Logic Pro 16, and will not run with a different active device.

			//The trigger tests were designed to be used with a Logic Pro 8 and a signal generator producing a 1 Hz square wave on channel 0.

			tests.TriggerOffsetTest();
			//tests.RunAllUnitTests();





		}
	}
}
