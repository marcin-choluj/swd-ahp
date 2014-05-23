using System;

namespace Swd.BackEnd
{
    public class Matrix
    {
        private readonly double[,] _data;
        private readonly int _length;
        private double[] _sVector;
        private bool _sVectorCalculated;

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
            for (int i = 0; i < _length; i++)
            {
                for (int j = 0; j < _length; j++)
                {
                    tmpVector[i] += _data[j, i];
                }
            }

            for (int i = 0; i < _length; i++)
            {
                for (int j = 0; j < _length; j++)
                {
                    _data[j, i] /= tmpVector[i];
                }
            }

            return this;
        }

        public Matrix CalculateSVector()
        {
            _sVector = new double[_length];
            for (int i = 0; i < _length; i++)
            {
                double sum = 0;
                for (int j = 0; j < _length; j++)
                {
                    sum += _data[i, j];
                }
                _sVector[i] = sum / _length;
            }

            _sVectorCalculated = true;
            return this;
        }
    }
}