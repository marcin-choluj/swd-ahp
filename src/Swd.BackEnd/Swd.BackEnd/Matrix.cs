using System;
using System.Linq;

namespace Swd.BackEnd
{
    public class Matrix
    {
        private readonly double[,] _data;
        private readonly int _length;
        private double[] _sVector;
        private bool _sVectorCalculated;
        private static readonly double[] CohesionValues = { 0, 0, 0.52, 0.89, 1.11, 1.25, 1.35, 1.40 };

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
                    tmpVector[i] += _data[j, i];
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
                    _data[i, j] = 1/_data[j, i];
                }
            }
        }

        private void RepairCohesion()
        {
            //throw new NotImplementedException();
        }
    }
}