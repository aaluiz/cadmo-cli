using Models;


namespace Services.Commands
{
	partial class GenerateModelByScript {
		
		private string GetAnnotationRequired(RequiredField requiredField)
		{
			if (string.IsNullOrEmpty(requiredField.ErrorMessage))
			{
				return "[Required]";
			}
			else
			{
				return $"[Required(ErrorMessage = \"{requiredField.ErrorMessage}\")]";
			}
		}

		private string GetAnnotationStringLength(StringLength stringLength)
		{
			if (stringLength.ErrorMessage != null &&
				stringLength.MaximumLength != null &&
				stringLength.MinimumLength != null)
			{
				return $"[StringLength({stringLength.MaximumLength}, ErrorMessage = \"{stringLength.ErrorMessage}\", MinimumLength = {stringLength.MinimumLength})]";
			}
			if (stringLength.ErrorMessage != null &&
				stringLength.MaximumLength != null)
			{
				return $"[StringLength({stringLength.MaximumLength}, ErrorMessage = \"{stringLength.ErrorMessage}\")]";
			}
			if (stringLength.ErrorMessage != null)
			{
				return $"[StringLength(ErrorMessage = \"{stringLength.ErrorMessage}\")]";
			}
			return "";
		}

		private string GetAnnotationDataType(string dataType)
		{
			return $"[DataType(DataType.{dataType})]";
		}

		private string GetAnnotationColumn(string typeName)
		{
			return $"[Column(TypeName = \"{typeName}\")]";
		}

	}	
}