using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perlin : MonoBehaviour
{
    public List<float> fPerlinNoise2D = new List<float>();
    public List<float> fNoiseSeed2D = new List<float>();
    public int outputWidth, outputHeight = 256;
    float scalingBias = 1;
    int octaves = 1;

    public void PerlinNoise(int outputWidthIn, int outputHeightIn, float scalingBiasIn, int octavesIn, bool keepSeed)
    {
        outputWidth = outputWidthIn;
        outputHeight = outputHeightIn;
        scalingBias = scalingBiasIn;
        octaves = octavesIn;
        if (!keepSeed || fNoiseSeed2D.Count == 0)
        {
            NewSeed2D();
        }
        PerlinNoise2D(octaves, scalingBias);
    }

    public void NewSeed2D()
    {
        ResetArrays();
        for (int i = 0; i < outputWidth * outputHeight; i++)
        {
            fNoiseSeed2D.Add((float)Random.value);
            fPerlinNoise2D.Add((float)0);
        }
    }

    public void PerlinNoise2D(int nOctaves, float fScalingBias)
    {
        for (int x = 0; x < outputWidth; x++)
        {
            for (int y = 0; y < outputHeight; y++)
            {
                double fNoise = 0.0f;
                double fScale = 1.0f;
                double fScaleSum = 0.0f;

                for (int j = 0; j < nOctaves; j++)
                {
                    // Make sample points that can later be linearly interpolated between
                    int nPitch = outputWidth >> j;
                    int xSample1 = (x / nPitch) * nPitch;
                    int ySample1 = (y / nPitch) * nPitch;
                    // % makes the samples able to wrap around in array
                    int xSample2 = (xSample1 + nPitch) % outputWidth;
                    int ySample2 = (ySample1 + nPitch) % outputWidth;

                    float xfBlend = (float)(x - xSample1) / (float)nPitch;
                    float yfBlend = (float)(y - ySample1) / (float)nPitch;

                    // Linear interpolation formula
                    float fSampleT = (1.0f - xfBlend) * fNoiseSeed2D[ySample1 * outputWidth + xSample1] + xfBlend * fNoiseSeed2D[ySample1 * outputWidth + xSample2];
                    float fSampleB = (1.0f - xfBlend) * fNoiseSeed2D[ySample2 * outputWidth + xSample1] + xfBlend * fNoiseSeed2D[ySample2 * outputWidth + xSample2];

                    fScaleSum += fScale;
                    fNoise += (yfBlend * (fSampleB - fSampleT) + fSampleT) * fScale;
                    fScale = fScale / fScalingBias;
                }
                fPerlinNoise2D[y * outputWidth + x] = (float)(fNoise / fScaleSum);
            }
        }
    }

    public void ResetArrays()
    {
        fNoiseSeed2D.Clear();
        fPerlinNoise2D.Clear();
    }

    public List<float> GetfPerlinNoise2D()
    {
        return fPerlinNoise2D;
    }
}