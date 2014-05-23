using System.Diagnostics;

namespace Swd.BackEnd
{
    public class Algorithm
    {
        //kolejność cech: prestige,job,fun,financies,easyness
        public string[] Preferences { get; set; }
        private const int PreferencesCount = 5;
        public Matrix PreferencesMatrix { get; set; }

        public Algorithm(string preferences)
        {
            ExtractPreferences(preferences);
            CalculatePreferencesMatrix();
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
            Preferences=preferences.Split(',');
        }
    }
}