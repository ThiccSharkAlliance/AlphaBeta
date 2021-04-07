/**
 * K.jpg's OpenSimplex 2, smooth variant ("SuperSimplex")
 *
 * - 2D is standard simplex, modified to support larger kernels.
 *   Implemented using a lookup table.
 * - 3D is "Re-oriented 8-point BCC noise" which constructs a
 *   congruent BCC lattice in a much different way than usual.
 * - 4D uses a naïve pregenerated lookup table, and averages out
 *   to the expected performance.
 *
 * Multiple versions of each function are provided. See the
 * documentation above each, for more info.
 */

using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;

namespace SimplexNoise
{
    public class OpenSimplex2SJobs
    {
        private const int PSIZE = 2048;
        private const int PMASK = 2047;

        private NativeArray<short> perm;
        private NativeArray<double2> permGrad2;

        public OpenSimplex2SJobs(long seed)
        {
            perm = new NativeArray<short>(PSIZE, Allocator.TempJob);
            permGrad2 = new NativeArray<double2>(PSIZE, Allocator.TempJob);
            NativeArray<short> source = new NativeArray<short>(PSIZE, Allocator.Temp);
            for (short i = 0; i < PSIZE; i++)
                source[i] = i;
            for (int i = PSIZE - 1; i >= 0; i--)
            {
                seed = seed * 6364136223846793005L + 1442695040888963407L;
                int r = (int)((seed + 31) % (i + 1));
                if (r < 0)
                    r += (i + 1);
                perm[i] = source[r];
                permGrad2[i] = GRADIENTS_2D[perm[i]];
                source[r] = source[i];
            }

            source.Dispose();
        }

        /*
         * Noise Evaluators
         */

        /**
         * 2D SuperSimplex noise, with Y pointing down the main diagonal.
         * Might be better for a 2D sandbox style game, where Y is vertical.
         * Probably slightly less optimal for heightmaps or continent maps.
         */
        public double Noise2_XBeforeY(double x, double y)
        {

            // Skew transform and rotation baked into one.
            double xx = x * 0.7071067811865476;
            double yy = y * 1.224744871380249;

            return noise2_Base(yy + xx, yy - xx);
        }

        /**
         * 2D SuperSimplex noise base.
         * Lookup table implementation inspired by DigitalShadow.
         */
        private double noise2_Base(double xs, double ys)
        {
            double value = 0;

            // Get base points and offsets
            int xsb = fastFloor(xs), ysb = fastFloor(ys);
            double xsi = xs - xsb, ysi = ys - ysb;

            // Index to point list
            int a = (int)(xsi + ysi);
            int index =
                (a << 2) |
                (int)(xsi - ysi / 2 + 1 - a / 2.0) << 3 |
                (int)(ysi - xsi / 2 + 1 - a / 2.0) << 4;

            double ssi = (xsi + ysi) * -0.211324865405187;
            double xi = xsi + ssi, yi = ysi + ssi;

            // Point contributions
            for (int i = 0; i < 4; i++)
            {
                double2x2 c = LOOKUP_2D[index + i];

                double dx = xi + c.c1.x, dy = yi + c.c1.y;
                double attn = 2.0 / 3.0 - dx * dx - dy * dy;
                if (attn <= 0) continue;

                int pxm = (xsb + (int)c.c0.x) & PMASK, pym = (ysb + (int)c.c0.y) & PMASK;
                double2 grad = permGrad2[perm[pxm] ^ pym];
                double extrapolation = grad.x * dx + grad.y * dy;

                attn *= attn;
                value += attn * attn * extrapolation;
            }

            return value;
        }

        /*
         * Utility
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int fastFloor(double x)
        {
            int xi = (int)x;
            return x < xi ? xi - 1 : xi;
        }

        /*
         * Lookup Tables & Gradients
         */

        private static NativeArray<double2x2> LOOKUP_2D;

        private const double N2 = 0.05481866495625118;
        private static NativeArray<double2> GRADIENTS_2D;

