using System;
using System.Collections.Generic;
using System.Data;
using BankProject_DataAccessLayer;

namespace BankProject_BusinessLayer
{
    public class clsPerson
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int PersonID { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public string Gendar {  set; get; }

        public string FullName { get; }

        public clsPerson()
        {
            PersonID = -1;
            FirstName = "";
            LastName = "";
            Email = "";
            Phone = "";
            Gendar = "";
            FullName = "";
            Mode = enMode.AddNew;
        }

        private clsPerson(int personID , string firstName, string lastName, string email, string phone , string gendar)
        {
            PersonID = personID;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Gendar = gendar;
            FullName = firstName.Trim() + " " + lastName.Trim();
            Mode = enMode.Update;
        }


        private bool _AddNewPerson()
        {
            this.PersonID = clsPersonData.AddNewPerson(this.FirstName, this.LastName , this.Email , this.Phone, this.Gendar);

            return (this.PersonID != -1);
        }


        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(this.PersonID, this.FirstName, this.LastName, this.Email, this.Phone, this.Gendar);

        }

        public static clsPerson Find(int PersonID)
        {

            string firstName = "", lastName = "", email = "", phone = "", gendar = "";

            if (clsPersonData.GetPersonInfoByID(PersonID, ref firstName, ref lastName, ref gendar, ref phone, ref email))
            {
                return new clsPerson(PersonID, firstName, lastName, gendar, phone, email);
            }
            else
            {
                return null;
            }

        }

        public bool Save()
        {

            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdatePerson();

            }

            return false;
        }


        public static DataTable GetAllPeople()
        {
            return clsPersonData.GetAllPeople();

        }

        public static bool DeletePerson(int ID)
        {
            return clsPersonData.DeletePerson(ID);
        }

        public static bool IsPersonExist(int ID)
        {
            return clsPersonData.IsPersonExist(ID);
        }


        public static bool IsPersonAllreadyHasClient(int PersonID)
        {
            return clsPersonData.IsPersonHasClient(PersonID);
        }

        public static bool IsPersonAllreadyHasUser(int PersonID)
        {
            return clsPersonData.IsPersonHasUser(PersonID);
        }
    }

}

