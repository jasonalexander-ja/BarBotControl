using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBotControl.Worker.Models;

public class SchedulingMessage
{
	public int Position { get; set; }

	public SchedulingMessage(int position)
	{
		Position = position;
	}
}
