﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Swd.BackEnd.Tests
{
    [TestClass]
    public class MatrixTests
    {
        [TestMethod]
        public void NormalizeTest()
        {
            var sampleData = new double[,]
            {
                {5, 10, 5},
                {2, 5, 2},
                {3, 1, 3}
            };
            var mat = new Matrix(sampleData);
            var sampleOutput = new[,]
            {
                {0.5, 0.625, 0.5},
                {0.2, 0.3125, 0.2},
                {0.3, 0.0625, 0.3}
            };

            mat.Normalize();
            Assert.IsTrue(ArraysEqual(mat.Data, sampleOutput));
        }

        [TestMethod]
        public void CalculateSVectorTest()
        {
            var sampleData = new double[,]
            {
                {15, 15, 15},
                {20, 20, 20},
                {15, 15, 15}
            };
            var mat = new Matrix(sampleData);
            var sampleOutput = new[] { 0.3, 0.40000000000000008, 0.3 };

            mat.Normalize().CalculateSVector();

            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(mat.SVector, sampleOutput));
        }

        static bool ArraysEqual<T>(T[,] a1, T[,] a2)
        {
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a1.GetLength(0); i++)
            {
                for (int j = 0; j < a1.GetLength(1); j++)
                {
                    if (!comparer.Equals(a1[i, j], a2[i, j])) return false;
                }
            }
            return true;
        }
    }
}