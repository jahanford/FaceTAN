using Amazon.Rekognition.Model;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace FaceTAN.Core
{
    public abstract class FacialRecognitionAPI<TAccess, TSecret, TConfig>
    {
        public abstract string APIName { get; }
        public FacialRecognitionAPI(TAccess accessKey, TSecret secretKey, TConfig configure) { }
    }
}

