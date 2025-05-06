// Шифр незнакомки

private static string DecodeMessage(string[] lines)
{
    string decodedMessage = "";
    for (int i = lines.Length - 1; i >= 0; i--)
    {
        string word = lines[i];
        string[] words = word.Split(' ');

        for (int k = words.Length - 1; k >= 0; k--)
        {
            string res = words[k];
            if ((res.Length != 0) && (char.IsUpper(res[0])))
            {
                decodedMessage = decodedMessage + res + " ";
            }
        }
    }
    return decodedMessage;
}

// Полезные знакомства

private static Dictionary<string, List<string>> OptimizeContacts(List<string> contacts)
{
    var dictionary = new Dictionary<string, List<string>>();
    foreach (var contact in contacts)
    {
        var name = contact.Substring(0, 2).Replace(":", "");
        if (!dictionary.ContainsKey(name))
            dictionary[name] = new List<string>();
        dictionary[name].Add(contact);
    }
    return dictionary;
}

// Карты памяти

1. C
2. A
3. A

// Закон Бенфорда

public static bool isDigit(string word)
{
    string digits = "1234567890";

    return digits.Contains(word[0]);
}
public static int[] GetBenfordStatistics(string text)
{
    string[] substings = text.Split(' ');

    List<String> onlyDigits = new List<String>();

    foreach (string s in substings)
    {
        if (isDigit(s))
        {
            onlyDigits.Add(s);
        }
    }
    var statistics = new int[10];

    foreach (string s in onlyDigits)
    {
        int a = int.Parse(s[0].ToString());
        statistics[a]++;

    }


    return statistics;
}

// Split и Join

public static string ReplaceIncorrectSeparators(string text)
{
    return text.Replace(" ", "\t").Replace(",", "").Replace(";", "").Replace("-", "").Replace(":", "").Replace("\t\t", "\t");
}

// Снова незнакомка

public static string ApplyCommands(string[] commands)
{
    var result = new StringBuilder();

    for (int i = 0; i < commands.Length; i++)
    {
        if (commands[i].StartsWith("push"))
            result.Append(commands[i].Substring(5));
        else if (commands[i].StartsWith("pop"))
        {
            int pop = int.Parse(commands[i].Substring(4));
            result.Remove(pop <= result.Length ? result.Length - pop : 0, pop);
        }
    }
    return result.ToString();
}

// Работа со строками

1.Этот код может быть существенно оптимизирован по производительности

Неверно

2. Выберите все верные утверждения: 

Строки являются типами-ссылками (Reference Type)

3. Что напечатает код Console.WriteLine("12345\n321");

две строки: 12345 и 321

4. Спецсимволы

\n Перевод строки
\t Табуляция
\\ Слэш
\r Возврат каретки
\" Кавычка

5. Как улучшить этот код? 

Использовать форматированный вывод

// Работа с файлами

1. System.IO
2. System.IO.Directory
3. 9
4. Формат записи латинских букв в кодировке UTF-8 совпадает с форматом в старой кодировке ASCII
Кодировка — это способ преобразования символов в байты и обратно
5. UTF-8
6. Код вызовет ошибку при исполнении

