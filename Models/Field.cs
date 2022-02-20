namespace Models
{
	public class Field
	{
		public string? Name { get; set; }
		public bool ViewModelVisibility { get; set; }
		public string? DataType { get; set; }
		public string? Type { get; set; }
		public StringLength? StringLength { get; set; }
		public RequiredField? Required { get; set; }

		public Column? Column { get; set; }
		public ForeignKey? ForeignKey { get; set; }
	}
}