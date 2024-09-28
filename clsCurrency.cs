using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BankProject_DataAccessLayer;
using System.IO;
using System.Data;
using System.Runtime.Remoting.Messaging;

namespace BankProject_BusinessLayer
{
    public class clsCurrency
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int CurrencyID { get; set; }
        public string CountryName { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public double Rate { get; set; }

        public clsCurrency()
        {
            //CurrencyID = -1;
            CurrencyName = "";
            CurrencyCode = "";
            CountryName = "";
            Rate = 0;
            Mode = enMode.AddNew;
        }

        public clsCurrency(int currencyID, string countryName, string currencyCode, string currencyName, double rate)
        {
            CurrencyID = currencyID;
            CurrencyName = currencyName;
            CurrencyCode = currencyCode;
            CountryName = countryName;
            Rate = rate;
            Mode = enMode.Update;
        }


        //You Will User These Methods Just For One Time//---------------------------------------------------------------------------
        private static clsCurrency _ConvertLineCurrencyObject(string Line, string Seperator = "#//#")
        {
            string[] CurrencyRecords = Line.Split(new string[] { Seperator }, StringSplitOptions.None);

            return new clsCurrency(-1 , CurrencyRecords[0], CurrencyRecords[1], CurrencyRecords[2], Convert.ToDouble(CurrencyRecords[3]));
        }

        private static List<clsCurrency> _LoadeCurrencysDataFromFile()
        {
            List<clsCurrency> LCurrency = new List<clsCurrency>();
            var File = new StreamReader(@"C:\Users\Windows 10\Desktop\repos\BankProject-DataAccessLayer\Currencies.txt");

            string Line = "";
            while ((Line = File.ReadLine()) != null)
            {
                clsCurrency Currency = _ConvertLineCurrencyObject(Line);
                LCurrency.Add(Currency);   
            }
            File.Close();
            return LCurrency;
        }

        //just use it for one time << Warrning >>
        public static void AddAllCurrencies()
        {
            List<clsCurrency> AllCurrenciesObjects = _LoadeCurrencysDataFromFile();

            foreach(clsCurrency currency in AllCurrenciesObjects)
            {
                if (currency.AddNewCurrency())
                {
                    Console.WriteLine(currency.CurrencyID);
                }
                else
                {
                    Console.WriteLine("currency does'nt added Successfuly");
                }
            }
        }
        //---------------------------------------------------------------------------


        public bool AddNewCurrency()
        {
            this.CurrencyID = clsCurrenciesData.AddNewCurrency(this.CountryName, this.CurrencyCode, this.CurrencyName, this.Rate);
            return (this.CurrencyID != -1);
        }

        public static DataTable GetAllCurrencies()
        {
            return clsCurrenciesData.GetAllCurrencies();
        }

        public static clsCurrency Find(int CurrencyID)
        {
            string CountryName = "", CurrencyCode = "", CurrencyName = "";
            double Rate = 0;
            if (clsCurrenciesData.GetCurrenciesInfoByID(CurrencyID, ref CountryName , ref CurrencyCode, ref CurrencyName, ref Rate))
                return new clsCurrency(CurrencyID, CountryName, CurrencyCode, CurrencyName, Rate);
            else
                return null;
        }

    }
}






/*
 * var x = new StreamReader();
            x.Read();
            x.Close();
 */