﻿namespace ComposedHealthBase.Server.Entities
{
	public interface IEntity
	{
		public long Id { get; set; }
		public bool IsActive { get; set; }
		public int CreatedBy { get; set; }
		public int LastModifiedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime ModifiedDate { get; set; }
	}
}
