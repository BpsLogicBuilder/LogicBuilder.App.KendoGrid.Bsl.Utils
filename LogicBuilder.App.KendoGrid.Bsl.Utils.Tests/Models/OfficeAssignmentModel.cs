using LogicBuilder.Domain;
using System.ComponentModel.DataAnnotations;


namespace LogicBuilder.App.KendoGrid.Bsl.Utils.Tests.Models
{
    public class OfficeAssignmentModel : BaseModel
    {
		public int InstructorID { get; set; }

		[StringLength(50)]
		[Display(Name = "Office Location")]
		public string? Location { get; set; }
    }
}