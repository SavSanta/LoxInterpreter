namespace LoxInterpreter.Interpreter
{
    public interface IStatementVisitor
    {
        void visitExpressionStatement(ExpressionStatement statement);
        void visitPrintStatement(PrintStatement statement);
        void visitReturnStatement(ReturnStatement statement);
        void visitIfStatement(IfStatement statement);
        void visitWhileStatement(WhileStatement statement);
        void visitForStatement(ForStatement statement);
        void visitBlockStatement(BlockStatement block);
        void visitVariableDeclaration(VariableDeclaration declaration);
        void visitFunctionDeclaration(FunctionDeclaration declaration);
        void visitMethodDeclaration(MethodDeclaration declaration);
        void visitClassDeclaration(ClassDeclaration declaration);
    }

    // Genericized Version
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
}