using CsvHelper.Configuration;
using System;
using System.Collections.Generic;

namespace Entities
{
    public class Recipe
    {
        public Identify Identify;
        public Identify Result;
        public List<Tuple<Identify, int>> Materials;
    }

    public sealed class RecipeMap : ClassMap<Recipe>
    {
        public RecipeMap()
        {
            Map(x => x.Identify).Index(0);
            Map(x => x.Result).Index(1);
            Map(x => x.Materials).ConvertUsing(row =>
            {
                var res = new List<Tuple<Identify, int>>();
                for (int i = 0; i < 4; i++)
                {
                    Identify identify;
                    int num;
                    row.TryGetField(2 + i * 2, out identify);
                    row.TryGetField(3 + i * 2, out num);
                    if (identify != Identify.Empty && num > 0)
                    {
                        res.Add(Tuple.Create(identify, num));
                    }
                }
                return res;
            });
        }
    }
}
