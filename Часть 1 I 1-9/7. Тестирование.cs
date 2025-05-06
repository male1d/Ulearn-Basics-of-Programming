// Тестирование

1.Всё
2.Всё кроме Тестирование ускоряет процесс разработки на начальном этапе и Тестирование позволяет гарантировать, что в создаваемом ПО не будет ни одной ошибки

// Модульные тесты

1. Чтобы можно было повторно использовать код из библиотек в других проектах
2. Всё кроме Тестировать программу через ввод-вывод невозможно и Это незыблемое правило, которому нужно следовать всегда без обсуждений и раздумий
3. Всё
4. Всё кроме Он проверяет работу только на одном примере — этого не достаточно!

// Модульные тесты 2

1. ... долю строк кода тестируемой программы, выполнившихся при запуске комплекта тестов
2. Всё кроме Чем выше процент кода, покрытого тестами, тем корректнее работает код
3. Всё кроме Покрытие кода тестами часто используется при подходе "Черный ящик"
4. Всё кроме Повышается скорость разработки
5. RepeatAttribute
6. TimeoutAttribute

// Практика «Поля в кавычках»

using System.Text;
using NUnit.Framework;
 
namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("''", 0, "", 2)]
        [TestCase("'1'", 0, "1", 3)]
        [TestCase("2 \"1'\"", 2, "1'", 4)]
        [TestCase("'1'2", 0, "1", 3)]
        [TestCase("1'2'", 1, "2", 3)]
        [TestCase(@"'1\' 2'", 0, "1' 2", 7)]
        public void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
        }
    }

    class QuotedFieldTask
    {
        public static Token ReadQuotedField(string line, int startIndex)
        {
            var builder = new StringBuilder();
            var tokenLength = 1;
            for (int i = startIndex + 1; i < line.Length; i++)
            {
                if (line[i - 1] != '\\' && line[startIndex] == line[i])
                {
                    tokenLength++;
                    break;
                }
                if (line[i] != '\\')
                {
                    builder.Append(line[i]);
                }
                tokenLength++;
            }
            return new Token(builder.ToString(), startIndex, tokenLength);
        }
    }
}

// Практика «Тестирование»

[TestCase("text", new[] { "text" })]
[TestCase("hello world", new[] { "hello", "world" })]
[TestCase("", new string[0])]
[TestCase("'a b'", new[] { "a b" })]
[TestCase(@"""1 '2' '3' 4""", new[] { "1 '2' '3' 4" })]
[TestCase(@"1""2 3 4 5""", new[] { "1", "2 3 4 5" })]
[TestCase(@"'""a""", new[] { @"""a""" })]
[TestCase(@"""a b c d""e", new[] { "a b c d", "e" })]
[TestCase(@"""a \""b\""""", new[] { @"a ""b""" })]
[TestCase(" a ", new[] { "a" })]
[TestCase(@"'a\'a\'a'", new[] { "a'a'a" })]
[TestCase(@" '' ", new[] { "" })]
[TestCase(@"""\\""", new[] { "\\" })]
[TestCase("'x ", new[] { "x " })]
[TestCase("hello  world", new[] { "hello", "world" })]
public static void RunTests(string input, string[] expectedOutput)
{
    // Тело метода изменять не нужно
    Test(input, expectedOutput);
}

// Практика «Парсер полей»

using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
 
namespace TableParser
{
    [TestFixture]
    public class FieldParserTaskTests
    {
        public static void Test(string input, string[] expectedResult)
        {
            var actualResult = FieldsParserTask.ParseLine(input);
            Assert.AreEqual(expectedResult.Length, actualResult.Count);
            for (int i = 0; i < expectedResult.Length; ++i)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i].Value);
            }
        }

        [TestCase("text", new[] { "text" })]
        [TestCase("", new string[0])]
        [TestCase("\"\\\"text\\\"\"", new[] { "\"text\"" })]
        [TestCase("'\\\'text\\\''", new[] { "'text'" })]
        [TestCase("'\"text\"", new[] { "\"text\"" })]
        [TestCase("hello  world ", new[] { "hello", "world" })]
        [TestCase("\"'hello' world\"", new[] { "'hello' world" })]
        [TestCase("\"hello\"world", new[] { "hello", "world" })]
        [TestCase(@"""\\""", new[] { "\\" })]
        [TestCase("hello\"world\"", new[] { "hello", "world" })]
        [TestCase("' ", new[] { " " })]
        [TestCase("\'\'", new[] { "" })]

        public static void RunTests(string input, string[] expectedOutput)
        {
            Test(input, expectedOutput);
        }
    }

    public class FieldsParserTask
    {
        public static List<Token> ParseLine(string line)
        {
            var list = new List<Token>();
            for (var i = 0; i < line.Length; i++)
            {
                if (line[i] == ' ')
                    continue;
                var token = TakeToken(line, i);
                list.Add(token);
                i = token.GetIndexNextToToken() - 1;
            }
            return list;
        }

        public static Token TakeToken(string line, int i)
        {
            if (line[i] == '\'' || line[i] == '\"')
                return ReadQuotedField(line, i);
            return ReadField(line, i);
        }

        private static Token ReadField(string line, int startIndex)
        {
            var stringBuilder = new StringBuilder();
            for (var i = startIndex; i < line.Length; i++)
            {
                if (line[i] == '\'' || line[i] == '\"' || line[i] == ' ')
                    break;
                stringBuilder.Append(line[i]);
            }
            return new Token(stringBuilder.ToString(), startIndex, stringBuilder.Length);
        }

        public static Token ReadQuotedField(string line, int startIndex)
        {
            return QuotedFieldTask.ReadQuotedField(line, startIndex);
        }
    }
}
