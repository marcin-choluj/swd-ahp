using System;
using System.Linq;

namespace Swd.BackEnd
{
    public class Matrix
    {
        private double[,] _data;
        private double[,] _unnormalizedData;
        private readonly int _length;
        private double[] _sVector;
        private bool _sVectorCalculated;
        private static readonly double[] CohesionValues = { 0, 0, 0.58, 0.89, 1.11, 1.25, 1.35, 1.40 };

        private double Cohesion
        {
            get { return CohesionValues[_length - 1]; }
        }

        public double[] SVector
        {
            get
            {
                if (_sVectorCalculated) return _sVector;
                throw new InvalidOperationException("SVector not calculated");
            }
        }

        public double[,] Data
        {
            get { return _data; }
        }

        public Matrix(int dimension)
        {
            _length = dimension;
            _data = new double[dimension, dimension];
        }

        public Matrix(double[,] data)
        {
            _length = data.GetLength(0);
            _data = data;
        }

        public double this[int index1, int index2]
        {
            get { return _data[index1, index2]; }
            set { _data[index1, index2] = value; }
        }

        public Matrix Normalize()
        {
            _unnormalizedData = _data.Copy();
            var tmpVector = new double[_length];
            for (var i = 0; i < _length; i++)
            {
                for (var j = 0; j < _length; j++)
                {
                    tmpVector[i] += _data[j, i];
                }
            }

            for (var i = 0; i < _length; i++)
            {
                for (var j = 0; j < _length; j++)
                {
                    _data[j, i] /= tmpVector[i];
                }
            }

            return this;
        }

        public Matrix CalculateSVector()
        {
            _sVector = new double[_length];
            for (var i = 0; i < _length; i++)
            {
                double sum = 0;
                for (var j = 0; j < _length; j++)
                {
                    sum += _data[i, j];
                }
                _sVector[i] = sum / _length;
            }

            _sVectorCalculated = true;
            return this;
        }

        public Matrix CheckCohesion()
        {
            var tmpVector = new double[_length];
            for (var i = 0; i < _length; i++)
            {
                for (var j = 0; j < _length; j++)
                {
                    tmpVector[i] += _unnormalizedData[j, i];
                }
            }

            var alfa = _sVector.Select((t, i) => t * tmpVector[i]).Sum();
            alfa -= _length;
            alfa /= (_length - 1);
            alfa /= Cohesion;
            if (alfa > 0.1)
                RepairCohesion();
            return this;
        }

        public void CalculatePreferences()
        {
            for (int i = 1; i < _length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    _data[i, j] = 1 / _data[j, i];
                }
            }
        }

        private void RepairCohesion()
        {
            var dokladnosc = 0.001; // w prezentacji e - do którego miejsca po przecinku kolejne wektory priorytetow sa takie same

            var staryWektorPriorytetow = new double[_length];
            var wektorPriorytetow = new double[_length];

            var czySzukac = true;

            while (czySzukac)
            {
                SquareMatrix();
                double sumaWektora = 0;
                for (int k = 0; k < _length; k++)
                {
                    for (int l = 0; l < _length; l++)
                    {
                        wektorPriorytetow[k] += Data[k, l];
                        sumaWektora += Data[k, l];
                    }
                }
                for (int k = 0; k < _length; k++)
                {
                    wektorPriorytetow[k] = wektorPriorytetow[k] / sumaWektora;
                }

                var czySpelnia = true;

                for (int k = 0; k < _length; k++)
                {
                    if (Math.Abs(wektorPriorytetow[k] - staryWektorPriorytetow[k]) > dokladnosc)
                        czySpelnia = false;
                }

                if (czySpelnia)
                {
                    czySzukac = false;
                }

                staryWektorPriorytetow = wektorPriorytetow.Copy();
            }

            _sVector = wektorPriorytetow;
        }

        public void SquareMatrix()
        {
            var newMatrix = new double[_length, _length];
            for (int i = 0; i < _length; i++)
            {
                for (int j = 0; j < _length; j++)
                {
                    for (int k = 0; k < _length; k++)
                    {
                        newMatrix[i, j] += Data[i, k] * Data[k, j];
                    }
                }
            }

            _data = newMatrix;
        }
    }
}