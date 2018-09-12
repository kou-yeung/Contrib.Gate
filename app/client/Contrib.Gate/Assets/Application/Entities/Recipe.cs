using CsvHelper.Configuration;
using System;
using System.Collections.Generic;

namespace Entities
{
    public class Recipe
    {
        public Identify Identify;
        public Identify Result;

        public Identify Material1;
        public int Num1;
        public Identify Material2;
        public int Num2;
        public Identify Material3;
        public int Num3;
        public Identify Material4;
        public int Num4;

        List<Tuple<Identify,int>> materials;
        public List<Tuple<Identify, int>> Materials
        {
            get
            {
                if (materials == null)
                {
                    materials = new List<Tuple<Identify, int>>();
                    Add(Material1, Num1);
                    Add(Material2, Num2);
                    Add(Material3, Num3);
                    Add(Material4, Num4);
                }
                return materials;
            }
        }
        void Add(Identify identify, int num)
        {
            if (identify == Identify.Empty) return;
            if (num <= 0) return;
            materials.Add(Tuple.Create(identify, num));
        }
    }

    public sealed class RecipeMap : ClassMap<Recipe>
    {
        public RecipeMap()
        {
            Map(x => x.Identify).Index(0);
            Map(x => x.Result).Index(1);
            Map(x => x.Material1).Index(2).Default(Identify.Empty);
            Map(x => x.Num1).Index(3).Default(0);
            Map(x => x.Material2).Index(4).Default(Identify.Empty);
            Map(x => x.Num2).Index(5).Default(0);
            Map(x => x.Material3).Index(6).Default(Identify.Empty);
            Map(x => x.Num3).Index(7).Default(0);
            Map(x => x.Material4).Index(8).Default(Identify.Empty);
            Map(x => x.Num4).Index(9).Default(0);
        }
    }
}
