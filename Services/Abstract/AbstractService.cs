
public class AbstractService{

	protected bool IsValidArgs(string[] args){
		return (args.Length == 2 || args.Length == 3);
	}
	
}