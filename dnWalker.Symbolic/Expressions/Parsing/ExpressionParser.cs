using dnlib.DotNet;

using dnWalker.Symbolic.Variables;
using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace dnWalker.Symbolic.Expressions.Parsing
{
    // based on https://jack-vanlightly.com/blog/2016/2/3/creating-a-simple-tokenizer-lexer-in-c

    /// <summary>
    /// Very simple and naive expression parser, need to add more functionality to parse sep logic fomulae.
    /// </summary>
    public class ExpressionParser
    {
        public static readonly IEnumerable<Tokenizer<TokenType>.TokenDefinition> TokenDefinitions = new Tokenizer<TokenType>.TokenDefinition[]
        {
            new Tokenizer<TokenType>.TokenDefinition(new Regex("^(true|false)", RegexOptions.IgnoreCase), TokenType.BoolConstant),
            new Tokenizer<TokenType>.TokenDefinition(new Regex(@"^[0-9]*\.[0-9]+"), TokenType.DecimalConstant),
            new Tokenizer<TokenType>.TokenDefinition(new Regex(@"^[0-9]+"), TokenType.IntegerConstant),
            new Tokenizer<TokenType>.TokenDefinition(new Regex(@"^'[^']*'"), TokenType.StringConstant),
            new Tokenizer<TokenType>.TokenDefinition(new Regex(@"^null"), TokenType.Null),
            new Tokenizer<TokenType>.TokenDefinition(new Regex(@"^[a-zA-Z_][a-zA-Z0-9_]*"), TokenType.Variable),
            new Tokenizer<TokenType>.TokenDefinition(new Regex(@"^\("), TokenType.OpenBracket),
            new Tokenizer<TokenType>.TokenDefinition(new Regex(@"^\)"), TokenType.CloseBracket),
            new Tokenizer<TokenType>.TokenDefinition(new Regex(@"^&&"), TokenType.And),
            new Tokenizer<TokenType>.TokenDefinition(new Regex(@"^\|\|"), TokenType.Or),
            new Tokenizer<TokenType>.TokenDefinition(new Regex(@"^\+"), TokenType.Add),
            new Tokenizer<TokenType>.TokenDefinition(new Regex("^-"), TokenType.Substract),
            new Tokenizer<TokenType>.TokenDefinition(new Regex(@"^\*"), TokenType.Multiply),
            new Tokenizer<TokenType>.TokenDefinition(new Regex(@"^/"), TokenType.Divide),
            new Tokenizer<TokenType>.TokenDefinition(new Regex(@"^%"), TokenType.Modulo),
            new Tokenizer<TokenType>.TokenDefinition(new Regex(@"^=="), TokenType.Equals),
            new Tokenizer<TokenType>.TokenDefinition(new Regex(@"^!="), TokenType.NotEquals),
            new Tokenizer<TokenType>.TokenDefinition(new Regex(@"^>="), TokenType.GreaterThanOrEqual),
            new Tokenizer<TokenType>.TokenDefinition(new Regex(@"^<="), TokenType.LessThanOrEqual),
            new Tokenizer<TokenType>.TokenDefinition(new Regex(@"^<"), TokenType.LessThan),
            new Tokenizer<TokenType>.TokenDefinition(new Regex(@"^>"), TokenType.GreaterThan),
            new Tokenizer<TokenType>.TokenDefinition(new Regex(@"^!"), TokenType.Not),
        };
        private readonly Tokenizer<TokenType> _tokenizer;

        public enum TokenType
        {
            Invalid,
            BoolConstant,
            DecimalConstant,
            IntegerConstant,
            StringConstant,
            Variable,
            Null,
            OpenBracket,
            CloseBracket,
            And,
            Or,
            Not,
            Add,
            Substract,
            Multiply,
            Divide,
            Modulo,
            Equals,
            NotEquals,
            GreaterThan,
            GreaterThanOrEqual,
            LessThan,
            LessThanOrEqual
        }


        private readonly ExpressionFactory _expressionFactory;
        private readonly IReadOnlyDictionary<string, Expression> _freeVariables;

        public ExpressionParser(IReadOnlyDictionary<string, TypeSig> freeVariables, ExpressionFactory expressionFactory)
        {
            _expressionFactory = expressionFactory;
            _freeVariables = new Dictionary<string, Expression>(freeVariables.Select(p => KeyValuePair.Create(p.Key, expressionFactory.MakeVariable(new NamedVariable(p.Value, p.Key)))));

            _tokenizer = new Tokenizer<TokenType>(TokenDefinitions);
        }

        public Expression? Parse(string expressionString)
        {
            Queue<Tokenizer<TokenType>.Token> tokens = new Queue<Tokenizer<TokenType>.Token>(_tokenizer.Tokenize(expressionString, TokenType.Invalid));

            Expression? expression = BuildExpression(tokens);
            return expression;
        }

        private Expression? BuildExpression(Queue<Tokenizer<TokenType>.Token> tokens)
        {
            ExpressionFactory ef = _expressionFactory;

            Stack<Expression> operands = new Stack<Expression>();
            Stack<Operator> operators = new Stack<Operator>();

            while (tokens.TryDequeue(out Tokenizer<TokenType>.Token? token))
            {
                int offset = token.Offset;
                string str = token.Value;
                switch (token.Type)
                {
                    case TokenType.BoolConstant:
                        ProcessOperand(ef.MakeBooleanConstant(bool.Parse(str)));
                        break;

                    case TokenType.DecimalConstant:
                        ProcessOperand(ef.MakeRealConstant(double.Parse(str)));
                        break;

                    case TokenType.IntegerConstant:
                        ProcessOperand(ef.MakeIntegerConstant(long.Parse(str)));
                        break;

                    case TokenType.StringConstant:
                        ProcessOperand(ef.MakeStringConstant(str.Trim('\'', '"')));
                        break;

                    case TokenType.Variable:
                        ProcessOperand(_freeVariables[str]);
                        break;

                    case TokenType.Null:
                        {
                            Expression toPush = ef.NullExpression;
                            if (operands.TryPeek(out Expression? e))
                            {
                                if (e.Type.IsString())
                                {
                                    toPush = ef.StringNullExpression;
                                }
                            }
                            ProcessOperand(toPush);
                            break;
                        }

                    case TokenType.OpenBracket:
                        ProcessOperand(BuildExpression(tokens) ?? throw new FormatException($"Empty brackets at {offset}."));
                        break;

                    case TokenType.CloseBracket:
                        {
                            if (operands.Count == 0) throw new FormatException($"Empty brackets at {offset}.");
                            return operands.Pop();
                        }

                    case TokenType.And:
                        operators.Push(Operator.And);
                        break;

                    case TokenType.Or:
                        operators.Push(Operator.Or);
                        break;

                    case TokenType.Not:
                        operators.Push(Operator.Not);
                        break;

                    case TokenType.Add:
                        operators.Push(Operator.Add);
                        break;

                    case TokenType.Substract:
                        operators.Push(Operator.Subtract);
                        break;

                    case TokenType.Multiply:
                        operators.Push(Operator.Multiply);
                        break;

                    case TokenType.Divide:
                        operators.Push(Operator.Divide);
                        break;

                    case TokenType.Modulo:
                        operators.Push(Operator.Modulo);
                        break;

                    case TokenType.Equals:
                        operators.Push(Operator.Equal);
                        break;

                    case TokenType.NotEquals:
                        operators.Push(Operator.NotEqual);
                        break;

                    case TokenType.GreaterThan:
                        operators.Push(Operator.GreaterThan);
                        break;

                    case TokenType.GreaterThanOrEqual:
                        operators.Push(Operator.GreaterThanOrEqual);
                        break;

                    case TokenType.LessThan:
                        operators.Push(Operator.LessThan);
                        break;

                    case TokenType.LessThanOrEqual:
                        operators.Push(Operator.LessThanOrEqual);
                        break;


                    case TokenType.Invalid:
                    default:
                        throw new FormatException($"Invalid token at {offset}.");
                }
            }

            if (operands.Count == 0) return null;
            return operands.Pop();

            void ProcessOperand(Expression e)
            {
                operands.Push(e);
                if (operators.TryPeek(out Operator op))
                {
                    if (op.IsBinary() && operands.Count >= 2) 
                    {
                        Expression rhs = operands.Pop();
                        Expression lhs = operands.Pop();
                        operators.Pop();
                        ProcessOperand(ef.MakeBinary(op, lhs, rhs));
                    }
                    else if (op.IsUnary() && operands.Count >= 1)
                    {
                        Expression operand = operands.Pop();
                        operators.Pop();
                        ProcessOperand(ef.MakeUnary(op, operand));
                    }
                }
            }
        }
    }
}
