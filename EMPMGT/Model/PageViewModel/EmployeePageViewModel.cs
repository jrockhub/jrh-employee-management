﻿using DocumentFormat.OpenXml.Drawing.Charts;
using Jrockhub.DataTablesServerSide.Attributes;
using System.ComponentModel;

namespace EMPMGT.Model.PageViewModel
{
	public class EmployeePageViewModel
	{
		[JqueryDataTableColumn(Order = 1)]
		public string Action { get; set; }

		[JqueryDataTableColumn(Order = 8)]
		public int Id { get; set; }

		[DisplayName("Email Address")]
		[JqueryDataTableColumn(Order = 2)]
		[SearchableString(EntityProperty = "Email")]
		[Sortable(EntityProperty = "Email", Default = true)]
		public string Email { get; set; }

		[DisplayName("First Name")]
		[JqueryDataTableColumn(Order = 3)]
		[SearchableString(EntityProperty = "FirstName")]
		[Sortable(EntityProperty = "FirstName", Default = true)]
		public string FirstName { get; set; }

		[DisplayName("Last Name")]
		[JqueryDataTableColumn(Order = 4)]
		[SearchableString(EntityProperty = "LastName")]
		[Sortable(EntityProperty = "LastName", Default = true)]
		public string LastName { get; set; }

		[DisplayName("Date Of Birth")]
		[JqueryDataTableColumn(Order = 5)]
		[SearchableString(EntityProperty = "DateOfBirthFormatted")]
		[Sortable(EntityProperty = "DateOfBirthFormatted", Default = true)]
		public string DateOfBirthFormatted { get; set; }



		[DisplayName("Department")]
		[JqueryDataTableColumn(Order = 6)]
		[SearchableString(EntityProperty = "Department")]
		[Sortable(EntityProperty = "Department", Default = true)]
		public string Department { get; set; }


		[JqueryDataTableColumn(Exclude = true)]
		public bool IsActive { get; set; }

		[DisplayName("Status")]
		[JqueryDataTableColumn(Order = 7)]
		[SearchableString(EntityProperty = "IsActiveFormatted")]
		[Sortable(EntityProperty = "IsActiveFormatted", Default = true)]
		public string IsActiveFormatted { get; set; }
	}
}
