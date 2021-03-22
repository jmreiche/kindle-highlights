using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using kindle_highlight_app.ClientApp.Dtos;
using System.Collections.Generic;
using System.IO;

namespace kindle_highlight_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileUploadController
    {
        [HttpPost]
        public async Task<FileContent> UploadFile([FromForm] FileUpload fileUpload)
        {
            return await Task.Run(() =>
            {
                Dictionary<string, SortedDictionary<string, List<string>>> result = new Dictionary<string, SortedDictionary<string, List<string>>>();
                int counter = 0;
                string line;

                // Read the file and display it line by line.  
                using (var file = new StreamReader(fileUpload.File.OpenReadStream()))
                {
                    string bookTitle = "";
                    string location = "";
                    while ((line = file.ReadLine()) != null)
                    {
                        if (!line.StartsWith("=========="))
                        {
                            switch (counter)
                            {
                                // Book title
                                case 0:
                                    bookTitle = line;
                                    if (result.ContainsKey(line) == false)
                                    {
                                        result.Add(bookTitle, new SortedDictionary<string, List<string>>());
                                    }
                                    break;
                                // Location information
                                case 1:
                                    var stringArray = line.Split(" ");
                                    for (int i = 0; i < stringArray.Length; i++)
                                    {
                                        string word = stringArray[i];
                                        // Find actual location after word "location"
                                        if (word.Equals("location"))
                                        {
                                            location = stringArray[i + 1];
                                        }
                                    }
                                    break;
                                // Line 3 is empty
                                case 2:
                                    break;
                                default:
                                    if (!string.IsNullOrWhiteSpace(line))
                                    {
                                        if (result[bookTitle].ContainsKey(location))
                                            result[bookTitle][location].Add(line);
                                        else
                                            result[bookTitle].Add(location, new List<string>() { line });
                                    }

                                    break;

                            }
                            counter++;
                        }
                        else
                        {
                            // Reset
                            counter = 0;
                        }



                    }
                }

                return new FileContent() { Content = result };
            });

        }

    }
}