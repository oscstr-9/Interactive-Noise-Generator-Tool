using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class NoiseToImage : MonoBehaviour
{
    // Noise parameters //
    public int width = 128;
    public int height = 128;
    public Color color = Color.black;
    // Perlin noise
    public float scalingBias = 1;
    public int octaves = 1;
    // Blue noise
    public float bNoiseMinDist = 5; // min dist 1.5 for good visuals



    // Start is called before the first frame update
    void Start()
    {
        UploadPNG(1);
    }

    public void UploadPNG(int noiseNum)
    {
        // Create a texture the size of the screen, RGB24 format

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        GetComponent<Renderer>().material.mainTexture = tex;

        switch (noiseNum){
        case 1:
        // White Noise
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    color = ((Random.Range(1, 3)) <= 1 ? Color.black : Color.white);
                    tex.SetPixel(x, y, color);
                }
            }
            break;
        case 2:
        // Blue Noise
            PoissonDiscSampler bNoise = new PoissonDiscSampler(height,width,bNoiseMinDist);
            foreach (Vector2 sample in bNoise.Samples())
            {
                tex.SetPixel((int)sample.x, (int)sample.y, color);
            }
            break;
        case 3:
        // Perlin Noise
            Perlin pNoise = gameObject.GetComponent<Perlin>();
            pNoise.PerlinNoise(width, height, scalingBias, octaves);
            List<float> perlinNoise = pNoise.GetfPerlinNoise2D();
            Color perlinColor = Color.white;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    perlinColor = new Color((float)perlinNoise[y * width + x], (float)perlinNoise[y * width + x], (float)perlinNoise[y * width + x], 1.0f);
                    tex.SetPixel(x, y, perlinColor);
                }
            }
            break;
        case 4:
        // Worley Noise
            Color worleyColor = Color.red;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tex.SetPixel(x, y, worleyColor);
                }
            }
            break;
        default:
            break;
        }

        tex.Apply();

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        //Object.Destroy(tex);

        // For testing purposes, also write to a file in the project folder
        File.WriteAllBytes(Application.dataPath + "/../Assets/TestPNGs/SavedScreen.png", bytes);
    }
}
