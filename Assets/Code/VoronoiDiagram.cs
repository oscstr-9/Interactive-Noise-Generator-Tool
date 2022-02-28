using System.Collections.Generic;
using UnityEngine;

public class VoronoiDiagram
{
    Vector2Int texSize;
    public int regionAmount;

    public Color[] GetDiagram(int texSizeIn, int regionAmountIn, Vector3 colors)
    {
        texSize = new Vector2Int(texSizeIn, texSizeIn);
        regionAmount = regionAmountIn;
        Vector2Int[] centroids = new Vector2Int[regionAmount];
        Color[] regions = new Color[regionAmount];
        for (int i = 0; i < regionAmount; i++)
        {
            centroids[i] = new Vector2Int(Random.Range(0, texSize.x), Random.Range(0, texSize.y));
            regions[i] = new Color(
                colors.x < 0 ? Random.Range(0f, 1f) : colors.x,
                colors.y < 0 ? Random.Range(0f, 1f) : colors.y,
                colors.z < 0 ? Random.Range(0f, 1f) : colors.z);
        }
        Color[] pixelColors = new Color[texSize.x * texSize.y];
        for (int x = 0; x < texSize.x; x++)
        {
            for (int y = 0; y < texSize.y; y++)
            {
                int index = x * texSize.x + y;
                pixelColors[index] = regions[GetClosestCentroidIndex(new Vector2Int(x,y), centroids)];
            }
        }
        return pixelColors;
    }

    int GetClosestCentroidIndex(Vector2Int pixelPos, Vector2Int[] centroids)
    {
        float smallestDist = float.MaxValue;
        int index = 0;
        for (int i = 0; i < centroids.Length; i++)
        {
            if (Vector2.Distance(pixelPos, centroids[i]) < smallestDist)
            {
                smallestDist = Vector2.Distance(pixelPos, centroids[i]);
                index = i;
            }
        }
        return index;
    }

    Texture2D GetImageFromColorArray(Color[] pixelColors)
    {
        Texture2D tex = new Texture2D(texSize.x, texSize.y);
        tex.filterMode = FilterMode.Point;
        tex.SetPixels(pixelColors);
        tex.Apply();
        return tex;
    }
}





//// Original Author: Scrawk https://github.com/Scrawk/Procedural-Noise.git
//// Edited by: Oscar Östryd

//public enum VORONOI_DISTANCE { EUCLIDIAN, MANHATTAN, CHEBYSHEV }
//public enum VORONOI_COMBINATION { D0, D1_D0, D2_D0}

//public class WorleyNoise
//{

//    private static readonly float[] OFFSET_F = new float[] { -0.5f, 0.5f, 1.5f };

//    private const float K = 1.0f / 7.0f;

//    private const float Ko = 3.0f / 7.0f;

//    public float jitter { get; set; }
//    public float amplitude { get; set; }
//    public float frequency { get; set; }
//    public Vector3 offset { get; set; }

//    public VORONOI_DISTANCE Distance { get; set; }

//    public VORONOI_COMBINATION Combination { get; set; }

//    private PermutationTable Perm { get; set; }

//    public WorleyNoise(int seedIn, float frequencyIn, float jitterIn, float amplitudeIn = 1.0f)
//    {

//        frequency = frequencyIn;
//        amplitude = amplitudeIn;
//        offset = Vector3.zero;
//        jitter = jitterIn;
//        Distance = VORONOI_DISTANCE.EUCLIDIAN;
//        Combination = VORONOI_COMBINATION.D1_D0;

//        Perm = new PermutationTable(1024, 255, seedIn);
//    }

//    /// <summary>
//    /// Update the seed.
//    /// </summary>
//    public void UpdateSeed(int seed)
//    {
//        Perm.Build(seed);
//    }


//    /// </summary>
//    public float Sample2D(float x, float y)
//    {

//        x = (x + offset.x) * frequency;
//        y = (y + offset.y) * frequency;

//        int Pi0 = (int)Mathf.Floor(x);
//        int Pi1 = (int)Mathf.Floor(y);

//        float Pf0 = Frac(x);
//        float Pf1 = Frac(y);

//        Vector3 pX = new Vector3();
//        pX[0] = Perm[Pi0 - 1];
//        pX[1] = Perm[Pi0];
//        pX[2] = Perm[Pi0 + 1];

//        float d0, d1, d2;
//        float F0 = float.PositiveInfinity;
//        float F1 = float.PositiveInfinity;
//        float F2 = float.PositiveInfinity;

