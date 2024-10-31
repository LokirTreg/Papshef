using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Papchef;
public enum LexemeType
{
    Keyword,
    Identifier,
    Constant,
    Operator,
    Separator,
    Unknown
}

public class Lexeme
{
    public LexemeType Type { get; }
    public string Value { get; }

    public Lexeme(LexemeType type, string value)
    {
        Type = type;
        Value = value;
    }

    public override string ToString()
    {
        return $"[{Type}, {Value}]";
    }
}

public class Lexer
{
    private static readonly string[] Keywords = { "while", "do", "loop", "and", "or" };
    private static readonly string[] Operators = { "<", "<=", ">", ">=", "=", "+", "-", "*", "/" };
    private static readonly char[] Separators = { '(', ')', ';', ',' };

    public List<Lexeme> Analyze(string input)
    {
        var lexemes = new List<Lexeme>();
        var tokens = input.Split(new[] { ' ', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var token in tokens)
        {
            if (Array.Exists(Keywords, k => k == token))
            {
                lexemes.Add(new Lexeme(LexemeType.Keyword, token));
            }
            else if (Regex.IsMatch(token, @"^\d+$"))
            {
                lexemes.Add(new Lexeme(LexemeType.Constant, token));
            }
            else if (Array.Exists(Operators, op => op == token))
            {
                lexemes.Add(new Lexeme(LexemeType.Operator, token));
            }
            else if (Array.Exists(Separators, sep => sep == token[0]))
            {
                lexemes.Add(new Lexeme(LexemeType.Separator, token));
            }
            else if (Regex.IsMatch(token, @"^[a-zA-Z_]\w*$"))
            {
                lexemes.Add(new Lexeme(LexemeType.Identifier, token));
            }
            else
            {
                lexemes.Add(new Lexeme(LexemeType.Unknown, token));
            }
        }

        return lexemes;
    }
}

public class Parser
{
    private List<Lexeme> lexemes;
    private int currentLexemeIndex;

    public Parser(List<Lexeme> lexemes)
    {
        this.lexemes = lexemes;
        this.currentLexemeIndex = 0;
    }

    public void Parse()
    {
        Console.WriteLine("\nParsing Result:");
        var tree = ParseCycle();
        if (tree != null && currentLexemeIndex == lexemes.Count)
        {
            Console.WriteLine("Expression is syntactically correct.");
            PrintSyntaxTree(tree, "");
        }
        else
        {
            Console.WriteLine("Syntax error in expression.");
        }
    }
    public bool Валидатор()
    {
        Console.WriteLine("\nParsing Result:");
        var tree = ParseCycle();
        if (tree != null && currentLexemeIndex == lexemes.Count)
        {
            Console.WriteLine("Expression is syntactically correct.");
            return true;
        }
        else
        {
            Console.WriteLine("Syntax error in expression.");
            return false;
        }
    }
    private Node ParseCycle()
    {
        if (CurrentLexemeIs(LexemeType.Keyword, "do"))
        {
            var doLexeme = NextLexeme();
            if (CurrentLexemeIs(LexemeType.Keyword, "while"))
            {
                var whileLexeme = NextLexeme();
                var expression = ParseExpression();
                if (expression != null && CurrentLexemeIs(LexemeType.Keyword, "loop"))
                {
                    var loopLexeme = NextLexeme();
                    return new Node(doLexeme, new Node(whileLexeme, expression, new Node(loopLexeme)));
                }
            }
        }
        return null;
    }

    private Node ParseExpression()
    {
        var left = ParseTerm();
        while (CurrentLexemeIs(LexemeType.Keyword, "and", "or"))
        {
            var operatorLexeme = NextLexeme();
            var right = ParseTerm();
            left = new Node(operatorLexeme, left, right);
        }
        return left;
    }

    private Node ParseTerm()
    {
        var left = ParseFactor();
        while (CurrentLexemeIs(LexemeType.Operator, "<", "<=", ">", ">=", "+", "-", "*", "/"))
        {
            var operatorLexeme = NextLexeme();
            var right = ParseFactor();
            left = new Node(operatorLexeme, left, right);
        }
        return left;
    }

    private Node ParseFactor()
    {
        if (CurrentLexemeIs(LexemeType.Identifier) || CurrentLexemeIs(LexemeType.Constant))
        {
            return new Node(NextLexeme());
        }
        return null;
    }

    private Lexeme NextLexeme()
    {
        return lexemes[currentLexemeIndex++];
    }

    private bool CurrentLexemeIs(LexemeType type, params string[] values)
    {
        if (currentLexemeIndex >= lexemes.Count) return false;

        var lexeme = lexemes[currentLexemeIndex];
        if (lexeme.Type != type) return false;
        return values.Length == 0 || Array.Exists(values, value => value == lexeme.Value);
    }

    private void PrintSyntaxTree(Node node, string indent)
    {
        if (node == null) return;
        Console.WriteLine($"{indent}{node.Lexeme}");
        PrintSyntaxTree(node.Left, indent + "  ");
        PrintSyntaxTree(node.Right, indent + "  ");
    }
}

public class Node
{
    public Lexeme Lexeme { get; }
    public Node Left { get; }
    public Node Right { get; }

    public Node(Lexeme lexeme, Node left = null, Node right = null)
    {
        Lexeme = lexeme;
        Left = left;
        Right = right;
    }
}

class Program
{
    static void Main()
    {
        // Пример входных данных в JSON формате
        string jsonInput = "{ \"code\": \"do while a < b and c >= d or e + f * g - h / i loop\" }";

        // Десериализация JSON
        var input = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonInput);
        string code = input["code"];

        Lexer lexer = new Lexer();
        List<Lexeme> lexemes = lexer.Analyze(code);

        Console.WriteLine("Lexical Analysis:");
        foreach (var lexeme in lexemes)
        {
            Console.WriteLine(lexeme);
        }

        Parser parser = new Parser(lexemes);
        //parser.Parse();
        parser.Валидатор();
        int totalSteps = 10;
        ConsoleProgressBar progressBar = new ConsoleProgressBar(0, Console.GetCursorPosition().Top, totalSteps);
        Console.F
        for (int i = 0; i <= totalSteps; i++)
        {
            
            progressBar.ShowProgress(i, $"Выполнено {i} из {totalSteps}");
            Thread.Sleep(100); // Задержка для имитации работы
        }
    }
}
