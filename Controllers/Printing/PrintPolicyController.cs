using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using RCTPL_WebProjects.Models;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace RCTPL_WebProjects.Controllers.Printing
{
    public class PrintPolicyController : Controller
    {

        private RCTPLEntities db = new RCTPLEntities();
        private string notEncrypted;
        private string fullyEncrypted;
        private const int Keysize = 256;
        private const int DerivationIterations = 1000;
        private string crpathLTO = "/CrystalReport/RptLTO.rpt";
        private string crpathNONLTO = "/CrystalReport/RptNonLTO.rpt";

        [HttpGet]
        public ActionResult PrintLTO(string tCOI, string LTOType, string PrefixLTO)
        {
            var ListLTO = (from chrgd in db.T_BILLCHRGD
                           from mpaip in db.M_PAIP
                           from chrgh in db.T_BILLCHRGH
                           where chrgd.BCHCHRGNO == chrgh.BCHCHRGNO
                           && mpaip.PAPIN == chrgh.PAPIN
                           && chrgd.COLORNUM != null
                           && chrgd.COI == tCOI
                           && mpaip.NON_LTO == LTOType
                           select new
                           { chrgh.BCHDTE,
                           mpaip.PALNAME,
                           mpaip.FIRST_NAME,
                           mpaip.MIDDLE_NAME,
                           mpaip.COMPANY_NAME,
                           mpaip.BY_COMPNAME,
                           chrgd.BCDPATCOVER,
                           chrgd.BCDSERIES,
                           chrgd.SUM_INSIRED,
                           chrgd.BCDTPPCOVER,
                           chrgd.BCHCHRGNO,
                           mpaip.MODEL_YR,
                           chrgd.COI,
                           chrgd.AUTHEN_CODE,
                           chrgd.BCDSINO,
                           mpaip.PAADDRESS,
                           mpaip.PLATE_NO,
                           mpaip.NON_LTO,
                           mpaip.MAKE,
                           mpaip.SERIAL_NO,
                           mpaip.COLOR,
                           mpaip.UN_WEIGHT,
                           mpaip.COMP_ID,
                           chrgd.INCEPTION_FROM,
                           chrgd.INCEPTION_TO,
                           mpaip.MOTOR_NO,
                           chrgd.COLORNUM,
                           mpaip.SERVICE_TYPE,
                           mpaip.SEATING_CAPACITY,
                           mpaip.MV_FILENO,
                           chrgd.PRNT,
                           chrgd.DATE_PRINTED,
                           chrgd.COC_SERIES}).FirstOrDefault();

            var listR_SGSI = (from sgseries in db.R_SGSI
                              where sgseries.PREFIX == PrefixLTO
                              select sgseries).FirstOrDefault();

            var m_CompID = (from compid in db.M_COMPANY
                                     select compid.COMP_ID).FirstOrDefault();

            decimal currentSERIES = Convert.ToDecimal(listR_SGSI.SGSERIES) + 1;
            var prefix = listR_SGSI.PREFIX;

            var PolicyNo = prefix + currentSERIES.ToString("0000000");
            var CertOfCover = m_CompID + currentSERIES.ToString("0000000");

            LTO oLTO = new LTO();
            oLTO.AUTHENCODE = ListLTO.AUTHEN_CODE;
            oLTO.BCDPATCOVER = Convert.ToDecimal(ListLTO.BCDPATCOVER);
            oLTO.BCDSERIES = ListLTO.BCDSERIES;
            oLTO.BCDSINO = ListLTO.BCDSINO;
            oLTO.BCDTPPCOVER = Convert.ToDecimal(ListLTO.BCDTPPCOVER);
            oLTO.BCHCHRGNO = ListLTO.BCHCHRGNO;
            oLTO.BCHDTE = Convert.ToDateTime(ListLTO.BCHDTE);
            oLTO.ByCOMPNAME = ListLTO.BY_COMPNAME.ToString();

            //Incremented COC
            oLTO.COCSERIES = CertOfCover;
            oLTO.COI = PolicyNo;

            //sdkjhaskdhaskjhdkasj
            oLTO.COLOR = ListLTO.COLOR;
            oLTO.COLORNUM = ListLTO.COLORNUM;
            oLTO.CompanyName = ListLTO.COMPANY_NAME;
            oLTO.COMPID = ListLTO.COMP_ID;
            oLTO.DATEPRINTED = Convert.ToDateTime(ListLTO.DATE_PRINTED);
            oLTO.FirstName = ListLTO.FIRST_NAME;
            oLTO.INCEPTIONFROM = Convert.ToDateTime(ListLTO.INCEPTION_FROM);
            oLTO.INCEPTIONTO = Convert.ToDateTime(ListLTO.INCEPTION_TO);
            oLTO.MAKE = ListLTO.MAKE;
            oLTO.MiddleName = ListLTO.MIDDLE_NAME;
            oLTO.MODELYR = ListLTO.MODEL_YR;
            oLTO.MOTORNO = ListLTO.MOTOR_NO;
            oLTO.MVFILENO = ListLTO.MV_FILENO;
            oLTO.NONLTO = ListLTO.NON_LTO.ToString();
            oLTO.PAADDRESS = ListLTO.PAADDRESS;
            oLTO.PALNAME = ListLTO.PALNAME;
            oLTO.PLATENO = ListLTO.PLATE_NO;
            oLTO.PRNT = ListLTO.PRNT;
            oLTO.SEATINGCAPACITY = ListLTO.SEATING_CAPACITY;
            oLTO.SERIALNO = ListLTO.SERIAL_NO;

            //Add to encryption string
            notEncrypted = "PAT COVER: " + oLTO.BCDPATCOVER + " PLATE: " + oLTO.PLATENO + " AUTHENTICATION CODE: " + oLTO.AUTHENCODE ;

            if (ListLTO.SERVICE_TYPE == null)
            {
                oLTO.SERVICETYPE = "";
            }
            else
            {
                oLTO.SERVICETYPE = ListLTO.SERVICE_TYPE;
            }
            
            oLTO.SUMINSIRED = Convert.ToDecimal(ListLTO.SUM_INSIRED);
            oLTO.UNWEIGHT = ListLTO.UN_WEIGHT;

            
            //Missing encryption for reference

            //Increments and updates SGSERIES
            
            //return View();

            //updateSGSERIES(PrefixLTO);
            return generateLTORPT(oLTO, LTOType);
        }

        private void updateSGSERIES(string prefixToUpdate)
        {
            var sgSeries = (from abc in db.R_SGSI
                               where abc.PREFIX == prefixToUpdate
                               select abc).FirstOrDefault();
            sgSeries.SGSERIES += 1;
            db.SaveChanges();
        }

        public ActionResult generateLTORPT(LTO listLTOObject, string ltoTypeParam)
        {
            List<LTO> ltoList = new List<LTO>();
            ltoList.Add(listLTOObject);

            var path = "";

            switch (ltoTypeParam)
            {
                case "0":
                    path = crpathLTO;
                    break;
                case "1":
                    path = crpathNONLTO;
                    break;
                default:
                    break;
            }

            if (ltoTypeParam == "0")
            {
                RptLTO rptH = new RptLTO();
                rptH.FileName = Server.MapPath(path);
                rptH.Load();
                rptH.SetDataSource(ltoList);
                rptH.SetParameterValue("encodedField", generateEncodedField());
                Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
            else if (ltoTypeParam == "1")
            {
                RptNonLTO rptNON = new RptNonLTO();
                rptNON.FileName = Server.MapPath(path);
                rptNON.Load();
                rptNON.SetDataSource(ltoList);
                rptNON.SetParameterValue("encodedField", generateEncodedField());
                Stream stream = rptNON.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
            else
            {
                return null;
            }
        }

        private string generateEncodedField()
        {
            var passwordRctpl = "rctpl";
            fullyEncrypted = Encrypt(notEncrypted, passwordRctpl);
            return fullyEncrypted;
        }

        public static string Encrypt(string plainText, string passPhrase)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        //public ActionResult PrintNONLTO(string tBCDSINO)
        //{
        //    var ListLTO = (from chrgd in db.T_BILLCHRGD
        //                   from mpaip in db.M_PAIP
        //                   from chrgh in db.T_BILLCHRGH
        //                   where chrgd.BCHCHRGNO == chrgh.BCHCHRGNO
        //                   && mpaip.PAPIN == chrgh.PAPIN
        //                   && chrgd.COLORNUM != null
        //                   && chrgd.BCDSINO == tBCDSINO
        //                   && mpaip.NON_LTO == "1"
        //                   select new
        //                   {
        //                       chrgh.BCHDTE,
        //                       mpaip.PALNAME,
        //                       mpaip.FIRST_NAME,
        //                       mpaip.MIDDLE_NAME,
        //                       mpaip.COMPANY_NAME,
        //                       mpaip.BY_COMPNAME,
        //                       chrgd.BCDPATCOVER,
        //                       chrgd.BCDSERIES,
        //                       chrgd.SUM_INSIRED,
        //                       chrgd.BCDTPPCOVER,
        //                       chrgd.BCHCHRGNO,
        //                       mpaip.MODEL_YR,
        //                       chrgd.COI,
        //                       chrgd.AUTHEN_CODE,
        //                       chrgd.BCDSINO,
        //                       mpaip.PAADDRESS,
        //                       mpaip.PLATE_NO,
        //                       mpaip.NON_LTO,
        //                       mpaip.MAKE,
        //                       mpaip.SERIAL_NO,
        //                       mpaip.COLOR,
        //                       mpaip.UN_WEIGHT,
        //                       mpaip.COMP_ID,
        //                       chrgd.INCEPTION_FROM,
        //                       chrgd.INCEPTION_TO,
        //                       mpaip.MOTOR_NO,
        //                       chrgd.COLORNUM,
        //                       mpaip.SERVICE_TYPE,
        //                       mpaip.SEATING_CAPACITY,
        //                       mpaip.MV_FILENO,
        //                       chrgd.PRNT,
        //                       chrgd.DATE_PRINTED,
        //                       chrgd.COC_SERIES
        //                   }).FirstOrDefault();

        //    LTO oLTO = new LTO();

        //    oLTO.AUTHENCODE = ListLTO.AUTHEN_CODE;
        //    oLTO.BCDPATCOVER = ListLTO.BCDPATCOVER;
        //    oLTO.BCDSERIES = ListLTO.BCDSERIES;
        //    oLTO.BCDSINO = ListLTO.BCDSINO;
        //    oLTO.BCDTPPCOVER = ListLTO.BCDTPPCOVER;
        //    oLTO.BCHCHRGNO = ListLTO.BCHCHRGNO;
        //    oLTO.BCHDTE = ListLTO.BCHDTE;
        //    oLTO.ByCOMPNAME = Convert.ToChar(ListLTO.BY_COMPNAME);
        //    oLTO.COCSERIES = ListLTO.COC_SERIES;

        //    //Increment COI and reconvert to string
        //    int coiINT = Convert.ToInt16(ListLTO.COI);
        //    coiINT = +1;
        //    string coiSTR = Convert.ToString(coiINT);
        //    oLTO.COI = coiSTR;

        //    oLTO.COLOR = ListLTO.COLOR;
        //    oLTO.COLORNUM = ListLTO.COLORNUM;
        //    oLTO.CompanyName = ListLTO.COMPANY_NAME;
        //    oLTO.COMPID = ListLTO.COMP_ID;
        //    oLTO.DATEPRINTED = ListLTO.DATE_PRINTED;
        //    oLTO.FirstName = ListLTO.FIRST_NAME;
        //    oLTO.INCEPTIONFROM = ListLTO.INCEPTION_FROM;
        //    oLTO.INCEPTIONTO = ListLTO.INCEPTION_TO;
        //    oLTO.MAKE = ListLTO.MAKE;
        //    oLTO.MiddleName = ListLTO.MIDDLE_NAME;
        //    oLTO.MODELYR = ListLTO.MODEL_YR;
        //    oLTO.MOTORNO = ListLTO.MOTOR_NO;
        //    oLTO.MVFILENO = ListLTO.MV_FILENO;
        //    oLTO.NONLTO = Convert.ToChar(ListLTO.NON_LTO);
        //    oLTO.PAADDRESS = ListLTO.PAADDRESS;
        //    oLTO.PALNAME = ListLTO.PALNAME;
        //    oLTO.PLATENO = ListLTO.PLATE_NO;
        //    oLTO.PRNT = ListLTO.PRNT;
        //    oLTO.SEATINGCAPACITY = ListLTO.SEATING_CAPACITY;
        //    oLTO.SERIALNO = ListLTO.SERIAL_NO;
        //    oLTO.SERVICETYPE = ListLTO.SERVICE_TYPE;
        //    oLTO.SUMINSIRED = ListLTO.SUM_INSIRED;
        //    oLTO.UNWEIGHT = ListLTO.UN_WEIGHT;

        //    return View(oLTO);
        //}

    }
}