using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using NazdaqSearch.Models;

namespace NazdaqSearch.Logic.UserLogic
{
    public static class InformationMethods
    {

        private static string path = System.Web.Hosting.HostingEnvironment.MapPath(@"~/UserData.txt");

        public static void createUserData(userModel item)
        {
            string[] userData = new string[] {item.UserID, item.Password, item.ID.ToString() };
            System.IO.File.WriteAllLines(path, userData);
        }

        public static userModel retrieveUserData()
        {
            var CurrentUser = new userModel();
            var fileData = System.IO.File.ReadAllLines(path);

            CurrentUser.UserID = fileData[0];
            CurrentUser.Password = fileData[1];
            CurrentUser.ID = int.Parse(fileData[2]);

            return CurrentUser;

        }

        public static void logout()
        {
            DirectoryInfo toBeErased = new DirectoryInfo(path);

            foreach (FileInfo file in toBeErased.GetFiles()) file.Delete();
        }

    }
}