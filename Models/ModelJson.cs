namespace Models
{
	public class Model
	{
		public string? Name { get; set; }
		public List<Field>? Fields { get; set; }
	}


	public class ModelJson{
		public Model? Model { get; set; }
	}
}