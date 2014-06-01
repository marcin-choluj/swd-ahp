using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using Funq;
using MongoDB.Driver.Linq;
using Swd.Backend;
using Swd.BackEnd.Entities;
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

        public University ResultUniversity { get; set; }

        public Algorithm(string preferences)
        {
            ExtractPreferences(preferences);
            CalculatePreferencesMatrix();

            var query = from universities in new DbDriver().UniversitiesCollection.AsQueryable<University>()
                        select universities;

            var calculated = CalculateAverages(query.ToList().GroupBy(e => e.Name));

            calculated.Sort((u1, u2) => u1.Name.CompareTo(u2.Name));

            CreatePropertyMatrix(calculated, university => university.Easyness, matrix => EasynessMatrix = matrix);
            CreatePropertyMatrix(calculated, university => university.Job, matrix => JobMatrix = matrix);
            CreatePropertyMatrix(calculated, university => university.Financies, matrix => FinanciesMatrix = matrix);
            CreatePropertyMatrix(calculated, university => university.Prestige, matrix => PrestigeMatrix = matrix);
            CreatePropertyMatrix(calculated, university => university.Fun, matrix => FunMatrix = matrix);

            var preferencesVector = PreferencesMatrix.Normalize().CalculateSVector().CheckCohesion().SVector;
            var easynessVector = EasynessMatrix.Normalize().CalculateSVector().CheckCohesion().SVector;
            var jobVector = JobMatrix.Normalize().CalculateSVector().CheckCohesion().SVector;
            var financiesVector = FinanciesMatrix.Normalize().CalculateSVector().CheckCohesion().SVector;
            var prestigeVector = PrestigeMatrix.Normalize().CalculateSVector().CheckCohesion().SVector;
            var funVector = FunMatrix.Normalize().CalculateSVector().CheckCohesion().SVector;

            //kolejność cech: prestige,job,fun,financies,easyness
            var decisionVector = new double[easynessVector.Length];
            for (int i = 0; i < easynessVector.Length; i++)
            {
                decisionVector[i] += preferencesVector[0] * prestigeVector[i];
                decisionVector[i] += preferencesVector[1] * jobVector[i];
                decisionVector[i] += preferencesVector[2] * funVector[i];
                decisionVector[i] += preferencesVector[3] * financiesVector[i];
                decisionVector[i] += preferencesVector[4] * easynessVector[i];
            }
            var result = decisionVector.GetHighestIndex();

            ResultUniversity = calculated[result];
        }

        public static List<University> CalculateAverages(IEnumerable<IGrouping<string, University>> data)
        {
            var ret = new List<University>();
            foreach (var group in data)
            {
                var calculatedUniversity = new University();
                calculatedUniversity.Easyness = 0;
                calculatedUniversity.Job = 0;
                calculatedUniversity.Financies = 0;
                calculatedUniversity.Fun = 0;
                calculatedUniversity.Prestige = 0;
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
                calculatedUniversity.Name = group.Key;
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
            Preferences = preferences.TrimEnd(',').Split(',');
        }
    }
}