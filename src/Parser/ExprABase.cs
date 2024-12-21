
using System.Data;
using System.Linq.Expressions;

namespace LoxInterpreter {
	namespace Parser {

        public abstract class ExprBase {
		public abstract IExprVisitor Accept(IExprVisitor visitor);	
		
		}
	}

}