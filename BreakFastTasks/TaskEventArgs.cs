using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakFastTasks
{
	internal class TaskEventArgs:EventArgs
	{
		public int progressPercent{ get; init;}
	}
}
