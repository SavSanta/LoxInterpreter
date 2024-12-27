
using System.Data;
using System.Linq.Expressions;
using LoxInterpreter.Interpreter;

namespace LoxInterpreter {
	namespace Parser {

        public abstract class ExprBase {
		public abstract string Accept(IExprVisitor visitor);
        public abstract object Accept(IVisitorInterpreter visitorInterpreter);

        }
	}

}