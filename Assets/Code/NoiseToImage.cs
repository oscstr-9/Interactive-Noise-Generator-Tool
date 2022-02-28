using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using UnityEditor;

public class NoiseToImage : MonoBehaviour
{
    byte[] bytes;

    // Noise parameters //
    public int width = 128;
    public int height = 128;
    public Color mainColor = Color.black;
    public Color bgColor = Color.white;
    public Color extraColor1 = Color.black;
    public Color extraColor2 = Color.black;
    public InputField textureSize;
    int currentNoise = 1;
    public Toggle keepSeed;
    int maxTexSize;


    // Single Color
    public Dropdown mainColorDD;
    public Dropdown bgColorDD;

    // Multi-Color
    public Dropdown extraColor1DD;
    public Dropdown extraColor2DD;

    // Perlin Noise
    public float scalingBias = 1;
    public int octaves = 1;
    public InputField scalingBiasIF;
    public InputField octavesIF;
    public Toggle strictColors;

    public InputField pColorsRed;
    public InputField pColorsGreen;
    public InputField pColorsBlue;

    // Voronoi Noise
    public InputField regionAmountIF;
    public int regionAmount;
    public Toggle randomColors;
    public Vector3 voronoiColors;
    public InputField vColorsRed;
    public InputField vColorsGreen;
    public InputField vColorsBlue;


    // Blue Noise
    public float bNoiseMinDist = 5; // min dist 1.5 for good visuals
    public InputField minDistBlueNoise;

    // White Noise
    List<int> whiteNoiseSeed = new List<int>();


    // Start is called before the first frame update
    void Start()
    {
        UploadPNG(1);
    }

    public void SaveTexture()
    {
        string path = EditorUtility.SaveFilePanel("Show all images (.png)", Application.dataPath, "GeneratedNoise","png");
        File.WriteAllBytes(path, bytes);
    }

    public void GenNewNoise()
    {
        UploadPNG(currentNoise);
    }
    void GetValuesFromOptions(int noiseNum)
    {
        if(keepSeed.isOn && Mathf.Abs(int.Parse(textureSize.text)) > maxTexSize)
        {
            width = maxTexSize;
            height = width;
            textureSize.text = maxTexSize.ToString();
        }
        else if (Mathf.Abs(int.Parse(textureSize.text)) > 8192)
        {
            width = 8192;
            height = 8192;
            textureSize.text = "8192";
        }
        else
        {
            width = Mathf.Abs(int.Parse(textureSize.text));
            height = width;
        }

        switch (noiseNum)
        {
            case 2:
                // Blue Noise
                if (Mathf.Abs(int.Parse(minDistBlueNoise.text)) < 1)
                {
                    bNoiseMinDist = 1;
                    minDistBlueNoise.text = "1";
                }
                else
                {
                    bNoiseMinDist = Mathf.Abs(int.Parse(minDistBlueNoise.text));
                }
                break;
            case 3:
                // Perlin Noise

                // Makes the program not crash from incorrect float syntax
                if (scalingBiasIF.text.Contains("."))
                {
                    scalingBiasIF.text = scalingBiasIF.text.Replace(".", ",");

                }
                scalingBias = Mathf.Abs(float.Parse(scalingBiasIF.text));
                octaves = Mathf.Abs(int.Parse(octavesIF.text));

                // Stops user from dividing by zero
                float i = width;
                int j = 1;
                while (true)
                {
                    i /= 2;
                    if (i < 1)
                    {
                        break;
                    }
                    j += 1; 
                }
                if(octaves > j)
                {
                    octaves = j;
                    octavesIF.text = j.ToString();
                }
                break;
            case 4:
                // Voronoi Noise
                if(Mathf.Abs(int.Parse(regionAmountIF.text)) < 1)
                {
                    regionAmount = 1;
                    regionAmountIF.text = "1";

                }
                else
                {
                    regionAmount = Mathf.Abs(int.Parse(regionAmountIF.text));
                }

                break;
            default:
                break;
        }

        GetColorFromOptions(noiseNum);
    }

    void GetColorFromOptions(int noiseNum)
    {
        switch (mainColorDD.value)
        {
            case 0:
                mainColor = Color.black;
                break;
            case 1:
                mainColor = Color.white;
                break;
            case 2:
                mainColor = Color.red;
                break;
            case 3:
                mainColor = Color.green;
                break;
            case 4:
                mainColor = Color.blue;
                break;
        }

        switch (bgColorDD.value)
        {
            case 0:
                bgColor = Color.black;
                break;
            case 1:
                bgColor = Color.white;
                break;
            case 2:
                bgColor = Color.red;
                break;
            case 3:
                bgColor = Color.green;
                break;
            case 4:
                bgColor = Color.blue;
                break;
        }

        if (noiseNum == 3 && strictColors.isOn)
        {
            switch (extraColor1DD.value)
            {
                case 0:
                    extraColor1 = Color.black;
                    break;
                case 1:
                    extraColor1 = Color.white;
                    break;
                case 2:
                    extraColor1 = Color.red;
                    break;
                case 3:
                    extraColor1 = Color.green;
                    break;
                case 4:
                    extraColor1 = Color.blue;
                    break;
            }

            switch (extraColor2DD.value)
            {
                case 0:
                    extraColor2 = Color.black;
                    break;
                case 1:
                    extraColor2 = Color.white;
                    break;
                case 2:
                    extraColor2 = Color.red;
                    break;
                case 3:
                    extraColor2 = Color.green;
                    break;
                case 4:
                    extraColor2 = Color.blue;
                    break;
            }

        }
        else if(noiseNum == 4)
        {
            if (vColorsRed.text.Contains("."))
            {
                vColorsRed.text = vColorsRed.text.Replace(".", ",");
            }
            if (vColorsGreen.text.Contains("."))
            {
                vColorsGreen.text = vColorsGreen.text.Replace(".", ",");
            }
            if (vColorsBlue.text.Contains("."))
            {
                vColorsBlue.text = vColorsBlue.text.Replace(".", ",");
            }

            voronoiColors = new Vector3(
                float.Parse(vColorsRed.text),
                float.Parse(vColorsGreen.text),
                float.Parse(vColorsBlue.text));
        }
    }

