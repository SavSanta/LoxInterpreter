
using System.Data;
using System.Linq.Expressions;

namespace LoxInterpreter {
	namespace Parser {

	abstract public class ExprBase {
		abstract public void accept(Visitor<T> visitor);	
		
		}
	}
}