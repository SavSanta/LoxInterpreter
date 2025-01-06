
using System.Security.Claims;
using LoxInterpreter.Interpreter;
using LoxInterpreter.Parser;
using LoxInterpreter.Statement;

namespace LoxInterpreter.Statement { 


  interface IStatementVisitor
  {
     void visitBlockStmt(Block stmt);
     void visitPrintStmt(Print stmt);
     void visitVarStmt(Var stmt);
  }



    public class Block : Stmt 
    {
        public Block(List<Stmt> statements) 
        {
              this.statements = statements;
        }

        public override
        object Accept(IStatementVisitor visitor) 
        {
              return visitor.visitBlockStmt(this);
        }
        public List<Stmt> statements;
    }
public class Expression : Stmt {

    public Expression(ExprBase expr)
    {
        this.expr = expr;
    }

    public override
    object Accept(IStatementVisitor visitor) 
    {
        return visitor.visitExpressionStmt(this);
    }

    public ExprBase expr;
    }
public class Print : Stmt {

    public Print(ExprBase expression) {
      this.expression = expression;
    }

    public override
    object Accept(IStatementVisitor visitor) {
      return visitor.visitPrintStmt(this);
    }
    public ExprBase expression;
  }
public class Var : Stmt {

    public Var(Token name, ExprBase? initializer = null) {
      this.name = name;
      this.initializer = initializer;
    }

    public override
    object Accept(IStatementVisitor visitor) {
      return visitor.visitVarStmt(this);
    }
    public Token name;
    public ExprBase initializer;
  }


 }
