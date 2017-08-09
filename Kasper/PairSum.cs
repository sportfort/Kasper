using System.Collections.Generic;

namespace Kasper
{
    public static class PairSum
    {
        public static Dictionary<int, int> GetAllPairsWithSum(int[] input, int x)
        {
            var result = new Dictionary<int, int>();
            if (input == null || input.Length < 2)
                return result;

            //каждое число будем сравнивать с уже просмотренными
            //для этого используем хэшсет, куда будем записывать просмотренные числа
            //поиск по хэшсету имеет сложность О(1), поэтому поиск будет быстрым
            HashSet<int> inspected = new HashSet<int> {input[0]};

            for (int i = 1; i < input.Length; i++)
            {
                var current = input[i];
                var compliment = x - input[i];
                if (inspected.Contains(compliment))
                {
                    //учитываем только уникальные пары, если мы нашли пару "2 и 5", то "5 и 2" уже можно отбросить
                    if (!result.ContainsKey(current) && !result.ContainsKey(compliment))
                        result.Add(current, compliment);
                }
                else
                {
                    //в просмотренные числа имеет смысл добавлять только числа без пар
                    inspected.Add(current);
                }
            }
            return result;
        }
    }
}