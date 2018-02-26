using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace FaceTAN.Core
{
    public class AzureFaceApiHandler: FacialRecognitionAPI<string, string, string>
    {
        private IFaceServiceClient client;

        public override string APIName => "Azure Face API";

        public AzureFaceApiHandler(string subscriptionKey, string region, [Optional]string config) : base (subscriptionKey, region, config)
        {
            client = new FaceServiceClient(subscriptionKey, region);
        }

        /*
         *  Order of methods to match Microsoft Library 
         *  
         *  [[Face]]
         *  DetectFaces
         *  Verify/CompareFaces
         *  Identify
         *  
         *  [[PersonGroup]]
         *  CreatePersonGroup
         *  GetPersonGroup
         *  UpdatePersonGroup
         *  DeletePersonGroup
         *  ListPersonGroups
         *  TrainPersonGroup
         *  GetPersonGroupTrainingStatus
         *  
         *  [[Person]]
         *  CreatePerson
         *  GetPerson
         *  DeletePerson
         *  ListPersons
         *  AddPersonFace (personGroupID, personID, imageStream, [Optional]targetFaceRectangle)
         *  GetPersonFace
         *  UpdatePersonFace
         *  DeletePersonFacre
         *  
         *  FindSimilar
         *  Group
         *  
         *  [[FaceList]]
         *  CreateFaceList
         *  GetFaceList
         *  ListFaceLists
         *  UpdateFaceList
         *  DeleteFaceList
         *  AddFaceToFaceList
         *  DeleteFaceFromFaceList
         *  
        */


        //
        // DETECT FACES
        //
        public Face[] DetectFaces(string imagePath)
        {
            return AsyncHelpers.RunSync(() => AsyncDetectFaces(imagePath));
        }

        private async Task<Face[]> AsyncDetectFaces(string imagePath)
        {

            // The list of Face attributes to return.
            IEnumerable<FaceAttributeType> faceAttributes =
                new FaceAttributeType[] { FaceAttributeType.Gender, FaceAttributeType.Age, FaceAttributeType.Smile, FaceAttributeType.Emotion, FaceAttributeType.Glasses, FaceAttributeType.Hair };

            try
            {
                using (Stream imageStream = File.OpenRead(imagePath))
                {
                    return await client.DetectAsync(imageStream, returnFaceId: true, returnFaceLandmarks: true, returnFaceAttributes: faceAttributes);
                }
            }
            // Catch and display Face API errors.
            catch (FaceAPIException f)
            {
                Console.Write(f.ErrorMessage, f.ErrorCode);
                return new Face[0];
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
        }

        //
        // COMPARE FACES
        //
        public VerifyResult CompareFaces(Guid faceId1, Guid faceId2)
        {
            return AsyncHelpers.RunSync(() => CompareFacesAsync(faceId1, faceId2));
        }

        private async Task<VerifyResult> CompareFacesAsync(Guid faceId1, Guid faceId2)
        {
            try
            {
                return await client.VerifyAsync(faceId1, faceId2);

            }
            catch (FaceAPIException f)
            {
                Console.Write(f.ErrorMessage, f.ErrorCode);
                return null;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
        }

        //
        // IDENTIFY
        //
        public IdentifyResult[] Identify(String personGroupID, Guid[] faceIds)
        {
            return AsyncHelpers.RunSync(() => IdentifyAsync(personGroupID, faceIds));
        }

        public async Task<IdentifyResult[]> IdentifyAsync(String personGroupId, Guid[] faceIds)
        {
            try
            {
                return await client.IdentifyAsync(personGroupId, faceIds);
            }
            catch (FaceAPIException f)
            {
                Console.Write(f.ErrorMessage, f.ErrorCode);
                return null;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
        }

        //
        // CREATE PERSON
        //
        public CreatePersonResult CreatePerson(String name, [Optional]String userData)
        {
            return AsyncHelpers.RunSync(() => AsyncCreatePerson(name, userData));
        }

        private async Task<CreatePersonResult> AsyncCreatePerson(String name, [Optional]String userData)
        {
            try
            {
                return await client.CreatePersonAsync(name, userData);
            }
            // Catch and display Face API errors.
            catch (FaceAPIException f)
            {
                Console.Write(f.ErrorMessage, f.ErrorCode);
                return null;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
        }

        //
        // GET PERSON
        //


        //
        // CREATE PERSON GROUP
        //
        public Boolean CreatePersonGroup(String personGroupId, String name, [Optional]String userData)
        {
            AsyncCreatePersonGroup(personGroupId, name, userData);
            var compareGroup = GetPersonGroup(personGroupId);
            return (compareGroup.PersonGroupId == personGroupId && compareGroup.Name == name);
        }

        private async void AsyncCreatePersonGroup(String personGroupId, String name, [Optional]String userData)
        {
            
            try
            {
                await client.CreatePersonGroupAsync(personGroupId, name, userData);
            }
            // Catch and display Face API errors.
            catch (FaceAPIException f)
            {
                Console.Write(f.ErrorMessage, f.ErrorCode);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }

        //
        //  GET PERSON GROUP
        //
        public PersonGroup GetPersonGroup(String personGroupId)
        {
            return AsyncHelpers.RunSync(() => AsyncGetPersonGroup(personGroupId));
        }

        private async Task<PersonGroup> AsyncGetPersonGroup(String personGroupId)
        {
            try
            {
                return await client.GetPersonGroupAsync(personGroupId);
            }
            // Catch and display Face API errors.
            catch (FaceAPIException f)
            {
                Console.Write(f.ErrorMessage, f.ErrorCode);
                return null;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
        }
    }
}
