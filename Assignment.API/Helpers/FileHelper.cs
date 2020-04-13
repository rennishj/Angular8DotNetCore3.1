using Assignment.API.Model;
using Assignment.Model.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.API.Helpers
{
    public  class FileHelper
    {
        public static List<string> HeaderColumns
        {
            get { return new List<string> { "InsuranceCompany", "FirstName", "LastName", "UserId", "Version", }; }
        }

        public static List<string> StateFileHeaderColumns
        {
            get { return new List<string> { "State", "Code"}; }
        }

        public static string BurstedFileCreationPath { get; set; }

        public static async Task<List<string>> BurstEnrollmentFile(FileUploadDto dto)
        {
            BurstedFileCreationPath = dto.PathToBurstedFile;
            var burstedFileList = new List<string>();
            var enrollmentFileList = new List<EnrollmentFile>();
            var file = dto.FileContent;
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                string line = "";
                string prevInsuranceCompany = null;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    string[] values = line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    var isHeaderRow = values.ToList().Any(v => HeaderColumns.Any(h => h == v));
                    if (isHeaderRow)
                        continue;
                    var currentInsuranceCompany = values[0]; //This assumes the first column is the insurance company
                    if (!string.IsNullOrWhiteSpace(currentInsuranceCompany) && !string.IsNullOrWhiteSpace(prevInsuranceCompany))
                    {
                        if (!currentInsuranceCompany.ToLower().Equals(prevInsuranceCompany.ToLower()))
                        {
                            var dedupedList = RemoveDuplicates(
                                enrollmentFileList.OrderBy(o => o.LastName)
                                .ThenBy(o => o.FirstName)
                                .ToList()

                                );
                            burstedFileList.AddRange(await GenerateCsvFile(dedupedList));
                            enrollmentFileList = null;
                        }
                    }
                    var enrollmentFile = new EnrollmentFile
                    {
                        InsuranceCompany = values[0],
                        FirstName = values[1],
                        LastName = values[2],
                        UserId = values[3],
                        Version = int.TryParse(values[4], out var version) ? version : -1
                    };
                    if (enrollmentFileList == null)
                        enrollmentFileList = new List<EnrollmentFile>();
                    enrollmentFileList.Add(enrollmentFile);
                    prevInsuranceCompany = currentInsuranceCompany;
                    currentInsuranceCompany = null;
                }
            }
            if (enrollmentFileList != null && enrollmentFileList.Count() > 0)
            {
                var dedupedList = RemoveDuplicates(
                                enrollmentFileList.OrderBy(o => o.LastName)
                                .ThenBy(o => o.FirstName)
                                .ToList()

                                );
                burstedFileList.AddRange(await GenerateCsvFile(dedupedList));
            }
            return burstedFileList;
        }

        private static List<EnrollmentFile> RemoveDuplicates(List<EnrollmentFile> enrollmentList)
        {            

            var groupById = enrollmentList.GroupBy(e => e.UserId)
                .SelectMany(m => m.Where(f => f.Version == m.Max(c => c.Version)))
                .ToList();
            return groupById;
        }

        public static async Task<List<string>> GenerateCsvFile(List<EnrollmentFile> enrollmentList)
        {
            var burstedFiles = new List<string>();
            var insuranceCompanyName = enrollmentList.FirstOrDefault()?.InsuranceCompany;
            var completeFileName = Path.Combine(BurstedFileCreationPath + "\\" + insuranceCompanyName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff")) + ".csv";
            burstedFiles.Add(completeFileName);
            var headerRow = string.Join(",", HeaderColumns);
            using (var sw = new StreamWriter(completeFileName))
            {
                await sw.WriteLineAsync(headerRow);
                foreach (var item in enrollmentList)
                {
                    await sw.WriteLineAsync(item.ToString());                    
                }
            }
            return burstedFiles;

        }

        public static List<State>  GetAllStatesFromFile(string filePath)
        {            
            List<State> allStates = new List<State>();            
            using (var reader = new StreamReader(filePath))
            {
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    string[] values = line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    var isHeaderRow = values.ToList().Any(v => StateFileHeaderColumns.Any(h => h == v));
                    if (isHeaderRow)
                        continue;
                    var state = new State
                    {
                        Name = values[0],
                        Code = values[1]
                    };
                    allStates.Add(state);

                }
            }
            return allStates;
        }
    }
}
