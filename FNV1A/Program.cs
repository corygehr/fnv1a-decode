namespace FNV1A
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Numerics;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Text;

    class Program
    {
        static HashAlgorithm Algo = new Fnv1a128();

        static void Main(string[] args)
        {
            if(args.Length > 3)
            {
                string hashPath = args[0];
                string wordPath = args[1];
                string outputPath = args[2];
                string remainingHashesPath = String.Empty;

                if(args.Length == 4)
                {
                    remainingHashesPath = args[3];
                }

                try
                {
                    // Must preserve order
                    string[] hashLines = System.IO.File.ReadAllLines(hashPath);
                    string[] outputLines = new string[hashLines.Length];

                    // initialize
                    for (int i = 0; i < outputLines.Length; i++)
                    {
                        outputLines[i] = String.Empty;
                    }

                    using (StreamReader reader = new StreamReader(wordPath))
                    {
                        string line;
                        int lineCount = 0;
                        while((line = reader.ReadLine()) != null)
                        {
                            string encodedWord = Fnv1a128s(line).ToLower();

                            // Check if encoding matches in password list
                            for(int i=0; i<hashLines.Length; i++)
                            {
                                string hash = hashLines[i];
                                if(encodedWord == hash)
                                {
                                    outputLines[i] = line;
                                }
                            }

                            lineCount++;

                            if(lineCount % 100 == 0)
                            {
                                Console.WriteLine(lineCount + " words processed.");
                            }
                        }
                    }

                    // Dump results to file
                    int writtenCount = 0;
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(outputPath))
                    {
                        for (int i = 0; i < outputLines.Length; i++)
                        {
                            if (!String.IsNullOrEmpty(outputLines[i]))
                            {
                                file.WriteLine(String.Format("{0}: {1}", hashLines[i], outputLines[i]));
                                writtenCount++;
                            }
                        }
                    }

                    Console.WriteLine("Wrote " + writtenCount + " results.");

                    // Save remaining hashes to another file
                    if(!String.IsNullOrEmpty(remainingHashesPath))
                    {
                        int remainingCount = 0;
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(remainingHashesPath))
                        {
                            for (int i = 0; i < outputLines.Length; i++)
                            {
                                if (String.IsNullOrEmpty(outputLines[i]))
                                {
                                    file.WriteLine(hashLines[i]);
                                    remainingCount++;
                                }
                            }
                        }

                        Console.WriteLine(remainingCount + " remain.");
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Failed: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Provide a path to the input hashes (0), wordlist (1), output path (2), and optionally a file to store uncracked hashes (3).");
            }
        }

        /// <summary>
        /// Computes the FNV-1a 128-bit hash for the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The FNV-1a 128-bit hash of the specified data.</returns>
        // ReSharper disable once InconsistentNaming
        private static string Fnv1a128s(string data)
        {
            using (HashAlgorithm alg = new Fnv1a128())
            {
                string value = new BigInteger(alg.ComputeHash(Encoding.UTF8.GetBytes(data)).AddZero()).ToString("X32", System.Globalization.CultureInfo.InvariantCulture);

                return value.Substring(value.Length - 32);
            }
        }
    }
}