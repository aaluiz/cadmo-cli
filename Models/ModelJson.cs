namespace Models
{
	public class Model
	{
		public string? Name { get; set; }

		public List<Dependency>? Dependencies { get; set; }

		public List<Field>? Fields { get; set; }
	}


	public class ModelJson{
		public Model? Model { get; set; }
	}

	public class Dependency{
		public string? Package { get; set; }
	}
}