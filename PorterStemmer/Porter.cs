using System.Text.RegularExpressions;
// ReSharper disable InconsistentNaming

namespace PorterStemmer;

public static class Porter
{
    private static readonly Regex PERFECTIVEGROUND = new ("((ив|ивши|ившись|ыв|ывши|ывшись)|((<;=[ая])(в|вши|вшись)))$");
    private static readonly Regex REFLEXIVE = new ("(с[яь])$");
    private static readonly Regex ADJECTIVE = new("(ее|ие|ые|ое|ими|ыми|ей|ий|ый|ой|ем|им|ым|ом|его|ого|ему|ому|их|ых|ую|юю|ая|яя|ою|ею)$");
    private static readonly Regex PARTICIPLE = new("((ивш|ывш|ующ)|((?<=[ая])(ем|нн|вш|ющ|щ)))$");
    private static readonly Regex VERB = new("((ила|ыла|ена|ейте|уйте|ите|или|ыли|ей|уй|ил|ыл|им|ым|ен|ило|ыло|ено|ят|ует|уют|ит|ыт|ены|ить|ыть|ишь|ую|ю)|((?<=[ая])(ла|на|ете|йте|ли|й|л|ем|н|ло|но|ет|ют|ны|ть|ешь|нно)))$");
    private static readonly Regex NOUN = new("(а|ев|ов|ие|ье|е|иями|ями|ами|еи|ии|и|ией|ей|ой|ий|й|иям|ям|ием|ем|ам|ом|о|у|ах|иях|ях|ы|ь|ию|ью|ю|ия|ья|я)$");
    private static readonly Regex RVRE = new("^(.*?[аеиоуыэюя])(.*)$");
    private static readonly Regex DERIVATIONAL = new(".*[^аеиоуыэюя]+[аеиоуыэюя].*ость?$");
    private static readonly Regex DER = new("ость?$");
    private static readonly Regex SUPERLATIVE = new("(ейше|ейш)$");
    private static readonly Regex I = new("и$");
    private static readonly Regex P = new("ь$");
    private static readonly Regex NN = new("нн$");

    public static string TransformingWord(string word)
    {
        word = word.ToLower();
        word = word.Replace('ё', 'е');
        var m = RVRE.Matches(word);
        if (m.Count > 0)
        {
            var match = m[0]; // only one match in this case 
            var groupCollection = match.Groups;
            var pre = groupCollection[1].ToString();
            var rv = groupCollection[2].ToString();

            var temp = PERFECTIVEGROUND.Matches(rv);
            var StringTemp = ReplaceFirst(temp, rv);


            if (StringTemp.Equals(rv))
            {
                var tempRV = REFLEXIVE.Matches(rv);
                rv = ReplaceFirst(tempRV, rv);
                temp = ADJECTIVE.Matches(rv);
                StringTemp = ReplaceFirst(temp, rv);
                if (!StringTemp.Equals(rv))
                {
                    rv = StringTemp;
                    tempRV = PARTICIPLE.Matches(rv);
                    rv = ReplaceFirst(tempRV, rv);
                }
                else
                {
                    temp = VERB.Matches(rv);
                    StringTemp = ReplaceFirst(temp, rv);
                    if (StringTemp.Equals(rv))
                    {
                        tempRV = NOUN.Matches(rv);
                        rv = ReplaceFirst(tempRV, rv);
                    }
                    else
                    {
                        rv = StringTemp;
                    }
                }

            }
            else
            {
                rv = StringTemp;
            }

            var tempRv = I.Matches(rv);
            rv = ReplaceFirst(tempRv, rv);
            if (DERIVATIONAL.Matches(rv).Count > 0)
            {
                tempRv = DER.Matches(rv);
                rv = ReplaceFirst(tempRv, rv);
            }

            temp = P.Matches(rv);
            StringTemp = ReplaceFirst(temp, rv);
            if (StringTemp.Equals(rv))
            {
                tempRv = SUPERLATIVE.Matches(rv);
                rv = ReplaceFirst(tempRv, rv);
                tempRv = NN.Matches(rv);
                rv = ReplaceFirst(tempRv, rv);
            }
            else
            {
                rv = StringTemp;
            }
            word = pre + rv;

        }

        return word;
    }

    private static string ReplaceFirst(MatchCollection collection, string part)
    {
        if (collection.Count == 0)
        {
            return part;
        }

        var stringTemp = part;
        for (var i = 0; i < collection.Count; i++)
        {
            var GroupCollection = collection[i].Groups;
            if (stringTemp.Contains(GroupCollection[i].ToString()))
            {
                var deletePart = GroupCollection[i].ToString();
                stringTemp = stringTemp.Replace(deletePart, "");
            }

        }
        return stringTemp;
    }
}