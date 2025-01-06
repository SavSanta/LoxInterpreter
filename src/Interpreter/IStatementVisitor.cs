using LoxInterpreter.Parser;

namespace LoxInterpreter.Statement
{
    public abstract class Stmt
    {
        abstract public object Accept(IStatementVisitor visitor);
        public interface IStatementVisitor
        {
            object visitPrintStmt(Print statement);
            object visitBlockStmt(Block block);
            object visitVarStmt(Var declaration);
            object visitExpressionStmt(Expression statement);
            object visitAssignExpr(Binary.Assign assignment);
            /*        object visitReturnStatement(ReturnStatement statement);
                    object visitIfStatement(IfStatement statement);
                    object visitWhileStatement(WhileStatement statement);
                    object visitForStatement(ForStatement statement);
                    object visitFunctionDeclaration(FunctionDeclaration declaration);
                    object visitMethodDeclaration(MethodDeclaration declaration);
                    object visitClassDeclaration(ClassDeclaration declaration);
             */
        }

        // Genericized Version
        /*
            public interface IStatementVisitor<out T>
            {
                T visitExpressionStatement(ExpressionStatement statement);
                T visitPrintStatement(PrintStatement statement);
                T visitReturnStatement(ReturnStatement statement);
                T visitIfStatement(IfStatement statement);
                T visitWhileStatement(WhileStatement statement);
                T visitForStatement(ForStatement statement);
                T visitBlockStatement(BlockStatement block);
                T visitVariableDeclaration(VariableDeclaration declaration);
                T visitFunctionDeclaration(FunctionDeclaration declaration);
                T visitMethodDeclaration(MethodDeclaration declaration);
                T visitClassDeclaration(ClassDeclaration declaration);
            }
        */
    }
}