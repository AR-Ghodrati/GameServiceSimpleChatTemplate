using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Utils
{
    public class ImageUtil
    {
        public static IEnumerator DownloadImage(string mediaUrl,RawImage image)
        {   
            var request = UnityWebRequestTexture.GetTexture(mediaUrl);
            yield return request.SendWebRequest();
            image.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
        }    
        
        /// <summary>
        /// Converts this RenderTexture to a RenderTexture in ARGB32 format.
        /// Can also return the original RenderTexture if it's already in ARGB32 format.
        /// Note that the resulting temporary RenderTexture should be released if no longer used to prevent a memory leak.
        /// </summary>
        public static RenderTexture ConvertToArgb32(RenderTexture self)
        {
            if (self.format == RenderTextureFormat.ARGB32) return self;
            var result = RenderTexture.GetTemporary(self.width, self.height, 0, RenderTextureFormat.ARGB32);
            Graphics.Blit(self, result);
            return result;
        }
    }
}