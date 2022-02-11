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
        Perlin pNoise = gameObject.GetComponent<Perlin>();
        BlueNoise bNoise = gameObject.GetComponent<BlueNoise>();

        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        GetComponent<Renderer>().material.mainTexture = tex;

        // Read screen contents into the texture
        for (int y = 0; y < height; y++){
            for (int x = 0; x < width; x++){
                Color color = Color.green;
                switch (noiseNum){
                case 1:
                // White Noise
                    color = ((Random.Range(1, 3)) <=1 ? Color.black : Color.white);
                    break;
                case 2:
                // Blue Noise
                color = Color.blue;
                    break;
                case 3:
                // Perlin Noise
                    double colorValue = pNoise.OctavePerlin(x/width,y/height, 1, 8, 256);
                    color = new Color((float)colorValue,0.0f,0.5f,1.0f);
                    break;
                case 4:
                // Worley Noise
                    color = Color.red;
                    break;
                default:
                    break;
                }
                
                tex.SetPixel(x, y, color);
            }
        }
        tex.Apply();

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        //Object.Destroy(tex);

        // For testing purposes, also write to a file in the project folder
        File.WriteAllBytes(Application.dataPath + "/../Assets/TestPNGs/SavedScreen.png", bytes);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            print(i);
            i+=1;
            if(i>4){
                i=1;
            }
            UploadPNG(i);
        }
    }
}
