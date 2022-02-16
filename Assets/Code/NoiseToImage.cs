using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class NoiseToImage : MonoBehaviour
{
    public int i = 1;
    // Start is called before the first frame update
    void Start()
    {
        UploadPNG(1);
    }

    public void UploadPNG(int noiseNum)
    {
        // Create a texture the size of the screen, RGB24 format
        int width = 128;
        int height = 128;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        GetComponent<Renderer>().material.mainTexture = tex;

        Perlin pNoise = gameObject.GetComponent<Perlin>();
        pNoise.PerlinNoise(width, height);
        BlueNoise bNoise = gameObject.GetComponent<BlueNoise>();

        Color color = Color.green;
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
            color = Color.blue;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tex.SetPixel(x, y, color);
                }
            }            

            break;
        case 3:
         // Perlin Noise
            List<float> perlinNoise = pNoise.GetfPerlinNoise2D();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    color = new Color((float)perlinNoise[y * width + x], (float)perlinNoise[y * width + x], (float)perlinNoise[y * width + x], 1.0f);
                    tex.SetPixel(x, y, color);
                }
            }
            break;
        case 4:
        // Worley Noise
            color = Color.red;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tex.SetPixel(x, y, color);
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
