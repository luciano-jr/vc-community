﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Catalog.Model
{
	public enum ReviewState
	{
		Draft,
		PendingApproval,
		Active,
		Rejected,
		Inactive
	}
}
