using System.Collections.Immutable;
using Models;

namespace Services.Commands.Tools
{
	public static class CodesForNewProject
	{

		public static FileCode GetLoggerFile(){
			var result = new FileCode();
			result.Code = @"using System;

namespace Contracts
{
    public interface ILoggerManager
    {
        void LogInfo(string message);
        void LogWarn(string message);
        void LogDebug(string message);
        void LogError(string message);
    }
}
";
			result.FileName = "ILoggerManager.cs";

			return result;
		}
		public static ImmutableList<FileCode> ServiceAbstractCodes()
		{
			var result = new List<FileCode>();
			FileCode IUpdate = CreateIUpdate();
			result.Add(IUpdate);
			FileCode ISelectOne = CreateISelectOne();
			result.Add(ISelectOne);
			FileCode ISelectAll = CreateISelectAll();
			result.Add(ISelectAll);
			FileCode ISelectAllView = CreaateISelectAllView();
			result.Add(ISelectAllView);
			FileCode ISelectOneView = CreateISelectOneView();
			result.Add(ISelectOneView);
			FileCode IControlError = CreateIControlError();
			result.Add(IControlError);
			FileCode IDelete = CreateIDelete();
			result.Add(IDelete);
			FileCode IInsert = CreateIInsert();
			result.Add(IInsert);
			return result.ToImmutableList();
		}

		private static FileCode CreateIInsert()
		{
			var IInsert = new FileCode();
			IInsert.Code = @"namespace Contratos.Service.Abstract
{
    public interface IInsert<ViewModel>
    {
        bool NewRecord(ViewModel ViewModel);
    }
}
			";
			IInsert.FileName = "IInsert.cs";
			return IInsert;
		}

		private static FileCode CreateIDelete()
		{
			var IDelete = new FileCode();
			IDelete.Code = @"namespace Contracts.Service.Abstract
{
    public interface IDelete
    {
        bool Delete(int id);
    }
}
			";
			IDelete.FileName = "IDelete.cs";
			return IDelete;
		}

		private static FileCode CreateIControlError()
		{
			var IControlError = new FileCode();
			IControlError.Code = @"using System.Collections.Generic;

namespace Contracts.Service.Abstract
{
    public interface IControlError
    {
        bool HasErro { get; }

        List<string> ErroMessage { get; }
    }
}

			";
			IControlError.FileName = "IControlError.cs";
			return IControlError;
		}

		private static FileCode CreateISelectOneView()
		{
			var ISelectOneView = new FileCode();
			ISelectOneView.Code = @"namespace Contracts.Service.Abstract
{
    public interface ISelectOneView<ViewGetModel>
    {
        ViewGetModel SelectOneViewById(int Id);
    }
}

			";
			ISelectOneView.FileName = "ISelectOneView.cs";
			return ISelectOneView;
		}

		private static FileCode CreaateISelectAllView()
		{
			var ISelectAllView = new FileCode();
			ISelectAllView.Code = @"using System.Collections.Generic;

namespace Contracts.Service.Abstract
{
    public interface ISelectAllView<ViewGetModel>
    {
        List<ViewGetModel> SelectAllView();
    }

    public interface ISelectAllWithDependencyView<ViewGetModel>
    {
        List<ViewGetModel> SelectAllView(int RelationId);
    }
}
			";
			ISelectAllView.FileName = "ISelectAllViw.cs";
			return ISelectAllView;
		}

		private static FileCode CreateISelectAll()
		{
			var ISelectAll = new FileCode();
			ISelectAll.Code = @"using System.Collections.Generic;

namespace Contracts.Service.Abstract
{
    public interface ISelectAll<Model>
    {
        List<Model> SelectAll();
    }

    public interface ISelectAllWithDependency<Model>
    {
        List<Model> SelectAll(int RelationId);
    }
}
			";
			ISelectAll.FileName = "ISelectAll.cs";
			return ISelectAll;
		}

		private static FileCode CreateISelectOne()
		{
			var ISelectOne = new FileCode();
			ISelectOne.Code = @"
			namespace Contracts.Service.Abstract
{
    public interface ISelectOne<Model>
    {
        Model SelectById(int Id);
    }
}
			";
			ISelectOne.FileName = "ISelectOne.cs";
			return ISelectOne;
		}

		private static FileCode CreateIUpdate()
		{
			var IUpdate = new FileCode();
			IUpdate.Code = @"namespace Contracts.Service.Abstract
{
    public interface IUpdate<ViewModel>
    {
        bool Atualizar(ViewModel ViewModel);
    }
}
			";
			IUpdate.FileName = "IUpdate.cs";
			return IUpdate;
		}


		public static string JsonSchemaFile(){

			return @"
			
			";

		}
	}
}
