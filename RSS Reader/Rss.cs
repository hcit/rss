using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
	public class Rss
	{
		[Serializable]
		public struct Items
		{
			public DateTime Date;

			public string Title;

			public string Description;

			public string Link;
		}
	}
}

