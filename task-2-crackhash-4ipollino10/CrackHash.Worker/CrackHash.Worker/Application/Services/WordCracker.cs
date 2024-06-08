using System.Security.Cryptography;
using System.Text;
using CrackHash.Worker.Application.Exceptions;
using CrackHash.Common.MessagingContract;

namespace CrackHash.Worker.Application.Services;

public static class WordCracker
{
    public static List<string>? CrackWord(WorkerTaskRequestMessage message)
    {
        var hashBytes = new byte[message.Hash.Length / 2];
        for (var i = 0; i < message.Hash.Length; i += 2)
        {
            try
            {
                hashBytes[i / 2] = Convert.ToByte(message.Hash.Substring(i, 2), 16);
            }
            catch (Exception)
            {
                throw new IncorrectHashException("Некорректный хэш");
            }
        }

        var wordsDictionary = BruteDictionary.GetWordsDictionary(message.MaxLength);

        var result = new List<string>();
        
        using (var md5 = MD5.Create())
        {
            foreach (var wordsSet in wordsDictionary.Values)
            {
                var chunkSize = (int)Math.Ceiling(wordsSet.Count / 10.0);

                var chunk = wordsSet.Skip(chunkSize * message.Skip).Take(chunkSize).ToList();

                foreach (var word in chunk)
                {
                    var bruteHash = md5.ComputeHash(Encoding.UTF8.GetBytes(word));
                    
                    if (bruteHash.SequenceEqual(hashBytes))
                    {
                        result.Add(word);
                    }
                }
            }
        }

        return result.Count == 0 ? null : result;
    }
}