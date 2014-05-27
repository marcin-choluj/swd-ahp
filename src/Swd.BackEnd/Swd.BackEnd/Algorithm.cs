using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Funq;
using Swd.BackEnd.Services;

namespace Swd.BackEnd
{
    public class Algorithm
    {
        //kolejność cech: prestige,job,fun,financies,easyness
        public string[] Preferences { get; set; }
        private const int PreferencesCount = 5;
        public Matrix PreferencesMatrix { get; set; }


        public Matrix PrestigeMatrix { get; set; }
        public Matrix JobMatrix { get; set; }
        public Matrix FunMatrix { get; set; }
        public Matrix FinanciesMatrix { get; set; }
        public Matrix EasynessMatrix { get; set; }

        public Algorithm(string preferences)
        {
            ExtractPreferences(preferences);
            CalculatePreferencesMatrix();

            List<University> calculated;
            //using (var ctx = new DatabaseEntities())
            //{
            //    calculated = CalculateAverages(ctx.Universities.GroupBy(e => e.Name));
            //}
            calculated=new List<University>();
            calculated.Add(new University { Name = "PWR", Easyness = 20, Financies = 50, Fun = 90, Job = 70, Prestige = 59 });
            calculated.Add(new University { Name = "UWR", Easyness = 60, Financies = 15, Fun = 45, Job = 65, Prestige = 45 });
            calculated.Add(new University { Name = "DSW", Easyness = 80, Financies = 65, Fun = 86, Job = 12, Prestige = 65 });

            calculated.Sort((u1, u2) => u1.Name.CompareTo(u2.Name));

            CreatePropertyMatrix(calculated, university => university.Easyness, matrix => EasynessMatrix = matrix);
            CreatePropertyMatrix(calculated, university => university.Job, matrix => JobMatrix = matrix);
            CreatePropertyMatrix(calculated, university => university.Financies, matrix => FinanciesMatrix = matrix);
            CreatePropertyMatrix(calculated, university => university.Prestige, matrix => PrestigeMatrix = matrix);
            CreatePropertyMatrix(calculated, university => university.Financies, matrix => FunMatrix = matrix);


        }

        public static List<University> CalculateAverages(IEnumerable<IGrouping<string, University>> data)
        {
            var ret = new List<University>();
            foreach (var group in data)
            {
                var calculatedUniversity = new University();
                foreach (var university in @group)
                {
                    calculatedUniversity.Easyness += university.Easyness;
                    calculatedUniversity.Job += university.Job;
                    calculatedUniversity.Financies += university.Financies;
                    calculatedUniversity.Fun += university.Fun;
                    calculatedUniversity.Prestige += university.Prestige;
                }
                calculatedUniversity.Easyness /= @group.Count();
                calculatedUniversity.Job /= @group.Count();
                calculatedUniversity.Financies /= @group.Count();
                calculatedUniversity.Fun /= @group.Count();
                calculatedUniversity.Prestige /= @group.Count();
                ret.Add(calculatedUniversity);
            }
            return ret;
        }

        private void CreatePropertyMatrix(IEnumerable<University> universities, Func<University, double?> property, Action<Matrix> matrix)
        {
            var enumerable = universities as University[] ?? universities.ToArray();
            var mat = new Matrix(enumerable.Count());

            for (int i = 0; i < enumerable.Count(); i++)
            {
                for (int j = 0; j < enumerable.Count(); j++)
                {
                    if (!(property(enumerable[j]).HasValue && property(enumerable[i]).HasValue))
                        throw new Exception("Brak wartości cech.");
                    mat[i, j] = property(enumerable[j]).Value / property(enumerable[i]).Value;
                }
            }

            matrix(mat);
        }

        private void CalculatePreferencesMatrix()
        {
            PreferencesMatrix = new Matrix(5);
            PreferencesMatrix[0, 0] = 1;
            PreferencesMatrix[1, 1] = 1;
            PreferencesMatrix[2, 2] = 1;
            PreferencesMatrix[3, 3] = 1;
            PreferencesMatrix[4, 4] = 1;

            PreferencesMatrix[0, 1] = Preferences.GetIndex("job") / Preferences.GetIndex("prestige");
            PreferencesMatrix[0, 2] = Preferences.GetIndex("fun") / Preferences.GetIndex("prestige");
            PreferencesMatrix[0, 3] = Preferences.GetIndex("financies") / Preferences.GetIndex("prestige");
            PreferencesMatrix[0, 4] = Preferences.GetIndex("easyness") / Preferences.GetIndex("prestige");
            PreferencesMatrix[1, 2] = Preferences.GetIndex("fun") / Preferences.GetIndex("job");
            PreferencesMatrix[1, 3] = Preferences.GetIndex("financies") / Preferences.GetIndex("job");
            PreferencesMatrix[1, 4] = Preferences.GetIndex("easyness") / Preferences.GetIndex("job");
            PreferencesMatrix[2, 3] = Preferences.GetIndex("financies") / Preferences.GetIndex("fun");
            PreferencesMatrix[2, 4] = Preferences.GetIndex("easyness") / Preferences.GetIndex("fun");
            PreferencesMatrix[3, 4] = Preferences.GetIndex("easyness") / Preferences.GetIndex("financies");

            PreferencesMatrix.CalculatePreferences();
        }

        private void ExtractPreferences(string preferences)
        {
            Preferences = preferences.Split(',');
        }
    }
}