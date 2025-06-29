﻿using ComposedHealthBase.Shared.DTOs;
using Shared.DTOs;

namespace Shared.DTOs.CRM
{
	public class CustomerDto : IDto
	{
		public long Id { get; set; }
		public bool IsActive { get; set; }
		public int CreatedBy { get; set; }
		public int LastModifiedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime ModifiedDate { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Telephone { get; set; } = string.Empty;
		public int NumberOfEmployees { get; set; }
		public string Site { get; set; } = string.Empty;
		public string OHServicesRequired { get; set; } = string.Empty;
		public string Address { get; set; } = string.Empty;
		public string Industry { get; set; } = string.Empty;
		public string Postcode { get; set; } = string.Empty;
		public string Website { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string InvoiceEmail { get; set; } = string.Empty;
		public string? Notes { get; set; }
		public HashSet<ContractDto> Contracts { get; set; } = new();
	}
}
