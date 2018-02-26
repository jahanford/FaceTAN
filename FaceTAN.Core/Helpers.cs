using Amazon.Rekognition.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceTAN.Core
{
    public static class Helpers
    {

        public static void PrintFaceDetailList(List<FaceDetail> list, String appendTitle = "")
        {
            //Track the number of faces
            int faceCounter = 1;

            foreach (FaceDetail face in list)
            {

                Console.WriteLine("\n\t## " + appendTitle + " Face Number " + faceCounter + " ##\n");

                if (face.Confidence > 0) Console.WriteLine("\tOverall Confidence: " + face.Confidence);

                if (face.Gender != null) Console.WriteLine("\tGender: " + face.Gender.Value + "\tConfidence: " + face.Gender.Confidence);
                if (face.AgeRange != null) Console.WriteLine("\tAge Range: " + face.AgeRange.Low + " - " + face.AgeRange.High);

                if (face.Emotions != null) Console.WriteLine("\tEmotions: ");
                PrintEmotionList(face.Emotions, "\t\t");

                if (face.Beard != null) Console.WriteLine("\tBeard: " + face.Beard.Value + "\tConfidence: " + face.Beard.Confidence);
                if (face.Mustache != null) ;

                if (face.EyesOpen != null) ;
                if (face.MouthOpen != null) ;
                if (face.Smile != null) ;
                if (face.Sunglasses != null) ;

                if (face.Quality != null) ;

                faceCounter++;
            }
            Console.WriteLine();
        }

        private static void PrintEmotionList(List<Emotion> list, String appendLine = "")
        {
            foreach (Emotion emotion in list)
            {
                Console.WriteLine(appendLine + emotion.Type + "\tConfidence: " + emotion.Confidence);
            }
        }

        public static MemoryStream PathToMemoryStream(String ImagePath)
        {
            return new MemoryStream(File.ReadAllBytes(ImagePath)); ;
        }

        public static MemoryStream ImageToMemoryStream(System.Drawing.Image inputImage)
        {
            using (MemoryStream m = new MemoryStream())
            {
                inputImage.Save(m, inputImage.RawFormat);
                return m;
            }
        }

        public static List<System.Drawing.Image> CropDetectedFaces(List<FaceDetail> listOfFaces, String imagePath, Boolean saveCroppedImage)
        {
            List<System.Drawing.Image> croppedFaces = new List<System.Drawing.Image>();
            System.Drawing.Image inputImage = System.Drawing.Image.FromFile(imagePath);

            int faceCounter = 0;

            foreach (FaceDetail face in listOfFaces)
            {
                faceCounter++;

                System.Drawing.Image faceImage = CropFaceToImage(inputImage, face);
                croppedFaces.Add(faceImage);
                if (saveCroppedImage) faceImage.Save(imagePath.Insert(imagePath.Length - 4, "AMAZON"));

            }
            return croppedFaces;
        }

        public static System.Drawing.Image CropFaceToImage(System.Drawing.Image inputImage, Microsoft.ProjectOxford.Face.Contract.Face detectedFace)
        {
            try
            {
                //Creates Rectangle of Detected Face Specified Location and Size 
                Rectangle cropArea = new Rectangle(detectedFace.FaceRectangle.Left, detectedFace.FaceRectangle.Top, detectedFace.FaceRectangle.Width, detectedFace.FaceRectangle.Height);

                Bitmap srcImage = inputImage as Bitmap;
                Bitmap croppedImage = new Bitmap(cropArea.Width, cropArea.Height);

                using (Graphics g = Graphics.FromImage(croppedImage))
                {
                    g.DrawImage(srcImage, new Rectangle(0, 0, croppedImage.Width, croppedImage.Height), cropArea, GraphicsUnit.Pixel);
                }
                return croppedImage;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        private static System.Drawing.Image CropFaceToImage(System.Drawing.Image inputImage, FaceDetail detectedFace)
        {
            try
            {
                //Creates Rectangle of Detected Face Specified Location and Size 
                Rectangle cropArea = new Rectangle(XcordFromFace(inputImage, detectedFace), YcordFromFace(inputImage, detectedFace), WidthFromFace(inputImage, detectedFace), HeightFromFace(inputImage, detectedFace));

                Bitmap srcImage = inputImage as Bitmap;
                Bitmap croppedImage = new Bitmap(cropArea.Width, cropArea.Height);

                using (Graphics g = Graphics.FromImage(croppedImage))
                {
                    g.DrawImage(srcImage, new Rectangle(0, 0, croppedImage.Width, croppedImage.Height), cropArea, GraphicsUnit.Pixel);
                }
                return croppedImage;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        private static int XcordFromFace(System.Drawing.Image image, FaceDetail face)
        {
            return Convert.ToInt32(image.Width * face.BoundingBox.Left);
        }

        private static int YcordFromFace(System.Drawing.Image image, FaceDetail face)
        {
            return Convert.ToInt32(image.Height * face.BoundingBox.Top);
        }

        private static int WidthFromFace(System.Drawing.Image image, FaceDetail face)
        {
            return Convert.ToInt32(image.Width * face.BoundingBox.Width);
        }
        private static int HeightFromFace(System.Drawing.Image image, FaceDetail face)
        {
            return Convert.ToInt32(image.Height * face.BoundingBox.Height);
        }
    }
}