    public void SetMaxTexSize()
    {
        maxTexSize = width;
    }

    public void UploadPNG(int noiseNum)
    {
        GetValuesFromOptions(noiseNum);
        currentNoise = noiseNum;

        // Create a texture the size of the screen, RGB24 format

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        GetComponent<Renderer>().material.mainTexture = tex;

        switch (noiseNum){
        case 1:
        // White Noise
            Color color = Color.black;
            if (!keepSeed.isOn)
            {
                whiteNoiseSeed.Clear();
            }
            for (int y = 0; y < height; y++)
            {
            for (int x = 0; x < width; x++)
            {
                if(!keepSeed.isOn || whiteNoiseSeed.Count == 0)
                    {
                        whiteNoiseSeed.Add((Random.Range(1, 3)));
                    }
                color = whiteNoiseSeed[y * width + x] <= 1 ? mainColor : bgColor;
                tex.SetPixel(x, y, color);
            }
            }
            break;
        case 2:
        // Blue Noise
            PoissonDiscSampler bNoise = new PoissonDiscSampler(height,width,bNoiseMinDist);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tex.SetPixel(x, y, bgColor);
                }
            }
            foreach (Vector2 sample in bNoise.Samples())
            {
                tex.SetPixel((int)sample.x, (int)sample.y, mainColor);
            }
            break;
        case 3:
        // Perlin Noise
            Perlin pNoise = gameObject.GetComponent<Perlin>();
            pNoise.PerlinNoise(width, height, scalingBias, octaves, keepSeed.isOn);
            List<float> perlinNoise = pNoise.GetfPerlinNoise2D();
            Color perlinColor = Color.white;
            if (strictColors.isOn)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if(perlinNoise[y * width + x] <= 0.25f)
                        {
                            perlinColor = mainColor;
                        }
                        else if (perlinNoise[y * width + x] <= 0.5f)
                        {
                            perlinColor = bgColor;
                        }
                        else if (perlinNoise[y * width + x] <= 0.75f)
                        {
                            perlinColor = extraColor1;
                        }
                        else
                        {
                            perlinColor = extraColor2;
                        }
                           
                        tex.SetPixel(x, y, perlinColor);
                    }
                }
            }

            else
            {

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                            tex.SetPixel(x, y, perlinColor);
                    }
                }

                // Makes the program not crash from incorrect float syntax
                if (pColorsRed.text.Contains("."))
                {
                    pColorsRed.text = pColorsRed.text.Replace(".", ",");
                }
                if (pColorsGreen.text.Contains("."))
                {
                    pColorsGreen.text = pColorsGreen.text.Replace(".", ",");
                }
                if (pColorsBlue.text.Contains("."))
                {
                    pColorsBlue.text = pColorsBlue.text.Replace(".", ",");
                }

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        //perlinColor = new Color((float)perlinNoise[y * width + x], (float)perlinNoise[y * width + x], (float)perlinNoise[y * width + x], 1.0f);
                        perlinColor = new Color(
                            float.Parse(pColorsRed.text) < 0 ? (float)perlinNoise[y * width + x] : float.Parse(pColorsRed.text),
                            float.Parse(pColorsGreen.text) < 0 ? (float)perlinNoise[y * width + x] : float.Parse(pColorsGreen.text),
                            float.Parse(pColorsBlue.text) < 0 ? (float)perlinNoise[y * width + x] : float.Parse(pColorsBlue.text),
                            1.0f);
                        tex.SetPixel(x, y, perlinColor);
                        }
                }
            }
            break;
        case 4:
        // Voronoi Noise
            Color worleyColor = Color.blue;
            VoronoiDiagram voronoiNoise = new VoronoiDiagram();
                if (randomColors.isOn)
                {
                    voronoiColors = new Vector3(-1, -1, -1);
                }
            tex.SetPixels(voronoiNoise.GetDiagram(width, regionAmount, voronoiColors));

            break;
        default:
            break;
        }

        tex.Apply();

        // Encode texture into PNG
        bytes = tex.EncodeToPNG();
    }
}
