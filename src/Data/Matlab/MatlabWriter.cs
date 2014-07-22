﻿// <copyright file="MatlabWriter.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
// http://mathnetnumerics.codeplex.com
//
// Copyright (c) 2009-2014 Math.NET
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace MathNet.Numerics.Data.Matlab
{
    /// <summary>
    /// Writes matrices to a MATLAB Level-5 Mat file.
    /// </summary>
    public static class MatlabWriter
    {
        public static void Store(Stream stream, IEnumerable<MatlabMatrix> matrices)
        {
            Formatter.FormatFile(stream, matrices);
        }

        public static void Store(string filePath, IEnumerable<MatlabMatrix> matrices)
        {
            using (var stream = File.OpenWrite(filePath))
            {
                Store(stream, matrices);
            }
        }

        /// <typeparam name="TDataType">The data type of the Matrix. It can be either: double, float, Complex, or Complex32.</typeparam>
        public static MatlabMatrix Pack<TDataType>(Matrix<TDataType> matrix, string matrixName)
            where TDataType : struct, IEquatable<TDataType>, IFormattable
        {
            return Formatter.FormatMatrix(matrix, matrixName);
        }

        /// <typeparam name="TDataType">The data type of the Matrix. It can be either: double, float, Complex, or Complex32.</typeparam>
        public static void Write<TDataType>(Stream stream, Matrix<TDataType> matrix, string matrixName)
            where TDataType : struct, IEquatable<TDataType>, IFormattable
        {
            Store(stream, new[] { Pack(matrix, matrixName) });
        }

        /// <typeparam name="TDataType">The data type of the Matrix. It can be either: double, float, Complex, or Complex32.</typeparam>
        public static void Write<TDataType>(string filePath, Matrix<TDataType> matrix, string matrixName)
            where TDataType : struct, IEquatable<TDataType>, IFormattable
        {
            Store(filePath, new[] { Pack(matrix, matrixName) });
        }

        /// <typeparam name="TDataType">The data type of the Matrix. It can be either: double, float, Complex, or Complex32.</typeparam>
        public static void Write<TDataType>(Stream stream, IList<Matrix<TDataType>> matrices, IList<string> names)
            where TDataType : struct, IEquatable<TDataType>, IFormattable
        {
            if (matrices.Count != names.Count)
            {
                throw new ArgumentException("Each matrix must have a name. Number of matrices must equal to the number of names.");
            }

            Store(stream, matrices.Zip(names, Pack));
        }

        /// <typeparam name="TDataType">The data type of the Matrix. It can be either: double, float, Complex, or Complex32.</typeparam>
        public static void Write<TDataType>(string filePath, IList<Matrix<TDataType>> matrices, IList<string> names)
            where TDataType : struct, IEquatable<TDataType>, IFormattable
        {
            if (matrices.Count != names.Count)
            {
                throw new ArgumentException("Each matrix must have a name. Number of matrices must equal to the number of names.");
            }

            Store(filePath, matrices.Zip(names, Pack));
        }

        /// <typeparam name="TDataType">The data type of the Matrix. It can be either: double, float, Complex, or Complex32.</typeparam>
        public static void Write<TDataType>(Stream stream, IEnumerable<KeyValuePair<string, Matrix<TDataType>>> matrices)
            where TDataType : struct, IEquatable<TDataType>, IFormattable
        {
            Store(stream, matrices.Select(kv => Pack(kv.Value, kv.Key)));
        }

        /// <typeparam name="TDataType">The data type of the Matrix. It can be either: double, float, Complex, or Complex32.</typeparam>
        public static void Write<TDataType>(string filePath, IEnumerable<KeyValuePair<string, Matrix<TDataType>>> matrices)
            where TDataType : struct, IEquatable<TDataType>, IFormattable
        {
            Store(filePath, matrices.Select(kv => Pack(kv.Value, kv.Key)));
        }
    }
}