//        int px, py, pz;
//        float oxx, oxy, oxz;
//        float oyx, oyy, oyz;

//        for (int i = 0; i < 3; i++)
//        {
//            px = Perm[(int)pX[i], Pi1 - 1];
//            py = Perm[(int)pX[i], Pi1];
//            pz = Perm[(int)pX[i], Pi1 + 1];

//            oxx = Frac(px * K) - Ko;
//            oxy = Frac(py * K) - Ko;
//            oxz = Frac(pz * K) - Ko;

//            oyx = Mod(Mathf.Floor(px * K), 7.0f) * K - Ko;
//            oyy = Mod(Mathf.Floor(py * K), 7.0f) * K - Ko;
//            oyz = Mod(Mathf.Floor(pz * K), 7.0f) * K - Ko;

//            d0 = Distance2(Pf0, Pf1, OFFSET_F[i] + jitter * oxx, -0.5f + jitter * oyx);
//            d1 = Distance2(Pf0, Pf1, OFFSET_F[i] + jitter * oxy, 0.5f + jitter * oyy);
//            d2 = Distance2(Pf0, Pf1, OFFSET_F[i] + jitter * oxz, 1.5f + jitter * oyz);

//            if (d0 < F0) { F2 = F1; F1 = F0; F0 = d0; }
//            else if (d0 < F1) { F2 = F1; F1 = d0; }
//            else if (d0 < F2) { F2 = d0; }

//            if (d1 < F0) { F2 = F1; F1 = F0; F0 = d1; }
//            else if (d1 < F1) { F2 = F1; F1 = d1; }
//            else if (d1 < F2) { F2 = d1; }

//            if (d2 < F0) { F2 = F1; F1 = F0; F0 = d2; }
//            else if (d2 < F1) { F2 = F1; F1 = d2; }
//            else if (d2 < F2) { F2 = d2; }

//        }

//        return Combine(F0, F1, F2) * amplitude;
//    }



//    private float Mod(float x, float y)
//    {
//        return x - y * Mathf.Floor(x / y);
//    }

//    private float Frac(float v)
//    {
//        return v - Mathf.Floor(v);
//    }

//    private float Distance2(float p1x, float p1y, float p2x, float p2y)
//    {
//        switch (Distance)
//        {
//            case VORONOI_DISTANCE.EUCLIDIAN:
//                return (p1x - p2x) * (p1x - p2x) + (p1y - p2y) * (p1y - p2y);

//            case VORONOI_DISTANCE.MANHATTAN:
//                return Math.Abs(p1x - p2x) + Math.Abs(p1y - p2y);

//            case VORONOI_DISTANCE.CHEBYSHEV:
//                return Math.Max(Math.Abs(p1x - p2x), Math.Abs(p1y - p2y));
//        }

//        return 0;
//    }



//    private float Combine(float f0, float f1, float f2)
//    {
//        switch (Combination)
//        {
//            case VORONOI_COMBINATION.D0:
//                return f0;

//            case VORONOI_COMBINATION.D1_D0:
//                return f1 - f0;

//            case VORONOI_COMBINATION.D2_D0:
//                return f2 - f0;
//        }

//        return 0;
//    }
//}


//internal class PermutationTable
//{

//    public int Size { get; private set; }

//    public int Seed { get; private set; }

//    public int Max { get; private set; }

//    public float Inverse { get; private set; }

//    private int Wrap;

//    private int[] Table;

//    internal PermutationTable(int size, int max, int seed)
//    {
//        Size = size;
//        Wrap = Size - 1;
//        Max = Math.Max(1, max);
//        Inverse = 1.0f / Max;
//        Build(seed);
//    }

//    internal void Build(int seed)
//    {
//        if (Seed == seed && Table != null) return;

//        Seed = seed;
//        Table = new int[Size];

//        System.Random rnd = new System.Random(Seed);

//        for (int i = 0; i < Size; i++)
//        {
//            Table[i] = rnd.Next();
//        }
//    }

//    internal int this[int i]
//    {
//        get
//        {
//            return Table[i & Wrap] & Max;
//        }
//    }

//    internal int this[int i, int j]
//    {
//        get
//        {
//            return Table[(j + Table[i & Wrap]) & Wrap] & Max;
//        }
//    }

//    internal int this[int i, int j, int k]
//    {
//        get
//        {
//            return Table[(k + Table[(j + Table[i & Wrap]) & Wrap]) & Wrap] & Max;
//        }
//    }

//}