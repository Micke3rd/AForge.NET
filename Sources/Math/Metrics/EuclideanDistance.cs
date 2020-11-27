﻿// AForge Math Library
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright © AForge.NET, 2007-2010
// contacts@aforgenet.com
//

namespace AForge.Math.Metrics
{
    using System;

    /// <summary>
    /// Euclidean distance metric.
    /// </summary>
    /// 
    /// <remarks><para>This class represents the 
    /// <a href="http://en.wikipedia.org/wiki/Euclidean_distance">Euclidean distance metric.</a></para>
    /// 
    /// <para>Sample usage:</para>
    /// <code>
    /// // instantiate new distance class
    /// EuclideanDistance dist = new EuclideanDistance( );
    /// // create two vectors for inputs
    /// double[] p = new double[] { 2.5, 3.5, 3.0, 3.5, 2.5, 3.0 };
    /// double[] q = new double[] { 3.0, 3.5, 1.5, 5.0, 3.5, 3.0 };
    /// // get distance between the two vectors
    /// double distance = dist.GetDistance( p, q );
    /// </code>
    /// </remarks>
    /// 
    public sealed class EuclideanDistance : IDistance
    {
        /// <summary>
        /// Returns distance between two N-dimensional double vectors.
        /// </summary>
        /// 
        /// <param name="p">1st point vector.</param>
        /// <param name="q">2nd point vector.</param>
        /// 
        /// <returns>Returns Euclidean distance between two supplied vectors.</returns>
        /// 
        /// <exception cref="ArgumentException">Thrown if the two vectors are of different dimensions (if specified
        /// array have different length).</exception>
        /// 
        public double GetDistance( double[] p, double[] q )
        {
            double distance = 0;
            double diff = 0;

            if ( p.Length != q.Length )
                throw new ArgumentException( "Input vectors must be of the same dimension." );

            for ( int x = 0, len = p.Length; x < len; x++ )
            {
                diff = p[x] - q[x];
                distance += diff * diff;
            }

            return System.Math.Sqrt( distance );
        }
    }
}