        static OpenSimplex2SJobs()
        {
            LOOKUP_2D = new NativeArray<double2x2>(8 * 4, Allocator.TempJob);

            for (int i = 0; i < 8; i++)
            {
                int i1, j1, i2, j2;
                if ((i & 1) == 0)
                {
                    if ((i & 2) == 0) { i1 = -1; j1 = 0; } else { i1 = 1; j1 = 0; }
                    if ((i & 4) == 0) { i2 = 0; j2 = -1; } else { i2 = 0; j2 = 1; }
                }
                else
                {
                    if ((i & 2) != 0) { i1 = 2; j1 = 1; } else { i1 = 0; j1 = 1; }
                    if ((i & 4) != 0) { i2 = 1; j2 = 2; } else { i2 = 1; j2 = 0; }
                }
                LOOKUP_2D[i * 4 + 0] = LatticePoint2DConvert(0, 0);
                LOOKUP_2D[i * 4 + 1] = LatticePoint2DConvert(1, 1);
                LOOKUP_2D[i * 4 + 2] = LatticePoint2DConvert(i1, j1);
                LOOKUP_2D[i * 4 + 3] = LatticePoint2DConvert(i2, j2);
            }

            GRADIENTS_2D = new NativeArray<double2>(PSIZE, Allocator.TempJob);
            double2[] grad2 = {
                new double2( 0.130526192220052,  0.99144486137381),
                new double2( 0.38268343236509,   0.923879532511287),
                new double2( 0.608761429008721,  0.793353340291235),
                new double2( 0.793353340291235,  0.608761429008721),
                new double2( 0.923879532511287,  0.38268343236509),
                new double2( 0.99144486137381,   0.130526192220051),
                new double2( 0.99144486137381,  -0.130526192220051),
                new double2( 0.923879532511287, -0.38268343236509),
                new double2( 0.793353340291235, -0.60876142900872),
                new double2( 0.608761429008721, -0.793353340291235),
                new double2( 0.38268343236509,  -0.923879532511287),
                new double2( 0.130526192220052, -0.99144486137381),
                new double2(-0.130526192220052, -0.99144486137381),
                new double2(-0.38268343236509,  -0.923879532511287),
                new double2(-0.608761429008721, -0.793353340291235),
                new double2(-0.793353340291235, -0.608761429008721),
                new double2(-0.923879532511287, -0.38268343236509),
                new double2(-0.99144486137381,  -0.130526192220052),
                new double2(-0.99144486137381,   0.130526192220051),
                new double2(-0.923879532511287,  0.38268343236509),
                new double2(-0.793353340291235,  0.608761429008721),
                new double2(-0.608761429008721,  0.793353340291235),
                new double2(-0.38268343236509,   0.923879532511287),
                new double2(-0.130526192220052,  0.99144486137381)
            };
            for (int i = 0; i < grad2.Length; i++)
            {
                grad2[i].x /= N2; grad2[i].y /= N2;
            }
            for (int i = 0; i < PSIZE; i++)
            {
                GRADIENTS_2D[i] = grad2[i % grad2.Length];
            }
        }

        private static double2x2 LatticePoint2DConvert(int xsv, int ysv)
        {
            double ssv = (xsv + ysv) * -0.211324865405187;
            double2x2 returnDouble = new double2x2 {c0 = {x = xsv, y = ysv}, c1 = {x = -xsv - ssv, y = -ysv - ssv}};
            return returnDouble;
        }

        private struct LatticePoint2D
        {
            public int xsv, ysv;
            public double dx, dy;
            public LatticePoint2D(int xsv, int ysv)
            {
                this.xsv = xsv; this.ysv = ysv;
                double ssv = (xsv + ysv) * -0.211324865405187;
                this.dx = -xsv - ssv;
                this.dy = -ysv - ssv;
            }
        }

        private struct Grad2
        {
            public double dx, dy;
            public Grad2(double dx, double dy)
            {
                this.dx = dx; this.dy = dy;
            }
        }

        public void Dispose()
        {
            perm.Dispose();
            permGrad2.Dispose();
            LOOKUP_2D.Dispose();
            GRADIENTS_2D.Dispose();
        }
    }
}
