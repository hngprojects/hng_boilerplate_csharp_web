using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Dashboard.Dtos
{
	public class NavigationDataDto
	{
		public List<ReportDataDto> Overview { get; set; }
	}

	public class ReportDataDto
	{
		public int Month { get; set; }
		public decimal Revenue { get; set; }
	}
}
