using Combinatorics.Collections;

namespace CrackHash.Worker.Application;

public static class BruteDictionary
{
    private static readonly List<char> Dictionary = new()
    {
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
        'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
        'u', 'v', 'w', 'x', 'y', 'z',
        /*'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'к', 
        'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф',
        'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я',
        'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'К', 
        'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф',
        'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ъ', 'Ы', 'Ь', 'Э', 'Ю', 'Я',
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
        'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
        'U', 'V', 'W', 'X', 'Y', 'Z', 
        '1', '2', '3', '4', '5', '6', '7', '8', '9', '0',
        '!', '@', '#', '$', '%', '&', '?', '*', '-'*/
    };
    
    public static Dictionary<int, List<string>> GetWordsDictionary(int maxLength)
    {
        var wordsDictionary = new Dictionary<int, List<string>>();
        
        for (var i = 1; i <= maxLength; ++i)
        {
            var variations = new Variations<char>(Dictionary, i, GenerateOption.WithRepetition);
            var stringWords = variations.Select(variation => string.Join("", variation)).ToList();
            wordsDictionary.Add(i, stringWords);
        }

        return wordsDictionary;
    }
}