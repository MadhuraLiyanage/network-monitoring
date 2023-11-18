using MailKit.Net.Smtp;
using Newtonsoft.Json;
using Serilog;
using TPOSMonitoring.Models;
using TPOSMonitoring.WS.Enum;
using TPOSMonitoring.WS.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace TPOSMonitoring.WS
{
    public class CheckStatus
    {
        private readonly ITPOSMonitoringRepository _skyBuysRepository = new TPOSMonitoringRepository();

        public async void SendSkyBuysFile()
        {
            await ProcessCSVFile();
        }
        private async Task<bool> ProcessCSVFile()
        {
            TextLogger.LogToText(LoogerType.Information, $"Extracting Details to Create CSV");
            TextLogger.LogToText(LoogerType.Information, "Data extractions started");

            try
            {
                File.Delete(GlobalStaticVaiables.SkyBuysFilePath + GlobalStaticVaiables.SkyBuysFileName);
                TextLogger.LogToText(LoogerType.Information, "Existing Items data extraction file deleted successfully");
            }
            catch (Exception ex)
            {
                TextLogger.LogToText(LoogerType.Error, $"Error deleting existing Items data extraction file. Exception : {ex.Message}");
            }

            try
            {
                File.Delete(GlobalStaticVaiables.SkyBuysFilePath + GlobalStaticVaiables.SkyBuysPromFileName);
                TextLogger.LogToText(LoogerType.Information, "Existing Promo data extraction file deleted successfully");
            }
            catch (Exception ex)
            {
                TextLogger.LogToText(LoogerType.Error, $"Error deleting existing Promo data extraction file. Exception : {ex.Message}");
            }



            try
            {

                IEnumerable<SkyBuysItem> skyBuysItems = _skyBuysRepository.GetSkyBuysItems();
                TextLogger.LogToText(LoogerType.Information, "Product data extracted successfully");
                //create CSV file
                TextLogger.LogToText(LoogerType.Information, "Product CSV file building started");

                using (StreamWriter sw = new StreamWriter(GlobalStaticVaiables.SkyBuysFilePath + GlobalStaticVaiables.SkyBuysFileName))
                {
                    foreach (SkyBuysItem skyBuysItem in skyBuysItems)
                    {
                        sw.WriteLine($"{skyBuysItem.Brand},{skyBuysItem.ShortDescription.Replace("\n", "").Replace("\r", "")},{skyBuysItem.LongDescription.Replace("\n", "")}," +
                            $"{skyBuysItem.Size},{skyBuysItem.ImageURL},{skyBuysItem.Sku},{skyBuysItem.RrpPrice}," +
                            $"{skyBuysItem.Category},{skyBuysItem.SubCategory},{skyBuysItem.Location},{skyBuysItem.AdditionProductImage}," +
                            $"{skyBuysItem.AdditionalProductVideo},{skyBuysItem.Type},{skyBuysItem.SOH}");
                    }
                }
                TextLogger.LogToText(LoogerType.Information, "Product CSV file building completed successfully");
            }
            catch(Exception ex)
            {
                TextLogger.LogToText(LoogerType.Error, $"Error Processing Product CSV File. Exception : {ex.Message}");
                return false;
            }

            try
            {
                //generate Promo file
                IEnumerable<SkyBuysPromo> skyBuysPromos = _skyBuysRepository.GetSkyBuysPromo();
                TextLogger.LogToText(LoogerType.Information, "Promo data extracted successfully");
                //create CSV file
                TextLogger.LogToText(LoogerType.Information, "Promo CSV file building started");
                using (StreamWriter sw = new StreamWriter(GlobalStaticVaiables.SkyBuysFilePath + GlobalStaticVaiables.SkyBuysPromFileName))
                {
                    foreach (SkyBuysPromo skyBuysPromo in skyBuysPromos)
                    {
                        sw.WriteLine($"{skyBuysPromo.PromotionId},{skyBuysPromo.PromotionType},{skyBuysPromo.OnSaleFrom}," +
                            $"{skyBuysPromo.OnSaleTo},{skyBuysPromo.PromotionDescription.Replace("\n", "").Replace("\r","")},{skyBuysPromo.Sku},{skyBuysPromo.StandardSalesPrice}," +
                            $"{skyBuysPromo.PromotionSalesPrice}");
                    }
                }

            }
            catch(Exception ex)
            {
                TextLogger.LogToText(LoogerType.Error, $"Error Processing Promo CSV File. Exception : {ex.Message}");
                return false;
            }

            TextLogger.LogToText(LoogerType.Information, "Promo CSV file building completed successfully");

            return true;
        }
    }
}

