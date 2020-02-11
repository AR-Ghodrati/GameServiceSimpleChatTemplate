// <copyright file="FileUtil.cs" company="Firoozeh Technology LTD">
// Copyright (C) 2019 Firoozeh Technology LTD. All Rights Reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>


/**
* @author Alireza Ghodrati
*/

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Utils
{
    public static class FileUtil
    {
        public static void SaveUserToken(string userToken)
        {
            var bf = new BinaryFormatter();
            var file = File.Create (Application.persistentDataPath + "/Login.dat");
            bf.Serialize(file,userToken);
            file.Close();
        }

        public static string GetUserToken()
        {
            if (!File.Exists(Application.persistentDataPath + "/Login.dat")) return null;
            var bf = new BinaryFormatter();
            var file = File.Open(Application.persistentDataPath + "/Login.dat", FileMode.Open);
            if (file.Length == 0) return null;
            var userToken = (string)bf.Deserialize(file);
            file.Close();
            return userToken;
        }

        public static bool IsLoginBefore()
        {
            return GetUserToken() != null;
        }
      
    }
}