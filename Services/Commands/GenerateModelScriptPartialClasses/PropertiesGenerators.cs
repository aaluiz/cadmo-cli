using System.Text;
using Models;


namespace Services.Commands
{
	public partial class GenerateModelByScript
	{
			
		private Property ProcessField(Field field)
		{
			if (field.ForeignKey != null) return ProcessForeignKey(field);
			return ProcessBasicFieldElements(ProcessAnnotations(field), field);
		}

		private Property ProcessFieldViewModel(Field field)
		{
			return ProcessBasicFieldElements(field);
		}

		private Property ProcessBasicFieldElements(Property property, Field field)
		{
			var result = property;
			result.TypeProperty = field.Type;
			result.hasGeterAndSeter = true;
			result.Name = field.Name;
			return result;
		}
		private Property ProcessBasicFieldElements(Field field)
		{
			var result = new Property();
			result.TypeProperty = field.Type;
			result.hasGeterAndSeter = true;
			result.Name = field.Name;
			return result;
		}

		private Property ProcessForeignKey(Field field)
		{
			var result = new Property
			{
				Visibility = Visibility.Public,
				TypeProperty = (field.ForeignKey!.Relationship == "OneToOne")
					? $"{field.ForeignKey.ModelName}? "
					: $"ICollection<{field.ForeignKey.ModelName}>? ",
				Name = field.Name
			};
			return result;
		}

		private Property ProcessAnnotations(Field field)
		{

			bool hasRequired = ((field.Required == null) ? false : true);
			bool hasStringLength = (field.StringLength != null) ? true : false;
			bool hasDataType = !string.IsNullOrEmpty(field.DataType);
			bool hasColumn = (field.Column == null) ? false : !string.IsNullOrEmpty(field.Column.TypeName);

			StringBuilder annotations = new StringBuilder();

			annotations.AppendLine((hasRequired) ? GetAnnotationRequired(field.Required!) : null);
			annotations.AppendLine((hasStringLength) ? GetAnnotationStringLength(field.StringLength!) : null);
			annotations.AppendLine((hasDataType) ? GetAnnotationDataType(field.DataType!) : null);
			annotations.AppendLine((hasColumn) ? GetAnnotationColumn(field.Column!.TypeName!) : null);

			var result = new Property
			{
				Annotations = annotations.ToString(),
			};

			return result;
		}

	}	
}