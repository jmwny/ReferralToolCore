using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReferralToolCore.Models
{
    class Database
    {
        private static readonly HttpClient client = new HttpClient();
        private string Magick;

        public Database()
        {
            Magick = "0";
            //client.BaseAddress = new Uri("https://localhost:44334");
            client.BaseAddress = new Uri("https://referral.helix-1.com");
            client.DefaultRequestHeaders.Add("User-Agent", $"{Environment.UserName.ToLower()}");
        }

        // API
        public async Task<bool?> HasMagick()
        {
            string newMagick;

            try
            {
                var response = await client.GetStringAsync("MagickUpdate");
                var tempMagick = JsonSerializer.Deserialize<Dictionary<string, int>>(response);
                newMagick = tempMagick["magick"].ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"Exception in HasMagick(): {ex.Message}");
                return null;
            }

            if (newMagick != Magick)
            {
                Magick = newMagick;
                return true;
            }
            return false;
        }

        // API
        public async Task<List<Dictionary<string, string>>> GetOpenCollection()
        {
            List<Dictionary<string, string>> collectionData;

            try
            {
                var response = await client.GetStringAsync($"ReferralCollection");
                collectionData = JsonSerializer.Deserialize<List<Dictionary<string, string>>> (response);
            }
            catch (Exception ex)
            {
                collectionData = null;
                System.Diagnostics.Trace.WriteLine($"Exception in GetOpenCollection(): {ex.Message}");
            }
            return collectionData;
        }

        // API
        public async Task<List<Dictionary<string, string>>> GetHistoryCollection(string reqDate)
        {
            List<Dictionary<string, string>> collectionData;

            try
            {
                var response = await client.GetStringAsync($"ReferralCollection/{reqDate}");
                collectionData = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(response);
            }
            catch (Exception)
            {
                collectionData = null;
            }
            return collectionData;
        }

        // API
        public async Task<string> InsertIntoCollection(ReferralData newReferral)
        {
            string returnString;

            var values = new Dictionary<string, string>
            {
                { "PatientName", newReferral.PatientName},
                { "CAD", newReferral.CAD},
                { "CallStatus", newReferral.CallStatus},
                { "DateOfDischarge", newReferral.DateOfDischarge},
                { "RequestedTime", newReferral.RequestedTime},
                { "CallTaker", newReferral.CallTaker},
                { "Nature", newReferral.Nature},
                { "Provider", newReferral.Provider},
                { "CreatedDate", newReferral.CreatedDate},
                { "CreatedTime", newReferral.CreatedTime}
            };

            try
            {
                var encodedContent = new FormUrlEncodedContent(values);
                var response = await client.PostAsync("ReferralItem", encodedContent);
                returnString = response.StatusCode.ToString();
            }
            catch (Exception e)
            {
                returnString = e.Message.ToString();
            }

            return ($"InsertIntoCollection: {returnString}");
        }

        // API
        public async Task<string> UpdateReferral(ReferralData updatedReferral)
        {
            string returnString;

            var values = new Dictionary<string, string>
            {
                { "ID", updatedReferral.ID.ToString() },
                { "EditTime", DateTime.Now.ToString("HH:mm:ss") },
                { "CallTaker", Environment.UserName.ToLower()},
                { "PatientName", updatedReferral.PatientName },
                { "CAD", updatedReferral.CAD },
                { "CallStatus", updatedReferral.CallStatus },
                { "DateOfDischarge", updatedReferral.DateOfDischarge },
                { "RequestedTime", updatedReferral.RequestedTime },
                { "Nature", updatedReferral.Nature },
                { "Provider", updatedReferral.Provider }
            };

            try
            {
                var encodedContent = new FormUrlEncodedContent(values);
                var response = await client.PutAsync("ReferralItem", encodedContent);
                returnString = await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                returnString = e.Message.ToString();
            }

            return (returnString);
        }

        // API
        public async Task<List<Dictionary<string, string>>> GetReferralHistoryDetails(string reqId)
        {
            List<Dictionary<string, string>> collectionData;

            try
            {
                var response = await client.GetStringAsync($"ReferralItem?id={reqId}");
                collectionData = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(response);
            }
            catch (Exception)
            {
                collectionData = null;
            }
            return collectionData;
        }
    }
}